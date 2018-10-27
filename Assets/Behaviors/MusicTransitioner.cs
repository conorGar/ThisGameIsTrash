using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTransitioner : MonoBehaviour {
	public AudioClip newMusic;
	AudioClip musicToSwitchTo;
	public AudioClip baseWorldMusic;
	public bool botToTopDirection;
	public bool topToBotDirection;


	bool fadeToNew;
	bool fadeToBase;


	int trackPlaying = 1;
	bool fadeOut;
	bool fadeIn;
	float audioVolume;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(fadeToNew){
			if(fadeOut){
				if(audioVolume  > .1f){
					audioVolume -= 0.2f*Time.deltaTime;
					SoundManager.instance.musicSource.volume = audioVolume;
				}else{	
					fadeOut = false;
					fadeIn = true;
					if(trackPlaying == 1){
						SoundManager.instance.musicSource.clip = musicToSwitchTo;
						SoundManager.instance.musicSource.Play();
						trackPlaying = 2;
					}else{
						SoundManager.instance.musicSource.clip = baseWorldMusic;
						SoundManager.instance.musicSource.Play();
						trackPlaying = 1;
					}
				}
			}
			if(fadeIn){
				//fade in new sound

				if(audioVolume <.7f){//TODO: check for globally set max volume
					audioVolume += 0.2f*Time.deltaTime;
					SoundManager.instance.musicSource.volume = audioVolume;
				}else{
					fadeIn = false;
				}
			}

		}
	}


	void OnTriggerEnter2D(Collider2D collider){
		Debug.Log("Player enters audio switcher");
		audioVolume = SoundManager.instance.musicSource.volume;
		if(collider.gameObject.tag == "Player"){
			if(botToTopDirection){
				if(collider.gameObject.transform.position.y < transform.position.y){
					fadeToNew = true;
					musicToSwitchTo = newMusic;
					fadeOut = true;
				}else{
					fadeToNew = true;
					musicToSwitchTo = baseWorldMusic;
					fadeOut = true;
				}
			}else if(topToBotDirection && collider.gameObject.transform.position.y > transform.position.y){
				fadeToNew = true;
				musicToSwitchTo = baseWorldMusic;
				fadeOut = true;
			}
		}
	}
}
