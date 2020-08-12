using UnityEngine;
using System.Collections;

public class ConstructDeck : Deck {
    private string datetimeCreation;
    private string datetimeLastSave;
    private string savePath;
    private string format; //Standard, No Restrictions, etc. //TODO: Implement format bar in scene
    private string creator; 

    public ConstructDeck() : base()
    {
    }
    /// <summary>
    /// writes current deck state and relevant creation info to xml file at specified path
    /// </summary>
	void OutputStateToFile() 
    {

    }

    /// <summary>
    /// sets current deck state and relevant creation info to xml file at specified path, creator will be overwritten on alteration and format/datetimes/path can be altered
    /// </summary>
    void SetStateFromFile()
    {

        
    }

    void removeAllCopiesOfCard(Card c)
    {

    }




}
