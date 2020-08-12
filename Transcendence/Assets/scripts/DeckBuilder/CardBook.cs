using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;
using System.Collections;

[System.Serializable]
public class CardBook : MonoBehaviour
{

    //Reader
    private string Path { get; set; } //path of full database xml
    private List<Card> CardList { get; set; } //full list of all cards from XML
    private List<List<Card>> CardPages { get; set; } //book of cards organized by pages of ten
    public int CurrentPage; //keeps track of location in book
    private const int CARDS_PER_PAGE = 10; //Number of cards per page, must correspond with displayed slots in scene 
    private int NumPages;
    private bool Compiled { get; set; }

    public CardBook (String path)
    {
        Debug.Log("TRIGGER1");
        LoadDatabaseFromFile(path);
    }

    void Start()
	{
        Compiled = false;
        //load all cards in database by path with reader
    }

    public void LoadDatabaseFromFile(String path)
    {
        Debug.Log("TRIGGER:LoadDatabaseFromFile");
        Path = path;
        DeckReader reader = new DeckReader();
        if (File.Exists(Path))
        {
            CardList = reader.load(Path);
            Debug.Log("TRIGGER:CardListLoad");
        }
        else
        {
            Debug.LogError("Error: File not Found");
        }
        Debug.Log(CardList[0].CardName + " is held by CardList @ pos0");
        CompileBook();
    }

    public void CompileBook()
    {
        int numCards = 0; //TEST
        CardPages = new List<List<Card>>();
        NumPages = CardList.Count / 10;
        if (CardList.Count % 10 != 0) //If there are cards remaining, add another page
        {
            NumPages++;
        }
        Debug.Log(NumPages + "pgs");
        for (int Page = 0; Page < NumPages; Page++)
            {
            CardPages.Add(new List<Card>());
            Debug.Log("PageIteration:" + Page);
            for (int i = 0; i < CARDS_PER_PAGE; i++)
            {
                numCards++; //TEST
                Debug.Log("CardIteration:" + i);
                //if (CardList[i + (10 * Page)] != null)
                //{
                int element = i + (Page * 10);
                Debug.Log("CardList element of attempted access: " + element);
                Debug.Log("CardList element of attempted access name: " + CardList[element].CardName);
                CardPages[Page].Insert(i, CardList[element]);
                //}
            }
            }
        Compiled = true;
        Debug.LogError("There are {" + numCards + "} unique cards in database"); //TEST
        Debug.Log("FreeFromPagationLoop");
    }

    public List<Card> GetCardsOfPage(int PageRequested) //returns 10 cards for display later from passed page
    {
        Debug.Log("Attempting access of Page:" + PageRequested);
        if (PageRequested > CardPages.Count-1)
        {
            CurrentPage = 0;
        }
        else if (PageRequested < 0)
        {
            CurrentPage = CardPages.Count;
        }
        else
        {
            CurrentPage = PageRequested;
        }
        return CardPages[CurrentPage];
    }

    public void returnCardsToCurrentPage(List<Card> DisplayedCards)
    {
        Debug.LogError(CurrentPage + " recieved " + DisplayedCards[0].CardName);
        CardPages[CurrentPage] = DisplayedCards;
    }

    public Card GetCardByPageElement(int Page, int Element) //returns 10 cards for display later from passed page
    {
        List<Card> PageArr = GetCardsOfPage(Page);
        return PageArr[Element];

    }

    public void clearPage(int Page)
    {
            CardPages[Page].Clear();
    }

    public void clearCardByPageElement(int Page, int Element)
    {
        CardPages[Page].Clear();
    }

    public void ChangeCurrentPage(int direction)
    {
        switch (direction)
        {
            case -1:
                CurrentPage--;
                break;
            case 0:
                CurrentPage = 0;
                break;
            case 1:
                CurrentPage++;
                break;
        }
        if (CurrentPage < 0)
        {
            CurrentPage = NumPages - 1;
        }
    }




}

