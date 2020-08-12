using UnityEngine;
using UnityEngine.UI;
//using UnityEditor;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

// TODO refactor several methods with the current player object
public class GM : MonoBehaviour
{
    /*
    IMPORTANT NUMBERS:
    Temple is 0, Citadel is 1
    Player 1 is side 0, Player 2 is side 1.
    */

    // Player interaction 
    public Button player1EndTurnButton;
    public Button player2EndTurnButton;

    // Player stats
    public Text player1Mana;
    public Text player2Mana;
    public Text player1VP;
    public Text player2VP;

    // Important sprites, objects, and prefabs
    public Sprite cardBackTemple;
    public Sprite cardBackCitadel;
    public Sprite clear;
    public Canvas switchPlayerMenu;
    public GameObject prefabCreatureCard;
    public GameObject prefabSpellCard;
    public GameObject player1;
    public GameObject player2;
    public GameObject currentPlayer;
    public GameObject canvas;

    // All of the sprites for cards
    private Sprite[] spriteSheet;
    public Sprite activeGlow;
    public Sprite placeableGlow;

    // Locations
    private LocationManager locations;

    // Constants
    private const int LEFT_SQUARE_BONUS = 2;
    private const int MIDDLE_SQUARE_BONUS = 1;
    private const int RIGHT_SQUARE_BONUS = 2;

    /// <summary>
    /// Set basic information for each player, load their decks, and give them their mulliagns. Then pick a player to go first, and start his/her turn. 
    /// </summary>

    public void Start()
    {
        this.switchPlayerMenu.enabled = false;

        this.spriteSheet = Resources.LoadAll<Sprite>("Sprites");

        //Debug.Log(this.spriteSheet.Length + " IS THE NUMBER OF SPRITES LOADED");

        this.locations = GameObject.Find("Location Manager").GetComponent<LocationManager>();

        // Set up the player objects for the match
        try
        {
            this.player1.GetComponent<player>().DeckPath = GameObject.Find("Player1StartData").GetComponent<player>().DeckPath;
            this.player2.GetComponent<player>().DeckPath = GameObject.Find("Player2StartData").GetComponent<player>().DeckPath;

            this.player1.GetComponent<player>().Name = GameObject.Find("Player1StartData").GetComponent<player>().Name;
            this.player2.GetComponent<player>().Name = GameObject.Find("Player2StartData").GetComponent<player>().Name;
        }
        catch
        {
            //Debug.Log("THE PLAYER DATA AINT THERE, SON!");
        }

        this.player1.GetComponent<player>().PlayerSide = 0;
        this.player2.GetComponent<player>().PlayerSide = 1;

        this.player1.GetComponent<player>().ManaMax = 2;
        this.player2.GetComponent<player>().ManaMax = 2;

        // The initial mulligan
        this.player1.GetComponent<player>().loadDeck();
        this.player2.GetComponent<player>().loadDeck();

        DrawCard(player1, 4);
        DrawCard(player2, 4);

        Destroy(GameObject.Find("Player1StartData"));
        Destroy(GameObject.Find("Player2StartData"));

        int chance = UnityEngine.Random.Range(0, 2);

        // Determine who goes first, and start that player's turn
        if (chance == 0)
        {
            //Debug.Log("Player 1 goes first.");

            Transform hand1 = locations.GetLocationTransform(Location.Player1Hand);
            Transform hand2 = locations.GetLocationTransform(Location.Player2Hand);

            StartTurn(player1);

            currentPlayer = player1;

            player1.GetComponent<player>().IsTurn = true;
            player2.GetComponent<player>().IsTurn = false;

            foreach (Transform child in hand1)
            {
                child.Find("Card Back").GetComponent<Image>().sprite = clear;
            }

            foreach (Transform child in hand2)
            {
                child.Find("Card Back").GetComponent<Image>().sprite = cardBackCitadel;
            }

            TogglePlayerChangeMenu();
            switchPlayerMenu.transform.Find("CurrentPlayer").GetComponent<Text>().text = player1.GetComponent<player>().Name + " goes first!" + "\nGet 20 VP to win.";
            switchPlayerMenu.transform.Find("Player1Score").GetComponent<Text>().text = "";
            switchPlayerMenu.transform.Find("Player2Score").GetComponent<Text>().text = "";
        }
        else if (chance == 1)
        {
            //Debug.Log("Player 2 goes first.");

            Transform hand1 = locations.GetLocationTransform(Location.Player1Hand);
            Transform hand2 = locations.GetLocationTransform(Location.Player2Hand);

            currentPlayer = player2;

            StartTurn(player2);

            player1.GetComponent<player>().IsTurn = false;
            player2.GetComponent<player>().IsTurn = true;

            foreach (Transform child in hand1)
            {
                child.Find("Card Back").GetComponent<Image>().sprite = cardBackTemple;
            }

            foreach (Transform child in hand2)
            {
                child.Find("Card Back").GetComponent<Image>().sprite = clear;
            }

            TogglePlayerChangeMenu();
            switchPlayerMenu.transform.Find("CurrentPlayer").GetComponent<Text>().text = player2.GetComponent<player>().Name + " goes first!" + "\nGet 20 VP to win.";
            switchPlayerMenu.transform.Find("Player1Score").GetComponent<Text>().text = "";
            switchPlayerMenu.transform.Find("Player2Score").GetComponent<Text>().text = "";
        }

        this.UpdateCardColors();
    }

    /// <summary>
    /// Updates player mana and VP, checks for win conditions, and makes sure that the correct buttons are enabled. 
    /// </summary>
    public void Update()
    {
        // Update mana every frame **** CAUSE WHY NOT? ****
        player1Mana.text = "Mana: " + player1.GetComponent<player>().CurrentMana.ToString() + " / " + player1.GetComponent<player>().ManaMax.ToString();
        player2Mana.text = "Mana: " + player2.GetComponent<player>().CurrentMana.ToString() + " / " + player2.GetComponent<player>().ManaMax.ToString();

        player1VP.text = "VP: " + player1.GetComponent<player>().VictoryPoints.ToString();
        player2VP.text = "VP: " + player2.GetComponent<player>().VictoryPoints.ToString();

        if (currentPlayer.GetComponent<player>().VictoryPoints >= 20)
        {
            EndGame(currentPlayer);
        }

        if (player1.GetComponent<player>().IsTurn)
        {
            GameObject.Find("Player1HandRaycastBlocker").GetComponent<Image>().raycastTarget = false;
            GameObject.Find("Player2HandRaycastBlocker").GetComponent<Image>().raycastTarget = true;
        }
        else
        {
            GameObject.Find("Player1HandRaycastBlocker").GetComponent<Image>().raycastTarget = true;
            GameObject.Find("Player2HandRaycastBlocker").GetComponent<Image>().raycastTarget = false;
        }

        if (GameObject.Find("EndTurnPlayer1").GetComponent<Button>().interactable == switchPlayerMenu.enabled)
        {
            GameObject.Find("EndTurnPlayer1").GetComponent<Button>().interactable = !switchPlayerMenu.enabled;
        }

        foreach (GameObject card in GameObject.FindGameObjectsWithTag("Card"))
        {
            // Color the cards in the hand of the current player
            if (!card.GetComponent<Card>().HasBeenPlaced)
            {
                if (((currentPlayer.GetComponent<player>().CurrentMana - card.GetComponent<Card>().CurrentCost) >= 0)
                    && (card.GetComponent<Card>().BattleSide == currentPlayer.GetComponent<player>().PlayerSide))
                {
                    card.transform.FindChild("Outline").GetComponent<Image>().sprite = this.placeableGlow;
                }
                else
                {
                    card.transform.FindChild("Outline").GetComponent<Image>().sprite = this.clear;
                }
            }
        }

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            PointerEventData mouse = EventSystem.current.GetComponent<BetterStandaloneInputModule>().GetLastPointerEventData(-1);
            GameObject clickedObject = EventSystem.current.GetComponent<BetterStandaloneInputModule>().GetLastPointerEventData(-1).pointerCurrentRaycast.gameObject;

            Debug.Log("The clicked item is " + mouse.pointerCurrentRaycast.gameObject.name);

            if (GameObject.FindGameObjectWithTag("Attacking") != null 
                && GameObject.FindGameObjectWithTag("Attacking").GetComponent<Card>().BattleSide != clickedObject.GetComponent<Card>().BattleSide
                && GameObject.FindGameObjectWithTag("Attacking").GetComponent<Card>().HasBeenPlaced
                && !GameObject.FindGameObjectWithTag("Attacking").GetComponent<Card>().IsExhausted)
            {
                Debug.Log("Combat called");
            }
            else if (GameObject.FindGameObjectWithTag("Menu Open") == null 
                && clickedObject.GetComponent<Card>() != null 
                && clickedObject.GetComponent<Card>().OwnerTag.Equals(currentPlayer.GetComponent<player>().Name)
                && clickedObject.GetComponent<Card>().HasBeenPlaced)
            {
                Debug.Log("Opening menu for " + clickedObject.name);
                clickedObject.tag = "Menu Open";
                clickedObject.transform.FindChild("Function Menu").gameObject.SetActive(true);
            }
            else if (GameObject.FindGameObjectWithTag("Menu Open") != null
                && clickedObject.GetComponent<Card>() != null
                && clickedObject.GetComponent<Card>().OwnerTag.Equals(currentPlayer.GetComponent<player>().Name)
                && clickedObject.GetComponent<Card>().HasBeenPlaced)
            {
                Debug.Log("Closing menu for " + GameObject.FindGameObjectWithTag("Menu Open").name + ", Operning menu for " + clickedObject.name);

                GameObject.FindGameObjectWithTag("Menu Open").transform.FindChild("Function Menu").gameObject.SetActive(false);
                GameObject.FindGameObjectWithTag("Menu Open").tag = "Card";
                clickedObject.transform.FindChild("Function Menu").gameObject.SetActive(true);
                clickedObject.tag = "Menu Open";
            }


            //if pointerPress is null (it was either a mouse2 click or it was outside any gui), or if pointerPress doesn't have my context menu component.
        }
    }

    /// <summary>
    /// Starts the turn of a player by giving him/her mana and a card. 
    /// </summary>
    /// <param name="player"></param> The player whose turn is being started. 
    public void StartTurn(GameObject player)
    {
        if (player.GetComponent<player>().ManaMax < 13)
        {
            player.GetComponent<player>().ManaMax++;
        }
        player.GetComponent<player>().CurrentMana = player.GetComponent<player>().ManaMax;

        DrawCard(player, 1);
    }

    /// <summary>
    /// Ends the turn of the current player, tallies the VP of both players, and then starts the turn of the other player.
    /// </summary>
    public void EndTurn()
    {
        // Toggles to Player 2 turn if Player 1 pressed the button
        if (player1.GetComponent<player>().IsTurn)
        {
            // Set Player 1's card back to enabled, and Player 2's card back to clear
            Transform hand1 = locations.GetLocationTransform(Location.Player1Hand);

            foreach (Transform child in hand1)
            {
                child.Find("Card Back").GetComponent<Image>().sprite = cardBackTemple;
            }

            Transform hand2 = locations.GetLocationTransform(Location.Player2Hand);

            foreach (Transform child in hand2)
            {
                child.Find("Card Back").GetComponent<Image>().sprite = clear;
            }

            // Give Player 1 all earned VP
            VPTally(player1);

            // Start Player 2's turn
            StartTurn(player2);
            player1.GetComponent<player>().IsTurn = false;
            player2.GetComponent<player>().IsTurn = true;

            TogglePlayerChangeMenu();
            switchPlayerMenu.transform.Find("CurrentPlayer").GetComponent<Text>().text = "It is now " + player2.GetComponent<player>().Name + "'s turn";
            switchPlayerMenu.transform.Find("Player1Score").GetComponent<Text>().text = player1.GetComponent<player>().Name + " has " + player1.GetComponent<player>().VictoryPoints + " VP!";
            switchPlayerMenu.transform.Find("Player2Score").GetComponent<Text>().text = player2.GetComponent<player>().Name + " has " + player2.GetComponent<player>().VictoryPoints + " VP!";

            currentPlayer = player2;         
        }
        // Toggles to player 1 turn if Player 2 pressed the button
        else if (player2.GetComponent<player>().IsTurn)
        {
            // Set Player 2's card back to enabled, and Player 1's card back to clear
            Transform hand1 = locations.GetLocationTransform(Location.Player1Hand);

            foreach (Transform child in hand1)
            {
                child.Find("Card Back").GetComponent<Image>().sprite = clear;
            }

            Transform hand2 = locations.GetLocationTransform(Location.Player2Hand);

            foreach (Transform child in hand2)
            {
                child.Find("Card Back").GetComponent<Image>().sprite = cardBackCitadel;
            }

            // Give Player 2 all earned VP
            VPTally(player2);

            // Start Player 1's turn
            StartTurn(player1);
            player1.GetComponent<player>().IsTurn = true;
            player2.GetComponent<player>().IsTurn = false;

            TogglePlayerChangeMenu();
            switchPlayerMenu.transform.Find("CurrentPlayer").GetComponent<Text>().text = "It is now " + player1.GetComponent<player>().Name + "'s turn";
            switchPlayerMenu.transform.Find("Player1Score").GetComponent<Text>().text = player1.GetComponent<player>().Name + " has " + player1.GetComponent<player>().VictoryPoints + " VP!";
            switchPlayerMenu.transform.Find("Player2Score").GetComponent<Text>().text = player2.GetComponent<player>().Name + " has " + player2.GetComponent<player>().VictoryPoints + " VP!";

            currentPlayer = player1;
        }

        this.UpdateCardColors();

        this.DistributePlaceRewards();

        //Debug.Log("The current player is now " + currentPlayer.GetComponent<player>().Name);
    }

    /// <summary>
    /// Displays the victory screen. 
    /// </summary>
    /// <param name="winner"></param> The player who has won. 
    public void EndGame(GameObject winner)
    {
        switchPlayerMenu.transform.Find("CurrentPlayer").GetComponent<Text>().text = winner.GetComponent<player>().Name + " Has Transcended! \nCongrats!";
        switchPlayerMenu.transform.Find("Player1Score").GetComponent<Text>().text = "";
        switchPlayerMenu.transform.Find("Player2Score").GetComponent<Text>().text = "";
        switchPlayerMenu.transform.Find("Button").GetComponent<Button>().onClick.AddListener(delegate () { exitMenu(); });
    }

    /// <summary>
    /// Toggle the meby that displays between players' turns. 
    /// </summary>
    public void TogglePlayerChangeMenu()
    {
        if (switchPlayerMenu.enabled == false)
        {
            switchPlayerMenu.enabled = true;
        }
        else if (switchPlayerMenu.enabled == true)
        {
            switchPlayerMenu.enabled = false;
        }
    }

    /// <summary>
    /// Give bonuses appropriately at the end of a turn.
    /// </summary>
    public void DistributePlaceRewards()
    {
        List<GameObject> numberInLeftTemplePlayingSpaces = new List<GameObject>();
        List<GameObject> numberInMiddleTemplePlayingSpaces = new List<GameObject>();
        List<GameObject> numberInRightTemplePlayingSpaces = new List<GameObject>();
        List<GameObject> numberInLeftCitadelPlayingSpaces = new List<GameObject>();
        List<GameObject> numberInMiddleCitadelPlayingSpaces = new List<GameObject>();
        List<GameObject> numberInRightCitadelPlayingSpaces = new List<GameObject>();

        foreach (GameObject card in GameObject.FindGameObjectsWithTag("Card"))
        {
            if (card.transform.parent.parent.gameObject.name.Equals("LeftTemplePlayingSpaces") && card.GetComponent<Card>().HasBeenPlaced == true)
            {
                numberInLeftTemplePlayingSpaces.Add(card);
            }

            if (card.transform.parent.parent.gameObject.name.Equals("MiddleTemplePlayingSpaces") && card.GetComponent<Card>().HasBeenPlaced == true)
            {
                numberInMiddleTemplePlayingSpaces.Add(card);
            }

            if (card.transform.parent.parent.gameObject.name.Equals("RightTemplePlayingSpaces") && card.GetComponent<Card>().HasBeenPlaced == true)
            {
                numberInRightTemplePlayingSpaces.Add(card);
            }

            if (card.transform.parent.parent.gameObject.name.Equals("LeftCitadelPlayingSpaces") && card.GetComponent<Card>().HasBeenPlaced == true)
            {
                numberInLeftCitadelPlayingSpaces.Add(card);
            }

            if (card.transform.parent.parent.gameObject.name.Equals("MiddleCitadelPlayingSpaces") && card.GetComponent<Card>().HasBeenPlaced == true)
            {
                numberInMiddleCitadelPlayingSpaces.Add(card);
            }

            if (card.transform.parent.parent.gameObject.name.Equals("RightCitadelPlayingSpaces") && card.GetComponent<Card>().HasBeenPlaced == true)
            {
                numberInRightCitadelPlayingSpaces.Add(card);
            }
        }

        // Left spaces logic
        if (numberInLeftTemplePlayingSpaces.Count > numberInLeftCitadelPlayingSpaces.Count)
        {
            player1.GetComponent<player>().CurrentMana += LEFT_SQUARE_BONUS;
        }
        else if (numberInLeftTemplePlayingSpaces.Count < numberInLeftCitadelPlayingSpaces.Count)
        {
            player2.GetComponent<player>().CurrentMana += LEFT_SQUARE_BONUS;
        }

        // Middle spaces logic
        if (numberInMiddleTemplePlayingSpaces.Count > numberInMiddleCitadelPlayingSpaces.Count)
        {
            foreach (GameObject card in numberInMiddleTemplePlayingSpaces)
            {
                card.GetComponent<Card>().CurrentHealth += MIDDLE_SQUARE_BONUS;
                card.transform.FindChild("Health").GetComponent<Text>().text = card.GetComponent<Card>().CurrentHealth.ToString();
                card.transform.FindChild("Health").GetComponent<Text>().color = Color.green;
            }
        }
        else if (numberInMiddleTemplePlayingSpaces.Count < numberInMiddleCitadelPlayingSpaces.Count)
        {
            foreach (GameObject card in numberInMiddleCitadelPlayingSpaces)
            {
                card.GetComponent<Card>().CurrentHealth += MIDDLE_SQUARE_BONUS;
                card.transform.FindChild("Health").GetComponent<Text>().text = card.GetComponent<Card>().CurrentHealth.ToString();
                card.transform.FindChild("Health").GetComponent<Text>().color = Color.green;
            }
        }

        // Right spaces logic
        if (numberInRightTemplePlayingSpaces.Count > numberInRightCitadelPlayingSpaces.Count)
        {
            player1.GetComponent<player>().VictoryPoints += LEFT_SQUARE_BONUS;
        }
        else if (numberInRightTemplePlayingSpaces.Count < numberInRightCitadelPlayingSpaces.Count)
        {
            player2.GetComponent<player>().VictoryPoints += LEFT_SQUARE_BONUS;
        }
    }

    /// <summary>
    /// Runs combat between two cards.
    /// </summary>
    /// <param name="attackingCard"></param> The card attacking.
    /// <param name="defendingCard"></param> The card being attacked. 
    public void Combat(GameObject attackingCard, GameObject defendingCard)
    {
        // Attacking card combat
        attackingCard.GetComponent<Card>().CurrentHealth = attackingCard.GetComponent<Card>().CurrentHealth - defendingCard.GetComponent<Card>().CurrentAttack;
        attackingCard.transform.Find("Health").gameObject.GetComponent<Text>().text = attackingCard.GetComponent<Card>().CurrentHealth.ToString();
        attackingCard.transform.FindChild("Health").GetComponent<Text>().color = Color.red;
        attackingCard.transform.Find("Outline").gameObject.GetComponent<Image>().sprite = clear;

        // Defending card combat
        defendingCard.GetComponent<Card>().CurrentHealth = defendingCard.GetComponent<Card>().CurrentHealth - attackingCard.GetComponent<Card>().CurrentAttack;
        defendingCard.transform.Find("Health").gameObject.GetComponent<Text>().text = defendingCard.GetComponent<Card>().CurrentHealth.ToString();
        defendingCard.transform.FindChild("Health").GetComponent<Text>().color = Color.red;

        // Destroy attacking cards
        if (attackingCard.GetComponent<Card>().CurrentHealth <= 0)
        {
            Destroy(attackingCard);
            Debug.Log("Attacking Card Died!");
        }
        else
        {
            attackingCard.gameObject.tag = "Card";
            attackingCard.GetComponent<Card>().IsExhausted = true;
        }

        // Destroy dead defending card
        if (defendingCard.GetComponent<Card>().CurrentHealth <= 0)
        {
            Destroy(defendingCard);
            Debug.Log("Defending Card Died!");
        }
    }

    public void DisplayDiscription(GameObject card, bool display)
    {
        Card cardScript = card.GetComponent<Card>();
        if (display)
        {
            if (GameObject.Find("Card Hover") != null)
            {
                Destroy(GameObject.Find("Card Hover"));
            }

            GameObject hover = Instantiate((GameObject)Resources.Load("Prefabs/Card Hover"));
            hover.transform.SetParent(GameObject.Find("Canvas").transform);
            hover.transform.position = new Vector2(640,427);
            hover.transform.FindChild("Card").FindChild("Splash Image").GetComponent<Image>().sprite = card.transform.FindChild("Splash Image").GetComponent<Image>().sprite;
            hover.transform.FindChild("Card").FindChild("Card Frame").GetComponent<Image>().sprite = card.transform.FindChild("Card Frame").GetComponent<Image>().sprite;
            hover.transform.FindChild("Card").FindChild("Description").GetComponent<Text>().text = card.transform.FindChild("Description").GetComponent<Text>().text;
            hover.transform.FindChild("Card").FindChild("Title").GetComponent<Text>().text = card.transform.FindChild("Title").GetComponent<Text>().text;
            hover.transform.FindChild("Card").FindChild("Cost").GetComponent<Text>().text = card.transform.FindChild("Cost").GetComponent<Text>().text;
            hover.transform.FindChild("Card").FindChild("Attack").GetComponent<Text>().text = card.transform.FindChild("Attack").GetComponent<Text>().text;
            hover.transform.FindChild("Card").FindChild("Health").GetComponent<Text>().text = card.transform.FindChild("Health").GetComponent<Text>().text;
            hover.transform.FindChild("Card").FindChild("Defense").GetComponent<Text>().text = card.transform.FindChild("Defense").GetComponent<Text>().text;
            hover.transform.FindChild("Card").FindChild("Health").GetComponent<Text>().text = card.transform.FindChild("Health").GetComponent<Text>().text;
            hover.transform.FindChild("Card").FindChild("Range").GetComponent<Text>().text = card.transform.FindChild("Range").GetComponent<Text>().text;

            hover.name = "Card Hover";

            foreach (GameObject c in GameObject.FindGameObjectsWithTag("Card"))
            {
                if (c.GetComponent<isDraggable>().displayZoomedView == true && !card.Equals(c))
                {
                    c.GetComponent<isDraggable>().displayZoomedView = false;
                    Debug.Log("Display closed for " + c.name);
                }
            }
        }
        else if (!display && GameObject.Find("Card Hover") != null)
        {
            Destroy(GameObject.Find("Card Hover"));
        }
    }

    /// <summary>
    /// Draw card(s) for the player passed in, of the quantity passed in
    /// </summary>
    /// <param name="player"></param>
    /// <param name="howMany"></param>
    public void DrawCard(GameObject player, int howMany)
    {
        for (int i = 0; i < howMany; i++)
        {
            try
            {
                this.InstantiateCard(player, player.GetComponent<player>().Deck.poll());
            }
            catch
            {
                Debug.Log("Out of cards!");
            }
        }
    }

    public void UpdateCardColors()
    {
        // Set all placed cards to active
        foreach (GameObject card in GameObject.FindGameObjectsWithTag("Card"))
        {
            //Check to see if the card has been placed
            if (card.GetComponent<Card>().HasBeenPlaced)
            {
                // If it is, check to see if it is exhausted
                if (card.GetComponent<Card>().IsExhausted)
                {
                    // Set it to not exhausted if it is
                    card.GetComponent<Card>().IsExhausted = false;
                }
                // Color the card since it shouldn't be exhausted anymore
                if (card.GetComponent<Card>().BattleSide == currentPlayer.GetComponent<player>().PlayerSide)
                {
                    card.transform.FindChild("Outline").GetComponent<Image>().sprite = this.activeGlow;
                }
                else
                {
                    card.transform.FindChild("Outline").GetComponent<Image>().sprite = this.clear;
                }
            }      
        }
    }

    /// <summary>
    /// Tally a player's VP at the end of their turn
    /// </summary>
    /// <param name="player"></param> The player whose turn it currently is. 
    public void VPTally(GameObject player)
    {
        // Logic for Player 1
        if (player.GetComponent<player>().PlayerSide == 0)
        {
            if (locations.GetLocationTransform(Location.BottomLeft1).childCount == 1 
                || locations.GetLocationTransform(Location.BottomLeft2).childCount == 1 
                || locations.GetLocationTransform(Location.BottomLeft3).childCount == 1)
            {
                player1.GetComponent<player>().VictoryPoints++;
            }

            if (locations.GetLocationTransform(Location.BottomCenter1).childCount == 1 
                || locations.GetLocationTransform(Location.BottomCenter2).childCount == 1 
                || locations.GetLocationTransform(Location.BottomCenter3).childCount == 1)
            {
                player1.GetComponent<player>().VictoryPoints++;
            }

            if (locations.GetLocationTransform(Location.BottomRight1).childCount == 1
                || locations.GetLocationTransform(Location.BottomRight2).childCount == 1
                || locations.GetLocationTransform(Location.BottomRight3).childCount == 1)
            {
                player1.GetComponent<player>().VictoryPoints++;
            }
        }

        // Logic for Player 2
        if (player.GetComponent<player>().PlayerSide == 1)
        {
            if (locations.GetLocationTransform(Location.TopLeft1).childCount == 1
                || locations.GetLocationTransform(Location.TopLeft2).childCount == 1
                || locations.GetLocationTransform(Location.TopLeft3).childCount == 1)
            {
                player2.GetComponent<player>().VictoryPoints++;
            }

            if (locations.GetLocationTransform(Location.TopCenter1).childCount == 1
                || locations.GetLocationTransform(Location.TopCenter2).childCount == 1
                || locations.GetLocationTransform(Location.TopCenter3).childCount == 1)
            {
                player2.GetComponent<player>().VictoryPoints++;
            }

            if (locations.GetLocationTransform(Location.TopRight1).childCount == 1
                || locations.GetLocationTransform(Location.TopRight2).childCount == 1
                || locations.GetLocationTransform(Location.TopRight3).childCount == 1)
            {
                player2.GetComponent<player>().VictoryPoints++;
            }
        }
    }

    /// <summary>
    /// Create a card gameobject in the hand of a player.
    /// </summary>
    /// <param name="player"></param> The player to whom the card will be given.
    /// <param name="currentCard"></param> The next card that shall be drawn in that player's deck. 
    public void InstantiateCard(GameObject player, Card currentCard)
    {
        // Instantiate card for player 1
        if (player.GetComponent<player>().PlayerSide == 0)
        {
            Transform hand = locations.GetLocationTransform(Location.Player1Hand);

            if (hand.childCount < 6) //if at hand limit, throw out card
            {
                GameObject card;
                Card cardsScript;

                // Create the card with creature prefab
                if (currentCard.Type.Equals("Creature"))
                {
                    card = (GameObject)Instantiate(prefabCreatureCard, hand.position, hand.rotation);
                    card.transform.FindChild("Splash Image").gameObject.GetComponent<Image>().sprite = this.spriteSheet[Int64.Parse(currentCard.ID)];
                }
                // Or create it using the spell prefab
                else
                {
                    card = (GameObject)Instantiate(prefabSpellCard, hand.position, hand.rotation);
                    card.transform.FindChild("Splash Image").gameObject.GetComponent<Image>().sprite = this.spriteSheet[Int64.Parse(currentCard.ID)];
                }

                // Set the reference to the new card's script
                cardsScript = card.GetComponent<Card>();

                // Set the card's transform to that of its player's hand
                //card.transform.SetParent(Player2Hand.transform.parent);

                // Initialize the card's current values
                currentCard.SetCurrents();

                // Set the card's name
                card.GetComponentInChildren<Text>().text = currentCard.CardName;
                cardsScript.CardName = currentCard.CardName;
                card.name = currentCard.CardName;

                // Set all of the remaining information for the card
                cardsScript.ID = currentCard.ID;
                cardsScript.Image = currentCard.Image;
                cardsScript.Description = currentCard.Description;
                cardsScript.Alliance = currentCard.Alliance;
                cardsScript.Type = currentCard.Type;
                cardsScript.Cost = currentCard.Cost;
                cardsScript.Attack = currentCard.Attack;
                cardsScript.Health = currentCard.Health;
                cardsScript.Defense = currentCard.Defense;
                cardsScript.Range = currentCard.Range;
                cardsScript.TargetName = currentCard.TargetName;
                cardsScript.CurrentID = currentCard.CurrentID;
                //Debug.Log(currentCard.CurrentID);
                cardsScript.CurrentCost = currentCard.CurrentCost;
                cardsScript.CurrentAttack = currentCard.CurrentAttack;
                cardsScript.CurrentHealth = currentCard.CurrentHealth;
                cardsScript.CurrentDefense = currentCard.CurrentDefense;
                cardsScript.CurrentRange = currentCard.CurrentRange;
                cardsScript.OwnerTag = player1.GetComponent<player>().Name;
                cardsScript.BattleSide = player.GetComponent<player>().PlayerSide;
                cardsScript.EffectName = currentCard.EffectName;

                // Set the effect for each card
                foreach (Effect effect in Enum.GetValues(typeof(Effect)))
                {
                    // Set the effect if the card is a spell and has an effect
                    if (effect.ToString().Equals(currentCard.EffectName))
                    {
                        cardsScript.Effect = effect;
                        //Debug.Log(effect.ToString());
                    }
                    else if (effect.ToString().Equals(currentCard.TargetName))
                    {
                        cardsScript.Target = effect;
                        //Debug.Log(effect.ToString());
                    }
                }

                // Move the card onto the canvas
                card.transform.SetParent(hand);

                // Set the parentToReturnTo for the card
                card.GetComponent<isDraggable>().parentToReturnTo = hand;

                // Set the visible attributes of the card game object to those stored in it's card script parameters
                card.transform.Find("Title").gameObject.GetComponent<Text>().text = cardsScript.CardName;
                card.transform.Find("Description").gameObject.GetComponent<Text>().text = cardsScript.Description;
                card.transform.Find("Attack").gameObject.GetComponent<Text>().text = cardsScript.Attack;
                card.transform.Find("Defense").gameObject.GetComponent<Text>().text = cardsScript.Defense;
                card.transform.Find("Health").gameObject.GetComponent<Text>().text = cardsScript.Health;
                card.transform.Find("Range").gameObject.GetComponent<Text>().text = cardsScript.Range;
                card.transform.Find("Cost").gameObject.GetComponent<Text>().text = cardsScript.Cost;
            }
        }

        // Instanstiate card for player 2
        if (player.GetComponent<player>().PlayerSide == 1)
        {
            Transform hand = locations.GetLocationTransform(Location.Player2Hand);

            if (hand.transform.childCount < 6) { //if at hand limit, throw out card

                // The new card, and its script
                GameObject card;
                Card cardsScript;

                // Create the card with the creature prefab
                if (currentCard.Type.Equals("Creature"))
                {
                    card = (GameObject)Instantiate(prefabCreatureCard, hand.transform.position, hand.rotation);
                    card.transform.FindChild("Splash Image").gameObject.GetComponent<Image>().sprite = spriteSheet[Int64.Parse(currentCard.ID)];
                }
                // Or create it with the spell prefab
                else
                {
                    card = (GameObject)Instantiate(prefabSpellCard, hand.transform.position, hand.rotation);
                    card.transform.FindChild("Splash Image").gameObject.GetComponent<Image>().sprite = spriteSheet[Int64.Parse(currentCard.ID)];
                }

                // Set the reference to the new card's script
                cardsScript = card.GetComponent<Card>();

                // Set the card's transform to that of its player's hand
                card.transform.SetParent(hand.transform.parent);

                // Initialize the card's current values
                currentCard.SetCurrents();

                // Set the card's name
                card.GetComponentInChildren<Text>().text = currentCard.CardName;
                cardsScript.CardName = currentCard.CardName;
                card.name = currentCard.CardName;

                // Set all of the remaining information for the card
                cardsScript.ID = currentCard.ID;
                cardsScript.Image = currentCard.Image;
                cardsScript.Description = currentCard.Description;
                cardsScript.Alliance = currentCard.Alliance;
                cardsScript.Type = currentCard.Type;
                cardsScript.Cost = currentCard.Cost;
                cardsScript.Attack = currentCard.Attack;
                cardsScript.Health = currentCard.Health;
                cardsScript.Defense = currentCard.Defense;
                cardsScript.Range = currentCard.Range;
                cardsScript.TargetName = currentCard.TargetName;
                cardsScript.CurrentID = currentCard.CurrentID;
                cardsScript.CurrentCost = currentCard.CurrentCost;
                cardsScript.CurrentAttack = currentCard.CurrentAttack;
                cardsScript.CurrentHealth = currentCard.CurrentHealth;
                cardsScript.CurrentDefense = currentCard.CurrentDefense;
                cardsScript.CurrentRange = currentCard.CurrentRange;
                cardsScript.OwnerTag = player2.GetComponent<player>().Name;
                cardsScript.BattleSide = player.GetComponent<player>().PlayerSide;
                cardsScript.EffectName = currentCard.EffectName;

                // Set the effect for each card
                foreach (Effect effect in Enum.GetValues(typeof(Effect)))
                {
                    // Set the effect if the card is a spell and has an effect
                    if (effect.ToString().Equals(currentCard.EffectName))
                    {
                        cardsScript.Effect = effect;
                        //Debug.Log(effect.ToString());
                    }
                    else if (effect.ToString().Equals(currentCard.TargetName))
                    {
                        cardsScript.Target = effect;
                        //Debug.Log(effect.ToString());
                    }
                }

                // Move the card onto the canvas
                card.transform.SetParent(hand);

                // Set the parentToReturnTo for the card
                card.GetComponent<isDraggable>().parentToReturnTo = hand;

                // Set the visible attributes of the card game object to those stored in it's card script parameters
                card.transform.Find("Title").gameObject.GetComponent<Text>().text = cardsScript.CardName;
                card.transform.Find("Description").gameObject.GetComponent<Text>().text = cardsScript.Description;
                card.transform.Find("Attack").gameObject.GetComponent<Text>().text = cardsScript.Attack;
                card.transform.Find("Defense").gameObject.GetComponent<Text>().text = cardsScript.Defense;
                card.transform.Find("Health").gameObject.GetComponent<Text>().text = cardsScript.Health;
                card.transform.Find("Range").gameObject.GetComponent<Text>().text = cardsScript.Range;
                card.transform.Find("Cost").gameObject.GetComponent<Text>().text = cardsScript.Cost;
            }
         }
    }

    public void exitMenuPress()
    {
        if (GameObject.Find("ExitMeby").GetComponent<Canvas>().enabled)
        {
            GameObject.Find("ExitMeby").GetComponent<Canvas>().enabled = false;
        }
        else
        {
            GameObject.Find("ExitMeby").GetComponent<Canvas>().enabled = true;
        }
    }

    public void exitMenu()
    {
        SceneManager.LoadScene(1);
    }

    /*
    The following methods are for testing purposes only, and are cut down versions of what will be in game logic
    */

    /*
    Here is old logic I may want to see later:
        // Runs once at the start of Player 1's turn
        //if (player1.GetComponent<player>().IsTurn == true && player1.GetComponent<player>().hasStartedGoing == false)
        //{
        //    turn(player1);
        //    player1Mana.text = player1.GetComponent<player>().CurrentMana.ToString();
        //    player1.GetComponent<player>().hasStartedGoing = true;

        //}

        // Runs once at the start of Player 2's turn
        //if (player2.GetComponent<player>().IsTurn == true && player2.GetComponent<player>().hasStartedGoing == false)
        //{
        //    turn(player1);
        //    player2Mana.text = player2.GetComponent<player>().CurrentMana.ToString();
        //    player2.GetComponent<player>().hasStartedGoing = true;

        //}

        // The check for the end turn buttons loop
        //if (player1.GetComponent<player>().IsTurn == true)
        //{
        //    player1EndTurnButton.interactable = true;
        //    player2EndTurnButton.interactable = true;
        //}

        //if (player2.GetComponent<player>().IsTurn == true)
        //{
        //    player2EndTurnButton.interactable = true;
        //    player1EndTurnButton.interactable = true;
        //}

                /*List<Card> deck = new List<Card>();

        DeckPath = Application.dataPath + "/scripts/xml/cards.xml";

        DeckReader reader = new DeckReader();

        List<Card> archiveDeck = reader.load(DeckPath);

        Debug.Log(archiveDeck.Count);

        for (int i = 0; i < archiveDeck.Count; i++)
        {
            deck.Add(archiveDeck[i]);

            if (i == 26)
            {
                Debug.Log("End of the line");
                Debug.Log(deck[10].cardName);
            }
        }

        ///// UPDATE USING THE DECK THAT IS IN THE PLAYER OBJECT - MAKE SURE THE PLAYER DECK IS LOADED IN START!

        
    String DeckPath = Application.dataPath + "/scripts/xml/cards.xml";

    DeckReader reader = new DeckReader();

    List<Card> deck = new List<Card>();
    List<Card> archiveDeck = reader.load(DeckPath);

        for (int i = 0; i<archiveDeck.Count; i++)
        {
            deck.Add(archiveDeck[i]);
        }
*/

    //should create cards for each one in a deck and then instanstiate them in the game
    public void generateDeck(GameObject player)
    {
        string path;
        path = Application.dataPath + player.GetComponent<player>().DeckPath;

        List<Card> cards = new List<Card>();
        DeckReader reader = new DeckReader();

        if (path.Equals(Application.dataPath + ""))
        {
            //EditorUtility.DisplayDialog("No Deck Selected", "You need to load a deck!", "Alright. God.");
            Application.Quit();
        }
        else
        {
            cards = reader.load(path);

            //information about the size of the array of cards, and what card the while loop is on
            int totalCards = cards.Count;
            int currentCard = 0;

            //goes through the cards one by one and adds them to the playing field
            while (currentCard < totalCards)
            {
                InstantiateCard(player, cards[currentCard]);
                currentCard++;
            }
        }
    }

    //should create one single card from a list of cards, given player object
    public void generateCard(GameObject player)
    {
        string path;
        int card = 0;
        //string path, int cardLocation
        //a list that will hold all the cards
        List<Card> cards = new List<Card>();

        //player.GetComponent<player>().DeckPath = Application.dataPath + player.GetComponent<player>().DeckPath;

        //loads the cards into the list from an XML file
        DeckReader reader = new DeckReader();

        path = Application.dataPath + player.GetComponent<player>().DeckPath;

        if (path.Equals(Application.dataPath + ""))
        {
            //EditorUtility.DisplayDialog("No Deck Selected", "You need to load a deck!", "Alright. God.");
        }
        else
        {
            cards = reader.load(path);
            InstantiateCard(player, cards[card]);
            card++;

            if (card == cards.Count)
            {
                card = 0;
            }
        }

        //generates the card using the prefabCard prefab
        Debug.Log("Method Called");

    }

}