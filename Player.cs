using System;
using System.Collections;
using Cards.Interfaces;


namespace Cards
{
    enum CardPlayRule : byte
    {
        Position,
        SameRank,
        SameSuit,
        Random,
        LowestRank,
        HighestRank,
        HigherThan,
    }

    enum CardPosition : byte
    {
        first,
        last
    }

    struct GameStatistics
    {
        public int GamesPlayed { get; set; }
        public int GamesWon { get; set; }
    }

    class Player(string name) : IPlayer
    {
        public string Name { get; set; } = name;
        private ArrayList Hand { get; set; } = [];

        private GameStatistics Stats = new ();

        public CardPlayRule CardPlayRule { get; set; }

        Random rnd = new ();

        public void AddPlayerStats(int gamesPlayed, int gamesWon)
        {
            Stats.GamesPlayed += gamesPlayed;
            Stats.GamesWon += gamesWon;
        }

        public void GetCard(ICard card)
        {
            Hand.Add(card);
            Console.WriteLine($"Card added to {Name}'s hand");
        }

        public void GetCards(ICard[] cards, CardPosition place = CardPosition.first)
        {
            // add multiple cards
            // place arg determines whether the cards are put `above` or `beneath` the current cards

            ICard[] currentCards = (ICard[])Hand.ToArray (typeof (ICard));
            ICard[] newCards = (place == CardPosition.first) ? [..cards, ..currentCards] : [..currentCards, ..cards];
            Hand.Clear();
            Hand.AddRange(newCards);
        }

        public ICard? PlayCard(ICard card)
        {
            try
            {
                Hand.Remove(card);
                Console.WriteLine($"Card played by {Name}:");
                // card.Face(); for card face down in War, card shouldnt be shown, check later if face is required for other games
                return card;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"No card played by {Name}.");
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public void DropHand()
        {
            Hand.Clear();

        }

        //public ICard? PlayCard(ArrayList hand, string suit, string rank)
        //{
        //    ArrayList play = new();
        //    foreach (ICard card in Hand)
        //    {
        //        if (card.Rank == rank) play.Add(card);
        //        if (card.Suit == suit) play.Add(card);
        //    }
        //    return null;
        //}

        public ICard? PlayCardByPosition(CardPosition position = CardPosition.last)
        {
            ICard card = (position == CardPosition.last) ? (ICard)Hand[^1]! : (ICard)Hand[0]!;
            return PlayCard(card);
        }

        public ICard? PlaySameRankCard(string rank)
        {
            foreach (ICard card in Hand)
            {
                if (card.Rank == rank)
                    return PlayCard(card);    
            }
            Console.WriteLine($"Player {Name} has no card of {rank} rank");
            return null;
        }


        public ICard? PlaySameSuitCard(string suit)
        {
            foreach (ICard card in Hand)
            {
                if (card.Suit == suit)
                    return PlayCard(card);
            }
            Console.WriteLine($"Player {Name} has no suit of {suit} suit");
            return null;
        }

        public ICard? PlayRandomCard()
        {
            if (Hand.Count == 0)
                throw new Exception($"Player {Name}'s hand is empty.");

            int pick = rnd.Next(Hand.Count);
            ICard card = (ICard)Hand[pick]!;
            return PlayCard(card);
        }

        public ICard? PlayLowestRankCard(Dictionary<string, int> cardRanking)
        {
            ICard lowestCard = (ICard)Hand[0]!;
            int lowest = 200;
            foreach (ICard card in Hand)
            {
                if (cardRanking[card.Rank] < lowest)
                {
                    lowestCard = card;
                    lowest = cardRanking[card.Rank];
                }
            }
            return PlayCard(lowestCard);
        }

        public ICard? PlayHighestRankCard(Dictionary<string, int> cardRanking)
        {
            ICard highestCard = (ICard)Hand[0]!;
            int highest = 0;
            foreach (ICard card in Hand)
            {
                if (cardRanking[card.Rank] > highest)
                {
                    highestCard = card;
                    highest = cardRanking[card.Rank];
                }
            }
            return PlayCard(highestCard);
        }

        public ICard? PlayHigherCard(Dictionary<string, int> cardRanking, ICard cardToBeat)
        {
            foreach (ICard card in Hand)
            {
                if (cardRanking[card.Rank] > cardRanking[cardToBeat.Rank])
                    return PlayCard(card);
            }
            return null;
        }
    }
}
