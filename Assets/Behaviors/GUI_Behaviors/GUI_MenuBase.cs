using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class GUI_MenuBase : MonoBehaviour {
	public int maxArrowPos;
	public bool upDownNav;
	public bool leftRightNav;

	public int valToIncreaseWhenDown = 1;
	public int arrowPos;

	public List<GameObject> optionIcons = new List<GameObject>();
	public GameObject selectionArrow;
	public PostProcessingProfile blur;
	public GameObject mainCam;

	public Vector3 currentSelectArrowPos;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	public void Update () {
		

	}

	public void Navigate(string dir){
		Debug.Log("Arrow Pos = " + arrowPos);
		currentSelectArrowPos = selectionArrow.transform.position;
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
		NavigateEffect();


	}

	public virtual void NavigateEffect(){
		//nothing for base menu
	}
}
