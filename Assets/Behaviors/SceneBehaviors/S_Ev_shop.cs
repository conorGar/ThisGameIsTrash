using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class S_Ev_shop : MonoBehaviour {

	//public GameObject tutorialPopup;
	//public Sprite tutPopupSprite;
    Ev_PinBehavior currentPin;
	public GUI_OptionsPopupBehavior purchasePopup;
	public GameObject harry;
	public GameObject priceDisplay;
	public TextMeshProUGUI priceDisplayText;
	public TextMeshProUGUI pinDescriptionText;
	public TextMeshProUGUI pinTitleText;
	public GameObject ppDisplay;
	public GameObject pinInfoDisplay;
	public List<GameObject> pedestalSpawnPoints = new List<GameObject>();
	public GameObject pinShopFront;
	public GameObject pinShopBack;
	//public BoxCollider2D enterShopTriggerBox;
	//public BoxCollider2D exitShopTriggerBox;

    public PinDefinition upgrade1;
    public PinDefinition upgrade2;
    public PinDefinition upgrade3;

	List<PinDefinition> nonShopPins = new List<PinDefinition>();
	public GameObject player;
	GameObject manager;
	public List<GameObject> todaysPins = new List<GameObject>(); 
	//bool enteredShop; //just used for shop outside visual changes


    void Start() {

    	
        /*if(GlobalVariableManager.Instance.WORLD_SIGNS_READ[0][9] != 'o'){
			if(GlobalVariableManager.Instance.TUT_POPUP_ON){
				Instantiate(tutorialPopup,transform.position,Quaternion.identity);
				tutorialPopup.GetComponent<SpriteRenderer>().sprite = tutPopupSprite;
			}
			GlobalVariableManager.Instance.WORLD_SIGNS_READ[0].Replace(GlobalVariableManager.Instance.WORLD_SIGNS_READ[0][9],'o');
		}*/

        manager = GameObject.Find("Manager");

        // TODO: Maybe move this to a configuration in the PinManager?
        //-----------cant spawn any pins that can be found in world ----------------//
        PIN NOT_FOR_SALE_PINS = PIN.FAITHFULWEAPON
            | PIN.MYLEGACY
            | PIN.SHARING
            | PIN.MOGARBAGEMOPROBLEMS
            | PIN.HAULINHERO
            // | PIN.PRETTYLUCKY  - pretty lucky = can be bought in shop currently
            | PIN.POINTYPIN
            | PIN.LUCKYDAY
            | PIN.THEPINTHATCANKILLTHEPAST
            | PIN.WASTEWARRIOR
            | PIN.STAYBACK
            | PIN.TALKYTIME
            | PIN.VITALITYVISION
            | PIN.CLAWPIN
            | PIN.WAIFUWANTED
            | PIN.PIERCINGPIN
            | PIN.SCRAPPYSHINOBI
            | PIN.NICEGUY
            | PIN.HARDLYWORKIN;

        GlobalVariableManager.Instance.shopPins = new List<PinDefinition> { null, null, null };




	

		//---------------------------------------set pins in today's shop------------------------------------------//

            //Uses shopPins list to place values for today's pins
            //Reset when activate "Continue" truck at hub.
            upgrade1 = null;
            upgrade2 = null;
            upgrade3 = null;

            // TODO: Maybe define this in the PinManager so it can be tweaked?
            int pinMin = 0;
            int pinMax = 39;

            // Get 3 random pins unless there aren't anymore to be found.
            upgrade1 = PinManager.Instance.GetRandomPin((x) => GlobalVariableManager.Instance.IsPinDiscovered(x.Type)
                && (NOT_FOR_SALE_PINS & x.Type) != x.Type,
                pinMin, pinMax);

            // first upgrade found
            if (upgrade1 != null)
            {
                GlobalVariableManager.Instance.shopPins[0] = upgrade1;
                upgrade2 = PinManager.Instance.GetRandomPin((x) => GlobalVariableManager.Instance.IsPinDiscovered(x.Type)
                    && (NOT_FOR_SALE_PINS & x.Type) != x.Type
                    && x.Type != upgrade1.Type, pinMin, pinMax);

                // second upgrade found
                if (upgrade2 != null)
                {
                    GlobalVariableManager.Instance.shopPins[1] = upgrade2;
                    upgrade3 = PinManager.Instance.GetRandomPin((x) => GlobalVariableManager.Instance.IsPinDiscovered(x.Type)
                        && (NOT_FOR_SALE_PINS & x.Type) != x.Type
                        && x.Type != upgrade1.Type && x.Type != upgrade2.Type, pinMin, pinMax);

                    if (upgrade3 != null)
                        GlobalVariableManager.Instance.shopPins[2] = upgrade3;
                }
            }

	

		SpawnPins();

        // Register Events
        purchasePopup.RegisterCloseEvent(OnCloseEvent);
        purchasePopup.RegisterOptionEvent(OnOptionsEvent);
    }

    void OnDisable()
    {
        PinManager.Instance.RefreshNewPinIcon();
    }

    /*void OnCollisionEnter2D(Collision2D col){
    	if(col.gameObject.name = enterShopTriggerBox.gameObject.name && !enteredShop){
    		pinShopFront.GetComponent<Animator>().Play("shopFrontFade",-1,0f);
    		pinShopBack.GetComponent<Animator>().Play("FadeOut",-1,0f);
    		enteredShop = true;
		}else if(col.gameObject.name = exitShopTriggerBox.gameObject.name && enteredShop){
			pinShopFront.GetComponent<SpriteRenderer>().color = Color.white;
			pinShopBack.GetComponent<SpriteRenderer>().color = Color.white;
			enteredShop = false;
    	}
    }*/

    public void EnterShop(){
		pinShopFront.GetComponent<Animator>().enabled = true;
		pinShopFront.GetComponent<Animator>().Play("shopFrontFade",-1,0f);
		pinShopBack.GetComponent<Animator>().enabled = true;

    	pinShopBack.GetComponent<Animator>().Play("FadeOut",-1,0f);
    }

    public void ExitShop(){
		pinShopFront.GetComponent<Animator>().enabled = false;
		pinShopBack.GetComponent<Animator>().enabled = false;
		pinShopFront.GetComponent<SpriteRenderer>().color = Color.white;
		pinShopBack.GetComponent<SpriteRenderer>().color = Color.white;
    }

    void OnDestroy()
    {
        // Unregister Events
        purchasePopup.UnregisterCloseEvent(OnCloseEvent);
        purchasePopup.UnregisterOptionEvent(OnOptionsEvent);
    }

    void OnCloseEvent()
    {
        if (currentPin != null) {
            currentPin.SetPopupBack();
        }
    }

    void OnOptionsEvent(int optionNum)
    {
        switch (optionNum) {
            case 0:
                if (currentPin != null) {
                    currentPin.ShopPurchase();
                }
                purchasePopup.gameObject.SetActive(false);
                break;
        }
    }

    public void SetCurrentPin(Ev_PinBehavior pin)
    {//used when purchase pin. Activated by Ev_pinBehavior
        currentPin = pin;
    }

    public void TogglePopupEnable(){
		if(purchasePopup.gameObject.activeInHierarchy){
			pinInfoDisplay.SetActive(true);
			purchasePopup.gameObject.SetActive(false);
		}else{
			purchasePopup.gameObject.SetActive(true);
			pinInfoDisplay.SetActive(false);
		}
	}
	
	void Update () {
		if(todaysPins.Count>2 && GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)){
			if(Vector2.Distance(todaysPins[0].transform.position,player.transform.position) <4 && todaysPins[0].GetComponent<Ev_PinBehavior>().bought == false){
				ShowThings(todaysPins[0].GetComponent<Ev_PinBehavior>().GetData());
			}else if(Vector2.Distance(todaysPins[1].transform.position,player.transform.position) <4 && todaysPins[1].GetComponent<Ev_PinBehavior>().bought == false){
				ShowThings(todaysPins[1].GetComponent<Ev_PinBehavior>().GetData());
			}else if(Vector2.Distance(todaysPins[2].transform.position,player.transform.position) <4 && todaysPins[2].GetComponent<Ev_PinBehavior>().bought == false){
				ShowThings(todaysPins[2].GetComponent<Ev_PinBehavior>().GetData());
			}else{
				HideThings();
			}
		}
	}

	public void ShowThings(PinDefinition pindata){
		//Debug.Log("Shop is showing things...");
		harry.GetComponent<tk2dSpriteAnimator>().Play("talk");
		priceDisplayText.text = pindata.price.ToString();
		pinDescriptionText.text = pindata.description;
		pinTitleText.text = pindata.displayName;
		priceDisplay.SetActive(true);
		pinInfoDisplay.SetActive(true);
		for(int i = 0; i < pindata.ppValue; i++){
			ppDisplay.transform.GetChild(i).gameObject.SetActive(true);
		}
	}

	public void HideThings(){
		harry.GetComponent<tk2dSpriteAnimator>().Play("idle");
		for(int i = 0; i < ppDisplay.transform.childCount; i++){
			ppDisplay.transform.GetChild(i).gameObject.SetActive(true);
		}
		priceDisplay.SetActive(false);
		pinInfoDisplay.SetActive(false);
	}

	void SpawnPins(){
		PinDefinition currentPin = null;

		tk2dSprite pinsSprite;
		GameObject spawnedPin;
		Vector2 spawnPos = new Vector2();
        for (int i = 0; i < 3; i++)
        {
			Debug.Log("**PIN SPAWN** - Got here - Pin Spawn for shop x*x*x*x*x*x*x*x*" + GlobalVariableManager.Instance.shopPins.Count);
			if(i == 0){
				currentPin = upgrade1;
				spawnPos = pedestalSpawnPoints[0].transform.position;
			}else if(i == 1){
                currentPin = upgrade2;
				spawnPos = pedestalSpawnPoints[1].transform.position;

			}else if(i == 2){
                currentPin = upgrade3;
				spawnPos = pedestalSpawnPoints[2].transform.position;

			}

            spawnedPin = ObjectPool.Instance.GetPooledObject("ShopPin", spawnPos);

            // populate the pin data
            spawnedPin.name = currentPin.displayName;
            Debug.Log("**PIN SPAWN** - spawned pin with name:" + spawnedPin.name +currentPin.sprite);
          //  var sprite = spawnedPin.GetComponent<tk2dSprite>();
          // sprite.SetSprite(currentPin.sprite);

            var behavior = spawnedPin.GetComponent<Ev_PinBehavior>();
            behavior.SetPinData(currentPin);
            behavior.SetSprite(currentPin.sprite);

            spawnedPin.SetActive(true);
            todaysPins.Add(spawnedPin);
            //spawnedPin.GetComponent<Ev_PinBehavior>().SetPinData(currentUpgradeCheck);
            //behavior.SetMySpot(i+1);
		}
	}
}
