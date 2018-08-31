using UnityEngine;
using System.Collections;

public class GUI_SaveFileSelect : GUI_MenuBase
{

	// Use this for initialization
	void Update ()
	{
		if(Input.GetKeyDown(KeyCode.Space)){
			LoadSave();
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

		selectionArrow.transform.position = new Vector2(optionIcons[arrowPos].transform.position.x-5f,optionIcons[arrowPos].transform.position.y);

	}
	
	public override void NavigateEffect(){
		optionIcons[arrowPos].GetComponent<GUI_Ev_buttonPopUp>().HighlightButton();

	}

	void LoadSave(){

	//load save

	}
}

