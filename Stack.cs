using System;
using System.Collections;
using System.Security.Cryptography;
using Cards.Interfaces;

namespace Cards
{
    class GStack : IStack
    {

        private ArrayList _Cards { get; set; } = new ArrayList();

        public ArrayList Cards { get { return _Cards; } }

        public bool Add(ICard card)
        {
            if (_Cards.Count == 0)
            {
                _Cards.Add(card);
                return true;
            }

            ICard current = (ICard)_Cards[0]!;
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
            ICard card = (ICard)_Cards[0]!;
            card.Face();
        }

        public ICard[] Empty()
        {
            ICard[] cards = (ICard[])_Cards.ToArray(typeof(ICard));
            _Cards.Clear();
            return cards;
        }
    }
}
