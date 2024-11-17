using System;
using System.Collections;
using Cards.Interfaces;


namespace Cards
{
    class Deck : IDeck
    {
        public string[] Suits { get; } = ["Spades", "Hearts", "Diamonds", "Clubs"];
        public string[] Ranks { get; } = ["Ace", "King", "Queen", "Jack", "10", "9", "7", "8", "6", "5", "4", "3", "2"];

        private const string RED = "Red";
        private const string BLACK = "Black";
        private ArrayList _Cards;

        public ICard[] Cards
        {
            get => (ICard[])_Cards.ToArray(typeof(ICard));
        }

        Random rnd = new();

        public Deck()
        {
            _Cards = Generate();
        }

        private ArrayList Generate()
        {
            ArrayList deck = new();
            string colour;

            foreach (string suit in Suits)
            {
                if (suit == "Hearts" || suit == "Diamonds")
                    colour = RED;
                else
                    colour = BLACK;

                foreach (string rank in Ranks)
                {
                    ICard card = new Card(suit: suit, rank: rank, colour: colour);
                    deck.Add(card);
                }
            }
            return deck;
        }

        public void Face()
        {
            if (_Cards.Count == 0)
                throw new Exception("Deck is empty.");

            foreach (ICard card in _Cards)
            {
                card.Face();
            }
        }

        public void Face(ICard[] cards)
        {
            if (cards == null || cards[0] == null)
                throw new ArgumentException("Deck is empty");
            try
            {
                foreach (ICard card in cards)
                {
                    card.Face();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void Shuffle(int shuffleCount = 10)
        {
            if (_Cards.Count == 0)
                throw new Exception("Deck is empty.");

            ICard[] deckToShuffle = (ICard[])_Cards.ToArray(typeof(ICard));
            for (int i = 0; i <= shuffleCount; i++)
            {
                rnd.Shuffle(deckToShuffle);
            }
            _Cards.Clear();
            _Cards.AddRange(deckToShuffle);
        }

        public ICard[] Shuffle(ICard[] cards, int shuffleCount = 10)
        {
            ICard[] deckToShuffle = new ICard[cards.Length];
            Array.Copy(cards, deckToShuffle, cards.Length);
            for (int i = 0; i <= shuffleCount; i++)
            {
                rnd.Shuffle(deckToShuffle);
            }
            return deckToShuffle;
        }

        public ICard Deal()
        {
            if (_Cards.Count == 0)
                throw new Exception("Deck is empty.");

            int pick = rnd.Next(_Cards.Count);
            ICard card = (ICard)_Cards[pick]!;
            _Cards.Remove(card);
            return card;
        }

        public ICard DealFirst()
        {
            if (_Cards.Count == 0)
                throw new Exception("Deck is empty.");

            ICard card = (ICard)_Cards[0]!;
            _Cards.Remove(card);
            return card;
        }

        public ICard DealLast()
        {
            if (_Cards.Count == 0)
                throw new Exception("Deck is empty.");

            ICard card = (ICard)_Cards[^1]!;
            _Cards.Remove(card);
            return card;
        }
        public ICard DealSuit(string suit)
        {
            if (_Cards.Count == 0)
                throw new Exception("Deck is empty.");

            if (!Suits.Contains(suit))
                throw new ArgumentException("Suit must be one of 'Clubs', 'Diamonds', 'Hearts' or 'Spades'");

            ArrayList suitCards = new();
            foreach (ICard card in _Cards)
            {
                if (card.Suit == suit)
                {
                    suitCards.Add(card);
                }
            }
            int pick = rnd.Next(suitCards.Count);
            ICard pickedCard = (ICard)suitCards[pick]!;
            _Cards.Remove(pickedCard);
            return pickedCard;
        }

        public ICard DealRank(string rank)
        {
            if (_Cards.Count == 0)
                throw new Exception("Deck is empty.");
            if (!Ranks.Contains(rank))
                throw new ArgumentException("Rank must be one of " +
                    "'Ace', '2', '3', '4', '5', '6', '7', '8', '9', '10', 'Jack', 'Queen' or 'King'");

            ArrayList rankCards = new();
            foreach (ICard card in _Cards)
            {
                if (card.Rank == rank)
                {
                    rankCards.Add(card);
                }
            }
            int pick = rnd.Next(rankCards.Count);
            ICard pickedCard = (ICard)rankCards[pick]!;
            _Cards.Remove(pickedCard);
            return pickedCard;
        }

        public (ICard[], ICard[], ICard[]?) Split()
        {
            // split `_Cards` deck in two
            // return three arrays with the halves in the first two and any remnant card in the third
            // if the deck is evenly numbered, the third array is empty 
            // at the end of this operation, the `_Cards` deck is left empty

            ArrayList firstHalf = new();
            ArrayList secondHalf = new();
            ICard[] remainder;

            if (_Cards.Count == 0)
                throw new Exception("Deck is empty.");

            if ((_Cards.Count % 2) == 0)
                remainder = [];
            else
            {
                ICard remCard = (ICard)_Cards[^1]!;
                _Cards.Remove(remCard);
                remainder = [remCard];
            }

            int mid = _Cards.Count / 2;
            Console.WriteLine($"mid {mid}");
            for (int i = 0; i < mid; i++)
            {
                firstHalf.Add(_Cards[i]);
                secondHalf.Add(_Cards[mid + i]);
            }
            _Cards.Clear();

            ICard[] firstDeck = (ICard[])firstHalf.ToArray(typeof(ICard));
            ICard[] secondDeck = (ICard[])secondHalf.ToArray(typeof(ICard));
            return (firstDeck, secondDeck, remainder);
        }

        public Dictionary<string, ICard[]> DealFillFirst(int numPlayers, int cardsPerPlayer, bool dealFromCardZero = true)
        {
            // deal cards by filling player's hand before moving to the next

            if (_Cards.Count == 0)
                throw new Exception("Deck is empty.");
            if (_Cards.Count < (cardsPerPlayer * numPlayers))
                throw new ArgumentException("Invalid argument. Cards in deck not sufficient to go round.");

            Dictionary<string, ICard[]> dealer = new();

            for (int handsDealt = 0; handsDealt < numPlayers; handsDealt++)
            {
                ArrayList hand = new();
                for (int cardsDealt = 0; cardsDealt < cardsPerPlayer; cardsDealt++)
                {
                    ICard card = dealFromCardZero ? DealFirst() : DealLast();
                    hand.Add(card);
                }
                ICard[] playerHand = (ICard[])hand.ToArray(typeof(ICard));
                string playerHandId = "player" + (handsDealt + 1).ToString();
                dealer.Add(playerHandId, playerHand);
            }
            return dealer;
        }

        public Dictionary<string, ICard[]> DealFillFirstRandom(int numPlayers, int cardsPerPlayer)
        {
            // deal cards in random manner, fill a player's hand before moving to the next
            if (_Cards.Count == 0)
                throw new Exception("Deck is empty.");
            if (numPlayers <= 0 || cardsPerPlayer <= 0)
                throw new ArgumentException("Invalid argument"); // basic exception handling
            if (_Cards.Count < (cardsPerPlayer * numPlayers))
                throw new ArgumentException("Invalid argument. Cards in deck not sufficient to go round.");

            Dictionary<string, ICard[]> dealer = new Dictionary<string, ICard[]>();

            for (int handsDealt = 0; handsDealt < numPlayers; handsDealt++)
            {
                ArrayList hand = new();
                for (int cardsDealt = 0; cardsDealt < cardsPerPlayer; cardsDealt++)
                {
                    ICard card = Deal();
                    hand.Add(card);
                }

                ICard[] playersHand = (ICard[])hand.ToArray(typeof(ICard));
                string playersHandId = "player" + (handsDealt + 1).ToString(); // player0, player1, player2 etc...
                dealer.Add(playersHandId, playersHand);
            }
            return dealer;
        }

        public void ReturnCard(ref ICard? card)
        {
            if (card == null)
                throw new ArgumentNullException();
            if (_Cards.Contains(card))
                throw new ArgumentException("Card already in deck");
            if (!Suits.Contains(card.Suit) || !Ranks.Contains(card.Rank))
                throw new ArgumentException("Invalid card");

            _Cards.Add(card);
            card = null;
        }

        public void ReturnCards(ref ICard[] cards)
        {
            foreach (ICard card in cards)
            {
                if (card == null)
                    throw new ArgumentNullException();
                if (_Cards.Contains(card))
                    throw new ArgumentException("Card already in deck");
                if (!Suits.Contains(card.Suit) || !Ranks.Contains(card.Rank))
                    throw new ArgumentException("Invalid card");
            }
            _Cards.AddRange(cards);
            cards = [];
        }
    }
}

