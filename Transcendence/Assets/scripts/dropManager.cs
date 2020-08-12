using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class dropManager : MonoBehaviour {

    // Variables needed for game logic
    private GM GM;
    private isDraggable D;
    private EffectManager EF;

    public void Start()
    {
        this.GM = GameObject.Find("GM").GetComponent<GM>();
        this.EF = GameObject.Find("Effect Manager").GetComponent<EffectManager>();
    }

    // Sorts the objects 
    public void Drop(PointerEventData data, GameObject draggingCardObject, Transform pointerObjectName)
    {
        GameObject pointerObject = pointerObjectName.gameObject;
        Card draggingCardScript = draggingCardObject.GetComponent<Card>();

        D = data.pointerDrag.GetComponent<isDraggable>();

        //Debug.Log("Dragging card is a " + draggingCardObject.GetComponent<Card>().Type);

        // Placing creature card
        if (pointerObject.CompareTag("Field") && !draggingCardScript.HasBeenPlaced && draggingCardScript.Type.Equals("Creature"))
        {
            // Run the logic for a card over a field
            //Debug.Log("Creature and Field conditions met.");
            TryPlace(draggingCardObject, draggingCardScript, pointerObject);
        }
        // Attacking creature card
        else if (pointerObject.CompareTag("Card")
            && draggingCardScript.Type.Equals("Creature")
            && !draggingCardScript.IsExhausted
            && draggingCardScript.HasBeenPlaced
            && !draggingCardScript.OwnerTag.Equals(pointerObject.GetComponent<Card>().OwnerTag)
            )
        {
            //Debug.Log("Placed Card and Enemy Card conditions met.");
            TryAttack(draggingCardObject, draggingCardScript, pointerObject);
        }
        // Single target spell card
        else if (pointerObject.CompareTag("Card") && pointerObject.GetComponent<Card>().Type.Equals("Creature") && draggingCardScript.Type.Equals("Spell") && draggingCardScript.Target.Equals(Effect.Single))
        {
            //Debug.Log("Single target spell conditions met.");
            TrySpell(draggingCardObject, draggingCardScript, pointerObject);
        }
        // Non-targeted spell card
        else if (draggingCardScript.Type.Equals("Spell") && draggingCardScript.Target.Equals(Effect.All))
        {
            //Debug.Log("Non-targeted spell conditions met.");
            TrySpell(draggingCardObject, draggingCardScript, pointerObject);
        }
        // Zone spell card
        else if (draggingCardScript.Target.Equals(Effect.Zone) && pointerObject.CompareTag("Card") && pointerObject.GetComponent<Card>().Type.Equals("Creature") && draggingCardScript.Type.Equals("Spell"))
        {
            //Debug.Log("Zone spell conditions met.");
            TrySpell(draggingCardObject, draggingCardScript, pointerObject);
        }
        // Turn card back to card if action conditions are met
        else
        {
            Debug.Log("No conditions met!");
            draggingCardObject.tag = "Card";
        }
    }

    /// <summary>
    /// Creature placement logics.
    /// </summary>
    /// <param name="draggingCardGameObject"></param> GameObject of the card being dragged.
    /// <param name="draggingCardScript"></param> Card script of the card being dragged. 
    /// <param name="pointerObject"></param> The object below the dragging card.
    public void TryPlace(GameObject draggingCardGameObject, Card draggingCardScript, GameObject pointerObject)
    {
        // Runs card placement for player 1
        if (pointerObject.GetComponent<Field>().side == GM.currentPlayer.GetComponent<player>().PlayerSide
            && GM.currentPlayer.GetComponent<player>().IsTurn
            && draggingCardScript.OwnerTag.Equals(GM.currentPlayer.GetComponent<player>().Name)
            && (GM.currentPlayer.GetComponent<player>().CurrentMana - draggingCardScript.CurrentCost >= 0)
            )
        {
            // Send card to its new field
            D.parentToReturnTo = pointerObject.GetComponent<Field>().transform;

            GM.currentPlayer.GetComponent<player>().CurrentMana = GM.currentPlayer.GetComponent<player>().CurrentMana - draggingCardScript.CurrentCost;

            GameObject.FindGameObjectWithTag("Player 1 Mana").gameObject.transform.FindChild("Text").GetComponent<Text>().text = GM.player1.GetComponent<player>().CurrentMana.ToString();
            GameObject.FindGameObjectWithTag("Player 2 Mana").gameObject.transform.FindChild("Text").GetComponent<Text>().text = GM.player2.GetComponent<player>().CurrentMana.ToString();

            // Set the card tag back to "card" 
            draggingCardScript.HasBeenPlaced = true;
            draggingCardGameObject.tag = "Card";
            draggingCardGameObject.transform.FindChild("Outline").GetComponent<Image>().sprite = GM.clear;
        }
        // If neither is valid, card is set back to a card
        else
        {
            Debug.Log("Invalid move!");
            draggingCardGameObject.tag = "Card";
        }
    }

    /// <summary>
    /// Creature attacking logics.
    /// </summary>
    /// <param name="draggingCardGameObject"></param> GameObject of the card being dragged.
    /// <param name="draggingCardScript"></param> Card script of the card being dragged. 
    /// <param name="pointerObject"></param> The object below the dragging card.
    // Logic for a creature with a card below it
    public void TryAttack(GameObject draggingCardGameObject, Card draggingCardScript, GameObject pointerObject)
    {
        // Checks if it is owner's turn
        if (draggingCardScript.OwnerTag.Equals(GM.currentPlayer.GetComponent<player>().Name))
        {
            Debug.Log("Combat conditions met");

            GM.Combat(draggingCardGameObject, pointerObject);
        }
        // If combat is not valid, make the card a card again
        else
        {
            Debug.Log("Combat conditions not met");
            draggingCardGameObject.tag = "Card";
        }
    }

    /// <summary>
    /// Spell use logics.
    /// </summary>
    /// <param name="draggingCardGameObject"></param>
    /// <param name="draggingCardScript"></param>
    /// <param name="pointerObject"></param>
    // Logic for a spell with a card below it
    public void TrySpell(GameObject draggingCardGameObject, Card draggingCardScript, GameObject pointerObject)
    {
        // if the spell is owned by the current player, and it does not exceed that player's mana, then
        //actionCard.ownerTag.Equals(gm.currentPlayer.tag)
        if ((GM.currentPlayer.GetComponent<player>().CurrentMana - draggingCardScript.CurrentCost >= 0))
        {
            Debug.Log("Spell conditions met");
            // Subtract mana cost of the spell from the current player's mana
            GM.currentPlayer.GetComponent<player>().CurrentMana -= draggingCardScript.CurrentCost;

            EF.RunEffect(draggingCardScript.Effect, pointerObject, draggingCardGameObject);

            if (draggingCardScript.Effect == Effect.None)
            {
                draggingCardGameObject.tag = "Card";
            }
        }
        else
        {
            Debug.Log("No spell conditions were met");
            draggingCardGameObject.tag = "Card";
        }    
    }
}

    /// <remarks>
    /// The following are spell method that are deprecated. Dont use!
    /// </remarks>

    /*
    // Basic fireball method. Does damage based on the attack of the spell card played. 
    public void fireball(GameObject targetedCard, GameObject spellCard)
    {
        targetedCard.GetComponent<Card>().currentHealth = targetedCard.GetComponent<Card>().currentHealth - spellCard.GetComponent<Card>().currentAttack;

        DestroyObject(spellCard);

        targetedCard.transform.FindChild("Health").GetComponent<Text>().text = targetedCard.GetComponent<Card>().currentHealth.ToString();

        if (targetedCard.GetComponent<Card>().currentHealth <= 0)
        {
            DestroyObject(targetedCard);
            Debug.Log("the card has died to a fireball!");
        }
    }

    public void heal(GameObject targetedCard, GameObject spellCard)
    {
        targetedCard.GetComponent<Card>().currentHealth = targetedCard.GetComponent<Card>().currentHealth + spellCard.GetComponent<Card>().currentHealth;

        DestroyObject(spellCard);

        targetedCard.transform.FindChild("Health").GetComponent<Text>().text = targetedCard.GetComponent<Card>().currentHealth.ToString();
    }

    
    public void zoneDamage(GameObject targtedCard, GameObject spellCard)
    {
        if (targtedCard.transform.parent.transform.parent.name.Equals("leftTemplePlayingSpaces"))
        {
            if (GameObject.Find("bottomLeft1").transform.Find("cardSmall") != null)
            {
                GameObject.Find("bottomLeft1").transform.Find("cardSmall").GetComponent<Card>().currentHealth = GameObject.Find("bottomLeft1").transform.Find("cardSmall").GetComponent<Card>().currentHealth - spellCard.GetComponent<Card>().currentAttack;
                GameObject.Find("bottomLeft1").transform.Find("cardSmall").transform.FindChild("Health").GetComponent<Text>().text = GameObject.Find("bottomLeft1").transform.Find("cardSmall").GetComponent<Card>().currentHealth.ToString();

            }
            if (GameObject.Find("bottomLeft2").transform.Find("cardSmall") != null)
            {
                GameObject.Find("bottomLeft2").transform.Find("cardSmall").GetComponent<Card>().currentHealth = GameObject.Find("bottomLeft2").transform.Find("cardSmall").GetComponent<Card>().currentHealth - spellCard.GetComponent<Card>().currentAttack;
                GameObject.Find("bottomLeft2").transform.Find("cardSmall").transform.FindChild("Health").GetComponent<Text>().text = GameObject.Find("bottomLeft2").transform.Find("cardSmall").GetComponent<Card>().currentHealth.ToString();

            }
            if (GameObject.Find("bottomLeft3").transform.Find("cardSmall") != null)
            {
                GameObject.Find("bottomLeft3").transform.Find("cardSmall").GetComponent<Card>().currentHealth = GameObject.Find("bottomLeft3").transform.Find("cardSmall").GetComponent<Card>().currentHealth - spellCard.GetComponent<Card>().currentAttack;
                GameObject.Find("bottomLeft3").transform.Find("cardSmall").transform.FindChild("Health").GetComponent<Text>().text = GameObject.Find("bottomLeft3").transform.Find("cardSmall").GetComponent<Card>().currentHealth.ToString();

            }
        }
        if (targtedCard.transform.parent.transform.parent.name.Equals("middleTemplePlayingSpaces"))
        {
            if (GameObject.Find("bottomCenter1").transform.Find("cardSmall") != null)
            {
                GameObject.Find("bottomCenter1").transform.Find("cardSmall").GetComponent<Card>().currentHealth = GameObject.Find("bottomCenter1").transform.Find("cardSmall").GetComponent<Card>().currentHealth - spellCard.GetComponent<Card>().currentAttack;
                GameObject.Find("bottomCenter1").transform.Find("cardSmall").transform.FindChild("Health").GetComponent<Text>().text = GameObject.Find("bottomCenter1").transform.Find("cardSmall").GetComponent<Card>().currentHealth.ToString();

            }
            if (GameObject.Find("bottomCenter2").transform.Find("cardSmall") != null)
            {
                GameObject.Find("bottomCenter2").transform.Find("cardSmall").GetComponent<Card>().currentHealth = GameObject.Find("bottomCenter2").transform.Find("cardSmall").GetComponent<Card>().currentHealth - spellCard.GetComponent<Card>().currentAttack;
                GameObject.Find("bottomCenter2").transform.Find("cardSmall").transform.FindChild("Health").GetComponent<Text>().text = GameObject.Find("bottomCenter2").transform.Find("cardSmall").GetComponent<Card>().currentHealth.ToString();

            }
            if (GameObject.Find("bottomCenter3").transform.Find("cardSmall") != null)
            {
                GameObject.Find("bottomCenter3").transform.Find("cardSmall").GetComponent<Card>().currentHealth = GameObject.Find("bottomCenter3").transform.Find("cardSmall").GetComponent<Card>().currentHealth - spellCard.GetComponent<Card>().currentAttack;
                GameObject.Find("bottomCenter3").transform.Find("cardSmall").transform.FindChild("Health").GetComponent<Text>().text = GameObject.Find("bottomCenter3").transform.Find("cardSmall").GetComponent<Card>().currentHealth.ToString();

            }
        }
        if (targtedCard.transform.parent.transform.parent.name.Equals("rightTemplePlayingSpaces"))
        {
            if (GameObject.Find("topRight1").transform.Find("cardSmall") != null)
            {
                GameObject.Find("topRight1").transform.Find("cardSmall").GetComponent<Card>().currentHealth = GameObject.Find("topRight1").transform.Find("cardSmall").GetComponent<Card>().currentHealth - spellCard.GetComponent<Card>().currentAttack;
                GameObject.Find("topRight1").transform.Find("cardSmall").transform.FindChild("Health").GetComponent<Text>().text = GameObject.Find("topRight1").transform.Find("cardSmall").GetComponent<Card>().currentHealth.ToString();

            }
            if (GameObject.Find("topRight2").transform.Find("cardSmall") != null)
            {
                GameObject.Find("topRight2").transform.Find("cardSmall").GetComponent<Card>().currentHealth = GameObject.Find("topRight2").transform.Find("cardSmall").GetComponent<Card>().currentHealth - spellCard.GetComponent<Card>().currentAttack;
                GameObject.Find("topRight2").transform.Find("cardSmall").transform.FindChild("Health").GetComponent<Text>().text = GameObject.Find("topRight2").transform.Find("cardSmall").GetComponent<Card>().currentHealth.ToString();

            }
            if (GameObject.Find("topRight3").transform.Find("cardSmall") != null)
            {
                GameObject.Find("topRight3").transform.Find("cardSmall").GetComponent<Card>().currentHealth = GameObject.Find("topRight3").transform.Find("cardSmall").GetComponent<Card>().currentHealth - spellCard.GetComponent<Card>().currentAttack;
                GameObject.Find("topRight3").transform.Find("cardSmall").transform.FindChild("Health").GetComponent<Text>().text = GameObject.Find("topRight3").transform.Find("cardSmall").GetComponent<Card>().currentHealth.ToString();

            }
        }
        if (targtedCard.transform.parent.transform.parent.name.Equals("leftCitadelPlayingSpaces"))
        {
            if (GameObject.Find("topLeft1").transform.Find("cardSmall") != null)
            {
                GameObject.Find("topLeft1").transform.Find("cardSmall").GetComponent<Card>().currentHealth = GameObject.Find("topLeft1").transform.Find("cardSmall").GetComponent<Card>().currentHealth - spellCard.GetComponent<Card>().currentAttack;
                GameObject.Find("topLeft1").transform.Find("cardSmall").transform.FindChild("Health").GetComponent<Text>().text = GameObject.Find("topLeft1").transform.Find("cardSmall").GetComponent<Card>().currentHealth.ToString();

            }
            if (GameObject.Find("topLeft2").transform.Find("cardSmall") != null)
            {
                GameObject.Find("topLeft2").transform.Find("cardSmall").GetComponent<Card>().currentHealth = GameObject.Find("topLeft2").transform.Find("cardSmall").GetComponent<Card>().currentHealth - spellCard.GetComponent<Card>().currentAttack;
                GameObject.Find("topLeft2").transform.Find("cardSmall").transform.FindChild("Health").GetComponent<Text>().text = GameObject.Find("topLeft2").transform.Find("cardSmall").GetComponent<Card>().currentHealth.ToString();

            }
            if (GameObject.Find("topLeft3").transform.Find("cardSmall") != null)
            {
                GameObject.Find("topLeft3").transform.Find("cardSmall").GetComponent<Card>().currentHealth = GameObject.Find("topLeft3").transform.Find("cardSmall").GetComponent<Card>().currentHealth - spellCard.GetComponent<Card>().currentAttack;
                GameObject.Find("topLeft3").transform.Find("cardSmall").transform.FindChild("Health").GetComponent<Text>().text = GameObject.Find("topLeft3").transform.Find("cardSmall").GetComponent<Card>().currentHealth.ToString();

            }
        }
        if (targtedCard.transform.parent.transform.parent.name.Equals("middleCitadelPlayingSpaces"))
        {
            if (GameObject.Find("topMiddle1").transform.Find("cardSmall") != null)
            {
                GameObject.Find("topMiddle1").transform.Find("cardSmall").GetComponent<Card>().currentHealth = GameObject.Find("top Middle1").transform.Find("cardSmall").GetComponent<Card>().currentHealth - spellCard.GetComponent<Card>().currentAttack;
                GameObject.Find("to Middle1").transform.Find("cardSmall").transform.FindChild("Health").GetComponent<Text>().text = GameObject.Find("top Middle1").transform.Find("cardSmall").GetComponent<Card>().currentHealth.ToString();

            }
            if (GameObject.Find("topMiddle2").transform.Find("cardSmall") != null)
            {
                GameObject.Find("topMiddle2").transform.Find("cardSmall").GetComponent<Card>().currentHealth = GameObject.Find("top Middle2").transform.Find("cardSmall").GetComponent<Card>().currentHealth - spellCard.GetComponent<Card>().currentAttack;
                GameObject.Find("topMiddle2").transform.Find("cardSmall").transform.FindChild("Health").GetComponent<Text>().text = GameObject.Find("top Middle2").transform.Find("cardSmall").GetComponent<Card>().currentHealth.ToString();

            }
            if (GameObject.Find("topMiddle3").transform.Find("cardSmall") != null)
            {
                GameObject.Find("topMiddle3").transform.Find("cardSmall").GetComponent<Card>().currentHealth = GameObject.Find("top Middle3").transform.Find("cardSmall").GetComponent<Card>().currentHealth - spellCard.GetComponent<Card>().currentAttack;
                GameObject.Find("topMiddle3").transform.Find("cardSmall").transform.FindChild("Health").GetComponent<Text>().text = GameObject.Find("top Middle3").transform.Find("cardSmall").GetComponent<Card>().currentHealth.ToString();

            }
        }
        if (targtedCard.transform.parent.transform.parent.name.Equals("rightCitadelPlayingSpaces"))
        {
            if (GameObject.Find("topRight1").transform.Find("cardSmall") != null)
            {
                GameObject.Find("topRight1").transform.Find("cardSmall").GetComponent<Card>().currentHealth = GameObject.Find("topRight1").transform.Find("cardSmall").GetComponent<Card>().currentHealth - spellCard.GetComponent<Card>().currentAttack;
                GameObject.Find("topRight1").transform.Find("cardSmall").transform.FindChild("Health").GetComponent<Text>().text = GameObject.Find("topRight1").transform.Find("cardSmall").GetComponent<Card>().currentHealth.ToString();

            }
            if (GameObject.Find("topRight2").transform.Find("cardSmall") != null)
            {
                GameObject.Find("topRight2").transform.Find("cardSmall").GetComponent<Card>().currentHealth = GameObject.Find("topRight2").transform.Find("cardSmall").GetComponent<Card>().currentHealth - spellCard.GetComponent<Card>().currentAttack;
                GameObject.Find("topRight2").transform.Find("cardSmall").transform.FindChild("Health").GetComponent<Text>().text = GameObject.Find("topRight2").transform.Find("cardSmall").GetComponent<Card>().currentHealth.ToString();
            }
            if (GameObject.Find("topRight3").transform.Find("cardSmall") != null)
            {
                GameObject.Find("topRight3").transform.Find("cardSmall").GetComponent<Card>().currentHealth = GameObject.Find("topRight3").transform.Find("cardSmall").GetComponent<Card>().currentHealth - spellCard.GetComponent<Card>().currentAttack;
                GameObject.Find("topRight3").transform.Find("cardSmall").transform.FindChild("Health").GetComponent<Text>().text = GameObject.Find("topRight3").transform.Find("cardSmall").GetComponent<Card>().currentHealth.ToString();
            }
        }

        Destroy(spellCard);

    }

    public void draw(GameObject spellCard)
    {
       
        if (spellCard.GetComponent<Card>().battleSide == 0)
        {
            for (int i = 0; i < spellCard.GetComponent<Card>().currentCost; i++)
            {
                GM.drawCard(GameObject.Find("Player1"));
            }
        }

        if (spellCard.GetComponent<Card>().battleSide == 1)
        {
            for (int i = 0; i < spellCard.GetComponent<Card>().currentCost; i++)
            {
                GM.drawCard(GameObject.Find("Player2"));
            }
        }
    }
    */

