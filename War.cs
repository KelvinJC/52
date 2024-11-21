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
        private static IPlayer defaultPlayer = new Player("No Name");

        public static void Start(IDeck deck, List<IPlayer> cardPlayers)
        {
            // begin game
            deck.Shuffle(shuffleCount: 10);
            Console.WriteLine("Game Started!");

            // TODO: insert while loop here to play multiple rounds up to `i`
            //      Console.WriteLine("Round {i} Started!");

            List<IPlayer> orderedPlayers = OrderPlayers(players: cardPlayers, deck: deck, selection: SelectPlayer.HighestCard);
            Console.WriteLine($"{orderedPlayers[0].Name} goes first.");

            bool cardsDealt = DealCardsToPlayers(players: orderedPlayers, deck: deck);
            IPlayer winner = new Player("No winner");

            if (cardsDealt)
            {
                winner = GameRound(players: orderedPlayers);
                if (winner.Name == "No Name")
                    Console.WriteLine("Game Round Ends Without Winner");
                else
                {
                    EndGame(winner);
                    // Edit players game stats
                    // Edit winner's game stats
                    foreach (IPlayer player in cardPlayers)
                    {
                        player.DropHand();
                    }
                }
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

        private static IPlayer GameRound(List<IPlayer> players)
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
            
            if (pile.Keys.Count == 1)
            {
                Console.WriteLine("One player played");
                pile.Keys.First().AcceptCard(pile.Values.First());
                return pile.Keys.First();
            }

            // compare cards
            List<IPlayer> highestCardPlayers = GetHighestCardPlayer(pile);
            IPlayer highestPlayer;
 
            // check for tie
            if (highestCardPlayers.Count > 1) 
            {
                highestPlayer = Battle(highestCardPlayers); // settle tie
            } 

            // award bounty to winner of round
            highestPlayer = highestCardPlayers[0];
            highestPlayer.AcceptCards([.. pile.Values]);
            return highestPlayer;
        }

        // Returns a list containing the player(s) who played the highest ranked card(s)
        private static List<IPlayer> GetHighestCardPlayer(Dictionary<IPlayer, ICard> pile)
        {           
            List<IPlayer> highestCardPlayers = [..pile.Keys];

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

        // With 2 players, each player plays 3 cards face-down
        //   then a card face-up to determine the winner of the War.
        // With 3 or 4 players,
        //   If two or more players tie for the highest card,
        //   then each player is to place a card face-down.
        //   Then everybody plays the next card face-up as they would during a non-War round.
        // The player with the highest card wins and claims all placed cards.
        // If there is another tie between these two or more players, the War continues.
        private static IPlayer Battle(List<IPlayer> tiedPlayers)
        {
            int numPlayers = tiedPlayers.Count;
            List<IPlayer> highestPlayers = [..tiedPlayers];
            List<ICard> bounty = new();
            Dictionary<IPlayer, ICard> cardFaceUpPile = new();
            IPlayer battleChampion = defaultPlayer;

            // if hand count of any player is 1 at this point they get to skip to cardFaceUp 
            List<IPlayer> playersWithZeroCards = tiedPlayers.Where(player => player.GetHandCount().Equals(0)).ToList();
            List<IPlayer> playersWithOnlyOneCard = tiedPlayers.Where(player => player.GetHandCount().Equals(1)).ToList();
            List<IPlayer> playersWith2CardsOrMore = tiedPlayers.Where(player => player.GetHandCount() > 1).ToList();
            List<IPlayer> playersWith4CardsOrMore = tiedPlayers.Where(player => player.GetHandCount() >= 4).ToList();
                
            // cards face down
            if (numPlayers > 2)
            {
                foreach(IPlayer player in playersWith2CardsOrMore)
                {
                    ICard? cardFaceDown = player.PlayCardByPosition(position: CardPosition.last);         
                    bounty.Add(cardFaceDown!);
                }
            }
            else if (numPlayers == 2)
            {
                if (playersWith4CardsOrMore.Count == numPlayers)
                {
                    for (int i = 0; i < 3; i++) // each player plays 3 cards
                    {
                        foreach (IPlayer player in tiedPlayers)
                        {
                            ICard? cardFaceDown = player.PlayCardByPosition(position: CardPosition.last);
                            bounty.Add(cardFaceDown!);
                        }
                    }
                }
                // both players have either 2 or 3 cards at this point,
                // they get to place handcount - 1 cards in bounty and the last in face up,
                else if (playersWithOnlyOneCard.Count == 0)  
                {
                    double cardPlays = Math.Min(
                        tiedPlayers[0].GetHandCount(), 
                        tiedPlayers[1].GetHandCount()
                    );

                    for (int i = 0; i < (cardPlays - 1); i++) 
                    {
                        foreach (IPlayer player in tiedPlayers)
                        {
                            ICard? cardFaceDown = player.PlayCardByPosition(position: CardPosition.last);
                            bounty.Add(cardFaceDown!);
                        }
                    }
                }
            }

            // cards face up
            List<IPlayer> cardFaceUpPlayers = playersWith2CardsOrMore.Union(playersWithOnlyOneCard).ToList();

            // return No Name player as winner.
            // To handle edge case where consecutive rounds of battles end in draws
            if (cardFaceUpPlayers.Count == 0)
            { 
                return battleChampion; 
            }

            foreach (IPlayer player in cardFaceUpPlayers)
            {
                ICard? cardFaceUp = player.PlayCardByPosition(position: CardPosition.last);
                cardFaceUpPile.Add(player, cardFaceUp!);
            }

            highestPlayers = GetHighestCardPlayer(pile: cardFaceUpPile);
            if (highestPlayers.Count > 1)
            {
                battleChampion = Battle(highestPlayers);
            }            

            // award bounty and card pile to winner
            battleChampion.AcceptCards(cards: bounty, place: CardPosition.first); 
            battleChampion.AcceptCards(cards: [..cardFaceUpPile.Values], place: CardPosition.first); 
            return battleChampion;
        }

        private static void EndGame(IPlayer winner)
        {
            Console.WriteLine("Game Over");
            Console.WriteLine($"{winner.Name} wins");
        }

        // Game should be in charge of returning stack to deck
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




