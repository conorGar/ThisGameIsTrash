using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class S_Ev_BagSelect : MonoBehaviour {
	public GameObject bag;
	public Sprite buddyTitle;
	public Sprite reggieTitle;
	public Sprite cassieTitle;
	public Sprite BAGtitle;
	public Sprite unknownTitle;
	public Text perk1;
	public Text perk2;
	public Text perk3;
	public GameObject Buttons;
	public GameObject smokePuffEffect;
	public Image butt1;
	public Image butt2;
	public Image butt3;


	GameObject spawnedBag;
	bool locked = false;
	bool selected = false;
	bool canNavigate = true;
	int selectedArrowPos;//used to hold current menu select stage postion when select bag(to return to if leave)
	int arrowPos = 0;
	public GameObject currentCam;
	GameObject bagTitle;
	Transform shadow;

	void Start () {
		//currentCam = GameObject.Find("tk2dCamera");
		spawnedBag = GameObject.FindGameObjectWithTag("Trash");
		GlobalVariableManager.Instance.MENU_SELECT_STAGE = 0;
		bagTitle = GameObject.Find("title");
	}
	
	void Update () {
		if(canNavigate ){
			if(selected != true){
				if(ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVERIGHT)
               || ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKRIGHT))
                {
					if(arrowPos < 3){
						arrowPos++;
					}else{
						arrowPos = 0;
					}
					NextBag("right");
				}else if(ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVELEFT)
                      || ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKLEFT))
                {
					if(arrowPos > 0){
						arrowPos--;
					}else{
						arrowPos = 3;
					}
					NextBag("left");
				}
			}else{//selected options
				if(ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVEDOWN)
                || ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKDOWN))
                {
					if(arrowPos < 2){
						arrowPos++;
					}
				}else if(ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVEUP)
                      || ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKUP))
                {
					if(arrowPos > 0){
						arrowPos--;
					}
				}
			}
			if(ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT) && canNavigate && locked == false){
				BagSelect();
			}
		}

	}

	void NextBag(string direction){
        // MENU_SELECT_STAGE is the bag selected.
        GameObject currentBag = GameObject.FindGameObjectWithTag("Trash");
		currentBag.GetComponent<Ev_BagSelect>().StartCoroutine("LeaveScreen");

        // If the bag is locked.
		if(GlobalVariableManager.Instance.IsBagLocked(GlobalVariableManager.Instance.MENU_SELECT_STAGE)){
			bagTitle.GetComponent<SpriteRenderer>().sprite = unknownTitle;
			perk1.text = "";
			perk2.text = "";
			perk3.text = "";
			locked = true;

        // If the bag is unlocked.
		}else{
			locked = false;
			if(arrowPos == 2){
				bagTitle.GetComponent<SpriteRenderer>().sprite = cassieTitle;
				perk1.text = "+1 Max HP";
				perk2.text = "Compost Can destroy Styrofoam blockades";
				perk3.text = "Compost have a chance to heal when picked up";
			}else if(arrowPos  == 1){
				bagTitle.GetComponent<SpriteRenderer>().sprite = reggieTitle;
				perk1.text = "Damage Armored Enemies";
				perk2.text = "Carry Metal Blockades";
				perk3.text = "Chance to Critical Hit";
			}else if(arrowPos  == 3){
				bagTitle.GetComponent<SpriteRenderer>().sprite = BAGtitle;
				perk1.text = "Bag Size + 5";
				perk2.text = "Speed Boost When Carrying Large Trash";
				perk3.text = " ";
			}else if(GlobalVariableManager.Instance.MENU_SELECT_STAGE  == 0){
				bagTitle.GetComponent<SpriteRenderer>().sprite = buddyTitle;
				perk1.text = "More Trash in World";
				perk2.text = "Enemies Drop More Scrap";
				perk3.text = " ";
			}
		}

		if(direction == "right"){
			currentBag.GetComponent<Rigidbody2D>().velocity = new Vector2(30f,0f);
			spawnedBag = Instantiate(bag,new Vector2(currentCam.transform.position.x - 6.45f,10.5f), Quaternion.identity);
			spawnedBag.GetComponent<Rigidbody2D>().velocity = new Vector2(30f,0f);
		}else if(direction == "left"){
			currentBag.GetComponent<Rigidbody2D>().velocity = new Vector2(-30f,0f);
			spawnedBag = Instantiate(bag,new Vector2(currentCam.transform.position.x + Screen.width/10 - 19f,10.5f), Quaternion.identity);
			spawnedBag.GetComponent<Rigidbody2D>().velocity = new Vector2(-30f,0f);
		}
		canNavigate = false; // set back by Trashbags 'Leave Screen() courotuine'

	}// end of NextBag()

	public void SetNavigate(bool v){
		canNavigate = v;
	}

	void BagSelect(){

		if(selected == false){
			selectedArrowPos = arrowPos;
			spawnedBag.GetComponent<SpecialEffectsBehavior>().SmoothMovementToPoint(13.5f,12.5f,.2f);
			Instantiate(smokePuffEffect,new Vector2(spawnedBag.transform.position.x, spawnedBag.transform.position.y +2f),Quaternion.identity);
			shadow  = spawnedBag.transform.GetChild(0); // get shadow
			shadow.parent = null; //detatch shadow
			//canNavigate = false;
			selected = true;
			arrowPos = 0;
			Buttons.SetActive(true);
		}else{
			if(arrowPos == 0){
				//back button
				spawnedBag.GetComponent<SpecialEffectsBehavior>().SmoothMovementToPoint(13.5f,10.5f,.2f);
				shadow.parent = GameObject.FindGameObjectWithTag("Trash").transform;//reattatch shadow
				//canNavigate = true;
				selected = false;
				arrowPos = selectedArrowPos;
				Buttons.SetActive(false);
			}else if(arrowPos == 2){
				//start day button
				gameObject.GetComponent<Ev_FadeHelper>().FadeToScene("DayDisplayScreen");
			}
		}
	}
}
