﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GUI_TutPopup : MonoBehaviour {

	public GameObject myDescription;
	public GameObject myImage;
	public GameObject myTitle;
	public GameObject mainCam;

	public Sprite largeTrashImage;
	public Sprite ArmoredEnemyImage;
	public Sprite PinsImage;
	public Sprite NightImage;
	public Sprite SneakingImage;
	public Sprite upgradeImage;
	public Sprite murderImage;
	public Sprite dojoImage;


	int phase;
	// Use this for initialization
	void Start () {

	}

	void OnEnable(){
		GlobalVariableManager.Instance.PLAYER_CAN_MOVE = false;
		gameObject.GetComponent<SpecialEffectsBehavior>().SmoothMovementToPoint(transform.position.x,transform.position.y +2.4f,.5f);
		StartCoroutine("Delays");
	}

	IEnumerator Delays(){
		if(phase == 0){
		yield return new WaitForSeconds(.5f);
		phase = 1;
		}else if(phase == 1){
			phase = 2;
			gameObject.GetComponent<SpecialEffectsBehavior>().SmoothMovementToPoint(transform.position.x,5f,.5f);
			yield return new WaitForSeconds(.5f);
			mainCam.GetComponent<Ev_MainCameraEffects>().ReturnFromCamEffect();
			GlobalVariableManager.Instance.PLAYER_CAN_MOVE = true;
			gameObject.SetActive(false);
		}

	}

	void Update(){

		if(Input.GetKeyDown(KeyCode.Space)){
			if(phase == 1){
				StartCoroutine("Delays");
			}
		}

	}

	public void SetData(string tutPopup){

		if(tutPopup == "LargeTrash"){
			myDescription.GetComponent<TextMeshProUGUI>().text = "Carry <color=#ffffb3>Large Trash</color> back to the dumpster " +
															"to earn <color=#ffffb3>Star Points</color>, which allow you to"+
															" upgrade your stats back at The Dump and " +
															"visit new worlds.";
			myImage.GetComponent<Image>().sprite = largeTrashImage;
			myTitle.GetComponent<TextMeshProUGUI>().text = "LARGE TRASH";
			GlobalVariableManager.Instance.TUT_POPUPS_SHOWN |= GlobalVariableManager.TUTORIALPOPUPS.LARGETRASH;
		}else if(tutPopup == "ArmoredEnemies"){
			myDescription.GetComponent<TextMeshProUGUI>().text = "<color=#ffffb3>Armored Enemies</color> are tough and impervious " +
															"to melee attacks. Could there be some other"+
															" way to defeat them? ";
			myImage.GetComponent<Image>().sprite = ArmoredEnemyImage;
			myTitle.GetComponent<TextMeshProUGUI>().text = "ARMORED ENEMIES";
			GlobalVariableManager.Instance.TUT_POPUPS_SHOWN |= GlobalVariableManager.TUTORIALPOPUPS.ARMOREDENEMIES;

		}else if(tutPopup == "DayNight"){
			myDescription.GetComponent<TextMeshProUGUI>().text = "Time flies when you're trash hunting! At the " +
																"end of each day, a garbage truck will return "+
																"you to <color=#ffffb3>The Dump</color> before night falls and "+
																"the <color=#ffffb3>bloodthirsty, vengeful raccoons</color> come out.";
			myImage.GetComponent<Image>().sprite = NightImage;
			myTitle.GetComponent<TextMeshProUGUI>().text = "NIGHTFALL";
			GlobalVariableManager.Instance.TUT_POPUPS_SHOWN |= GlobalVariableManager.TUTORIALPOPUPS.DAYNIGHT;

		}

	}

}