using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using I2.TextAnimation;
using UnityEngine.PostProcessing;

public class GUI_BaseStatUpgrade : GUI_MenuBase {


	public TextMeshProUGUI positiveText;
	public GUI_StarsAvailableHUD starsAvailableHUD;
	public ParticleSystem selectPS;
	public GameObject baseStatUpgrade;
	public AudioClip navSFX1;
	public AudioClip navSFX2;
	public AudioClip selectUpgradeSFX;

	void OnEnable(){
        GameStateManager.Instance.PushState(typeof(ShopState));
        CamManager.Instance.mainCamPostProcessor.profile = blur;
        starsAvailableHUD.gameObject.SetActive(true);
	}

    void OnDisable()
    {
        CamManager.Instance.mainCamPostProcessor.profile = null;
        GameStateManager.Instance.PopState();
    }

    // Update is called once per frame
    void Update () {
        if (GameStateManager.Instance.GetCurrentState() == typeof(ShopState)) {
            if (ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT)) {
                SelectUpgrade();
            }

            if (ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVELEFT) && leftRightNav && arrowPos > 0) {
                optionIcons[arrowPos].GetComponent<GUI_Ev_buttonPopUp>().UnhighlightButton();
                SoundManager.instance.PlaySingle(navSFX2);
                Navigate("left");
            }
            else if (ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVERIGHT) && leftRightNav && arrowPos < maxArrowPos) {
                Debug.Log("Arrow RIGHT");
                optionIcons[arrowPos].GetComponent<GUI_Ev_buttonPopUp>().UnhighlightButton();
                SoundManager.instance.PlaySingle(navSFX1);
                Navigate("right");
            }
            else if (ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVEUP) && upDownNav && arrowPos > 0) {
                SoundManager.instance.PlaySingle(navSFX2);
                optionIcons[arrowPos].GetComponent<GUI_Ev_buttonPopUp>().UnhighlightButton();
                Navigate("up");
            }
            else if (ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVEDOWN) && upDownNav && arrowPos < maxArrowPos) {
                SoundManager.instance.PlaySingle(navSFX1);
                optionIcons[arrowPos].GetComponent<GUI_Ev_buttonPopUp>().UnhighlightButton();
                Navigate("down");
            }

            //selectionArrow.transform.position = Vector3.Lerp(currentSelectArrowPos,optionIcons[arrowPos].transform.position,2*Time.deltaTime);
            if(selectionArrow != null)
            	selectionArrow.transform.position = optionIcons[arrowPos].transform.position;
        }
	}

	void SelectUpgrade(){
		if(GlobalVariableManager.Instance.STAR_POINTS_STAT.GetCurrent() > 0){
			SoundManager.instance.PlaySingle(selectUpgradeSFX);

			selectPS.transform.position = optionIcons[arrowPos].transform.position;
			selectPS.Play();
			if(arrowPos == 0){
				GlobalVariableManager.Instance.BAG_SIZE_STAT.UpdateMax(+2);
			}else if(arrowPos == 1){
				GlobalVariableManager.Instance.HP_STAT.UpdateMax(+1);
			}else if(arrowPos == 2){
				GlobalVariableManager.Instance.PP_STAT.UpdateMax(+2);	
			}
			GlobalVariableManager.Instance.STAR_POINTS_STAT.UpdateCurrent(-1);
            starsAvailableHUD.UpdateDisplay();
            PositiveText();
		}if(arrowPos == 3){
			starsAvailableHUD.gameObject.SetActive(false);
			baseStatUpgrade.SetActive(false);
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
