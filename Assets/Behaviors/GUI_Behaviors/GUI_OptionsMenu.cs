using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GUI_OptionsMenu : MonoBehaviour {

	AudioSource musicSource;
	AudioSource sfxSource;


	public AudioClip selectSound;

	public GameObject soundManager;

	public GameObject musicVol;
	public GameObject sfxVol;
	public GameObject fullscreenToggle;
	public GameObject tutToggle;
	public GameObject selectArrow;
	public GameObject PauseMenu;


	GameObject currentlySelectedOption;
	int arrowPos = 1;
	// Use this for initialization
	void Start () {
		AudioSource[] audioSources = soundManager.GetComponents<AudioSource>();
		musicSource = audioSources[1];
		sfxSource = audioSources[0];
		musicVol.GetComponent<Image>().fillAmount = musicSource.volume;
		sfxVol.GetComponent<Image>().fillAmount = sfxSource.volume;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.DownArrow) && arrowPos < 4){
			Time.timeScale = 1.0f- Time.timeScale;
			arrowPos++;
			SoundManager.instance.PlaySingle(selectSound);
			SelectNext();
			Time.timeScale = 0;
		}else if(Input.GetKeyDown(KeyCode.UpArrow) && arrowPos > 1f){
			Time.timeScale = 1.0f- Time.timeScale;
			arrowPos--;
			SoundManager.instance.PlaySingle(selectSound);
			SelectNext();
			Time.timeScale = 0;
		}else if(Input.GetKeyDown(KeyCode.RightArrow)){
			Time.timeScale = 1.0f- Time.timeScale;
			if(arrowPos == 1){
				if(musicSource.volume < 1){
					musicSource.volume += .16f;
					musicVol.GetComponent<Image>().fillAmount += .16f;
				}
			} else if(arrowPos == 2){
				if(sfxSource.volume < 1f){
					sfxSource.volume += .16f;
					sfxVol.GetComponent<Image>().fillAmount += .16f;
				}
			}
			Time.timeScale = 0;
		}else if(Input.GetKeyDown(KeyCode.LeftArrow)){
			Time.timeScale = 1.0f- Time.timeScale;
			if(arrowPos == 1){
				if(musicSource.volume > 0f){
						musicSource.volume -= .16f;
						musicVol.GetComponent<Image>().fillAmount -= .16f;
				}
			}else if(arrowPos == 2){
				if(sfxSource.volume > 0f){
					sfxSource.volume -= .16f;
					sfxVol.GetComponent<Image>().fillAmount -= .16f;
				}
			}
			Time.timeScale = 0;
		}

		if(Input.GetKeyDown(KeyCode.Return)){
			Time.timeScale = 1.0f- Time.timeScale;
			PauseMenu.GetComponent<GUI_PauseMenu>().enabled = true;
			gameObject.SetActive(false);
			Time.timeScale = 0;
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
