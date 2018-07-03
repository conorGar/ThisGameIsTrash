using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Ev_PinBehavior : MonoBehaviour {

	public GameObject smallTextDisplay;
	public GameObject ppDisplayIcon; //neded for pin equip screen
	public GameObject equippedBox;
	public string soldTextSprite = "sold";
    string myTitleAnimationName;
	string myDescription;
	int myPositionInString = 0;

	Image descriptionBox;
	Text descriptionText;

	int price;
	int setArrowPosOnce = 0;
	int popupOnce;
	int mySpotInShop;
	int mySpotInPinEquipScreen;
	int myPPvalue = 0;
	bool bought = false;
	bool drawDescription = false;
	bool inShop = false;
	float startingY;
	SpecialEffectsBehavior mySFX;
	GameObject pinTitleGameObject;
	GameObject ppDisplaySpawner;
	GameObject player;
	GameObject myManager;
	GameObject shopLight;
	GameObject displayPin;
	List<GameObject> myIcons = new List<GameObject>();
	GameObject myEquippedBox;

	void Start () {
		if(GlobalVariableManager.Instance.characterUpgradeArray[1][18] == 'o'){
			//pin perk 5 - every pin costs one less pp to equip
			if((price - 1) > 0){
				price -=1;
			}else{
				price = 1;
			}
		}

		mySFX = gameObject.GetComponent<SpecialEffectsBehavior>();
		pinTitleGameObject = GameObject.Find("pinTitles");
		player = GameObject.Find("Jim");
		descriptionText = GameObject.Find("Text").GetComponent<Text>();
		ppDisplaySpawner = GameObject.Find("PPDisplay");
		myManager = GameObject.Find("Manager");
		displayPin = GameObject.Find("displayPin");

		if(GlobalVariableManager.Instance.ROOM_NUM == 97){
			inShop = true;
			shopLight = GameObject.Find("shopLight");
			descriptionBox = GameObject.Find("description").GetComponent<Image>();
		}else if(GlobalVariableManager.Instance.ROOM_NUM == 101){
			if(GlobalVariableManager.Instance.characterUpgradeArray[0][myPositionInString] != 'o'){
				Debug.Log("MyPos:" + myPositionInString + "my char value here:" + GlobalVariableManager.Instance.characterUpgradeArray[0][myPositionInString]);
				gameObject.GetComponent<tk2dSprite>().color = new Color(0f,0f,0f,1f);//blacked out if not owned
			}else{
				float xSpawnAdjust = -.6f;

				for(int i = 0; i < myPPvalue; i++){//little pp displays
					GameObject icon = Instantiate(ppDisplayIcon, new Vector2(transform.position.x + xSpawnAdjust,transform.position.y - .9f),Quaternion.identity);
					icon.transform.parent = gameObject.transform;
					//icon.transform.position = new Vector2(transform.position.x + xSpawnAdjust,transform.position.y - 9.3f);
					icon.GetComponent<SpriteRenderer>().color = new Color(0f,0f,0f);
					myIcons.Add(icon);
					xSpawnAdjust += .33f;
				}
			}
		}

		if(inShop){
			if(GlobalVariableManager.Instance.characterUpgradeArray[0][myPositionInString] == 'o' && GlobalVariableManager.Instance.MENU_SELECT_STAGE != 10 && GlobalVariableManager.Instance.MENU_SELECT_STAGE != 20 && GlobalVariableManager.Instance.MENU_SELECT_STAGE != 30){
				GameObject myTextDisplay = Instantiate(smallTextDisplay,transform.position,Quaternion.identity);
				Color currentColor = gameObject.GetComponent<tk2dSprite>().color;
				gameObject.GetComponent<tk2dSprite>().color = new Color(currentColor.r,currentColor.g,currentColor.b,.4f);//fade
				myTextDisplay.GetComponent<tk2dSprite>().SetSprite(soldTextSprite);
				bought = true;
			}

			startingY = gameObject.transform.position.y;
		}//end of inShop Check
	}
	
	void Update () {


		if(inShop){

			if(Mathf.Abs(transform.position.x - player.transform.position.x) < 2 && Mathf.Abs(startingY - player.transform.position.y) < 3.6 && !bought){
				if(setArrowPosOnce == 0){
					mySFX.SmoothMovementToPoint(transform.position.x,startingY + 2,.2f);
					GlobalVariableManager.Instance.ARROW_POSITION = mySpotInShop;
					pinTitleGameObject.SetActive(true);
					descriptionText.enabled = true;
					descriptionBox.enabled = true;
					descriptionBox.GetComponent<GUIEffects>().Start();
					ppDisplaySpawner.GetComponent<Ev_PPDisplay>().SetDisplayedIcons(myPPvalue);
					myManager.GetComponent<S_Ev_shop>().ShowThings(price);
					shopLight.GetComponent<SpecialEffectsBehavior>().SmoothMovementToPoint(transform.position.x, transform.position.y +2,.4f); 
					setArrowPosOnce = 1;//makes sure doesnt set to zero when close to other pins in shop
					Debug.Log(setArrowPosOnce);
				}
			}else{
				if(setArrowPosOnce == 1){
					mySFX.SmoothMovementToPoint(transform.position.x,startingY,.2f);
					GlobalVariableManager.Instance.ARROW_POSITION = 0;
					pinTitleGameObject.SetActive(false);
					descriptionBox.enabled = false;
					descriptionText.enabled = false;
					ppDisplaySpawner.GetComponent<Ev_PPDisplay>().Clear();
					myManager.GetComponent<S_Ev_shop>().HideThings();
					setArrowPosOnce = 0;
					Debug.Log(setArrowPosOnce);
				}
			}



			//------------what to do when arrow position is my position------------/
			if(bought == false && GlobalVariableManager.Instance.ARROW_POSITION == mySpotInShop){
				if(pinTitleGameObject != null){
					pinTitleGameObject.GetComponent<tk2dSprite>().SetSprite(myTitleAnimationName);
				}
				if(descriptionText != null){
					descriptionText.text = myDescription;
				}

				if(Input.GetKeyDown(KeyCode.Space) && popupOnce == 0){
					ShopPurchase();
				}
			}
			//-----------------------------------------------------------------//


		}
	}

	public void AtEquipScreen(){
		if(GlobalVariableManager.Instance.characterUpgradeArray[0][myPositionInString] == 'o'){
			descriptionText.text = myDescription;
			pinTitleGameObject.GetComponent<tk2dSprite>().SetSprite(myTitleAnimationName);
			Debug.Log(this.gameObject.GetComponent<tk2dSprite>().CurrentSprite.name);
			Debug.Log(displayPin.GetComponent<tk2dSprite>().CurrentSprite.name);

			ppDisplaySpawner.GetComponent<Ev_PPDisplay>().SetDisplayedIcons(myPPvalue);
			displayPin.GetComponent<tk2dSprite>().SetSprite(gameObject.GetComponent<tk2dSprite>().CurrentSprite.name);
		}else{
			descriptionText.text = "Buy or find this Pin to learn what powers it holds!";
			pinTitleGameObject.GetComponent<tk2dSprite>().SetSprite("pinTitle_locked");
			ppDisplaySpawner.GetComponent<Ev_PPDisplay>().Clear();
		}
	}

	public void EquipPin(){
		bool isDejaVuPinAndIsUnlocked = false;
		if(GlobalVariableManager.Instance.characterUpgradeArray[0][18] == 'o' && myPositionInString == 18){
			isDejaVuPinAndIsUnlocked = true;
		}
		if(GlobalVariableManager.Instance.characterUpgradeArray[0][myPositionInString] == 'o'){
			if((GlobalVariableManager.Instance.pinsEquipped[myPositionInString+1] == 0 || isDejaVuPinAndIsUnlocked) && GlobalVariableManager.Instance.pinsEquipped[0] >= myPPvalue){ // equip unequipped pin
				if(myPositionInString !=18){ //set different for Pin that can kill the past
					GlobalVariableManager.Instance.pinsEquipped[myPositionInString+1] = 1; //set pin to active
				}else{ //Pin that can kill the past
					GlobalVariableManager.Instance.pinsEquipped[myPositionInString+1] += 4;
				}
				GlobalVariableManager.Instance.pinsEquipped[0] -= myPPvalue;
				for(int i = 0; i < myIcons.Count; i++){ //unshade the little pp icons below pin
					myIcons[i].GetComponent<SpriteRenderer>().color = new Color(255f,255f,255f,1f);
				}
				myEquippedBox = Instantiate(equippedBox,transform.position,Quaternion.identity);
				if(myPositionInString == 12){
				//Apple Plus pin
					GlobalVariableManager.Instance.characterUpgradeArray[3] = (int.Parse(GlobalVariableManager.Instance.characterUpgradeArray[3]) + 1).ToString();
				}else if(myPositionInString == 42){
				//Rotten Apple Plus
					GlobalVariableManager.Instance.characterUpgradeArray[3] = (int.Parse(GlobalVariableManager.Instance.characterUpgradeArray[3]) + 2).ToString();
				}else if(myPositionInString == 49){
				//Attack Pin
					GlobalVariableManager.Instance.characterUpgradeArray[5] = (int.Parse(GlobalVariableManager.Instance.characterUpgradeArray[5]) + 1).ToString();
				}else if(myPositionInString == 51){
				//Defense Pin
					GlobalVariableManager.Instance.characterUpgradeArray[6] = (int.Parse(GlobalVariableManager.Instance.characterUpgradeArray[6]) + 1).ToString();
				}
			}else if(GlobalVariableManager.Instance.pinsEquipped[myPositionInString+1] != 0){//Unequip pin
				GlobalVariableManager.Instance.pinsEquipped[0] += myPPvalue;
				if(myPositionInString !=18){ //set different for Pin that can kill the past
					GlobalVariableManager.Instance.pinsEquipped[myPositionInString+1] = 0; //set pin to active
				}else{ //Pin that can kill the past
					if(GlobalVariableManager.Instance.pinsEquipped[myPositionInString+1] != 3){
						GlobalVariableManager.Instance.pinsEquipped[myPositionInString+1] -= 4;
					}else{
						GlobalVariableManager.Instance.pinsEquipped[myPositionInString+1] = 99;
					}
				}
				for(int i = 0; i < myIcons.Count; i++){ //unshade the little pp icons below pin
					myIcons[i].GetComponent<SpriteRenderer>().color = new Color(0f,0f,0f,1f);
				}
				Destroy(myEquippedBox);
				if(myPositionInString == 12){
				//Apple Plus pin
					GlobalVariableManager.Instance.characterUpgradeArray[3] = (int.Parse(GlobalVariableManager.Instance.characterUpgradeArray[3]) - 1).ToString();
				}else if(myPositionInString == 42){
				//Rotten Apple Plus
					GlobalVariableManager.Instance.characterUpgradeArray[3] = (int.Parse(GlobalVariableManager.Instance.characterUpgradeArray[3]) - 2).ToString();
				}else if(myPositionInString == 49){
				//Attack Pin
					GlobalVariableManager.Instance.characterUpgradeArray[5] = (int.Parse(GlobalVariableManager.Instance.characterUpgradeArray[5]) - 1).ToString();
				}else if(myPositionInString == 51){
				//Defense Pin
					GlobalVariableManager.Instance.characterUpgradeArray[6] = (int.Parse(GlobalVariableManager.Instance.characterUpgradeArray[6]) - 1).ToString();
				}
			}//end of unequip part
		}
	}//end of EquipPins()

	public void ShopPurchase(){
		Debug.Log("Shop Purchase activated");
		if(GlobalVariableManager.Instance.TOTAL_TRASH >= price){
			if(!bought && popupOnce == 0){
				//S_Ev_shop myManager = GameObject.Find("Manager").GetComponent<S_Ev_shop>();
				myManager.GetComponent<S_Ev_shop>().TogglePopupEnable();
				Image purchasePopup = GameObject.Find("purchasePopup").GetComponent<Image>();
				purchasePopup.GetComponent<GUI_OptionsPopupBehavior>().setGameObjectToActivate(this.gameObject);
				GlobalVariableManager.Instance.PLAYER_CAN_MOVE = false;
				popupOnce = 1;
			}else if(popupOnce == 1){
				//activated by GUI_optionsPopupBehavior
				GlobalVariableManager.Instance.PLAYER_CAN_MOVE = true;
				GlobalVariableManager.Instance.characterUpgradeArray[0].Replace(GlobalVariableManager.Instance.characterUpgradeArray[0][myPositionInString],'o');
				gameObject.GetComponent<tk2dSprite>().color = new Color(255f,255f,255f,.1f); //fade
				GlobalVariableManager.Instance.TOTAL_TRASH -= price;
				Instantiate(smallTextDisplay,transform.position,Quaternion.identity);
				smallTextDisplay.GetComponent<tk2dSprite>().SetSprite(soldTextSprite);

				Image totalTrashDisplay = GameObject.Find("totalTrashDisplay").GetComponent<Image>();
				totalTrashDisplay.GetComponent<SpecialEffectsBehavior>().StartCoroutine("Shake",1f);
				bought = true;
				popupOnce = 0;
			}
		}

	}

	public void SetPopupBack(){
		//needed for 'no' options at shop purchase popup
		popupOnce = 0;
	}

	public void SetPinData(int myPos){

		if(myPos == 0){
			//Bulky Bag
			myTitleAnimationName = "pinTitle_bulkyBag";
			myDescription = "Bag holds more trash";
			myPPvalue = 2;
			price = 7;
		}else if(myPos == 1){
			//Time Enough
			myTitleAnimationName = "pinTitle_timeEnough";
			myDescription = "Clock pickups set the day back more.";
			myPPvalue = 2;
			mySpotInPinEquipScreen = 1;
			price = 5;
		}else if(myPos == 2){
			//Defense Up pin
			myTitleAnimationName = "pinTitle_speedy";
			myDescription = "+10% walking speed. That's it, really.";
			myPPvalue = 1;
			mySpotInPinEquipScreen = 2;
			price = 6;
		}else if(myPos == 3){
			//Cursed Pin
			myTitleAnimationName = "pinTitle_cursed";
			myDescription = "A random positive or negative curse in inflicted upon you each day.";
			myPPvalue = 1;
			mySpotInPinEquipScreen = 3;
			price = 4;
		}else if(myPos == 4){
			//Blistering Bond
			myTitleAnimationName = "pinTitle_blisteringBond";
			myDescription = "For each consecutive day worn +1 Max HP, but lose -1 HP at day start.";
			myPPvalue = 2;
			mySpotInPinEquipScreen = 10;
			price = 6;
		}else if(myPos == 5){
			//Passive Pillage
			myTitleAnimationName = "pinTitle_passivePillage";
			myDescription = "Speed increase for every Trash collected. Resets when take damage. It's supposed to be Ghandi.";
			myPPvalue = 3;
			mySpotInPinEquipScreen = 11;
			price = 10;
		}else if(myPos == 6){
			//Faithful Weapin
			myTitleAnimationName = "pinTitle_faithfulWeapin";
			myDescription = "Keep your melee weapon level when you die.";
			myPPvalue = 2;
			mySpotInPinEquipScreen = 12;
			price = 4;
		}else if(myPos == 7){
			//My Legacy
			myTitleAnimationName = "pinTitle_legacy";
			myDescription = "If you perish with less than 5 Trash, you hold on to the trash upon returning to the Dumpster.";
			myPPvalue = 1;
			mySpotInPinEquipScreen = 13;
			price = 7;
		}else if(myPos == 8){
			//Sharing Pin
			myTitleAnimationName = "pinTitle_sharing";
			myDescription = "Enemy is more likely to drop an item when killed.";
			myPPvalue = 1;
			mySpotInPinEquipScreen = 20;
			price = 12;
		}else if(myPos == 9){
			//mo' garbage mo' problems
			myTitleAnimationName = "pinTitle_moGarbageMoProblems";
			myDescription = "+2 Max HP when have less than 5 Trash";
			myPPvalue = 2;
			mySpotInPinEquipScreen = 21;
			price = 0;
		}else if(myPos == 11){
			//haulin hero
			myTitleAnimationName = "pinTitle_haulinHero";
			myDescription = "Slight increase in movement speed when carrying Large Trash.";
			myPPvalue = 1;
			mySpotInPinEquipScreen = 22;
			price = 5;
		}else if(myPos == 12){
			//Apple Plus
			myTitleAnimationName = "pinTitle_applePlus";
			myDescription = "Increases Max HP by 1.";
			myPPvalue = 2;
			mySpotInPinEquipScreen = 23;
			price = 8;
		}else if(myPos == 13){
			//Pretty Lucky
			myTitleAnimationName = "pinTitle_prettyLucky";
			myDescription = "Enemies and projectiles sometimes miss when hit player.";
			myPPvalue = 1;
			mySpotInPinEquipScreen = 30;
			price = 7;
		}else if(myPos == 14){
			//Lucky Day
			myTitleAnimationName = "pinTitle_luckyDay";
			myDescription = "Enemies and projectiles will often miss when they hit you.";
			myPPvalue = 3;
			mySpotInPinEquipScreen = 31;
			price = 14;
		}else if(myPos == 15){
			//Pointy Pin
			myTitleAnimationName = "pinTitle_pointyPin";
			myDescription = "Half the time you're hit, the enemy will take damage too.";
			myPPvalue = 2;
			mySpotInPinEquipScreen = 32;
			price = 8;
		}else if(myPos == 18){
			//The pin that can kill the past
			myTitleAnimationName = "pinTitle_pinThatCanKillThePast";
			myDescription = "The current day number does not increase while wearing this pin. Can only wear for 3 days.";
			myPPvalue = 3;
			mySpotInPinEquipScreen = 41;
			price = 0;
		}else if(myPos == 19){
			//Waste Warrior
			myTitleAnimationName = "pinTitle_wasteWarrior";
			myDescription = "Your melee weapon can be upgraded a fourth time.";
			myPPvalue = 4;
			mySpotInPinEquipScreen = 42;
			price = 0;
		}else if(myPos == 20){
			//Stay Back
			myTitleAnimationName = "pinTitle_stayBack";
			myDescription = "+1 Attack Damage when at 1 HP.";
			myPPvalue = 3;
			mySpotInPinEquipScreen = 43;
			price = 5;
		}else if(myPos == 21){
			//Scrap City
			myTitleAnimationName = "pinTitle_scrapCity";
			myDescription = "Enemies drop more Scrap when killed";
			myPPvalue = 2;
			mySpotInPinEquipScreen = 50;
			price = 6;
		}else if(myPos == 22){
			//Quenpin
			myTitleAnimationName = "pinTitle_quenpinTarantino";
			myDescription = "Getting hit causes bullets to fly around the area.";
			myPPvalue = 4;
			mySpotInPinEquipScreen = 51;
			price = 25;
		}else if(myPos == 23){
			//Hungry For More
			myTitleAnimationName = "pinTitle_hungryForMore";
			myDescription = "More Trash will be spawned in the current world.";
			myPPvalue = 2;
			mySpotInPinEquipScreen = 53;
			price = 8;
		}else if(myPos == 24){
			//Next of Pin
			myTitleAnimationName = "pinTitle_nextOfPin";
			myDescription = "Your dropped trash spawns in starting room.";
			myPPvalue = 3;
			mySpotInPinEquipScreen = 3;
			price = 0;
		}else if(myPos == 25){
			//Call of the Wild
			myTitleAnimationName = "pinTitle_callOfTheWild";
			myDescription = "Entering an area of the world you haven't previously explored gives you +1 trash.";
			myPPvalue = 6;
			mySpotInPinEquipScreen = 0;
			price = 13;
		}else if(myPos == 26){
			//Talky Time
			myTitleAnimationName = "pinTitle_talkyTime";
			myDescription = "Activities with friends take up less time.";
			myPPvalue = 2;
			mySpotInPinEquipScreen = 1;
			price = 11;
		}else if(myPos == 27){
			//Vitality Vision
			myTitleAnimationName = "pinTitle_vitalityVision";
			myDescription = "See the numbered HP of enemies and bosses.";
			myPPvalue = 1;
			mySpotInPinEquipScreen = 2;
			price = 0;
		}else if(myPos == 28){
			//Claw Pin
			myTitleAnimationName = "pinTitle_clawPin";
			myDescription = "Start the day off with a claw level weapon.";
			myPPvalue = 3;
			mySpotInPinEquipScreen = 3;
			price = 0;
		}else if(myPos == 29){
			//Waifu Wanted
			myTitleAnimationName = "pinTitle_waifuWanted";
			myDescription = "Small chance of spawned enemy turning into a harmless and beautiful Japanese woman.";
			myPPvalue = 1;
			mySpotInPinEquipScreen = 10;
			price = 6;
		}else if(myPos == 30){
			//Piercing Pin
			myTitleAnimationName = "pinTitle_piercingPin";
			myDescription = "Attacks can damage Armored Enemies, regardless of current Bag selected.";
			myPPvalue = 3;
			mySpotInPinEquipScreen = 3;
			price = 0;
		}else if(myPos == 31){
			//Scrappy Shinobi
			myTitleAnimationName = "pinTitle_scrappyShinobi";
			myDescription = "Faster walking speed. Faster swing speed.";
			myPPvalue = 2;
			mySpotInPinEquipScreen = 12;
			price = 14;
		}else if(myPos == 32){
			//Nice guy
			myTitleAnimationName = "pinTitle_niceGuy";
			myDescription = "Grants the ability to compliment eels";
			myPPvalue = 1;
			mySpotInPinEquipScreen = 13;
			price = 99;
		}else if(myPos == 33){
			//fortunes friend
			myTitleAnimationName = "pinTitle_fortunesFriend";
			myDescription = "Increases luck with positive chance-based pin effects.";
			myPPvalue = 1;
			mySpotInPinEquipScreen = 20;
			price = 9;
		}else if(myPos == 34){
			//Trashier Tmw
			myTitleAnimationName = "pinTitle_trashierTomorrow";
			myDescription = "Wearing this pin today increases amount of Trash in world tomorrow.";
			myPPvalue = 1;
			mySpotInPinEquipScreen = 21;
			price = 9;
		}else if(myPos == 35){
			//Fuel Efficient Pin
			myTitleAnimationName = "pinTitle_fuelEfficient";
			myDescription = "Ending the day at the Dumpster today increases the length of the day tomorrow.";
			myPPvalue = 1;
			mySpotInPinEquipScreen = 23;
			price = 11;
		}else if(myPos == 36){
			//Deaths Deal
			myTitleAnimationName = "pinTitle_deathsDeal";
			myDescription = "-1 Max HP. Revive in your current location the first time you fall in battle this day.";
			myPPvalue = 3;
			mySpotInPinEquipScreen = 22;
			price = 7;
		}else if(myPos == 37){
			//hardly workin
			myTitleAnimationName = "pinTitle_hardlyWorkin";
			myDescription = "Time goes by slower if you have zero trash.";
			myPPvalue = 1;
			mySpotInPinEquipScreen = 30;
			price = 7;
		}else if(myPos == 38){
			//MagneticPin
			myTitleAnimationName = "pinTitle_magneticPin";
			myDescription = "Trash in the area will graviate toward you.";
			myPPvalue = 1;
			mySpotInPinEquipScreen = 31;
			price = 7;
		}else if(myPos == 39){
			//Trash Power
			myTitleAnimationName = "pinTitle_trashPower";
			myDescription = "Collecting Trash increases your weapon upgrade bar.";
			myPPvalue = 2;
			mySpotInPinEquipScreen = 32;
			price = 0;
		}else if(myPos == 40){
			//Trash Return
			myTitleAnimationName = "pinTitle_trashReturn";
			myDescription = "Return Trash at Dumpster without having to end the day.";
			myPPvalue = 6;
			mySpotInPinEquipScreen = 33;
			price = 0;
		}else if(myPos == 41){
			//No Trash Left Behind
			myTitleAnimationName = "pinTitle_noTrashLeftBehind";
			myDescription = "+2 Max HP when have less than 5 Trash";
			myPPvalue = 2;
			mySpotInPinEquipScreen = 40;
			price = 0;
		}else if(myPos == 42){
			//Rotten Apple Plus
			myTitleAnimationName = "pinTitle_rottenApplePlus";
			myDescription = "+2 Max HP. Can't regain lost HP";
			myPPvalue = 3;
			mySpotInPinEquipScreen = 41;
			price = 0;
		}else if(myPos == 43){
			//Spiritual Safety
			myTitleAnimationName = "pinTitle_spiritualSafety";
			myDescription = "Significantly longer invincibility time after being hit.";
			myPPvalue = 2;
			mySpotInPinEquipScreen = 42;
			price = 0;
		}else if(myPos == 44){
			//Duck SFX
			myTitleAnimationName = "pinTitle_duckSFX";
			myDescription = "Change the sound of your attacks to duck sounds.";
			myPPvalue = 0;
			mySpotInPinEquipScreen = 43;
			price = 0;
		}else if(myPos == 45){
			//Workin' Hard
			myTitleAnimationName = "pinTitle_workinHard";
			myDescription = "Time goes by slower if your trash bag is full.";
			myPPvalue = 2;
			mySpotInPinEquipScreen = 50;
			price = 0;
		}else if(myPos == 46){
			//warp pin
			myTitleAnimationName = "pinTitle_warpPin";
			myDescription = "Return to Dumpster from pause menu. (Not while carrying Large Trash.)";
			myPPvalue = 5;
			mySpotInPinEquipScreen = 51;
			price = 0;
		}else if(myPos == 47){
			//Devils Deal
			myTitleAnimationName = "pinTitle_devilsDeal";
			myDescription = "30% longer day. If you die, the day ends immediately.";
			myPPvalue = 3;
			mySpotInPinEquipScreen = 52;
			price = 9;
		}else if(myPos == 48){
			//Hero of Grime
			myTitleAnimationName = "pinTitle_heroOfGrime";
			myDescription = "Melee weapon shoots beam when at full health.";
			myPPvalue = 2;
			mySpotInPinEquipScreen = 53;
			price = 0;
		}else if(myPos == 49){
			//Attack Up
			myTitleAnimationName = "pinTitle_attkUp1";
			myDescription = "Increases base attack damage by 1.";
			myPPvalue = 1;
			mySpotInPinEquipScreen = 0;
			price = 0;
		}else if(myPos == 50){
			//Attack up 2
			myTitleAnimationName = "pinTitle_attackUp2";
			myDescription = "Increases base attack damage by 1.";
			myPPvalue = 1;
			mySpotInPinEquipScreen = 1;
			price = 0;
		}else if(myPos == 51){
			//Defense Up pin
			myTitleAnimationName = "pinTitle_defUp1";
			myDescription = "Increase base Defense by 1.";
			myPPvalue = 1;
			mySpotInPinEquipScreen = 2;
			price = 13;
		}else if(myPos == 52){
			//Defense Up pin
			myTitleAnimationName = "pinTitle_defUp2";
			myDescription = "Increase base Defense by 1.";
			myPPvalue = 1;
			mySpotInPinEquipScreen = 3;
			price = 13;
		}

		myPositionInString = myPos; //needed for pin equip, just make it only happen on this scene if it causes problem with shop
	}//end of pin data set

	public void SetMySpot(int i){
		mySpotInShop = i;
	}
}

