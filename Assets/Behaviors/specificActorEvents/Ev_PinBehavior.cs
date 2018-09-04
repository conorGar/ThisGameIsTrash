using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 
public class Ev_PinBehavior : MonoBehaviour {

	public GameObject smallTextDisplay;
	public GameObject ppDisplayIcon; //neded for pin equip screen
	public GameObject equippedBox;
	public string soldTextSprite = "sold";
    PinDefinition pinData = null;
    //int myPositionInString = 0;

    Image descriptionBox;

	int setArrowPosOnce = 0;
	int popupOnce;
	int mySpotInShop;
	bool bought = false;
	bool inShop = false;
	float startingY;
	SpecialEffectsBehavior mySFX;
	GameObject player;
	GameObject shopLight;
	List<GameObject> myIcons = new List<GameObject>();
	GameObject myEquippedBox;

	void Start () {
		/*if(GlobalVariableManager.Instance.characterUpgradeArray[1][18] == 'o'){
			//pin perk 5 - every pin costs one less pp to equip
			if(pinData.price > 1){
                pinData.price -= 1;
			}else{
                pinData.price = 1;
			}
		}*/

		mySFX = gameObject.GetComponent<SpecialEffectsBehavior>();
		player = GameObject.Find("Jim");

		if(GlobalVariableManager.Instance.ROOM_NUM == 97){
			inShop = true;
			shopLight = GameObject.Find("shopLight");
			descriptionBox = GameObject.Find("description").GetComponent<Image>();
		}else if(GlobalVariableManager.Instance.ROOM_NUM == 101){
            if (!GlobalVariableManager.Instance.IsPinDiscovered(pinData.Type)){
                gameObject.GetComponent<tk2dSprite>().color = new Color(0f,0f,0f,1f);//blacked out if not owned
			}else{
				float xSpawnAdjust = -.6f;

				for(int i = 0; i < pinData.ppValue; i++){//little pp displays
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
			if(IsPinDiscovered() && GlobalVariableManager.Instance.MENU_SELECT_STAGE != 10 && GlobalVariableManager.Instance.MENU_SELECT_STAGE != 20 && GlobalVariableManager.Instance.MENU_SELECT_STAGE != 30){
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
                    PinManager.Instance.PinTitle.gameObject.SetActive(true);
					PinManager.Instance.DescriptionText.enabled = true;
					descriptionBox.enabled = true;
					descriptionBox.GetComponent<GUIEffects>().Start();
					PinManager.Instance.PPDisplay.SetDisplayedIcons(pinData.ppValue);
					PinManager.Instance.Shop.ShowThings(pinData.price);
					shopLight.GetComponent<SpecialEffectsBehavior>().SmoothMovementToPoint(transform.position.x, transform.position.y +2,.4f); 
					setArrowPosOnce = 1;//makes sure doesnt set to zero when close to other pins in shop
					Debug.Log(setArrowPosOnce);
				}
			}else{
				if(setArrowPosOnce == 1){
					mySFX.SmoothMovementToPoint(transform.position.x,startingY,.2f);
					GlobalVariableManager.Instance.ARROW_POSITION = 0;
                    PinManager.Instance.PinTitle.gameObject.SetActive(false);
					descriptionBox.enabled = false;
                    PinManager.Instance.DescriptionText.enabled = false;
                    PinManager.Instance.PPDisplay.Clear();
                    PinManager.Instance.Shop.HideThings();
					setArrowPosOnce = 0;
					Debug.Log(setArrowPosOnce);
				}
			}



			//------------what to do when arrow position is my position------------/
			if(bought == false && GlobalVariableManager.Instance.ARROW_POSITION == mySpotInShop){
				if(PinManager.Instance.PinTitle.gameObject != null){
                    PinManager.Instance.PinTitle.text = pinData.displayName;
				}
				if(PinManager.Instance.DescriptionText != null){
                    PinManager.Instance.DescriptionText.text = pinData.description;
				}

				if(ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT) && popupOnce == 0){
					ShopPurchase();
				}
			}
			//-----------------------------------------------------------------//


		}
	}

	public void AtEquipScreen(){
		if(IsPinDiscovered()){
            PinManager.Instance.DescriptionText.text = pinData.description;
            PinManager.Instance.PinTitle.text = pinData.displayName;
            PinManager.Instance.PPDisplay.SetDisplayedIcons(pinData.ppValue);
			PinManager.Instance.PinDisplaySprite.SetSprite(gameObject.GetComponent<tk2dSprite>().CurrentSprite.name);

            Debug.Log("Pin Sprite Name : " + this.gameObject.GetComponent<tk2dSprite>().CurrentSprite.name
                      + " Title Name : " + PinManager.Instance.PinTitle.text
                      + " Display Pin Sprite Name : " + PinManager.Instance.PinDisplaySprite.CurrentSprite.name);
        }
        else{
            PinManager.Instance.DescriptionText.text = "Buy or find this Pin to learn what powers it holds!";
            PinManager.Instance.PinTitle.text = "???";
            PinManager.Instance.PPDisplay.Clear();
		}
	}

	public bool EquipPin(){
        bool IsEquipped = false;
		bool isDejaVuPinAndIsUnlocked = false;
		if(GlobalVariableManager.Instance.IsPinDiscovered(PIN.THEPINTHATCANKILLTHEPAST) && pinData.Type == PIN.THEPINTHATCANKILLTHEPAST){
			isDejaVuPinAndIsUnlocked = true;
		}
		if(IsPinDiscovered()){
			if((!IsPinEquipped() || isDejaVuPinAndIsUnlocked) && GlobalVariableManager.Instance.PPVALUE >= pinData.ppValue){ // equip unequipped pin
                GlobalVariableManager.Instance.PINS_EQUIPPED |= pinData.Type; //set pin to active
                GlobalVariableManager.Instance.PPVALUE -= pinData.ppValue;
                IsEquipped = true;

                // Equip Pin that can kill the past
                if (pinData.Type == PIN.THEPINTHATCANKILLTHEPAST)
                    GlobalVariableManager.Instance.DEJAVUCOUNT += 4;

				for(int i = 0; i < myIcons.Count; i++){ //unshade the little pp icons below pin
					myIcons[i].GetComponent<SpriteRenderer>().color = new Color(255f,255f,255f,1f);
				}

				myEquippedBox = Instantiate(equippedBox,transform.position,Quaternion.identity);
				/*if(pinData.Type == PIN.APPLEPLUS){
				//Apple Plus pin
					GlobalVariableManager.Instance.characterUpgradeArray[3] = (int.Parse(GlobalVariableManager.Instance.characterUpgradeArray[3]) + 1).ToString();
				}else if(pinData.Type == PIN.ROTTENAPPLEPLUS){
				//Rotten Apple Plus
					GlobalVariableManager.Instance.characterUpgradeArray[3] = (int.Parse(GlobalVariableManager.Instance.characterUpgradeArray[3]) + 2).ToString();
				}else if(pinData.Type == PIN.ATTKUP1){
				//Attack Pin
					GlobalVariableManager.Instance.characterUpgradeArray[5] = (int.Parse(GlobalVariableManager.Instance.characterUpgradeArray[5]) + 1).ToString();
				}else if(pinData.Type == PIN.DEFUP1){
				//Defense Pin
					GlobalVariableManager.Instance.characterUpgradeArray[6] = (int.Parse(GlobalVariableManager.Instance.characterUpgradeArray[6]) + 1).ToString();
				}*/
			}else if(IsPinEquipped()){//Unequip pin
                GlobalVariableManager.Instance.PPVALUE += pinData.ppValue;
                GlobalVariableManager.Instance.PINS_EQUIPPED &= ~pinData.Type;
                IsEquipped = false;

                // Unequip Pin that can kill the past
                if (pinData.Type == PIN.THEPINTHATCANKILLTHEPAST){
					if(GlobalVariableManager.Instance.DEJAVUCOUNT != 3){
                        GlobalVariableManager.Instance.DEJAVUCOUNT -= 4;
					}else{
                        GlobalVariableManager.Instance.DEJAVUCOUNT = 99;
					}
				}
				for(int i = 0; i < myIcons.Count; i++){ //unshade the little pp icons below pin
					myIcons[i].GetComponent<SpriteRenderer>().color = new Color(0f,0f,0f,1f);
				}
				Destroy(myEquippedBox);

			
			}//end of unequip part
		}

        return IsEquipped;
	}//end of EquipPins()

	public void ShopPurchase(){
		Debug.Log("Shop Purchase activated");
		if(GlobalVariableManager.Instance.TOTAL_TRASH >= pinData.price){
			if(!bought && popupOnce == 0){
                PinManager.Instance.Shop.TogglePopupEnable();
				Image purchasePopup = GameObject.Find("purchasePopup").GetComponent<Image>();
				purchasePopup.GetComponent<GUI_OptionsPopupBehavior>().setGameObjectToActivate(this.gameObject);
				GlobalVariableManager.Instance.PLAYER_CAN_MOVE = false;
				popupOnce = 1;
			}else if(popupOnce == 1){
				//activated by GUI_optionsPopupBehavior
				GlobalVariableManager.Instance.PLAYER_CAN_MOVE = true;
                GlobalVariableManager.Instance.PINS_DISCOVERED |= pinData.Type;
                gameObject.GetComponent<tk2dSprite>().color = new Color(255f,255f,255f,.1f); //fade
				GlobalVariableManager.Instance.TOTAL_TRASH -= pinData.price;
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

	public void SetPinData(PinDefinition data){
        pinData = data;
	}//end of pin data set

	public void SetMySpot(int i){
		mySpotInShop = i;
	}

    //helpers
    private bool IsPinDiscovered()
    {
        return (GlobalVariableManager.Instance.PINS_DISCOVERED & pinData.Type) == pinData.Type;
    }

    public bool IsPinEquipped()
    {
        return (GlobalVariableManager.Instance.PINS_EQUIPPED & pinData.Type) == pinData.Type;
    }
}

