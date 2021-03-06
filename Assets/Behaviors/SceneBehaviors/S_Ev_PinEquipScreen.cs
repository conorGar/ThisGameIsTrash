﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.PostProcessing;

public class S_Ev_PinEquipScreen : MonoBehaviour {


	public GameObject leftSide;
	public GameObject rightSide;

	public GameObject returnButton;
	public GameObject tutorialPopup;
	public GameObject pin;
	public GameObject selectionArrow;
	public GameObject displayPin;
	public PostProcessingProfile blur;
	public TextMeshProUGUI currentPage;
	public AudioClip navLeftSFX;
	public AudioClip navRightSFX;
	public AudioClip openSfx;


	int arrowPos = 0;
	GameObject highlightedPin;
	public TextMeshProUGUI totalPPDisplay;
    List<GameObject> pinPageList = new List<GameObject>();

	void Start () {
        GlobalVariableManager.Instance.MENU_SELECT_STAGE = 10;

		/*if(GlobalVariableManager.Instance.ROOM_NUM != 112){
			GlobalVariableManager.Instance.ROOM_NUM = 101;
		}

        /*if(GlobalVariableManager.Instance.WORLD_SIGNS_READ[0][9] != 'o'){
			Instantiate(tutorialPopup,transform.position,Quaternion.identity);
			GlobalVariableManager.Instance.WORLD_SIGNS_READ[0].Replace(GlobalVariableManager.Instance.WORLD_SIGNS_READ[0][9],'o');
		}*/

        SetupPins();
    }
	void OnEnable(){
		SoundManager.instance.PlaySingle(openSfx);
        if (PinManager.Instance.DebugPins) {
            GlobalVariableManager.Instance.PINS_DISCOVERED = PIN.ALL;
        }

        GameStateManager.Instance.PushState(typeof(ShopState));
        leftSide.transform.localPosition = new Vector2(-140f,0f);
		rightSide.transform.localPosition = new Vector2(120f,0f);

		leftSide.GetComponent<SpecialEffectsBehavior>().SmoothMovementToPoint(0,0,.2f,true);
		rightSide.GetComponent<SpecialEffectsBehavior>().SmoothMovementToPoint(0,0,.2f,true);
        CamManager.Instance.mainCamPostProcessor.profile = blur;

        // Get the current pp stat by evaluating the pins equipped.
        GlobalVariableManager.Instance.PP_STAT.ResetCurrent();
        GlobalVariableManager.Instance.PP_STAT.UpdateCurrent(-PinManager.Instance.GetAllocatedPP());

        totalPPDisplay.text = GlobalVariableManager.Instance.PP_STAT.GetCurrent().ToString();

        arrowPos = 0;
        MoveArrow();
	}

    void OnDisable()
    {
        PinManager.Instance.RefreshNewPinIcon();
        GameStateManager.Instance.PopState();
    }

    void Update () {
        if (GameStateManager.Instance.GetCurrentState() == typeof(ShopState)) {
            if (ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT)) {
                MoveArrow();
                highlightedPin.GetComponent<Ev_PinBehavior>().EquipPin();

                /*if (highlightedPin.GetComponent<Ev_PinBehavior>().EquipPin())
                {
                    // Untilt if equipped
                    //if (highlightedPin != null)
                    //    highlightedPin.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
                }
                else
                {
                    // Tilt if unequipped
                    //if (highlightedPin != null)
                    //    highlightedPin.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 15f));
                }*/

                totalPPDisplay.text = GlobalVariableManager.Instance.PP_STAT.GetCurrent().ToString();
            }
            else if (ControllerManager.Instance.GetKeyDown(INPUTACTION.CANCEL)) {
                GameObject pinCase = GameObject.Find("hubWorld_pinCase");
                pinCase.GetComponent<Ev_PinDisplayOption>().enabled = true;
                CamManager.Instance.mainCamPostProcessor.profile = null;
				SoundManager.instance.PlaySingle(openSfx);
                this.gameObject.SetActive(false);
            }
            else {
                bool isNewPin = false;
                if (ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVELEFT)
                 || ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKLEFT)) {
                    SoundManager.instance.PlaySingle(navLeftSFX);
                    arrowPos--;
                    isNewPin = true;
                }
                else if (ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVERIGHT)
                      || ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKRIGHT)) {
                    SoundManager.instance.PlaySingle(navRightSFX);

                    arrowPos++;
                    isNewPin = true;
                }
                else if (ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVEDOWN)
                      || ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKDOWN)) {
                    SoundManager.instance.PlaySingle(navRightSFX);

                    arrowPos += PinManager.Instance.PinCol;
                    isNewPin = true;
                }
                else if (ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVEUP)
                      || ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKUP)) {
                    SoundManager.instance.PlaySingle(navLeftSFX);
                    arrowPos -= PinManager.Instance.PinCol;
                    isNewPin = true;
                }

                if (isNewPin) {
                    // Wrap from 0 to pin max.
                    if (arrowPos < 0)
                        arrowPos = PinManager.Instance.pinConfig.pinList.Count - 1;

                    if (arrowPos > PinManager.Instance.pinConfig.pinList.Count - 1)
                        arrowPos = 0;

                    // select the currentpage.
                    for (int i = 0; i < pinPageList.Count; ++i) {
                        if (i == arrowPos / (PinManager.Instance.PinCol * PinManager.Instance.PinRow)) {
                            currentPage.text = (i + 1).ToString();
                            pinPageList[i].SetActive(true);

                        }
                        else
                            pinPageList[i].SetActive(false);
                    }

                    if (highlightedPin != null)
                        highlightedPin.GetComponent<Ev_PinBehavior>().Unhighlight();

                    MoveArrow();
                }
            }

            totalPPDisplay.text = GlobalVariableManager.Instance.PP_STAT.GetCurrent().ToString();
        }
    }

	void MoveArrow(){
        int currentPage = arrowPos / (PinManager.Instance.PinCol * PinManager.Instance.PinRow);
        int currentPin = arrowPos % (PinManager.Instance.PinCol * PinManager.Instance.PinRow);
        GameObject pinPage = PinManager.Instance.PageRoot.transform.GetChild(currentPage).gameObject;

        highlightedPin = pinPage.transform.GetChild(currentPin).gameObject;
        highlightedPin.SetActive(true);
        selectionArrow.transform.position = new Vector3(highlightedPin.transform.position.x,
                                                        highlightedPin.transform.position.y,
                                                        selectionArrow.transform.position.z);

        var pinBehavior = highlightedPin.GetComponent<Ev_PinBehavior>();
        pinBehavior.Highlight();
        pinBehavior.AtEquipScreen();
	}

    void SetupPins()
    {
        // Set up all the pin pages.
        var pinsPerPage = PinManager.Instance.PinRow * PinManager.Instance.PinCol;
        var pinPageCount = PinManager.Instance.pinConfig.pinList.Count / pinsPerPage;
        pinPageList = new List<GameObject>();

        // add a page for any remaining pins.
        if (PinManager.Instance.pinConfig.pinList.Count % pinsPerPage > 0)
            pinPageCount += 1;

        for (int i = 0; i < pinPageCount; ++i) {
            GameObject pinPage = ObjectPool.Instance.GetPooledObject("PinPage");
            pinPage.transform.SetParent(PinManager.Instance.PageRoot.transform);
            pinPage.transform.position = PinManager.Instance.PageRoot.transform.position;
            pinPage.name = "Pin Page " + i;
            pinPage.SetActive(true);
            pinPageList.Add(pinPage);
        }

        for (int i = 0; i < PinManager.Instance.pinConfig.pinList.Count; i++) {
            PinDefinition pinDefinition = PinManager.Instance.pinConfig.pinList[i];
            int pageNum = i / pinsPerPage;
            Vector3 pagePos = pinPageList[pageNum].transform.position;
            var pin = ObjectPool.Instance.GetPooledObject("Pin").GetComponent<Ev_PinBehavior>();
            pin.transform.SetParent(pinPageList[pageNum].transform);

            // populate the pin data
            pin.name = pinDefinition.displayName;

            pin.sprite.SetSprite(pinDefinition.sprite);
            pin.SetPinData(pinDefinition);
            pin.gameObject.SetActive(true);

            var pinSize = pin.sprite.GetComponent<BoxCollider2D>().bounds.size;
            // x, at the page origin, per column, spaced with an offset
            // y, at the page origin, per row, per page, projected downward.
            pin.transform.position = new Vector3(pagePos.x + i % PinManager.Instance.PinCol * (pinSize.x + PinManager.Instance.PinOffsetX),
                                                 pagePos.y + -i / PinManager.Instance.PinCol % PinManager.Instance.PinRow * (pinSize.y + PinManager.Instance.PinOffsetY),
                                                 pagePos.z);
            Debug.Log("Got Here Pin Spawn");
        }

        for (int i = 0; i < pinPageList.Count; ++i) {
            // deactivate all but the first page.
            if (i != 0)
                pinPageList[i].SetActive(false);
        }

        arrowPos = 0;
        MoveArrow();
    }
}
