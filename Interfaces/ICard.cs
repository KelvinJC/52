namespace Cards.Interfaces
{
    interface ICard
    {
        string Suit { get; }
        string Rank { get; }
        string Colour { get; }

        void Face();
    }
}
