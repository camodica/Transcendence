using UnityEngine;
using System.Collections;

public class musicScript : MonoBehaviour {

	public AudioClip currentTrack;
	public AudioClip menuTrack, matchDayTrack, matchNightTrack, matchVictoryTrack, matchLossTrack;
	public AudioSource musicAudio;

	// Use this for initialization
	void Start () {
		musicAudio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void loopTrack(AudioClip track){

	}

	public void stopTrack(){

	}

	public void fadeOutTrack(){

	}

	public void pauseTrack(){

	}

	public void restartTrack(){

	}
	

}
