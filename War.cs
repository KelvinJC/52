using System;
using System.Collections;
using System.Collections.Generic;
using Cards.Interfaces;


namespace Cards
{
    enum SelectPlayer : byte
    {
        HighestCard = 0, 
        LowestCard = 1,
        Random,
    }

    class War 
    {
        static Random rnd = new ();
        static Dictionary<string, int> cardRank = new(){
            {"Ace", 12}, {"King", 11}, {"Queen", 10}, {"Jack", 9},
            {"10", 8}, {"9", 7}, {"7", 6}, {"8", 5}, {"6", 4},
            {"5", 3}, {"4", 2}, {"3", 1}, {"2", 0}
        };

        public static void Start(IDeck deck, List<IPlayer> cardPlayers)
        {
            // begin game
            deck.Shuffle(shuffleCount: 10);

            Console.WriteLine("Game Started!");
            Console.Write($"Current card: ");

            List<IPlayer> orderedPlayers = OrderPlayers(players: cardPlayers, deck: deck, selection: SelectPlayer.HighestCard);
            Console.WriteLine($"{orderedPlayers[0]} goes first.");

            bool cardsDealt = DealCardsToPlayers(players: orderedPlayers, deck: deck);

            if (cardsDealt)
            {
                GameRound(players: orderedPlayers);
            }
        }

        public static List<IPlayer> OrderPlayers(List<IPlayer> players, IDeck deck, SelectPlayer selection = SelectPlayer.Random)
        {
            // make selection
            IPlayer firstPlayer = players[0];
            if (selection == SelectPlayer.HighestCard)
            {
                firstPlayer = SelectPlayerByHighestCard(players, deck);
            }

            else if (selection == SelectPlayer.Random)
            {
                int pick = rnd.Next(players.Count);
                firstPlayer = players[pick];
            }

            if (firstPlayer != players[0])
            {   // move selected player to index 0
                List<IPlayer> otherPlayers = new();

                foreach (IPlayer player in players)
                {
                    if (player != firstPlayer)
                        otherPlayers.Add(player);
                }

                List<IPlayer> allPlayers = [firstPlayer, .. otherPlayers];
                return allPlayers;
            }
            return players;
        }

        private static IPlayer SelectPlayerByHighestCard(List<IPlayer> players, IDeck deck)
        {
            int highest = 0;
            IPlayer highestPlayer = players[0];
            Dictionary<IPlayer, string> cardRankCache = new();

            // deal a random card to each player
            foreach (IPlayer p in players)
            {
                ICard card = deck.Deal();
                cardRankCache.Add(p, card.Rank);
                deck.ReturnCard(ref card!);
            }

            // compare players' cards
            foreach (KeyValuePair<IPlayer, string> rank in cardRankCache)
            {
                if (cardRank[rank.Value] > highest)
                {
                    highest = cardRank[rank.Value];
                    highestPlayer = rank.Key;
                }
            }

            // check for tie
            int countPlayersWithSameRank = 0;
            foreach (KeyValuePair<IPlayer, string> rank in cardRankCache)
            {
                if (cardRank[rank.Value] == highest)
                {
                    countPlayersWithSameRank++;
                }
            }

            // break tie by retrying
            if (countPlayersWithSameRank > 1)
            {   
                return SelectPlayerByHighestCard(players, deck);
            }

            return highestPlayer;
        }

        public static bool DealCardsToPlayers(List<IPlayer> players, IDeck deck, bool dealFromCardZero = true)
        {
            // deal cards to players one card at a time
            // if dealFromCardZero param is false, dealing begins from last card in deck

            int playerCount = players.Count;
            int cardsPerPlayer = deck.Cards.Count / playerCount;

            try
            {
                for (int cardsDealt = 0; cardsDealt < cardsPerPlayer; cardsDealt++)
                {
                    foreach (IPlayer player in players)
                    {
                        ICard card = dealFromCardZero ? deck.DealFirst() : deck.DealLast();
                        player.AcceptCard(card);
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                foreach (IPlayer player in players)
                {
                    player.DropHand();
                }
                Console.WriteLine(e.Message);
            }
            return false;
        }

        // MIGHT HAVE TO RETURN A VALUE
        private static void GameRound(List<IPlayer> players)
        {
            // players play their cards
            Dictionary<IPlayer, ICard> pile = new ();

            foreach(IPlayer player in players)
            {
                ICard? cardPlayed = player.PlayCardByPosition(position: CardPosition.last);
                if (cardPlayed == null)
                    continue;
                else
                    pile.Add(player, cardPlayed);
            }
            // OR RETURN THIS FUNCTION AT THIS POINT AFTER CHECKING THAT ONLY A SINGLE CARD WAS ADDED TO PILE
            // DECLARE THE CARD PLAYER THE WINNER OF THE ROUND

            // compare cards
            List<IPlayer> highestCardPlayers = GetHighestCardPlayer(players, pile);

            IPlayer highestPlayer;
            // check for tie
            if (highestCardPlayers.Count > 0) 
            {
                int numPlayersInGame = players.Count;
                highestPlayer = Battle(highestCardPlayers, numPlayersInGame); // settle tie
            } 

            // award bounty to winner of round
            highestPlayer = highestCardPlayers[0];
            foreach (KeyValuePair<IPlayer, ICard> playedCard in pile)
            {
                highestPlayer.AcceptCard(playedCard.Value);
            }
        }

        private static List<IPlayer> GetHighestCardPlayer(List<IPlayer> players, Dictionary<IPlayer, ICard> pile)
        {
            // compare cards
            List<IPlayer> highestCardPlayers = [.. players];
            int highest = 0;
            foreach (KeyValuePair<IPlayer, ICard> playedCard in pile)
            {
                if (cardRank[playedCard.Value.Rank] > highest)
                {
                    highestCardPlayers = [playedCard.Key];
                    highest = cardRank[playedCard.Value.Rank];
                    continue;
                }
                if (cardRank[playedCard.Value.Rank] == highest)
                {
                    highestCardPlayers.Add(playedCard.Key);
                }
            }
            return highestCardPlayers;
        }

        //If you're playing with three or four players:
        //If two or more players are tie for the highest card,
        //then each player to place by one card as face-down.
        //Then everybody plays the next card face-up as they would during a non-War round.
        //The player with the highest card will win.
        //If there is another tie between these two or more players,
        //the War needs to continue.
        private static IPlayer Battle(List<IPlayer> tiedPlayers, int numAllPlayers)
        {
            List<IPlayer> highestPlayers = [..tiedPlayers];
            List<ICard> bounty = new();

            while (highestPlayers.Count > 1) // INSTEAD OF RECURSIVE FUNCTION
            {
                if (numAllPlayers > 2)
                {
                    // if hand count of any player is 1 at this point
                    // they get to skip to cardFaceUp 
                    foreach(IPlayer player in tiedPlayers)
                    {
                        ICard? cardFaceDown = player.PlayCardByPosition(position: CardPosition.last);
                        if (cardFaceDown == null)
                            // PLAYER 
                            // IS
                            // NOT
                            // KICKED
                            // OUT
                            // THEY get to play their last card as a face up card

                        { 
                            highestPlayers.Remove(player);
                            continue; 
                        }          
                        bounty.Add(cardFaceDown);
                    }
                }
                else if (numAllPlayers == 2)
                {
                    // if handcount of any player is 2 or 3 at this point,
                    //     they get to place handcount - 1 cards in bounty
                    //     and the last in face up,
                    // else if handcount is 1,
                    //     skip to face up



                    // play cards face down
                    for (int i = 0; i < 3; i++) // each player plays 3 cards
                    {
                        foreach (IPlayer player in tiedPlayers)
                        {
                            ICard? cardFaceDown = player.PlayCardByPosition(position: CardPosition.last);
                            if (cardFaceDown == null)
                                // handle case where a player runs out of cards for the battle
                                // cardFaceDown here would be null
                                // boot player out of battle

                                // might not be necessary due to GetHighestCardPlayer
                            {
                                highestPlayers.Remove(player);
                                return highestPlayers[0]; // other player wins round
                            }
                            bounty.Add(cardFaceDown);
                        }

                    }
                }
                // cards face up
                Dictionary<IPlayer, ICard> cardFaceUpPile = new();
                foreach (IPlayer player in tiedPlayers)
                {
                    ICard? cardFaceUp = player.PlayCardByPosition(position: CardPosition.last);
                    if (cardFaceUp == null)
                    {
                        highestPlayers.Remove(player);
                        continue;
                    }
                    else
                        cardFaceUpPile.Add(player, cardFaceUp);

                    if (highestPlayers.Count == 1)
                        return highestPlayers[0];
                    else
                    {
                        List<IPlayer> pp = GetHighestCardPlayer(players: highestPlayers, pile: cardFaceUpPile);
                        highestPlayers = [.. pp];
                    }
                }
            }
            IPlayer hh = highestPlayers[0];
            // award bounty to winner
            hh.AcceptCards(cards: bounty, place: CardPosition.first); 
            return hh;
        }

        private static void EndGame(IPlayer winner)
        {
            Console.WriteLine("Game Over");
            Console.WriteLine($"{winner.Name} wins");
        }

        ////Game should be in charge of returning stack to deck
        public bool ReturnStackToDeck(IDeck deck, IStack stack)
        {
            try
            {
                List<ICard> cards = stack.RetrieveAllCards();
                deck.ReturnCards(ref cards);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }
    }
}




