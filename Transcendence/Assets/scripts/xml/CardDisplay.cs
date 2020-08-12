using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class CardDisplay : MonoBehaviour {

    string path;
    
	// Use this for initialization
	void Start () {
        path = Application.dataPath + "/scripts/xml/cards.xml";
        DeckReader reader = new DeckReader();

        if (File.Exists(path))
        {
            List<Card> database = reader.load(path);

            foreach (Card card in database)
            {
                print(card.CardName + "_cost:" + card.Cost + " atk:" + card.Attack + " hp:" + card.Health);
            }

            Deck deck = new Deck();
            deck.activeDeck = new List<Card>();
            deck.archiveDeck = new List<Card>();
            foreach (Card c in database)
            {
                deck.archiveDeck.Add(c);
            }
            deck.resetActive();
            deck.shuffle();
            Debug.Log(deck.peek().CardName + "bypass archiving and reset active");

			deck.shuffle();
            Debug.Log("Post Filter");
            foreach (Card card in deck.activeDeck)
            {
                print(card.CardName + "_cost:" + card.Cost + " atk:" + card.Attack + " hp:" + card.Health);
            }
            Debug.Log("Printed list operated upon.");
        }
        else
        {
            Debug.Log("Error: File not Found");
        }
	}

}
