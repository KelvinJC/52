using System.Collections;

namespace Cards.Interfaces
{
    interface IDeck
    {
        string[]? Suits { get; }
        string[]? Ranks { get; }

        ICard[] Cards { get; }

        void Face();
        void Face(ICard[] cards);

        void Shuffle(int shuffleCount);
        ICard[] Shuffle(ICard[] cards, int shuffleCount);

        ICard Deal();

        ICard DealFirst();

        ICard DealLast();

        ICard DealSuit(string suit);

        ICard DealRank(string rank);

        (ICard[], ICard[], ICard[]?) Split();

        void ReturnCard(ref ICard? card);

        void ReturnCards(ref ICard[] cards);
    }
}





