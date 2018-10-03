﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GUI_OptionsMenu : MonoBehaviour {

   // AudioSource musicSource;
    //AudioSource sfxSource;


    public AudioClip selectSound;

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
       
        musicVol.GetComponent<Image>().fillAmount = SoundManager.instance.musicSource.volume;
		sfxVol.GetComponent<Image>().fillAmount = SoundManager.instance.sfxSource.volume;
    }

    // Update is called once per frame
    void Update() {
        if (ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVEDOWN)
        || ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKDOWN) && arrowPos < 4) {
            Time.timeScale = 1.0f - Time.timeScale;
            arrowPos++;
            SoundManager.instance.PlaySingle(selectSound);
            SelectNext();
            Time.timeScale = 0;
        } else if (ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVEUP)
               || ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKUP) && arrowPos > 1f) {
            Time.timeScale = 1.0f - Time.timeScale;
            arrowPos--;
            SoundManager.instance.PlaySingle(selectSound);
            SelectNext();
            Time.timeScale = 0;
        } else if (ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVERIGHT)
               || ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKRIGHT))
        {
            Time.timeScale = 1.0f - Time.timeScale;
            if (arrowPos == 1) {
				if (SoundManager.instance.musicSource.volume < 1) {
                    SoundManager.instance.musicSource.volume += .16f;
                    musicVol.GetComponent<Image>().fillAmount += .16f;
					GlobalVariableManager.Instance.MASTER_MUSIC_VOL = SoundManager.instance.musicSource.volume;

                }
            } else if (arrowPos == 2) {
				if (SoundManager.instance.sfxSource.volume < 1f) {
					SoundManager.instance.sfxSource.volume += .16f;
					GlobalVariableManager.Instance.MASTER_SFX_VOL = SoundManager.instance.sfxSource.volume;
                    sfxVol.GetComponent<Image>().fillAmount += .16f;
                }
            }
            Time.timeScale = 0;
        } else if (ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVELEFT)
               || ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKLEFT))
        {
            Time.timeScale = 1.0f - Time.timeScale;
            if (arrowPos == 1) {
				if (SoundManager.instance.musicSource.volume > 0f) {
					SoundManager.instance. musicSource.volume -= .16f;
                    musicVol.GetComponent<Image>().fillAmount -= .16f;
                }
            } else if (arrowPos == 2) {
				if (SoundManager.instance.sfxSource.volume > 0f) {
					SoundManager.instance.sfxSource.volume -= .16f;
                    sfxVol.GetComponent<Image>().fillAmount -= .16f;
                }
            }
            Time.timeScale = 0;
        }

        if (ControllerManager.Instance.GetKeyDown(INPUTACTION.CANCEL)
         || ControllerManager.Instance.GetKeyDown(INPUTACTION.PAUSE))
        {
			Time.timeScale = 1.0f- Time.timeScale;
			if(PauseMenu !=null){ //it should = null on title screen
				PauseMenu.GetComponent<GUI_PauseMenu>().enabled = true;
				Time.timeScale = 0;
			}

			gameObject.SetActive(false);
		}
	}

	void OnEnable(){
		arrowPos = 1;
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
