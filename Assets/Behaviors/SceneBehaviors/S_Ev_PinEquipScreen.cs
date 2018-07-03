using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class S_Ev_PinEquipScreen : MonoBehaviour {

	public GameObject returnButton;
	public GameObject tutorialPopup;
	public GameObject pin;
	public GameObject selectionArrow;
	public GameObject displayPin;


	float xAdjustmentValue;
	float yAdjustValue;
	int arrowPos = 0;
	GameObject highlightedPin;
	Text totalPPDisplay;



	void Start () {
		GlobalVariableManager.Instance.MENU_SELECT_STAGE = 10;

		if(GlobalVariableManager.Instance.ROOM_NUM != 112){
			GlobalVariableManager.Instance.ROOM_NUM = 101;
		}

		/*if(GlobalVariableManager.Instance.WORLD_SIGNS_READ[0][9] != 'o'){
			Instantiate(tutorialPopup,transform.position,Quaternion.identity);
			GlobalVariableManager.Instance.WORLD_SIGNS_READ[0].Replace(GlobalVariableManager.Instance.WORLD_SIGNS_READ[0][9],'o');
		}*/

		totalPPDisplay = GameObject.Find("totalPP").GetComponent<Text>();

		for(int i = 0; i<24; i++){
			xAdjustmentValue = 2.3f*(i%4);
			if(i%4 == 0 && i != 0){
				yAdjustValue -= 2f;
			}
			GameObject spawnedPin = Instantiate(pin, new Vector2( .76f + xAdjustmentValue, 12.3f + yAdjustValue),Quaternion.identity);
			tk2dSprite pinsSprite = spawnedPin.GetComponent<tk2dSprite>();
			spawnedPin.transform.parent = gameObject.transform;
			if(i == 0){
				pinsSprite.SetSprite("pin_bulkBag");
			}else if(i == 1){
				pinsSprite.SetSprite("pin_timeEnough");
			}else if(i == 2){
				pinsSprite.SetSprite("pin_speedy");
			}else if(i == 3){
				pinsSprite.SetSprite("pin_cursed");
			}else if(i == 4){
				pinsSprite.SetSprite("pin_blisteringBond");
			}else if(i == 5){
				pinsSprite.SetSprite("pin_passivePillage");
			}else if(i == 6){
				pinsSprite.SetSprite("pin_faithfulWeapin");
			}else if(i == 7){
				pinsSprite.SetSprite("pin_legacy");
			}else if(i == 8){
				pinsSprite.SetSprite("pin_dropChance");
			}else if(i == 9){
				pinsSprite.SetSprite("pin_moGarbageMoProblems");
			}else if(i == 11){
				pinsSprite.SetSprite("pin_haulingHero");
			}else if(i == 12){
				pinsSprite.SetSprite("pin_applePlus");
			}else if(i == 13){
				pinsSprite.SetSprite("pin_prettyLucky");
			}else if(i == 15){
				pinsSprite.SetSprite("pin_pointyPin");
			}else if(i == 16){
				pinsSprite.SetSprite("pin_lockOn");
			}else if(i == 17){
				pinsSprite.SetSprite("pin_lighterLoad");
			}else if(i == 21){
				pinsSprite.SetSprite("pin_scrapCity");
			}else if(i == 22){
				pinsSprite.SetSprite("pin_quenpin");
			}else if(i == 23){
				pinsSprite.SetSprite("pin_hungryForMore");
			}else if(i == 24){
				pinsSprite.SetSprite("pin_nextOfPin");
			}else if(i == 25){
				pinsSprite.SetSprite("pin_callOfTheWild");
			}else if(i == 26){
				pinsSprite.SetSprite("pin_talkyTime");
			}else if(i == 33){
				pinsSprite.SetSprite("pin_luckyPin");
			}else if(i == 34){
				pinsSprite.SetSprite("pin_trashierTmw");
			}else if(i == 35){
				pinsSprite.SetSprite("pin_fuelEfficient");
			}else if(i == 36){
				pinsSprite.SetSprite("pin_deathDeal");
			}else if(i == 38){
				pinsSprite.SetSprite("pin_magnet");
			}
			spawnedPin.GetComponent<Ev_PinBehavior>().SetPinData(i);
		}



	}
	
	void Update () {
		if(Input.GetKeyDown(KeyCode.LeftArrow) && arrowPos > 0){
			arrowPos --;
			if(highlightedPin != null)
				highlightedPin.transform.rotation =  Quaternion.Euler(new Vector3(0f,0f,0f));
			MoveArrow();
		}else if(Input.GetKeyDown(KeyCode.RightArrow) && arrowPos < 24){
			arrowPos ++;
			if(highlightedPin != null)
				highlightedPin.transform.rotation =  Quaternion.Euler(new Vector3(0f,0f,0f));
			MoveArrow();
		}else if(Input.GetKeyDown(KeyCode.DownArrow) && arrowPos < 20){
			arrowPos += 4;
			if(highlightedPin != null)
				highlightedPin.transform.rotation =  Quaternion.Euler(new Vector3(0f,0f,0f));
			MoveArrow();
		}else if(Input.GetKeyDown(KeyCode.UpArrow) && arrowPos > 2){
			arrowPos -= 4;
			if(highlightedPin != null)
				highlightedPin.transform.rotation =  Quaternion.Euler(new Vector3(0f,0f,0f));
			MoveArrow();
		}else if(Input.GetKeyDown(KeyCode.Space)){
			if(highlightedPin != null)
				highlightedPin.transform.rotation =  Quaternion.Euler(new Vector3(0f,0f,0f));
			highlightedPin = transform.GetChild(arrowPos).gameObject;
			highlightedPin.GetComponent<Ev_PinBehavior>().EquipPin();
		}


		totalPPDisplay.text = GlobalVariableManager.Instance.pinsEquipped[0].ToString();

	}

	void MoveArrow(){
		xAdjustmentValue = 2.35f*((arrowPos)%4);
		yAdjustValue = 2f * Mathf.Floor(arrowPos/4);
		selectionArrow.transform.position = new Vector2( .66f + xAdjustmentValue, 12.4f - yAdjustValue);
		highlightedPin = transform.GetChild(arrowPos).gameObject;
		highlightedPin.transform.rotation =  Quaternion.Euler(new Vector3(0f,0f,15f));
		highlightedPin.GetComponent<Ev_PinBehavior>().AtEquipScreen();
	

	}
}
