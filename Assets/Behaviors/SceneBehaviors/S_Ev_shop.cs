using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Ev_shop : MonoBehaviour {

	public GameObject tutorialPopup;
	public Sprite tutPopupSprite;
	public GameObject pin;
	public GameObject purchasePopup;
	public GameObject harry;
	public GameObject priceDisplay;
	public GameObject costText;

	int upgrade1 = 10;
	int upgrade2 = 10;
	int upgrade3 = 10;
	int pinsUnlockedAlready = 0;
	List<int> nonShopPins = new List<int>();
	GameObject player;
	GameObject manager;

	void Start () {


		/*if(GlobalVariableManager.Instance.WORLD_SIGNS_READ[0][9] != 'o'){
			if(GlobalVariableManager.Instance.TUT_POPUP_ON){
				Instantiate(tutorialPopup,transform.position,Quaternion.identity);
				tutorialPopup.GetComponent<SpriteRenderer>().sprite = tutPopupSprite;
			}
			GlobalVariableManager.Instance.WORLD_SIGNS_READ[0].Replace(GlobalVariableManager.Instance.WORLD_SIGNS_READ[0][9],'o');
		}*/

		player = GameObject.Find("Jim");
		manager = GameObject.Find("Manager");

		//-----------cant spawn any pins that can be found in world ----------------//
		nonShopPins.Add(6);
		nonShopPins.Add(7);
		nonShopPins.Add(8);
		nonShopPins.Add(9);
		nonShopPins.Add(10);
		//nonShopPins.Add(13);  - pretty lucky = can be bought in shop currently
		nonShopPins.Add(14);
		nonShopPins.Add(18);
		nonShopPins.Add(19);
		nonShopPins.Add(20);
		nonShopPins.Add(26);
		nonShopPins.Add(27);
		nonShopPins.Add(28);
		nonShopPins.Add(29);
		nonShopPins.Add(30); //piercing pin
		nonShopPins.Add(31);
		nonShopPins.Add(32);
		nonShopPins.Add(37);
		//---------------------------------------------------------------------//

		// VV----------just makes it so "dont you wanna check shop shop" dialog doesnt happen
		GlobalVariableManager.Instance.WORLD_SIGNS_READ[1].Replace(GlobalVariableManager.Instance.WORLD_SIGNS_READ[1][14],'o');


		GlobalVariableManager.Instance.ARROW_POSITION = 0;
		GlobalVariableManager.Instance.MENU_SELECT_STAGE = 9;//needed for proper purchasing action when hit SPACE that doesnt interefere with upgrade behavior
		GlobalVariableManager.Instance.ROOM_NUM = 97;

		for(int i = 0; i < GlobalVariableManager.Instance.characterUpgradeArray[0].Length; i++){
			//prevents infinite "while" loop from happening below if player gets nearly all pins in the game
			if(GlobalVariableManager.Instance.characterUpgradeArray[0][i] == 'o'){
				if(nonShopPins.Contains(i) && i >38){
					pinsUnlockedAlready++;
				}
			}
		}
		//---------------------------------------set pins in today's shop------------------------------------------//
		if(GlobalVariableManager.Instance.TIME_IN_DAY != GlobalVariableManager.Instance.DAY_NUMBER){
			//Uses shopPins list to place values for today's pins
			//Reset when activate "Continue" truck at hub.

			if(pinsUnlockedAlready < 40){
				while(GlobalVariableManager.Instance.characterUpgradeArray[0][upgrade1] == 'o' || nonShopPins.Contains(upgrade1)){
					upgrade1 = Random.Range(0,39);
					GlobalVariableManager.Instance.shopPins[0] = upgrade1;
				}
				if(pinsUnlockedAlready < 39){
					while(upgrade1 == upgrade2 ||GlobalVariableManager.Instance.characterUpgradeArray[0][upgrade2] == 'o' || nonShopPins.Contains(upgrade2)){
						upgrade2 = Random.Range(0,39);
						GlobalVariableManager.Instance.shopPins[1] = upgrade2;
					}
					if(pinsUnlockedAlready < 38){
						while(upgrade3 == upgrade2|| upgrade3 == upgrade1 ||GlobalVariableManager.Instance.characterUpgradeArray[0][upgrade3] == 'o' || nonShopPins.Contains(upgrade3)){
						upgrade3 = Random.Range(0,39);
						GlobalVariableManager.Instance.shopPins[2] = upgrade3;
						}
					}
				}
			}
			GlobalVariableManager.Instance.TIME_IN_DAY = GlobalVariableManager.Instance.DAY_NUMBER; //only happens once a day


		}else{
			upgrade1 = GlobalVariableManager.Instance.shopPins[0];
			upgrade2 = GlobalVariableManager.Instance.shopPins[1];
			upgrade3 = GlobalVariableManager.Instance.shopPins[2];
		}
		//-------------------------------------------------------------------------------------------------------//

		SpawnPins();

	}

	public void TogglePopupEnable(){
		if(purchasePopup.activeInHierarchy){
			purchasePopup.SetActive(false);
		}else{
			purchasePopup.SetActive(true);
		}
	}
	
	void Update () {

		//---------------Player Bounds-----------------//
		if(player != null){
			if(player.transform.position.y > 8.9f){
				player.transform.position = new Vector2(player.transform.position.x,8.9f);
			}else if(player.transform.position.y < 1.6){
				player.transform.position = new Vector2(player.transform.position.x,1.6f);
			}else if(player.transform.position.x > 22f){
				player.transform.position = new Vector2(22f,player.transform.position.y);
			}else if(player.transform.position.x < -1f && GlobalVariableManager.Instance.SCENE_IS_TRANSITIONING == false){
				//Leave room

				GlobalVariableManager.Instance.ROOM_NUM = 101;
				manager.GetComponent<Ev_FadeHelper>().FadeToScene("Hub");

				GlobalVariableManager.Instance.SCENE_IS_TRANSITIONING = true;
			}
		}
		//--------------------------------------------//
	}

	public void ShowThings(int cost){
		harry.GetComponent<tk2dSpriteAnimator>().Play("talk");
		costText.GetComponent<TextMesh>().text = cost.ToString();
		priceDisplay.SetActive(true);
	}

	public void HideThings(){
		harry.GetComponent<tk2dSpriteAnimator>().Play("idle");
		priceDisplay.SetActive(false);
	}

	void SpawnPins(){
		int currentUpgradeCheck = 0;
		float xPos = 0f;
		float yPos = 0f;
		tk2dSprite pinsSprite;
		GameObject spawnedPin;
		for(int i = 0; i<3;i++){
			
			Debug.Log("How many times does this for loop run? (SHould be 3)");
			if(i == 0){
				currentUpgradeCheck = upgrade1;
				xPos = 8.89f;
				yPos = 10f;
			}else if(i == 1){
				currentUpgradeCheck = upgrade2;
				xPos = 14f;
				yPos = 10.6f;
			}else if(i == 2){
				currentUpgradeCheck = upgrade3;
				xPos = 18.6f;
				yPos = 10f;
			}
			spawnedPin = Instantiate(pin,new Vector2(xPos,yPos),Quaternion.identity);
			pinsSprite = spawnedPin.GetComponent<tk2dSprite>();

			if(currentUpgradeCheck == 0){
				pinsSprite.SetSprite("pin_bulkBag");
			}else if(currentUpgradeCheck == 1){
				pinsSprite.SetSprite("pin_timeEnough");
			}else if(currentUpgradeCheck == 2){
				pinsSprite.SetSprite("pin_speedy");
			}else if(currentUpgradeCheck == 3){
				pinsSprite.SetSprite("pin_cursed");
			}else if(currentUpgradeCheck == 4){
				pinsSprite.SetSprite("pin_blisteringBond");
			}else if(currentUpgradeCheck == 5){
				pinsSprite.SetSprite("pin_passivePillage");
			}else if(currentUpgradeCheck == 6){
				pinsSprite.SetSprite("pin_faithfulWeapin");
			}else if(currentUpgradeCheck == 7){
				pinsSprite.SetSprite("pin_legacy");
			}else if(currentUpgradeCheck == 8){
				pinsSprite.SetSprite("pin_dropChance");
			}else if(currentUpgradeCheck == 9){
				pinsSprite.SetSprite("pin_moGarbageMoProblems");
			}else if(currentUpgradeCheck == 11){
				pinsSprite.SetSprite("pin_haulingHero");
			}else if(currentUpgradeCheck == 12){
				pinsSprite.SetSprite("pin_applePlus");
			}else if(currentUpgradeCheck == 13){
				pinsSprite.SetSprite("pin_prettyLucky");
			}else if(currentUpgradeCheck == 15){
				pinsSprite.SetSprite("pin_pointyPin");
			}else if(currentUpgradeCheck == 16){
				pinsSprite.SetSprite("pin_lockOn");
			}else if(currentUpgradeCheck == 17){
				pinsSprite.SetSprite("pin_lighterLoad");
			}else if(currentUpgradeCheck == 21){
				pinsSprite.SetSprite("pin_scrapCity");
			}else if(currentUpgradeCheck == 22){
				pinsSprite.SetSprite("pin_quenpin");
			}else if(currentUpgradeCheck == 23){
				pinsSprite.SetSprite("pin_hungryForMore");
			}else if(currentUpgradeCheck == 24){
				pinsSprite.SetSprite("pin_nextOfPin");
			}else if(currentUpgradeCheck == 25){
				pinsSprite.SetSprite("pin_callOfTheWild");
			}else if(currentUpgradeCheck == 26){
				pinsSprite.SetSprite("pin_talkyTime");
			}else if(currentUpgradeCheck == 33){
				pinsSprite.SetSprite("pin_luckyPin");
			}else if(currentUpgradeCheck == 34){
				pinsSprite.SetSprite("pin_trashierTmw");
			}else if(currentUpgradeCheck == 35){
				pinsSprite.SetSprite("pin_fuelEfficient");
			}else if(currentUpgradeCheck == 36){
				pinsSprite.SetSprite("pin_deathDeal");
			}else if(currentUpgradeCheck == 38){
				pinsSprite.SetSprite("pin_magnet");
			}
			spawnedPin.GetComponent<Ev_PinBehavior>().SetPinData(currentUpgradeCheck);
			spawnedPin.GetComponent<Ev_PinBehavior>().SetMySpot(i+1);
		}
	}
}
