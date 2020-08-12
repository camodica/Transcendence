using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class isDropableSurface : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {

    // Use this for initialization
    /*
    use a tag for the dragging card, set when the card starts dragging
    use findTag to get the GameObject of the card that is dragging to make sure that combat conditions and 
    all logic in this class has been moved to drop manager, and isDraggable (more appropriate places)
    */

    //private GM gm;
    //private dropManager dm;

    public void OnDrop(PointerEventData data)
    {
        /*
        dm = GameObject.Find("GM").GetComponent<dropManager>();

        GameObject draggingCard = GameObject.FindGameObjectWithTag("Dragging");
        GameObject pointerObject = data.pointerCurrentRaycast.gameObject;

        dm.drop(data, draggingCard, pointerObject.name);
        */
        /*
        // The location data of the thing underneath the card when it drops
        dm = GameObject.Find("GM").GetComponent<dropManager>();
        isDraggable d = data.pointerDrag.GetComponent<isDraggable>();

        // Checks to see if the card is a card and the thing below it is a field
        if (data.pointerCurrentRaycast.gameObject.CompareTag("Field") 
            && GameObject.FindGameObjectWithTag("Dragging").GetComponent<Card>().hasBeenPlaced == false 
            && GameObject.FindGameObjectWithTag("Dragging").GetComponent<Card>().type.Equals("Creature"))
        {

            // Runs card placement for player 1
            if ((data.pointerCurrentRaycast.gameObject.GetComponent<Field>().side.Equals("Player 1")) && gm.player1.GetComponent<player>().isTurn == true 
                && (GameObject.FindGameObjectWithTag("Dragging").GetComponent<Card>().ownerTag.Equals("Player 1")) 
                && (gm.player1.GetComponent<player>().currentMana - GameObject.FindGameObjectWithTag("Dragging").GetComponent<Card>().currentCost >= 0))
            {
                // Send card to its new field

                d.parentToReturnTo = this.transform;

                gm.player1.GetComponent<player>().currentMana = gm.player1.GetComponent<player>().currentMana - GameObject.FindGameObjectWithTag("Dragging").GetComponent<Card>().currentCost;              

                GameObject.FindGameObjectWithTag("Player 1 Mana").gameObject.transform.FindChild("Text").GetComponent<Text>().text = gm.player1.GetComponent<player>().currentMana.ToString();
           
                // Set the card tag back to "card" 
                GameObject.FindGameObjectWithTag("Dragging").GetComponent<Card>().hasBeenPlaced = true;
                GameObject.FindGameObjectWithTag("Dragging").tag = "Card";
            }
            // Runs card placement for player 2
            else if ((data.pointerCurrentRaycast.gameObject.GetComponent<Field>().side.Equals("Player 2")) && gm.player2.GetComponent<player>().isTurn == true && (GameObject.FindGameObjectWithTag("Dragging").GetComponent<Card>().ownerTag.Equals("Player 2")) && (gm.player2.GetComponent<player>().currentMana - GameObject.FindGameObjectWithTag("Dragging").GetComponent<Card>().currentCost >= 0))
            {
                // Send card to its new field

                d.parentToReturnTo = this.transform;

                gm.player2.GetComponent<player>().currentMana = gm.player2.GetComponent<player>().currentMana - GameObject.FindGameObjectWithTag("Dragging").GetComponent<Card>().currentCost;

                GameObject.FindGameObjectWithTag("Player 2 Mana").gameObject.transform.FindChild("Text").GetComponent<Text>().text = gm.player2.GetComponent<player>().currentMana.ToString();

                // Set the card tag back to "card" 
                GameObject.FindGameObjectWithTag("Dragging").GetComponent<Card>().hasBeenPlaced = true;
                GameObject.FindGameObjectWithTag("Dragging").tag = "Card";
            }
            // If neither is valid, card is set back to a card
            else
            {
                Debug.Log("Invalid move!");
                GameObject.FindGameObjectWithTag("Dragging").tag = "Card";
            }
        }      

        // Checks to see if the card is a card and the thing below it is a card
        else if (data.pointerCurrentRaycast.gameObject.CompareTag("Card") 
            && (GameObject.FindGameObjectWithTag("Dragging").GetComponent<Card>().isExhausted == false) 
            && (GameObject.FindGameObjectWithTag("Dragging").GetComponent<Card>().hasBeenPlaced == true) 
            && (GameObject.FindGameObjectWithTag("Dragging").GetComponent<Card>().ownerTag.Equals(data.pointerCurrentRaycast.gameObject.GetComponent<Card>().ownerTag) == false)
            && (GameObject.FindGameObjectWithTag("Dragging").GetComponent<Card>().type.Equals("Creature")))    
        {

            // Checks to see if it is the player's turn and they own the card
            if (gm.player1.GetComponent<player>().isTurn && GameObject.FindGameObjectWithTag("Dragging").GetComponent<Card>().ownerTag.Equals("Player 1"))
            {
                // Runs combat for the cards
                GM.combat(GameObject.FindGameObjectWithTag("Dragging"), data.pointerCurrentRaycast.gameObject);
            }
            // Checks to see if it is the player's turn and they own the card
            else if (gm.player2.GetComponent<player>().isTurn && GameObject.FindGameObjectWithTag("Dragging").GetComponent<Card>().ownerTag.Equals("Player 2"))
            {
                // Runs combat for the cards
                GM.combat(GameObject.FindGameObjectWithTag("Dragging"), data.pointerCurrentRaycast.gameObject);
            }
            // If combat is not valid, make the card a card again
            else
            {
                GameObject.FindGameObjectWithTag("Dragging").tag = "Card";
            }

        }

        else if (data.pointerCurrentRaycast.gameObject.CompareTag("Card")
            && (GameObject.FindGameObjectWithTag("Dragging").GetComponent<Card>().type.Equals("Spell")))
        {

            if (GameObject.FindGameObjectWithTag("Dragging").GetComponent<Card>().effect.Equals("fireball"))
            {
                if (!data.pointerCurrentRaycast.gameObject.Equals(null))
                {

                }
                else
                {
                    GameObject.FindGameObjectWithTag("Dragging").tag = "Card";
                }
            }

            if (GameObject.FindGameObjectWithTag("Dragging").GetComponent<Card>().effect.Equals("heal"))
            {
                if (!data.pointerCurrentRaycast.gameObject.Equals(null))
                {
                }
                else
                {
                    GameObject.FindGameObjectWithTag("Dragging").tag = "Card";
                }
            }

        }
        // If combat is not valid, make the card a card again
        else
        {
            GameObject.FindGameObjectWithTag("Dragging").tag = "Card";
        }
        */

    }
	public void OnPointerEnter(PointerEventData data)
    {

    }
    public void OnPointerExit(PointerEventData data)
    {

    }
	
}
