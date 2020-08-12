using UnityEngine;
using System;

/// <summary>
/// Holds the card info.
/// </summary>
	public class Card : MonoBehaviour {

    // Base values
    public string CardName { get; set; }
    public string ID { get; set; }
    public string Image { get; set; }
    public string Description { get; set; }
    public string Alliance { get; set; }
    public string Type { get; set; }
    public string Cost { get; set; }
    public string Attack { get; set; }
    public string Health { get; set; }
    public string Defense { get; set; }
    public string Range { get; set; }
    public string TargetName { get; set; }
    public string EffectName { get; set; }
    //public string effect;

    // Values for combat 
    public int CurrentID { get; set; }
    public int CurrentCost { get; set; }
    public double CurrentAttack { get; set; }
    public double CurrentHealth { get; set; }
    public int BattleSide { get; set; }
    public double CurrentDefense { get; set; }
    public string CurrentRange { get; set; }
    public bool IsExhausted { get; set; }
    public bool HasBeenPlaced { get; set; }
    public string OwnerTag { get; set; }

    //Values for Deckbuilder
    public int Count { get; set; }

    // Spell information
    public Effect Effect { get; set; }
    public Effect Target { get; set; }

    public Card clone(Card card)
    {
        return(Card)this.MemberwiseClone();
    }

    public void SetCurrents() {

		//sets the combat values to the base as the object is initialized
		CurrentID = Int32.Parse (ID);
		CurrentCost = Int32.Parse (Cost);
		CurrentAttack = Convert.ToDouble (Attack);
		CurrentHealth = Convert.ToDouble (Health);
		CurrentDefense = Convert.ToDouble (Defense);
		CurrentRange = Range;
	}


}




