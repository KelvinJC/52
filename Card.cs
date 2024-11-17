using Cards.Interfaces;

namespace Cards
{
    class Card : ICard
    {
        public string Suit { get; }
        public string Rank { get; }
        public string Colour { get; }
        string FancySuit { get; }


        public Card(string suit, string rank, string colour)
        {
            Suit = suit;
            Rank = rank;
            Colour = colour;
            FancySuit = GetFancySuit(suit);
        }

        public void Face()
        {
            Console.WriteLine($"Card - Suit: {FancySuit}, Rank: {Rank}, Colour: {Colour}");
        }

        private string GetFancySuit(string suit)
        {
            switch (suit)
            {
                case "Clubs":
                    return "Clubs (♣)";
                case "Diamonds":
                    return "Diamonds (♦)";
                case "Hearts":
                    return "Hearts (♥)";
                case "Spades":
                    return "Spades (♠)";
                default:
                    Console.WriteLine("Invalid suit");
                    break;
            }
            return suit;
        }
    }
}
