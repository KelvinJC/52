using System.Collections;

namespace Cards.Interfaces
{
    interface IStack
    {
        List<ICard> Cards { get; }

        bool Add(ICard card);

        void Show();

        List<ICard> RetrieveAllCards();
    }
}
