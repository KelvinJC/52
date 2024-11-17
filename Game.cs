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

            ICard startCard = deck.PickFirst();
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

        //private static void PushStackToDeck(IStack stack, IDeck deck)
        //{
        //    ICard[] cards = deck.Cards;
        //    ICard[] cc = stack.Cards();
        //    stack.ReturnToDeck(ref cards, deck: deck);

        //}

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
                ICard[] cards = stack.Empty();
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
