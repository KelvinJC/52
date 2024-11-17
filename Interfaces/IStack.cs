using System.Collections;

namespace Cards.Interfaces
{
    interface IStack
    {
        ArrayList Cards { get; }

        bool Add(ICard card);

        void Show();

        ICard[] Empty();
    }
}
