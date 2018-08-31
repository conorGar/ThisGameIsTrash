using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using I2.TextAnimation;
using UnityEngine.PostProcessing;

public class GUI_BaseStatUpgrade : GUI_MenuBase {


	public TextMeshProUGUI positiveText;
	public Hub_UpgradeStand stand;
	public GameObject starsAvailableHUD;
	public ParticleSystem selectPS;
	public GameObject baseStatUpgrade;

	void Start () {
		
	}

	void OnEnable(){
		mainCam.GetComponent<PostProcessingBehaviour>().profile = blur;
		starsAvailableHUD.SetActive(true);
		Navigate("");
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)){
			SelectUpgrade();
		}

		if(Input.GetKeyDown(KeyCode.LeftArrow) && leftRightNav && arrowPos>0){
			optionIcons[arrowPos].GetComponent<GUI_Ev_buttonPopUp>().UnhighlightButton();
			Navigate("left");
		}else if(Input.GetKeyDown(KeyCode.RightArrow) && leftRightNav && arrowPos<maxArrowPos){
			Debug.Log("Arrow RIGHT");
			optionIcons[arrowPos].GetComponent<GUI_Ev_buttonPopUp>().UnhighlightButton();
			Navigate("right");
		}else if(Input.GetKeyDown(KeyCode.UpArrow) && upDownNav && arrowPos>0){
			optionIcons[arrowPos].GetComponent<GUI_Ev_buttonPopUp>().UnhighlightButton();
			Navigate("up");
		}else if(Input.GetKeyDown(KeyCode.DownArrow) && upDownNav && arrowPos<maxArrowPos){
			optionIcons[arrowPos].GetComponent<GUI_Ev_buttonPopUp>().UnhighlightButton();
			Navigate("down");
		}

		//selectionArrow.transform.position = Vector3.Lerp(currentSelectArrowPos,optionIcons[arrowPos].transform.position,2*Time.deltaTime);
		selectionArrow.transform.position = optionIcons[arrowPos].transform.position;
	}

	void SelectUpgrade(){
		if(GlobalVariableManager.Instance.STAR_POINTS > 0){
			selectPS.transform.position = optionIcons[arrowPos].transform.position;
			selectPS.Play();
			if(arrowPos == 0){
				GlobalVariableManager.Instance.BAG_SIZE +=2;
				starsAvailableHUD.GetComponent<GUI_StarsAvailableHUD>().UpdateDisplay();
			}else if(arrowPos == 1){
				GlobalVariableManager.Instance.Max_HP +=1;
				starsAvailableHUD.GetComponent<GUI_StarsAvailableHUD>().UpdateDisplay();
			}else if(arrowPos == 2){
				GlobalVariableManager.Instance.PPVALUE +=3;
				starsAvailableHUD.GetComponent<GUI_StarsAvailableHUD>().UpdateDisplay();
			}
			GlobalVariableManager.Instance.STAR_POINTS--;
			PositiveText();
		}if(arrowPos == 3){
			this.gameObject.SetActive(false);
			stand.player.GetComponent<EightWayMovement>().enabled = false;
			mainCam.GetComponent<PostProcessingBehaviour>().profile = null;
			stand.ReturnFromDisplay();
			starsAvailableHUD.SetActive(false);
			baseStatUpgrade.SetActive(false);
			stand.enabled = true;
		}
	}

	void PositiveText(){
		int whichText = Random.Range(1,6);
		string positiveQuote = null;
		if(whichText == 1){
			positiveQuote = "You got this. Keep it up. Proud of you.";
		}else if(whichText == 2){
			positiveQuote = "Just keep going. One of these days you'll get it right, I'm sure of it.";
		}else if(whichText == 3){
			positiveQuote = "I believe in you. If you can't do it, nobody can.";
		}else if(whichText == 4){
			positiveQuote = "You smell very nice today.";
		}else if(whichText == 5){
			positiveQuote = "Wow, you're doing so great. It's really inspiring.";
		}else if(whichText == 6){
			positiveQuote = "The difference between winners and losers is that winners try one more time.";
		}
		positiveText.text = positiveQuote;
		positiveText.GetComponent<TextAnimation>().StartAgain();

	}

	public override void NavigateEffect(){
		optionIcons[arrowPos].GetComponent<GUI_Ev_buttonPopUp>().HighlightButton();
	}
}
