using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUI_MenuBase : MonoBehaviour {
	public int maxArrowPos;
	public bool upDownNav;
	public bool leftRightNav;

	public int valToIncreaseWhenDown = 1;
	public int arrowPos;

	public List<GameObject> optionIcons = new List<GameObject>();
	public GameObject selectionArrow;


	Vector3 currentSelectArrowPos;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.LeftArrow) && leftRightNav && arrowPos>0){
			Navigate("left");
		}else if(Input.GetKeyDown(KeyCode.RightArrow) && leftRightNav && arrowPos<maxArrowPos){
			Navigate("right");
		}else if(Input.GetKeyDown(KeyCode.UpArrow) && upDownNav && arrowPos>0){
			Navigate("up");
		}else if(Input.GetKeyDown(KeyCode.DownArrow) && upDownNav && arrowPos<maxArrowPos){
			Navigate("down");
		}



		selectionArrow.transform.position = Vector3.Lerp(currentSelectArrowPos,optionIcons[arrowPos].transform.position,2*Time.deltaTime);
		


	}

	void Navigate(string dir){
		currentSelectArrowPos = selectionArrow.transform.position;
		optionIcons[arrowPos].GetComponent<GUI_Ev_buttonPopUp>().UnhighlightButton();
		if(dir == "left"){
			arrowPos--;
		}else if(dir == "right"){
			arrowPos++;
		}else if(dir == "up"){
			arrowPos--;
		}else if(dir == "down"){
			if(valToIncreaseWhenDown == 0){
				arrowPos++;
			}else{
				arrowPos = valToIncreaseWhenDown; //varies sometimes
			}
		}
		optionIcons[arrowPos].GetComponent<GUI_Ev_buttonPopUp>().HighlightButton();



	}
}
