using System;
using System.Collections;
using Cards;
using Cards.Interfaces;

class App
{
    public static void Main(string[] args)
    {
        //// create cards
        IDeck deck = new Deck();
        Console.WriteLine("==== ++++ +++++ ===========");

        //// shuffle cards
        //deck.Shuffle(shuffleCount: 10);
        //deck.Face();

        IStack stack = new GStack();

        //Game.Start(deck, stack);




        IPlayer player1 = new Player(name: "Tre");
        IPlayer player2 = new Player(name: "Bic");
        IPlayer player3 = new Player(name: "Val");
        IPlayer player4 = new Player(name: "Val");

        IPlayer[] dealPlayers = new IPlayer[] {player1, player2, player3, player4};

        //// test dealCardsToPlayers method
        //War.Start(deck, stack);
        //bool shared = War.DealCardsToPlayers(players: dealPlayers, deck: deck);

        //Console.WriteLine($"Dealed cards success: {shared}");
        //Console.WriteLine("player1 ==");
        //ICard[] cards = (ICard[])player1.Hand.ToArray(typeof(ICard));
        //deck.Face(cards);
        //Console.WriteLine("==== player1");
        //ICard[] cards2 = (ICard[])player2.Hand.ToArray(typeof(ICard));
        //deck.Face(cards2);
        //Console.WriteLine("==     ==       ==== player3");
        //ICard[] cards3 = (ICard[])player3.Hand.ToArray(typeof(ICard));
        //deck.Face(cards3);
        //Console.WriteLine($"cards3 count {cards3.Length}");
        //Console.WriteLine("    ====      ========== ====  player4 =======");
        //ICard[] cards4 = (ICard[])player4.Hand.ToArray(typeof(ICard));
        //deck.Face(cards4);


        //IPlayer playerWon = War.SelectFirstPlayer(players: dealPlayers, deck: deck, selection: SelectPlayer.HighestCard);
        //Console.WriteLine($"Player selected: {playerWon.Name}");
        //Console.WriteLine($"times : {War.timesSelectRan}");

        player1.DropHand();
        //// display last card
        //ICard lastCard = deck.Cards[^1]; // [^1] shorthand for accesing the last element of a sequence
        //Console.WriteLine("==== LAST card ++++ +++++ ===========");
        //lastCard.Face();
        //Console.WriteLine("==== ++++ +++++ ===========");
        //ICard[] er = deck.Cards;

        //// pick a card
        //ICard pickRandom = deck.Pick();
        //pickRandom.Face();

        //ICard pick0 = deck.PickFirst();
        //pick0.Face();

        //ICard pickLast = deck.PickLast();
        //pickLast.Face();

        //ICard anyDiamond = deck.PickSuit(suit: "Diamonds");
        //anyDiamond.Face();

        //ICard anyQueen = deck.PickRank(rank: "Queen");
        //anyQueen.Face();

        //Console.WriteLine($"Confirm count of cards AFTER picks");


        //// split deck in 2
        //(ICard[] first, ICard[] second, ICard[]? remainder) = deck.Split();
        //deck.Face(cards: first);
        //Console.WriteLine($"===== ===== ===== {first.Length}");
        //deck.Face(cards: second);
        //Console.WriteLine($"===== ===== ===== {second.Length}");
        ////deck.Face(cards: remainder!);
        //Console.WriteLine($"Count of cards AFTER ALL is : {deck.Cards.Length}");

        //deck.Split();
        //Console.WriteLine("First split worked");
        //deck.Split();

        // deal cards
        //Dictionary<string, ICard[]> cardsDealt = deck.Deal(numPlayers: 6, cardsPerPlayer: 5, dealFromCardZero: true);
        //foreach (KeyValuePair<string, ICard[]> hand in cardsDealt)
        //{
        //    Console.WriteLine($"---- {hand.Key} START ---- ");
        //    deck.Face(hand.Value);
        //    Console.WriteLine($"---- {hand.Key} END ---- \n");
        //}



        //Console.WriteLine($"---- After dealing cards, the deck count is : {deck.Cards.Length} ---- \n");

        //Dictionary<string, ICard[]> cardsDealt = deck.Deal(numPlayers: 6, cardsPerPlayer: 2, dealFromCardZero: false);
        //foreach (KeyValuePair<string, ICard[]> hand in cardsDealt)
        //{
        //    Console.WriteLine($"---- {hand.Key} START ---- ");
        //    deck.Face(hand.Value);
        //    Console.WriteLine($"---- {hand.Key} END ---- \n");
        //}
        //Console.WriteLine($"---- After dealing cards, the deck count is : {deck.Cards.Length} ---- \n");

        //// returning cards to deck
        //ICard[]? player1Cards;
        //cardsDealt.TryGetValue("player6", out player1Cards);
        //deck.Face(player1Cards);

        //Console.WriteLine("Returning cards");

        ////deck.ReturnCards(ref player1Cards);
        ////deck.ReturnCards(player1Cards);
        //Console.WriteLine($"---- After returning cards, the deck count is : {deck.Cards.Length} ---- \n");
        //deck.ReturnCard(ref player1Cards[0]!);
        //Console.WriteLine($"---- After returning single card, the deck count is : {deck.Cards.Length} ---- \n");
        //deck.Face(player1Cards);
    }
}
