using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 
public class Ev_PinBehavior : MonoBehaviour {

    public tk2dSprite sprite;
    public GameObject smallTextDisplay;
	public GameObject ppDisplayIcon; //neded for pin equip screen
	public GameObject equippedBox;
	public GameObject highlightBox;
	public GameObject smallPPIcons;
	public string soldTextSprite = "sold";
    PinDefinition pinData = null;
	public AudioClip equipSFX;
	public AudioClip unEquipSFX;
    public GameObject newPinIcon;

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
	Animator myAnimator;
	void Start () {
		/*if(GlobalVariableManager.Instance.characterUpgradeArray[1][18] == 'o'){
			//pin perk 5 - every pin costs one less pp to equip
			if(pinData.price > 1){
                pinData.price -= 1;
			}else{
                pinData.price = 1;
			}
		}*/
		myAnimator = sprite.GetComponent<Animator>();
		mySFX = sprite.GetComponent<SpecialEffectsBehavior>();
		player = GameObject.Find("Jim");

        // Default the new pin to "off".
        newPinIcon.SetActive(false);

        if (GlobalVariableManager.Instance.ROOM_NUM == 97){
			inShop = true;
			shopLight = GameObject.Find("shopLight");
			descriptionBox = GameObject.Find("description").GetComponent<Image>();
		}else if(GlobalVariableManager.Instance.ROOM_NUM == 101){
            if (!IsPinDiscovered()){
                sprite.color = new Color(0f,0f,0f,1f);//blacked out if not owned
			}else{
                for (int i = 0; i < pinData.ppValue; i++) {
                    smallPPIcons.transform.GetChild(i).gameObject.SetActive(true);

                    if (IsPinEquipped()) {
                        smallPPIcons.transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                    }
                }

                if (!IsPinViewed())
                    newPinIcon.SetActive(true);
            }
		}

        for (int i = 0; i < pinData.ppValue; i++) {
            myIcons.Add(smallPPIcons.transform.GetChild(i).gameObject);
        }

        if (inShop){
			if(IsPinDiscovered() && GlobalVariableManager.Instance.MENU_SELECT_STAGE != 10 && GlobalVariableManager.Instance.MENU_SELECT_STAGE != 20 && GlobalVariableManager.Instance.MENU_SELECT_STAGE != 30){
				GameObject myTextDisplay = Instantiate(smallTextDisplay,transform.position,Quaternion.identity);
				Color currentColor = sprite.color;
                sprite.color = new Color(currentColor.r,currentColor.g,currentColor.b,.4f);//fade
				myTextDisplay.GetComponent<tk2dSprite>().SetSprite(soldTextSprite);
				bought = true;
			}

			startingY = gameObject.transform.position.y;
		}//end of inShop Check
	}

    void OnEnable()
    {
        Unhighlight();
    }

    void OnDisable()
    {
        Unhighlight();
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
			if(IsPinEquipped()){
				highlightBox.SetActive(true);
			}
            PinManager.Instance.DescriptionText.text = pinData.description;
            PinManager.Instance.PinTitle.text = pinData.displayName;
            PinManager.Instance.PPDisplay.SetDisplayedIcons(pinData.ppValue);
            PinManager.Instance.PinDisplaySprite.GetComponent<Renderer>().enabled = true;
            PinManager.Instance.PinDisplaySprite.SetSprite(sprite.CurrentSprite.name);
			
            Debug.Log("Pin Sprite Name : " + sprite.CurrentSprite.name
                      + " Title Name : " + PinManager.Instance.PinTitle.text
                      + " Display Pin Sprite Name : " + PinManager.Instance.PinDisplaySprite.CurrentSprite.name);
			if(!IsPinViewed()){
				Debug.Log("Added to PINS_VIEWED:" + pinData.displayName);
				GlobalVariableManager.Instance.PINS_VIEWED |= pinData.Type; //set pin as viewed
                newPinIcon.SetActive(false);
            }
        }
        else{
            PinManager.Instance.DescriptionText.text = "Buy or find this Pin to learn what powers it holds!";
            PinManager.Instance.PinTitle.text = "???";
            PinManager.Instance.PPDisplay.Clear();
            PinManager.Instance.PinDisplaySprite.GetComponent<Renderer>().enabled = false;
        }
	}

	public bool EquipPin(){
        bool IsEquipped = false;

        PinManager.Instance.equipSpark.transform.position = gameObject.transform.position;
        PinManager.Instance.equipSpark.Play();
		bool isDejaVuPinAndIsUnlocked = false;
		if(GlobalVariableManager.Instance.IsPinDiscovered(PIN.THEPINTHATCANKILLTHEPAST) && pinData.Type == PIN.THEPINTHATCANKILLTHEPAST){
			isDejaVuPinAndIsUnlocked = true;
		}
		if(IsPinDiscovered()){
			if((!IsPinEquipped() || isDejaVuPinAndIsUnlocked) && GlobalVariableManager.Instance.PP_STAT.GetCurrent() >= pinData.ppValue){ // equip unequipped pin
                GlobalVariableManager.Instance.PINS_EQUIPPED |= pinData.Type; //set pin to active
                GlobalVariableManager.Instance.PP_STAT.UpdateCurrent(-pinData.ppValue);
				myAnimator.SetTrigger("Equip");
                IsEquipped = true;
                SoundManager.instance.PlaySingle(equipSFX);
				highlightBox.SetActive(true);
                // Equip Pin that can kill the past
                if (pinData.Type == PIN.THEPINTHATCANKILLTHEPAST)
                    GlobalVariableManager.Instance.DEJAVUCOUNT += 4;

				for(int i = 0; i < myIcons.Count; i++){ //unshade the little pp icons below pin
					myIcons[i].GetComponent<SpriteRenderer>().color = new Color(255f,255f,255f,1f);
				}

				//myEquippedBox = Instantiate(equippedBox,transform.position,Quaternion.identity);
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
                GlobalVariableManager.Instance.PP_STAT.UpdateCurrent(+pinData.ppValue);
                GlobalVariableManager.Instance.PINS_EQUIPPED &= ~pinData.Type;
                IsEquipped = false;
				SoundManager.instance.PlaySingle(unEquipSFX);
				highlightBox.SetActive(false);
                // Unequip Pin that can kill the past
                if (pinData.Type == PIN.THEPINTHATCANKILLTHEPAST){
					if(GlobalVariableManager.Instance.DEJAVUCOUNT != 3){
                        GlobalVariableManager.Instance.DEJAVUCOUNT -= 4;
					}else{
                        GlobalVariableManager.Instance.DEJAVUCOUNT = 99;
					}
				}
				Debug.Log("UNEQUIPPED PIN" + myIcons.Count);

				for(int i = 0; i < myIcons.Count; i++){ //unshade the little pp icons below pin
					Debug.Log("Icons should've turned black!!");
					myIcons[i].GetComponent<SpriteRenderer>().color = Color.black;
				}
				//Destroy(myEquippedBox);

			
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
                PinManager.Instance.Shop.SetCurrentPin(this);
				GlobalVariableManager.Instance.PLAYER_CAN_MOVE = false;
				popupOnce = 1;
			}else if(popupOnce == 1){
				//activated by GUI_optionsPopupBehavior
				GlobalVariableManager.Instance.PLAYER_CAN_MOVE = true;
                GlobalVariableManager.Instance.PINS_DISCOVERED |= pinData.Type;
                sprite.color = new Color(255f,255f,255f,.1f); //fade
				GlobalVariableManager.Instance.TOTAL_TRASH -= pinData.price;
				Instantiate(smallTextDisplay,transform.position,Quaternion.identity);
                sprite.SetSprite(soldTextSprite);

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

    public void Highlight()
    {
        if (myAnimator != null)
            myAnimator.SetBool("IsHighlighted", true);
    }

    public void Unhighlight()
    {
        if (myAnimator != null)
            myAnimator.SetBool("IsHighlighted", false);
    }

    //helpers
    private bool IsPinDiscovered()
    {
        return (GlobalVariableManager.Instance.PINS_DISCOVERED & pinData.Type) == pinData.Type;
    }

    private bool IsPinViewed()
    {
        return (GlobalVariableManager.Instance.PINS_VIEWED & pinData.Type) == pinData.Type;
    }

    public bool IsPinEquipped()
    {
        return (GlobalVariableManager.Instance.PINS_EQUIPPED & pinData.Type) == pinData.Type;
    }
}

