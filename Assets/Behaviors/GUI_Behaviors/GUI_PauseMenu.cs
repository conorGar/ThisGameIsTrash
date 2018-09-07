using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_PauseMenu : MonoBehaviour {

	int arrowpos = 1;
	public GameObject optionsOption;
	public GameObject enddayOption;
	public GameObject optionsMenu;
	public GameObject returnToDumpsterOption;
	public GameObject endDayDisplayHUD;

	public Sprite optionsHLspr;
	public Sprite endayHLspr;
	public Sprite returnToDumpsterHlspr;


	public AudioClip selectSound;
	public AudioClip paperSlide;

	Sprite optionStartSpr;
	Sprite endDayStartSpr;
	Sprite returnStartSpr;
	Vector3 startPos;
	// Use this for initialization
	void Start () {
		startPos = gameObject.transform.localPosition;
		endDayStartSpr = enddayOption.GetComponent<Image>().sprite;
		optionStartSpr = optionsOption.GetComponent<Image>().sprite;
		endDayStartSpr = enddayOption.GetComponent<Image>().sprite;

		optionsOption.GetComponent<Image>().sprite = optionsHLspr;
		Time.timeScale = 0;
		SoundManager.instance.PlaySingle(paperSlide);

	}

	void OnEnable(){
		SoundManager.instance.PlaySingle(paperSlide);
		Time.timeScale = 0;

	}
	// Update is called once per frame
	void Update () {
		if(ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVERIGHT)
        || ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKRIGHT))
        {
			if(arrowpos < 2){
				Time.timeScale = 1.0f- Time.timeScale;
				arrowpos++;
				SoundManager.instance.PlaySingle(selectSound);
				enddayOption.GetComponent<Image>().sprite = endayHLspr;
				optionsOption.GetComponent<Image>().sprite = optionStartSpr;
				Time.timeScale = 0.0f;
			}
		}else if(ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVELEFT)
              || ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKLEFT))
        {
			if(arrowpos > 1){
				Time.timeScale = 1.0f- Time.timeScale;
				arrowpos--;
				SoundManager.instance.PlaySingle(selectSound);
				enddayOption.GetComponent<Image>().sprite = endDayStartSpr;
				optionsOption.GetComponent<Image>().sprite = optionsHLspr;
				Time.timeScale = 0.0f;
			}
		}else if (ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT))
        {
			if(arrowpos == 1){
				Time.timeScale = 1.0f- Time.timeScale;
				optionsMenu.SetActive(true);
				this.enabled = false;
				Time.timeScale = 0;
			}else if(arrowpos == 2){//end day
				Time.timeScale = 1.0f- Time.timeScale;
				endDayDisplayHUD.SetActive(true);
				endDayDisplayHUD.GetComponent<GUI_OptionsPopupBehavior>().pauseMenu = this;
				endDayDisplayHUD.GetComponent<GUI_OptionsPopupBehavior>().AtPauseScreen = true;
				this.enabled = false;
				Time.timeScale = 0;
			}
		}
		if(ControllerManager.Instance.GetKeyDown(INPUTACTION.CANCEL)
        || ControllerManager.Instance.GetKeyDown(INPUTACTION.PAUSE))
        {
			arrowpos = 1;
			gameObject.transform.localPosition = startPos;
			Time.timeScale = 1;
			SoundManager.instance.PlaySingle(paperSlide);
			gameObject.SetActive(false);
		}


		gameObject.transform.localPosition = Vector3.Lerp(gameObject.transform.localPosition,Vector3.zero,.1f*(Time.realtimeSinceStartup - Time.deltaTime));
	}
}
