using UnityEngine;
using System.Collections.Generic;

public class player : MonoBehaviour {

    /// <summary>
    /// The class for storing all player info
    /// </summary>

    public int PlayerSide { get; set; }
    public int CurrentHP { get; set; }
    public int ManaMax { get; set; }
    public int CurrentMana { get; set; }
    public int VictoryPoints { get; set; }
    public Deck Deck { get; set; }
    public bool IsTurn { get; set; }
    public string DeckPath { get; set; }
    public string Name { get; set; }

    void Start ()
    {
        this.VictoryPoints = 0;
        this.CurrentHP = 30;
        //this.IsTurn = false;
    }

    public void loadDeck()
    {
        Deck = new Deck();

        DeckReader reader = new DeckReader();

        List<Card> stock = reader.load(DeckPath);

        foreach (Card c in stock)
        {
            Deck.archiveDeck.Add(c);
        }

        Deck.resetActive();
        Deck.shuffle();
    }
}
