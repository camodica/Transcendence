using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;

public class MatchStartScript : MonoBehaviour {
	public Canvas exitMenu;
	public Canvas helperPrompt;
	public Dropdown p1Selector;
	public Dropdown p2Selector;
	public Button p1Lock;
	public Button p2Lock;
    public Text p1input;
    public Text p2input;
    public GameObject Player1Data;
    public GameObject Player2Data;
    public string tempName;
	public bool p1IsLocked;
	public bool p2IsLocked;
	public Button startMatch;
	public Button menuButton;
	public AudioClip clickSound;
	public AudioClip heavyClickSound;
	public AudioSource menuAudio;
	public List<Deck> deckPool = new List<Deck>();
	public List<string> deckNames;
    public List<string> paths;
    int i = 0;

	/// <remarks>
    /// This class should create some dummy decks for the user to select and load them into the player data object,
    /// and manage all of the button functions for the menu. 
    /// </remarks>

	void Start () {

        Player1Data.GetComponent<player>().Name = "Player 1";
        Player2Data.GetComponent<player>().Name = "Player 2";

        exitMenu.enabled = false;
		helperPrompt.enabled = false;

        DontDestroyOnLoad(Player1Data);
        DontDestroyOnLoad(Player2Data);

        

        /*
        List<TextAsset> decks = new List<TextAsset>();

        TextAsset[] stuff = (TextAsset[])Resources.LoadAll("Decks");

        foreach (TextAsset xmlDeck in stuff)
        {

            DeckReader reader = new DeckReader();

            List<Card> database = reader.load(xmlDeck);

            Deck deck = new Deck(reader.GetDeckName(xml));

            foreach (Card c in database)
            {
                deck.archiveDeck.Add(c);
            }

            deckPool.Add(deck);
        }
        */

        //paths.Add("/scripts/xml/cards.xml");
        //paths.Add("/scripts/xml/2cards.xml");
        //paths.Add("/scripts/xml/3cards.xml");
        //paths.Add("/scripts/xml/neutralsAndSpells.xml");
        // The newest spells that you can buy!
        
        paths.Add("/scripts/xml/spellsWithModifiers.xml");
        paths.Add("/scripts/xml/spellsWithModifiers.xml");
        paths.Add("/scripts/xml/spellsWithModifiers.xml");
        paths.Add("/scripts/xml/spellsWithModifiers.xml");
        paths.Add("/scripts/xml/spellsWithModifiers.xml");

        foreach (string p in paths)
        {

            //TEMP: method to create deck pool from all usable decks, for now, creates same deck off archive
            string path = Application.dataPath + p;

            DeckReader reader = new DeckReader();

            List<Card> database = reader.load(path);

            Deck deck = new Deck(path, reader.GetDeckName(path));

            foreach (Card c in database)
            {
                deck.archiveDeck.Add(c);
            }

            deckPool.Add(deck);
            i++;
        }
        

        foreach (Deck d in deckPool)
        {
            Dropdown.OptionData data = new Dropdown.OptionData(d.deckName);
            p1Selector.options.Add(data);
            p2Selector.options.Add(data);
            //Debug.Log("Option added of deckName " + d.deckName);
        }

        Player1Data.GetComponent<player>().DeckPath = deckPool[0].path;
        Player2Data.GetComponent<player>().DeckPath = deckPool[0].path;
    }
	
	// Update is called once per frame
	void Update () {	
	}

	//exit match menu popup, disables buttons
	public void MenuPress(){
		menuAudio.PlayOneShot(clickSound, 0.7F);
		exitMenu.enabled = true;
		IsComponentsInteractable (false);
	}

	//return to title menu and exit match menu
	public void ExitPress(){
		menuAudio.PlayOneShot(clickSound, 0.7F);
        SceneManager.LoadScene(0);
	}

	//disables the exit match menu prompt, reenables buttons
	public void CancelPress(){
		menuAudio.PlayOneShot(clickSound, 0.7F);
		exitMenu.enabled = false;
		IsComponentsInteractable (true);
	}

	//launches game with selected decks
	public void StartMatch() {
		menuAudio.PlayOneShot(heavyClickSound, 0.7F);
		if (p1IsLocked && p2IsLocked) { //are both players locked?
            SceneManager.LoadScene(2); //Level 1 should be the match scene
		}
		else {
			helperPrompt.enabled = true;
			IsComponentsInteractable (false);
		}
	}

	//dissmiss helper prompt with OK
	public void DismissPrompt() {
		helperPrompt.enabled = false;
		IsComponentsInteractable (true);
	}

	//player1 selector
	public void DeckSelectP1(){
        
		menuAudio.PlayOneShot(clickSound, 0.7F);
        tempName = p1Selector.GetComponent<Dropdown>().captionText.text;

        foreach (Deck d in deckPool)
        {
            if (d.deckName.Equals(tempName))
            {
                Player1Data.GetComponent<player>().DeckPath = d.path;
            }
        }
	}

	//player2 selector
	public void DeckSelectP2(){
		menuAudio.PlayOneShot(clickSound, 0.7F);
        tempName = p2Selector.GetComponent<Dropdown>().captionText.text;

        foreach (Deck d in deckPool)
        {
            if (d.deckName.Equals(tempName))
            {
                Player2Data.GetComponent<player>().DeckPath = d.path;
            }
        }
    }
	

	//when player1 lock is pressed
	public void LockPressP1(){
		menuAudio.PlayOneShot(heavyClickSound, 0.7F);
		if (p1Selector.interactable) {
			p1Selector.interactable = false;
			p1IsLocked = true;
		} else {
			p1Selector.interactable = true;
			p1IsLocked = false;
		}
	}

	//when player2 lock is pressed
	public void LockPressP2(){
		menuAudio.PlayOneShot(heavyClickSound, 0.7F);
		if (p2Selector.interactable) {
			p2Selector.interactable = false;
			p2IsLocked = true;
		} else {
			p2Selector.interactable = true;
			p2IsLocked = false;
		}
	}

    public void Player1NameSelect()
    {
        if (!this.p1input.Equals(""))
        {
            Player1Data.GetComponent<player>().Name = this.p1input.text;
        }
        Debug.Log(Player1Data.GetComponent<player>().Name + " is p1 name");
    }

    public void Player2NameSelect()
    {
        if (!this.p2input.Equals(""))
        {
            Player2Data.GetComponent<player>().Name = this.p2input.text;
        }
           
        Debug.Log(Player2Data.GetComponent<player>().Name + " is p2 name.");
    }

    //sets components of menu to all disabled or all interactable
    public void IsComponentsInteractable(bool state){
		//if statements prevent locking, opening exit menu, canceling, and overriding lock state
		if (!p1IsLocked) {
			p1Selector.interactable = state;
		}
		if (!p2IsLocked) {
			p2Selector.interactable = state;
		}
		p1Lock.interactable = state;
		p2Lock.interactable = state;
		startMatch.interactable = state;
		menuButton.interactable = state;
	}
}
