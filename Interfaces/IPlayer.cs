using System.Collections;

namespace Cards.Interfaces
{
    interface IPlayer
    {
        string Name { get; }

        void AcceptCard(ICard card);
        void AcceptCards(List<ICard> cards, CardPosition place = CardPosition.first);

        void DropHand();

        ICard? PlayCard(ICard card);

        ICard? PlayCardByPosition(CardPosition position = CardPosition.last);

        ICard? PlaySameRankCard(string rank);

        ICard? PlaySameSuitCard(string suit);

        ICard? PlayRandomCard();

        ICard? PlayLowestRankCard(Dictionary<string, int> cardRanking);

        ICard? PlayHighestRankCard(Dictionary<string, int> cardRanking);

        ICard? PlayHigherCard(Dictionary<string, int> cardRanking, ICard cardToBeat);
    }
}
