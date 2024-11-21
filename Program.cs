using System;
using System.Collections;
using Cards;
using Cards.Interfaces;

class App
{
    public static void Main(string[] args)
    {
        // create deck of cards
        IDeck deck = new Deck();
        Console.WriteLine("==== ++++ +++++ ===========");

        IPlayer player1 = new Player(name: "Tre");
        IPlayer player2 = new Player(name: "Bic");
        IPlayer player3 = new Player(name: "Val");
        IPlayer player4 = new Player(name: "Vale");

        // create players
        List<IPlayer> dealPlayers = [player1, player2, player3, player4];

        // start game
        War.Start(deck: deck, cardPlayers: dealPlayers);
    }
}
