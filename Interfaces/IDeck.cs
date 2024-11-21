using System.Collections;

namespace Cards.Interfaces
{
    interface IDeck
    {
        string[]? Suits { get; }
        string[]? Ranks { get; }

        List<ICard> Cards { get; }

        void Face();
        void Face(List<ICard> cards);

        void Shuffle(int shuffleCount);
        ICard[] Shuffle(ICard[] cards, int shuffleCount);
        List<ICard> Shuffle(List<ICard> cards, int shuffleCount);

        ICard Deal();

        ICard DealFirst();

        ICard DealLast();

        ICard DealSuit(string suit);

        ICard DealRank(string rank);

        (List<ICard>, List<ICard>, List<ICard>?) Split();

        void ReturnCard(ref ICard? card);

        void ReturnCards(ref List<ICard> cards);
    }
}





