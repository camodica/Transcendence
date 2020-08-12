using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class CardElementPanelScript : MonoBehaviour {
    const int Capacity = 30;
    const int CopiesPermitted = 3;

    Vector3 FirstElementLocation = new Vector3(2, -14, 0);
    float SeparationBetweenElements = 35;
    ConstructDeck Construct = new ConstructDeck();
	
    // Use this for initialization
	void Start () {
        //TEST
        GameObject element;

        for (int i = 0; i < Capacity; i++) {
            element = (GameObject)Instantiate(Resources.Load("prefabs/CardElementInDeck"));
            transform.parent = GameObject.Find("ConstructPanel").transform;
            element.transform.parent = transform;
            element.transform.localScale = new Vector3(1,0.15f,1);
            element.transform.position = new Vector3(1025, -100 - (i * SeparationBetweenElements), 0);
            
            //element.transform.localPosition = new Vector3(FirstElementLocation.x, FirstElementLocation.y - (SeparationBetweenElements * i));
        }
    }

    public void AddCard(Card card)
    {
       this.GetComponent<ConstructDeck>().insertCardToArchive(card);
        //Debug.LogError(Construct.archiveDeck.Count + " is count of archive");
       Debug.LogError(Construct.archiveDeck[0].CardName + " was added.**********");
        
    }

    public void RemoveCard()
    {

    }
	
	// Update is called once per frame
	void Update () {
	
	}

}
