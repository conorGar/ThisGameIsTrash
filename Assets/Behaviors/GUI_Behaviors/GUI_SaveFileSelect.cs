using UnityEngine;
using System.Collections;

public class GUI_SaveFileSelect : GUI_MenuBase
{

	// Use this for initialization
	public GameObject fadeHelper;

	void OnEnable(){

		GetComponent<SpecialEffectsBehavior>().SmoothMovementToPoint(12f,8.4f,.2f,true);
	}
	void Update ()
	{
		if(ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT)){
			LoadSave();
			//TODO: back button: enable scene event script again when do
		}

		if(ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVELEFT) && leftRightNav && arrowPos>0){
			optionIcons[arrowPos].GetComponent<GUI_Ev_buttonPopUp>().UnhighlightButton();
			Navigate("left");
		}else if(ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVERIGHT) && leftRightNav && arrowPos<maxArrowPos){
			Debug.Log("Arrow RIGHT");
			optionIcons[arrowPos].GetComponent<GUI_Ev_buttonPopUp>().UnhighlightButton();
			Navigate("right");
		}else if(ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVEUP) && upDownNav && arrowPos>0){
			optionIcons[arrowPos].GetComponent<GUI_Ev_buttonPopUp>().UnhighlightButton();
			Navigate("up");
		}else if(ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVEDOWN)  && upDownNav && arrowPos<maxArrowPos){
			optionIcons[arrowPos].GetComponent<GUI_Ev_buttonPopUp>().UnhighlightButton();
			Navigate("down");
		}

		selectionArrow.transform.position = new Vector2(optionIcons[arrowPos].transform.position.x-5f,optionIcons[arrowPos].transform.position.y);

	}
	
	public override void NavigateEffect(){
		optionIcons[arrowPos].GetComponent<GUI_Ev_buttonPopUp>().HighlightButton();

	}

	void LoadSave(){

		UserDataManager.Instance.SetSlot(arrowPos);
		StartCoroutine(UserDataManager.Instance.ReadAsync());
		fadeHelper.GetComponent<Ev_FadeHelper>().FadeToScene("1_1");

	}
}

