using System;
using System.Collections;
using Cards.Interfaces;


namespace Cards
{
    class Game()
    {
        public static void Start(IDeck deck, IStack stack)
        {
            // begin game
            deck.Shuffle(shuffleCount: 10);

            ICard startCard = deck.DealFirst();
            bool startCardAdded = stack.Add(startCard);

            if (!startCardAdded)
            {
                Console.WriteLine("Game failed to start. Starting card not added");
                return;
            }
           
            Console.WriteLine("Game Started!");
            Console.Write($"Current card: ");
            startCard.Face();
            
        }

        private static void AddPlayers(IPlayer[] players)
        {
            int r = 0;
        }

        private static bool DealCardsToPlayers(IPlayer[] players, IDeck deck)
        {
            return true;

        }

        private static void PlayGame(IPlayer[] players, IStack stack)
        {
            int r = 0;

        }

        private static void EndGame(IPlayer winner)
        {
            Console.WriteLine("Game Over");
            Console.WriteLine($"{winner.Name} wins");
        }

        ////Game should be in charge of returning stack to deck
        public bool ReturnStackToDeck(IDeck deck, IStack stack)
        {
            try
            {
                List<ICard> cards = stack.RetrieveAllCards();
                deck.ReturnCards(ref cards);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }
    }
}
