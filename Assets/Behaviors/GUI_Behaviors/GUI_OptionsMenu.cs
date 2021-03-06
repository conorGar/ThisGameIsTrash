﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GUI_OptionsMenu : MonoBehaviour {

   // AudioSource musicSource;
    //AudioSource sfxSource;


    public AudioClip selectSound;
    public AudioClip closeSound;

    //public GameObject soundManager;

    public GameObject musicVol;
    public GameObject sfxVol;
    public GameObject fullscreenToggle;
    public GameObject tutToggle;
    public GameObject selectArrow;
    public GameObject PauseMenu;


    GameObject currentlySelectedOption;
    int arrowPos = 1;
    // Use this for initialization
    void Start() { 
        musicVol.GetComponent<Image>().fillAmount = SoundManager.instance.musicSource.volume*2; //*2 because music vol is halved at pause menu
		sfxVol.GetComponent<Image>().fillAmount = SoundManager.instance.sfxSource.volume*2;
    }

    void OnEnable(){
    	SoundManager.instance.PlaySingle(closeSound);
    	arrowPos = 1;
    	SelectNext();
    }

    // Update is called once per frame
    void Update() {
		if (GameStateManager.Instance.GetCurrentState() == typeof(OptionsState)) {
	        if (ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVEDOWN)
	        || ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKDOWN) && arrowPos < 4) {
	            arrowPos++;
	            SoundManager.instance.PlaySingle(selectSound);
	            SelectNext();
	        } else if (ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVEUP)
	               || ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKUP) && arrowPos > 1f) {
	            arrowPos--;
	            SoundManager.instance.PlaySingle(selectSound);
	            SelectNext();
	        } else if (ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVERIGHT)
	               || ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKRIGHT))
	        {
	            if (arrowPos == 1) {
					if (SoundManager.instance.musicSource.volume < .5f) { // .5 because music is halved at pause screen
	                    SoundManager.instance.musicSource.volume += .08f;
	                    musicVol.GetComponent<Image>().fillAmount += .16f;
						GlobalVariableManager.Instance.MASTER_MUSIC_VOL = SoundManager.instance.musicSource.volume;

	                }
	            } else if (arrowPos == 2) {
					if (SoundManager.instance.sfxSource.volume < .5f) { // .5 because music is halved at pause screen
						SoundManager.instance.sfxSource.volume += .08f;
	                    sfxVol.GetComponent<Image>().fillAmount += .16f;
	                    GlobalVariableManager.Instance.MASTER_SFX_VOL = SoundManager.instance.sfxSource.volume;
	                }
	            }
	        } else if (ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVELEFT)
	               || ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKLEFT))
	        {
	            if (arrowPos == 1) {
					if (SoundManager.instance.musicSource.volume > 0f) {
						SoundManager.instance. musicSource.volume -= .08f;
	                    musicVol.GetComponent<Image>().fillAmount -= .16f;
	                    GlobalVariableManager.Instance.MASTER_MUSIC_VOL = SoundManager.instance.musicSource.volume;
	                }
	            } else if (arrowPos == 2) {
					if (SoundManager.instance.sfxSource.volume > 0f) {
						SoundManager.instance.sfxSource.volume -= .08f;
	                    sfxVol.GetComponent<Image>().fillAmount -= .16f;
	                    GlobalVariableManager.Instance.MASTER_SFX_VOL = SoundManager.instance.sfxSource.volume;
	                }
	            }
	        }

	        if (ControllerManager.Instance.GetKeyDown(INPUTACTION.CANCEL)
	         || ControllerManager.Instance.GetKeyDown(INPUTACTION.PAUSE))
	        {
	            // Back to the pause menu.
	            SoundManager.instance.PlaySingle(closeSound);
	            GameStateManager.Instance.PopState();
				gameObject.SetActive(false);
			}
		}
	}


	void SelectNext(){
		if(arrowPos == 1){
			currentlySelectedOption = musicVol;
		}else if(arrowPos == 2){
			currentlySelectedOption = sfxVol;
		}else if(arrowPos == 3){
			currentlySelectedOption = fullscreenToggle;
		}else if(arrowPos == 4){
			currentlySelectedOption = tutToggle;
		}

		selectArrow.transform.position = new Vector3(selectArrow.transform.position.x,currentlySelectedOption.transform.position.y,0f);

	}
}
