using System;
using System.Collections;
using System.Collections.Generic;
using Cards.Interfaces;


namespace Cards
{
    class Deck : IDeck
    {
        public string[] Suits { get; } = ["Spades", "Hearts", "Diamonds", "Clubs"];
        public string[] Ranks { get; } = ["Ace", "King", "Queen", "Jack", "10", "9", "7", "8", "6", "5", "4", "3", "2"];

        private const string RED = "Red";
        private const string BLACK = "Black";
        private List<ICard> _Cards;

        public List<ICard> Cards
        {
            get => _Cards;
        }

        Random rnd = new();

        public Deck()
        {
            _Cards = Generate();
        }

        private List<ICard> Generate()
        {
            List<ICard> deck = new();
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

        public void Face(List<ICard> cards)
        {
            if (cards.Count == 0)
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

            ICard[] deckToShuffle = [.._Cards];
            for (int i = 0; i <= shuffleCount; i++)
            {
                rnd.Shuffle(deckToShuffle);
            }
            _Cards.Clear();
            _Cards.AddRange(deckToShuffle);
        }

        public List<ICard> Shuffle(List<ICard> cards, int shuffleCount = 10)
        {
            ICard[] shuffleDuplicate = [..cards];
            for (int i = 0; i <= shuffleCount; i++)
            {
                rnd.Shuffle(shuffleDuplicate);
            }

            List<ICard> shuffledCards = [.. shuffleDuplicate];
            return shuffledCards;
        }

        public ICard[] Shuffle(ICard[] cards, int shuffleCount = 10)
        {
            for (int i = 0; i <= shuffleCount; i++)
            {
                rnd.Shuffle(cards);
            }
            return cards;
        }

        public ICard Deal()
        {
            if (_Cards.Count == 0)
                throw new Exception("Deck is empty.");

            int pick = rnd.Next(_Cards.Count);
            ICard card = _Cards[pick];
            _Cards.Remove(card);
            return card;
        }

        public ICard DealFirst()
        {
            if (_Cards.Count == 0)
                throw new Exception("Deck is empty.");

            ICard card = _Cards[0];
            _Cards.Remove(card);
            return card;
        }

        public ICard DealLast()
        {
            if (_Cards.Count == 0)
                throw new Exception("Deck is empty.");

            ICard card = _Cards[^1];
            _Cards.Remove(card);
            return card;
        }
        public ICard DealSuit(string suit)
        {
            if (_Cards.Count == 0)
                throw new Exception("Deck is empty.");

            if (!Suits.Contains(suit))
                throw new ArgumentException("Suit must be one of 'Clubs', 'Diamonds', 'Hearts' or 'Spades'");

            List<ICard> suitCards = new();
            foreach (ICard card in _Cards)
            {
                if (card.Suit == suit)
                {
                    suitCards.Add(card);
                }
            }
            int pick = rnd.Next(suitCards.Count);
            ICard pickedCard = suitCards[pick];
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

            List<ICard> rankCards = new();
            foreach (ICard card in _Cards)
            {
                if (card.Rank == rank)
                {
                    rankCards.Add(card);
                }
            }
            int pick = rnd.Next(rankCards.Count);
            ICard pickedCard = rankCards[pick];
            _Cards.Remove(pickedCard);
            return pickedCard;
        }

        public (List<ICard>, List<ICard>, List<ICard>?) Split()
        {
            // split `_Cards` deck in two
            // return three lists with the halves in the first two and any remnant card in the third
            // if the deck is evenly numbered, the third list is empty 
            // at the end of this operation, the `_Cards` deck is left empty

            List<ICard> firstHalf = [];
            List<ICard> secondHalf = [];
            List<ICard> remainder = [];

            if (_Cards.Count == 0)
                throw new Exception("Deck is empty.");

            if ((_Cards.Count % 2) != 0)
            {
                ICard remCard = _Cards[^1];
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

            List<ICard> firstDeck = firstHalf;
            List<ICard> secondDeck = secondHalf;
            return (firstDeck, secondDeck, remainder);
        }

        public Dictionary<string, List<ICard>> DealFillFirst(int numPlayers, int cardsPerPlayer, bool dealFromCardZero = true)
        {
            // deal cards by filling player's hand before moving to the next

            if (_Cards.Count == 0)
                throw new Exception("Deck is empty.");
            if (_Cards.Count < (cardsPerPlayer * numPlayers))
                throw new ArgumentException("Invalid argument. Cards in deck not sufficient to go round.");

            Dictionary<string, List<ICard>> dealer = new();

            for (int handsDealt = 0; handsDealt < numPlayers; handsDealt++)
            {
                List<ICard> hand = new();
                for (int cardsDealt = 0; cardsDealt < cardsPerPlayer; cardsDealt++)
                {
                    ICard card = dealFromCardZero ? DealFirst() : DealLast();
                    hand.Add(card);
                }
                string playerHandId = "player" + (handsDealt + 1).ToString();
                dealer.Add(playerHandId, hand);
            }
            return dealer;
        }

        public Dictionary<string, List<ICard>> DealFillFirstRandom(int numPlayers, int cardsPerPlayer)
        {
            // deal cards in random manner, fill a player's hand before moving to the next
            if (_Cards.Count == 0)
                throw new Exception("Deck is empty.");
            if (numPlayers <= 0 || cardsPerPlayer <= 0)
                throw new ArgumentException("Invalid argument"); // basic exception handling
            if (_Cards.Count < (cardsPerPlayer * numPlayers))
                throw new ArgumentException("Invalid argument. Cards in deck not sufficient to go round.");

            Dictionary<string, List<ICard>> dealer = new();

            for (int handsDealt = 0; handsDealt < numPlayers; handsDealt++)
            {
                List<ICard> hand = [];
                for (int cardsDealt = 0; cardsDealt < cardsPerPlayer; cardsDealt++)
                {
                    ICard card = Deal();
                    hand.Add(card);
                }

                string playersHandId = "player" + (handsDealt + 1).ToString(); // player0, player1, player2 etc...
                dealer.Add(playersHandId, hand);
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

        public void ReturnCards(ref List<ICard> cards)
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

