using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class menuScript : MonoBehaviour {

	public Canvas exitMenu;
	public Button deckButton;
	public Button playButton;
	public Button exitButton;
	public AudioClip clickSound;
	public AudioClip heavyClickSound;
	public AudioSource menuAudio;

	// Use this for initialization
	void Start () {
		exitMenu = exitMenu.GetComponent<Canvas> ();
		menuAudio = GetComponent<AudioSource>();
        DontDestroyOnLoad(GameObject.Find("Music"));
		deckButton = deckButton.GetComponent<Button> ();
		playButton = playButton.GetComponent<Button> ();
		exitButton = exitButton.GetComponent<Button> ();
        //deckButton.interactable = false;
        exitMenu.enabled = false;
	}

	public void ExitPress(){
		menuAudio.PlayOneShot(clickSound, 0.7F);
		exitMenu.enabled = true;
		deckButton.interactable = false;
		playButton.interactable = false;
		exitButton.interactable = false;
	}

	public void CancelPress(){
		menuAudio.PlayOneShot(clickSound, 0.7F);
		exitMenu.enabled = false;
		deckButton.interactable = true;
		playButton.interactable = true;
		exitButton.interactable = true;
	}

	public void StartMatch() {
		menuAudio.PlayOneShot(heavyClickSound, 0.7F);
        SceneManager.LoadScene(1); //Level 1 should be the match scene
	}

	public void StartDeckBuilder() {
		menuAudio.PlayOneShot(clickSound, 0.7F);
        SceneManager.LoadScene(3); //Level 3 should be the deck builder scene
	}

	public void ExitGame() {
		Application.Quit();
	}
	
}
