using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

/// <summary>
/// Class that controls all effects YO.
/// </summary>
public class EffectManager : MonoBehaviour {

    private GM gm;
    private Dictionary<Effect, spellDelegate> effectMap;
    private delegate void spellDelegate(GameObject targeted, GameObject targeter);

    void Start()
    {
        this.gm = GameObject.Find("GM").GetComponent<GM>();

        this.effectMap = new Dictionary<Effect, spellDelegate>();

        this.effectMap.Add(Effect.Fireball, new spellDelegate(Fireball));
        this.effectMap.Add(Effect.Heal, new spellDelegate(Heal));
        this.effectMap.Add(Effect.Firestorm, new spellDelegate(Firestorm));
        this.effectMap.Add(Effect.Draw, new spellDelegate(Draw));
    } 

    /// <summary>
    /// Run the effect of the given key. 
    /// </summary>
    /// <param name="key"></param> The key of the desired effect.
    /// <param name="targeted"></param> The targeted card (type GameObject).
    /// <param name="targeter"></param> The targeting card (type GameObject).
    public void RunEffect(Effect key, GameObject targeted, GameObject targeter)
    {
        this.effectMap[key].DynamicInvoke(targeted, targeter);
    }

    /// <summary>
    /// Basic fireball method. Does damage based on the attack of the spell card played. 
    /// </summary>
    /// <param name="targetedCard"></param>
    /// <param name="spellCard"></param>
    private static void Fireball(GameObject targetedCard, GameObject spellCard)
    {
        targetedCard.GetComponent<Card>().CurrentHealth -= 2;

        DestroyObject(spellCard);

        targetedCard.transform.FindChild("Health").GetComponent<Text>().text = targetedCard.GetComponent<Card>().CurrentHealth.ToString();
        targetedCard.transform.FindChild("Health").GetComponent<Text>().color = Color.red;

        if (targetedCard.GetComponent<Card>().CurrentHealth <= 0)
        {
            DestroyObject(targetedCard);
            //Debug.Log("The card has died to a fireball!");
        }
    }

    /// <summary>
    /// Heal the targeted card 2 hp.
    /// </summary>
    /// <param name="targetedCard"></param>
    /// <param name="spellCard"></param>
    private static void Heal(GameObject targetedCard, GameObject spellCard)
    {
        targetedCard.GetComponent<Card>().CurrentHealth += 2;
        targetedCard.transform.FindChild("Health").GetComponent<Text>().text = targetedCard.GetComponent<Card>().CurrentHealth.ToString();
        targetedCard.transform.FindChild("Health").GetComponent<Text>().color = Color.green;

        //Debug.Log("Targeted card was healed");
        DestroyObject(spellCard);     
    }

    /// <summary>
    /// Does 3 damage to all of the cards in the targeted zone.
    /// </summary>
    /// <param name="targtedCard"></param>
    /// <param name="spellCard"></param>
    private static void Firestorm(GameObject targtedCard, GameObject spellCard)
    {
        if (targtedCard.transform.parent.transform.parent.name.Equals("leftTemplePlayingSpaces"))
        {
            if (GameObject.Find("bottomLeft1").transform.Find("cardSmall") != null)
            {
                GameObject.Find("bottomLeft1").transform.Find("cardSmall").GetComponent<Card>().CurrentHealth = GameObject.Find("bottomLeft1").transform.Find("cardSmall").GetComponent<Card>().CurrentHealth - spellCard.GetComponent<Card>().CurrentAttack;
                GameObject.Find("bottomLeft1").transform.Find("cardSmall").transform.FindChild("Health").GetComponent<Text>().text = GameObject.Find("bottomLeft1").transform.Find("cardSmall").GetComponent<Card>().CurrentHealth.ToString();

            }
            if (GameObject.Find("bottomLeft2").transform.Find("cardSmall") != null)
            {
                GameObject.Find("bottomLeft2").transform.Find("cardSmall").GetComponent<Card>().CurrentHealth = GameObject.Find("bottomLeft2").transform.Find("cardSmall").GetComponent<Card>().CurrentHealth - spellCard.GetComponent<Card>().CurrentAttack;
                GameObject.Find("bottomLeft2").transform.Find("cardSmall").transform.FindChild("Health").GetComponent<Text>().text = GameObject.Find("bottomLeft2").transform.Find("cardSmall").GetComponent<Card>().CurrentHealth.ToString();

            }
            if (GameObject.Find("bottomLeft3").transform.Find("cardSmall") != null)
            {
                GameObject.Find("bottomLeft3").transform.Find("cardSmall").GetComponent<Card>().CurrentHealth = GameObject.Find("bottomLeft3").transform.Find("cardSmall").GetComponent<Card>().CurrentHealth - spellCard.GetComponent<Card>().CurrentAttack;
                GameObject.Find("bottomLeft3").transform.Find("cardSmall").transform.FindChild("Health").GetComponent<Text>().text = GameObject.Find("bottomLeft3").transform.Find("cardSmall").GetComponent<Card>().CurrentHealth.ToString();

            }
        }
        if (targtedCard.transform.parent.transform.parent.name.Equals("middleTemplePlayingSpaces"))
        {
            if (GameObject.Find("bottomCenter1").transform.Find("cardSmall") != null)
            {
                GameObject.Find("bottomCenter1").transform.Find("cardSmall").GetComponent<Card>().CurrentHealth = GameObject.Find("bottomCenter1").transform.Find("cardSmall").GetComponent<Card>().CurrentHealth - spellCard.GetComponent<Card>().CurrentAttack;
                GameObject.Find("bottomCenter1").transform.Find("cardSmall").transform.FindChild("Health").GetComponent<Text>().text = GameObject.Find("bottomCenter1").transform.Find("cardSmall").GetComponent<Card>().CurrentHealth.ToString();

            }
            if (GameObject.Find("bottomCenter2").transform.Find("cardSmall") != null)
            {
                GameObject.Find("bottomCenter2").transform.Find("cardSmall").GetComponent<Card>().CurrentHealth = GameObject.Find("bottomCenter2").transform.Find("cardSmall").GetComponent<Card>().CurrentHealth - spellCard.GetComponent<Card>().CurrentAttack;
                GameObject.Find("bottomCenter2").transform.Find("cardSmall").transform.FindChild("Health").GetComponent<Text>().text = GameObject.Find("bottomCenter2").transform.Find("cardSmall").GetComponent<Card>().CurrentHealth.ToString();

            }
            if (GameObject.Find("bottomCenter3").transform.Find("cardSmall") != null)
            {
                GameObject.Find("bottomCenter3").transform.Find("cardSmall").GetComponent<Card>().CurrentHealth = GameObject.Find("bottomCenter3").transform.Find("cardSmall").GetComponent<Card>().CurrentHealth - spellCard.GetComponent<Card>().CurrentAttack;
                GameObject.Find("bottomCenter3").transform.Find("cardSmall").transform.FindChild("Health").GetComponent<Text>().text = GameObject.Find("bottomCenter3").transform.Find("cardSmall").GetComponent<Card>().CurrentHealth.ToString();

            }
        }
        if (targtedCard.transform.parent.transform.parent.name.Equals("rightTemplePlayingSpaces"))
        {
            if (GameObject.Find("topRight1").transform.Find("cardSmall") != null)
            {
                GameObject.Find("topRight1").transform.Find("cardSmall").GetComponent<Card>().CurrentHealth = GameObject.Find("topRight1").transform.Find("cardSmall").GetComponent<Card>().CurrentHealth - spellCard.GetComponent<Card>().CurrentAttack;
                GameObject.Find("topRight1").transform.Find("cardSmall").transform.FindChild("Health").GetComponent<Text>().text = GameObject.Find("topRight1").transform.Find("cardSmall").GetComponent<Card>().CurrentHealth.ToString();

            }
            if (GameObject.Find("topRight2").transform.Find("cardSmall") != null)
            {
                GameObject.Find("topRight2").transform.Find("cardSmall").GetComponent<Card>().CurrentHealth = GameObject.Find("topRight2").transform.Find("cardSmall").GetComponent<Card>().CurrentHealth - spellCard.GetComponent<Card>().CurrentAttack;
                GameObject.Find("topRight2").transform.Find("cardSmall").transform.FindChild("Health").GetComponent<Text>().text = GameObject.Find("topRight2").transform.Find("cardSmall").GetComponent<Card>().CurrentHealth.ToString();

            }
            if (GameObject.Find("topRight3").transform.Find("cardSmall") != null)
            {
                GameObject.Find("topRight3").transform.Find("cardSmall").GetComponent<Card>().CurrentHealth = GameObject.Find("topRight3").transform.Find("cardSmall").GetComponent<Card>().CurrentHealth - spellCard.GetComponent<Card>().CurrentAttack;
                GameObject.Find("topRight3").transform.Find("cardSmall").transform.FindChild("Health").GetComponent<Text>().text = GameObject.Find("topRight3").transform.Find("cardSmall").GetComponent<Card>().CurrentHealth.ToString();

            }
        }
        if (targtedCard.transform.parent.transform.parent.name.Equals("leftCitadelPlayingSpaces"))
        {
            if (GameObject.Find("topLeft1").transform.Find("cardSmall") != null)
            {
                GameObject.Find("topLeft1").transform.Find("cardSmall").GetComponent<Card>().CurrentHealth = GameObject.Find("topLeft1").transform.Find("cardSmall").GetComponent<Card>().CurrentHealth - spellCard.GetComponent<Card>().CurrentAttack;
                GameObject.Find("topLeft1").transform.Find("cardSmall").transform.FindChild("Health").GetComponent<Text>().text = GameObject.Find("topLeft1").transform.Find("cardSmall").GetComponent<Card>().CurrentHealth.ToString();

            }
            if (GameObject.Find("topLeft2").transform.Find("cardSmall") != null)
            {
                GameObject.Find("topLeft2").transform.Find("cardSmall").GetComponent<Card>().CurrentHealth = GameObject.Find("topLeft2").transform.Find("cardSmall").GetComponent<Card>().CurrentHealth - spellCard.GetComponent<Card>().CurrentAttack;
                GameObject.Find("topLeft2").transform.Find("cardSmall").transform.FindChild("Health").GetComponent<Text>().text = GameObject.Find("topLeft2").transform.Find("cardSmall").GetComponent<Card>().CurrentHealth.ToString();

            }
            if (GameObject.Find("topLeft3").transform.Find("cardSmall") != null)
            {
                GameObject.Find("topLeft3").transform.Find("cardSmall").GetComponent<Card>().CurrentHealth = GameObject.Find("topLeft3").transform.Find("cardSmall").GetComponent<Card>().CurrentHealth - spellCard.GetComponent<Card>().CurrentAttack;
                GameObject.Find("topLeft3").transform.Find("cardSmall").transform.FindChild("Health").GetComponent<Text>().text = GameObject.Find("topLeft3").transform.Find("cardSmall").GetComponent<Card>().CurrentHealth.ToString();

            }
        }
        if (targtedCard.transform.parent.transform.parent.name.Equals("middleCitadelPlayingSpaces"))
        {
            if (GameObject.Find("topMiddle1").transform.Find("cardSmall") != null)
            {
                GameObject.Find("topMiddle1").transform.Find("cardSmall").GetComponent<Card>().CurrentHealth = GameObject.Find("top Middle1").transform.Find("cardSmall").GetComponent<Card>().CurrentHealth - spellCard.GetComponent<Card>().CurrentAttack;
                GameObject.Find("to Middle1").transform.Find("cardSmall").transform.FindChild("Health").GetComponent<Text>().text = GameObject.Find("top Middle1").transform.Find("cardSmall").GetComponent<Card>().CurrentHealth.ToString();

            }
            if (GameObject.Find("topMiddle2").transform.Find("cardSmall") != null)
            {
                GameObject.Find("topMiddle2").transform.Find("cardSmall").GetComponent<Card>().CurrentHealth = GameObject.Find("top Middle2").transform.Find("cardSmall").GetComponent<Card>().CurrentHealth - spellCard.GetComponent<Card>().CurrentAttack;
                GameObject.Find("topMiddle2").transform.Find("cardSmall").transform.FindChild("Health").GetComponent<Text>().text = GameObject.Find("top Middle2").transform.Find("cardSmall").GetComponent<Card>().CurrentHealth.ToString();

            }
            if (GameObject.Find("topMiddle3").transform.Find("cardSmall") != null)
            {
                GameObject.Find("topMiddle3").transform.Find("cardSmall").GetComponent<Card>().CurrentHealth = GameObject.Find("top Middle3").transform.Find("cardSmall").GetComponent<Card>().CurrentHealth - spellCard.GetComponent<Card>().CurrentAttack;
                GameObject.Find("topMiddle3").transform.Find("cardSmall").transform.FindChild("Health").GetComponent<Text>().text = GameObject.Find("top Middle3").transform.Find("cardSmall").GetComponent<Card>().CurrentHealth.ToString();

            }
        }
        if (targtedCard.transform.parent.transform.parent.name.Equals("rightCitadelPlayingSpaces"))
        {
            if (GameObject.Find("topRight1").transform.Find("cardSmall") != null)
            {
                GameObject.Find("topRight1").transform.Find("cardSmall").GetComponent<Card>().CurrentHealth = GameObject.Find("topRight1").transform.Find("cardSmall").GetComponent<Card>().CurrentHealth - spellCard.GetComponent<Card>().CurrentAttack;
                GameObject.Find("topRight1").transform.Find("cardSmall").transform.FindChild("Health").GetComponent<Text>().text = GameObject.Find("topRight1").transform.Find("cardSmall").GetComponent<Card>().CurrentHealth.ToString();

            }
            if (GameObject.Find("topRight2").transform.Find("cardSmall") != null)
            {
                GameObject.Find("topRight2").transform.Find("cardSmall").GetComponent<Card>().CurrentHealth = GameObject.Find("topRight2").transform.Find("cardSmall").GetComponent<Card>().CurrentHealth - spellCard.GetComponent<Card>().CurrentAttack;
                GameObject.Find("topRight2").transform.Find("cardSmall").transform.FindChild("Health").GetComponent<Text>().text = GameObject.Find("topRight2").transform.Find("cardSmall").GetComponent<Card>().CurrentHealth.ToString();
            }
            if (GameObject.Find("topRight3").transform.Find("cardSmall") != null)
            {
                GameObject.Find("topRight3").transform.Find("cardSmall").GetComponent<Card>().CurrentHealth = GameObject.Find("topRight3").transform.Find("cardSmall").GetComponent<Card>().CurrentHealth - spellCard.GetComponent<Card>().CurrentAttack;
                GameObject.Find("topRight3").transform.Find("cardSmall").transform.FindChild("Health").GetComponent<Text>().text = GameObject.Find("topRight3").transform.Find("cardSmall").GetComponent<Card>().CurrentHealth.ToString();
            }
        }

        Destroy(spellCard);

    }

    /// <summary>
    /// Draw 2 cards for a player.
    /// </summary>
    /// <param name="targetedGameobject"></param>
    /// <param name="spellCard"></param>
    private void Draw(GameObject targetedGameobject, GameObject spellCard)
    {
        gm.DrawCard(gm.currentPlayer, 2);

        Destroy(spellCard);
    }
}
