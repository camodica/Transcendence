using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

public class DisplayedCard : MonoBehaviour, IPointerClickHandler {
    ConstructDeck Construct;
    ConstructDeck testConstruct;
    Deck testDeck;
    //the parent panel for cards as they will appear in constructable deck, holds current deck
    //TODO: Is this the wrong place to do this? Let's move this script somewhere else and have DisplayedCard JUST handle self disabling, asking for count/can send, and sending on click
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.LogError(this.GetComponent<Card>().CardName + " clicked.");
        Debug.LogError("1");
        testConstruct.insertCardToActive(this.GetComponent<Card>().clone(this.GetComponent<Card>()));
        Debug.LogError("2");
        testDeck.insertCardToArchive(this.GetComponent<Card>().clone(this.GetComponent<Card>()));
        Debug.LogError("3");
        GameObject.Find("ConstructPanel").GetComponent<CardElementPanelScript>().AddCard(this.GetComponent<Card>().clone(this.GetComponent<Card>()));
        //Debug.LogError("Clonename" + this.GetComponent<Card>().clone(this.GetComponent<Card>()).CardName);
        //Debug.LogError("FirstElofConstruct:" + GameObject.Find("ConstructPanel").GetComponent<ConstructDeck>().archiveDeck[0].CardName);
        //Debug.LogError("ConstructSize: " + GameObject.Find("ConstructPanel").GetComponent<ConstructDeck>().getArchiveSize());

        /*if (this.GetComponent<Card>().Count > 0)
        {
            //Debug.LogError("TriggerOnPointerClick_name:" + this.GetComponent<Card>().CardName + "count:" + this.GetComponent<Card>().Count);
            //TODO: Pass card to current deck array 
            this.GetComponent<Card>().Count--;
            Construct.insertCardToArchive(this.GetComponent<Card>());
            Debug.LogError(Construct.archiveDeck[0].CardName + " was added.**********");
            Debug.LogError("Current deck has count:" + Construct.archiveDeck.Count);
            for (int i = 0; i < Construct.archiveDeck.Count; i++)
            {
                Debug.LogError("[" + Construct.archiveDeck[i].CardName + ";pos" + i + "]"); 
            }
            //find all Card in construct deck that match element[0] (change to i) and state count TODO:Move into for loop and replace with i
            List<Card> results = Construct.archiveDeck.FindAll(
            delegate (Card c)
            {
                return c.CardName.Equals(Construct.archiveDeck[0].CardName);
            }
            );
            Debug.LogError("PostDelegate");
            if (results.Count != 0)
            {
                Debug.Log("x" + results.Count);
            }
            else
            {
                Debug.Log("no match of" + Construct.archiveDeck[0].CardName);
            }

        }
        */
    }

    // Use this for initialization
    void Start () {
        Construct = GameObject.Find("ConstructPanel").GetComponent<ConstructDeck>();
        this.GetComponent<Card>().Count = 3;
    }
	
	void Update () {
	    if (this.GetComponent<Card>().Count < 1)
        {
            this.transform.FindChild("MaxIndicator").gameObject.SetActive(true);
            this.transform.Find("CountIndicator").transform.FindChild("Text").GetComponent<Text>().text = "";
        }
        else
        {
            this.transform.Find("CountIndicator").transform.FindChild("Text").GetComponent<Text>().text = "x" + this.GetComponent<Card>().Count;
        }
	}

    
}
