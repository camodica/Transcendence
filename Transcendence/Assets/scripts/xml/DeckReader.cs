using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using UnityEngine;

public class DeckReader: MonoBehaviour {

    //arr of card objects
    public List<Card> arr;

    //the path of the xml card data you want to load
    public string path;

    /// <summary>
    /// Load the card data from the xml file, and return the database as an ArrayList of Cards
    /// </summary>
    /// <param name="inputPath"></param> the path of the desired deck
    /// <returns = arr></returns> the deck returned
    public List<Card> load(string inputPath)
    {
        //follows the path of the xml, and loads it into a reader
        arr = new List<Card>();
        path = inputPath;
        XmlReader reader = XmlReader.Create(path);
       
        while(reader.Read())
        {
            //should check to see if the line is an element, and then if it is called 'card'
            if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "card"))
             {
                //generates new empty card object
                #pragma warning disable
                Card card = new Card();

                    //set values of new card Object from xml attributes
                    card.CardName = reader.GetAttribute("name");
                    card.ID = reader.GetAttribute("ID");
                    //card.image = reader.GetAttribute("image");
                    card.Description = reader.GetAttribute("description");
                    card.Alliance = reader.GetAttribute("alliance");
                    card.Type = reader.GetAttribute("type");
                    card.Cost = reader.GetAttribute("cost");
                    card.Attack = reader.GetAttribute("attack");
                    card.Defense = reader.GetAttribute("defense");
                    card.Range = reader.GetAttribute("range");
                    card.TargetName = reader.GetAttribute("target");
                    card.Health = reader.GetAttribute("health");
                    card.EffectName = reader.GetAttribute("effectName");
                    if (!(reader.GetAttribute("count") == null))
                    {
                    card.Count = Int32.Parse(reader.GetAttribute("count"));
                    }
                    
                //stores that new card in the DeckReader array list
                arr.Add(card);
            }
        }
        //returns the card arr after all cards have been added (PLS)
        return arr;
    }

    public List<Card> load(TextAsset stream)
    {
        arr = new List<Card>();

        XmlReader reader = XmlReader.Create(new StreamReader(stream.text));

        while (reader.Read())
        {
            //should check to see if the line is an element, and then if it is called 'card'
            if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "card"))
            {
                //generates new empty card object
                #pragma warning disable
                Card card = new Card();

                //set values of new card Object from xml attributes
                card.CardName = reader.GetAttribute("name");
                card.ID = reader.GetAttribute("ID");
                //card.image = reader.GetAttribute("image");
                card.Description = reader.GetAttribute("description");
                card.Alliance = reader.GetAttribute("alliance");
                card.Type = reader.GetAttribute("type");
                card.Cost = reader.GetAttribute("cost");
                card.Attack = reader.GetAttribute("attack");
                card.Defense = reader.GetAttribute("defense");
                card.Range = reader.GetAttribute("range");
                card.TargetName = reader.GetAttribute("target");
                card.Health = reader.GetAttribute("health");
                card.EffectName = reader.GetAttribute("effectName");
                if (!(reader.GetAttribute("count") == null))
                {
                    card.Count = Int32.Parse(reader.GetAttribute("count"));
                }

                //stores that new card in the DeckReader array list
                arr.Add(card);
            }
        }
        return arr;
    }

    public String GetDeckName(string path)
    {
        String name = "";
        this.path = path;

        XmlReader reader = XmlReader.Create(path);

        while (reader.Read())
        {
            //should check to see if the line is an element, and then if it is called 'card'
            if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "name"))
            {
                name = reader.GetAttribute("name");
            }
        }

        if(name.Equals(""))
        {
            name = "Null";
        }

        return name;
    }

    public String GetDeckName(TextAsset stream)
    {
        String name = "";

        XmlReader reader = XmlReader.Create(new StreamReader(stream.text));

        while (reader.Read())
        {
            //should check to see if the line is an element, and then if it is called 'card'
            if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "name"))
            {
                name = reader.GetAttribute("name");
            }
        }

        if (name.Equals(""))
        {
            name = "Null";
        }

        return name;
    }
}
