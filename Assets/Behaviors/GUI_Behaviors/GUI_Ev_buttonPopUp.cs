using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_Ev_buttonPopUp : MonoBehaviour {

	/// <summary>
	/// Just takes care of highlighting buttons when this buttons position = menuSelectStageValue
	/// </summary>
	Image myImage;
	Sprite startSprite;

	public Sprite highlightSprite;
	public int myPosition = 0;
	void Start () {
		myImage = gameObject.GetComponent<Image>();
		startSprite = myImage.sprite;

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
		myImage.color = new Color(255f,255f,255f);
		myImage.sprite = highlightSprite;
	}

	public void UnhighlightButton(){
		myImage.color = new Color(180f,180f,180f);
		myImage.sprite = startSprite;
	}
}
