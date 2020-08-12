using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//using UnityEditor;

public class deckBuilderMenuScript : MonoBehaviour
{

    public Button newDeckButton;
    public Button loadDeckButton;
    public Button backButton;
    public AudioClip clickSound;
    public AudioClip heavyClickSound;
    public AudioSource menuAudio;

    // Use this for initialization
    void Start()
    {
        //loadDeckButton = loadDeckButton.GetComponent<Button>();
       //newDeckButton = newDeckButton.GetComponent<Button>();
        backButton = backButton.GetComponent<Button>();
        Application.targetFrameRate = -1;


    }

    //for when the Create Deck button is pressed
    public void loadPress()
    {
        //menuAudio.PlayOneShot(heavyClickSound, 0.7F);
       // EditorUtility.DisplayDialog("Load Deck", "Please select a deck file to load!!!", "Fine. No need to be so enthusiastic.");
       // string path;
        //path = EditorUtility.OpenFilePanel("Load Deck", "", "XML");

        //Debug.Log(path);

       // this.gameObject.transform.FindChild("Player").gameObject.GetComponent<player>().deckPath = path;

        //Debug.Log("This works!\n" + this.gameObject.transform.FindChild("Player").gameObject.GetComponent<player>().deckPath);

    }

    //for when the Edit Deck button is pressed
    public void newPress()
    {
        //menuAudio.PlayOneShot(heavyClickSound, 0.7F);
        //Application.LoadLevel(-1); //Level number should be the edit menu for the deck builder

    }

    //for when the back button is pressed
    public void BackPress()
    {
        //menuAudio.PlayOneShot(heavyClickSound, 0.7F);
       //Level number should be the main menu
        SceneManager.LoadScene(0);
    }
}
