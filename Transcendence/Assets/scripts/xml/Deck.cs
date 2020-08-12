using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

[System.Serializable]
public class Deck : MonoBehaviour
{

    public string deckName;
    public string path;
    public List<Card> archiveDeck;
    public List<Card> activeDeck;
    public List<Card> discard;

    #pragma warning disable
    private static System.Random rng = new System.Random();

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public Deck(String path, String name)
    {
        activeDeck = new List<Card>();
        archiveDeck = new List<Card>();
        discard = new List<Card>();
        this.deckName = name;
        this.path = path;
    }

    public Deck(String name)
    {
        activeDeck = new List<Card>();
        archiveDeck = new List<Card>();
        discard = new List<Card>();
        this.deckName = name;
    }

    public Deck()
    {
        activeDeck = new List<Card>();
        archiveDeck = new List<Card>();
        discard = new List<Card>();
    }

    public int getArchiveSize() //returns size of deck before match started
    {
        return archiveDeck.Count;
    }

    public int getDeckSize() //returns size of deck as it is, active, in the game
    {
        return activeDeck.Count;
    }

    public int getDiscardSize() //returns size of discard list
    {
        return discard.Count;
    }

    public bool hasNext() //returns if there is readable card in list[0]
    {
        return activeDeck.Count > 0; 
    }

    public Card poll() //returns card of index 0 in deck, removes card from list
    {
        Card card = activeDeck[0];
        activeDeck.RemoveAt(0);
        return card;
    }

    public Card poll(int i) //returns card of index i in deck, removes card from list
    {
        Card card = activeDeck[i];
        activeDeck.RemoveAt(i);
        return card;
    }

    public Card peek() //returns card of index 0 in deck, does not remove card
    {
        return activeDeck[0];
    }

    public Card peek(int i) //returns card of index i in deck, does not remove card
    {
        return activeDeck[i];
    }

    public void insertCardToArchive(Card card, int index) //adds card to archive deck at specified index
    {
        archiveDeck.Insert(index, card);
    }

    public void insertCardToActive(Card card, int index) //adds card to active deck at specified index, deck top is 0
    {
        activeDeck.Insert(index, card);
    }

    public void insertCardToDiscard(Card card, int index) //adds card to discard at specified index, discard top is 0
    {
        discard.Insert(index, card);
    }

    public void insertCardToArchive(Card card)
    {
        archiveDeck.Add(card);
    }

    public void insertCardToActive(Card card)
    {
        activeDeck.Add(card);
    }

    public void insertCardToDiscard(Card card)
    {
        discard.Add(card);
    }


    public void resetActive() //clears active Deck and sets it equal to archive
    {
        activeDeck.Clear();
        foreach (Card c in archiveDeck)
        {
            activeDeck.Add(c);
        }
    }

    public void vacate() //clears Deck active and archive completely
    {
        activeDeck.Clear();
        archiveDeck.Clear();
        discard.Clear();
        deckName = null;
        path = null;

    }

    public void shuffle()
    { 
			for (int i = activeDeck.Count - 1; i > 0; i--) {
				int r = UnityEngine.Random.Range(0,i);
				Card tmp = activeDeck[i];
				activeDeck[i] = activeDeck[r];
				activeDeck[r] = tmp;
		}
    }

    public List<Card> filterName(string name)
    {
        List<Card> filtered = new List<Card>();
        foreach (Card c in activeDeck)
        {
            if (c.CardName.Equals(name, StringComparison.InvariantCultureIgnoreCase))
            {
                filtered.Add(c);
            }
        }
        return filtered;
    }

    public List<Card> filterID(string ID)
    {
        List<Card> filtered = new List<Card>();
        foreach (Card c in activeDeck)
        {
            if (c.ID.Equals(ID, StringComparison.InvariantCultureIgnoreCase))
            {
                filtered.Add(c);
            }
        }
        return filtered;
    }

    public List<Card> filterAlliance(string alliance)
    {
        List<Card> filtered = new List<Card>();
        foreach (Card c in activeDeck)
        {
            if (c.Alliance.Equals(alliance, StringComparison.InvariantCultureIgnoreCase))
            {
                filtered.Add(c);
            }
        }
        return filtered;
    }

    public List<Card> filterType(string type)
    {
        List<Card> filtered = new List<Card>();
        foreach (Card c in activeDeck)
        {
            if (c.Type.Equals(type, StringComparison.InvariantCultureIgnoreCase))
            {
                filtered.Add(c);
            }
        }
        return filtered;
    }

    public List<Card> filterCost(string cost)
    {
        List<Card> filtered = new List<Card>();
        foreach (Card c in activeDeck)
        {
            if (c.Cost.Equals(cost, StringComparison.InvariantCultureIgnoreCase))
            {
                filtered.Add(c);
            }
        }
        return filtered;
    }

    public List<Card> filterAttack(string attack)
    {
        List<Card> filtered = new List<Card>();
        foreach (Card c in activeDeck)
        {
            if (c.Attack.Equals(attack, StringComparison.InvariantCultureIgnoreCase))
            {
                filtered.Add(c);
            }
        }
        return filtered;
    }

    public List<Card> filterHealth(string health)
    {
        List<Card> filtered = new List<Card>();
        foreach (Card c in activeDeck)
        {
            if (c.Health.Equals(health, StringComparison.InvariantCultureIgnoreCase))
            {
                filtered.Add(c);
            }
        }
        return filtered;
    }

    public List<Card> filterDefense(string defense)
    {
        List<Card> filtered = new List<Card>();
        foreach (Card c in activeDeck)
        {
            if (c.Defense.Equals(defense, StringComparison.InvariantCultureIgnoreCase))
            {
                filtered.Add(c);
            }
        }
        return filtered;
    }

    public List<Card> filterRange(string range)
    {
        List<Card> filtered = new List<Card>();
        foreach (Card c in activeDeck)
        {
            if (c.Range.Equals(range, StringComparison.InvariantCultureIgnoreCase))
            {
                filtered.Add(c);
            }
        }
        return filtered;
    }

    /*
    public List<Card> filterTarget(string target)
    {
        List<Card> filtered = new List<Card>();
        foreach (Card c in activeDeck)
        {
            if (c.Target.Equals(target, StringComparison.InvariantCultureIgnoreCase))
            {
                filtered.Add(c);
            }
        }
        return filtered;
    }
    */

}





    //SORTING,SHUFFLING,SERIALIZING

