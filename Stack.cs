using System;
using System.Collections;
using System.Security.Cryptography;
using Cards.Interfaces;

namespace Cards
{
    class GStack : IStack
    {

        private List<ICard> _Cards { get; set; } = new ();

        public List<ICard> Cards { get { return _Cards; } }

        public bool Add(ICard card)
        {
            if (_Cards.Count == 0)
            {
                _Cards.Add(card);
                return true;
            }

            ICard current = _Cards[0];
            if (current.Rank == card.Rank || current.Suit == card.Suit)
            {
                _Cards.Add(card);
                _Cards.Reverse();
                return true;
            }

            return false;
        }
        public void Show()
        {
            ICard card = _Cards[0];
            card.Face();
        }

        public List<ICard> RetrieveAllCards()
        {
            List<ICard> cards = _Cards;
            _Cards.Clear();
            return cards;
        }
    }
}
