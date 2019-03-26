using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_Ev_buttonPopUp : MonoBehaviour {

	/// <summary>
	/// Just takes care of highlighting buttons when this buttons position = menuSelectStageValue
	/// </summary>
	Sprite startSprite;

	public Sprite highlightSprite;
	public int myPosition = 0;
	public bool floatUpWhenHL;
	Vector2 startingPosition;

	void Awake () {
		startingPosition = gameObject.transform.position;
		startSprite = GetComponent<Image>().sprite;

	}
	void OnEnable(){
		if(myPosition != 0)
			UnhighlightButton();
	}
	// Update is called once per frame
	void Update () {
		/*if(GlobalVariableManager.Instance.MENU_SELECT_STAGE == myPosition && myImage.sprite != highlightSprite){
			transform.position = new Vector2(transform.position.x +6f, transform.position.y);
			myImage.color = new Color(255f,255f,255f);
			myImage.sprite = highlightSprite;
		}else if(GlobalVariableManager.Instance.MENU_SELECT_STAGE != myPosition && myImage.sprite != startSprite){
			transform.position = new Vector2(transform.position.x -6f, transform.position.y);
			myImage.color = new Color(180f,180f,180f);
			myImage.sprite = startSprite;
		}*/
	}

	public void HighlightButton(){
        GetComponent<Image>().color = new Color(255f,255f,255f);
        GetComponent<Image>().sprite = highlightSprite;
		if(floatUpWhenHL){
			gameObject.GetComponent<SpecialEffectsBehavior>().SmoothMovementToPoint(startingPosition.x,startingPosition.y + 1f,.3f);
		}
	}

	public void UnhighlightButton(){
        GetComponent<Image>().color = new Color(180f,180f,180f);
        GetComponent<Image>().sprite = startSprite;
		if(floatUpWhenHL){
			gameObject.GetComponent<SpecialEffectsBehavior>().SmoothMovementToPoint(startingPosition.x,startingPosition.y,.3f);
		}
	}
}
