﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GUI_TutPopup : MonoBehaviour {

	public GameObject myDescription;
	public GameObject myImage;
	public GameObject myTitle;
	public AudioClip popupSFX;
	public AudioClip closeSfx;

	public Sprite largeTrashImage;
	public Sprite ArmoredEnemyImage;
	public Sprite PinsImage;
	public Sprite NightImage;
	public Sprite SneakingImage;
	public Sprite upgradeImage;
	public Sprite murderImage;
	public Sprite dojoImage;
	public Sprite shopImage;
	public Sprite toxicImage;

	Vector3 startPos;
	int phase;
	// Use this for initialization
	void Start () {
	}

	void OnEnable(){
		SoundManager.instance.PlaySingle(popupSFX);
		startPos = gameObject.transform.position;//reset position

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
			SoundManager.instance.PlaySingle(closeSfx);
			yield return new WaitForSeconds(.5f);

            CamManager.Instance.mainCamEffects.ReturnFromCamEffect();
			phase = 0;
			gameObject.transform.position = startPos;
            GameStateManager.Instance.PopState();
			gameObject.SetActive(false);

		}

	}

	void Update(){
        if (GameStateManager.Instance.GetCurrentState() == typeof(DialogState)) {
            if (ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT)) {
                if (phase == 1) {
                    StartCoroutine("Delays");
                }
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

		}else if(tutPopup == "Pins"){
			myDescription.GetComponent<TextMeshProUGUI>().text = "<color=#ffffb3>Pins</color> are equippable upgrades that you can " +
																"equip using your <color=#ffffb3>Pin Points</color>. They can be "+
																"found in each world or purchased in shops."+
																"You can equip them back at <color=#ffffb3>The Dump</color>!";
			myImage.GetComponent<Image>().sprite = PinsImage;
			myTitle.GetComponent<TextMeshProUGUI>().text = "PINS";
			GlobalVariableManager.Instance.TUT_POPUPS_SHOWN |= GlobalVariableManager.TUTORIALPOPUPS.PINS;

		}else if(tutPopup == "Shop"){
			myDescription.GetComponent<TextMeshProUGUI>().text = "<color=#ffffb3>Homeless Harry's Pin Shop</color> has a selection " +
																"of pins that can be purchased using <color=#ffffb3>trash pieces</color>"+
																" you have found in the world. Not available in Demo.";
			myImage.GetComponent<Image>().sprite = shopImage;
			myTitle.GetComponent<TextMeshProUGUI>().text = "SHOP";
			//GlobalVariableManager.Instance.TUT_POPUPS_SHOWN |= GlobalVariableManager.TUTORIALPOPUPS.PINS;

		}else if(tutPopup == "RadioactiveEnemy"){
			myDescription.GetComponent<TextMeshProUGUI>().text = "Enemies that give off a <color=#ffffb3>green glow</color> are radioactive." +
																 "Their attacks do <color=#ffffb3>2 damage</color>  instead of 1.";
			myImage.GetComponent<Image>().sprite = toxicImage;
			myTitle.GetComponent<TextMeshProUGUI>().text = "Radioactive Enemies";
			//GlobalVariableManager.Instance.TUT_POPUPS_SHOWN |= GlobalVariableManager.TUTORIALPOPUPS.PINS;
			GlobalVariableManager.Instance.TUT_POPUPS_SHOWN |= GlobalVariableManager.TUTORIALPOPUPS.TOXICENEMIES;

		}

        UserDataManager.Instance.SetDirty();
	}

}
