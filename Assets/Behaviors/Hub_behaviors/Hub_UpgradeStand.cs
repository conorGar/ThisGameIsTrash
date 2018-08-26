using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hub_UpgradeStand : MonoBehaviour {

	public Image bagUpgrade;
	public Image hpUpgrade;
	public Image pins;
	public Image returnButton;
	public Sprite highlightedPaper;
	public Sprite normalPaper;
	public Sprite StarsAvailableHUD;
	public Sprite totalTrashHUD;
	public Sprite returnButtonHLspr;
	public Sprite returnButtonSpr;

	public GameObject selectParticleEffect;

	int arrowPos = 0;
	int previousArrowPos = 0;
	Image currentHighlightedImage;
	float paperYPositions;


	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVERIGHT)
        || ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKRIGHT) && arrowPos < 2){
			arrowPos++;
			Debug.Log(arrowPos);
			MoveArrowPos();
		}else if(ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVELEFT)
              || ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKLEFT) && arrowPos > 0){
			arrowPos--;
			Debug.Log(arrowPos);
			MoveArrowPos();
		}else if(ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVEDOWN)
              || ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKDOWN) && arrowPos < 3){
			if(currentHighlightedImage != null){
				currentHighlightedImage.sprite = normalPaper;
			}
			returnButton.sprite = returnButtonHLspr;
			previousArrowPos = arrowPos;
			arrowPos = 3;
		}else if(ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVEUP)
              || ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKUP) && arrowPos == 3){
			arrowPos = previousArrowPos;
			returnButton.sprite = returnButtonSpr;
			MoveArrowPos();
		}else if(ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT))
        {
			SelectUpgrade();
		}
	}

	public void Activate(){
		for(int i = 0; i < gameObject.transform.childCount; i++){
			gameObject.transform.GetChild(i).gameObject.SetActive(true);
		}
		Image totalTrashDisplay = GameObject.Find("totalTrashDisplay").GetComponent<Image>();
		totalTrashDisplay.sprite = StarsAvailableHUD;
		totalTrashDisplay.GetComponent<Ev_TotalTrashDisplay>().ToggleStarDisplay(true);
		GlobalVariableManager.Instance.PLAYER_CAN_MOVE = false;
		arrowPos = 0;
		MoveArrowPos();
	}

	void Deactivate(){
		for(int i = 0; i < gameObject.transform.childCount; i++){
			gameObject.transform.GetChild(i).gameObject.SetActive(false);
		}
		Image totalTrashDisplay = GameObject.Find("totalTrashDisplay").GetComponent<Image>();
		totalTrashDisplay.sprite = totalTrashHUD;
		totalTrashDisplay.GetComponent<Ev_TotalTrashDisplay>().ToggleStarDisplay(false);
		GlobalVariableManager.Instance.PLAYER_CAN_MOVE = true;
		arrowPos = 0;
		GameObject.Find("upgradeStandButton").GetComponent<SE_GlowWhenClose>().enabled = true;
	}

	void MoveArrowPos(){
		if(paperYPositions == 0)
			paperYPositions =   pins.transform.position.y;


		if(currentHighlightedImage != null){
			currentHighlightedImage.sprite = normalPaper;
			currentHighlightedImage.GetComponent<SpecialEffectsBehavior>().SmoothMovementToPoint(currentHighlightedImage.transform.position.x, paperYPositions -8f,.3f);

		}


		if(arrowPos == 0){
			currentHighlightedImage = bagUpgrade;
		}else if(arrowPos == 1){
			currentHighlightedImage = hpUpgrade;
		}else if(arrowPos == 2){
			currentHighlightedImage = pins;
		}
		currentHighlightedImage.sprite = highlightedPaper;
		currentHighlightedImage.GetComponent<SpecialEffectsBehavior>().SmoothMovementToPoint(currentHighlightedImage.transform.position.x, paperYPositions,.3f);

	}

	void SelectUpgrade(){
		if(int.Parse(GlobalVariableManager.Instance.characterUpgradeArray[7]) > 0){
			if(arrowPos == 0){
				//increase bag size
				GlobalVariableManager.Instance.characterUpgradeArray[4] = (int.Parse(GlobalVariableManager.Instance.characterUpgradeArray[4]) + 2).ToString();
			}else if(arrowPos == 1){
				//increase hp
				GlobalVariableManager.Instance.characterUpgradeArray[3] = (int.Parse(GlobalVariableManager.Instance.characterUpgradeArray[3]) + 1).ToString();
			}else if(arrowPos == 2){
				//increase PP
				GlobalVariableManager.Instance.PPVALUE += 3;
			}

			GlobalVariableManager.Instance.characterUpgradeArray[7] = (int.Parse(GlobalVariableManager.Instance.characterUpgradeArray[7]) - 1).ToString();
		}
		if(arrowPos == 3){
			Deactivate();
		}
	}
}
