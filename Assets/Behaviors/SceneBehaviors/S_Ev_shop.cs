﻿using System.Collections;
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
	public GameObject descriptionPopup;


    public PinDefinition upgrade1;
    public PinDefinition upgrade2;
    public PinDefinition upgrade3;

	List<PinDefinition> nonShopPins = new List<PinDefinition>();
	GameObject player;
	GameObject manager;
	List<GameObject> todaysPins = new List<GameObject>(); 

    void Start() {


        /*if(GlobalVariableManager.Instance.WORLD_SIGNS_READ[0][9] != 'o'){
			if(GlobalVariableManager.Instance.TUT_POPUP_ON){
				Instantiate(tutorialPopup,transform.position,Quaternion.identity);
				tutorialPopup.GetComponent<SpriteRenderer>().sprite = tutPopupSprite;
			}
			GlobalVariableManager.Instance.WORLD_SIGNS_READ[0].Replace(GlobalVariableManager.Instance.WORLD_SIGNS_READ[0][9],'o');
		}*/

        player = GameObject.Find("Jim");
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
    {
        currentPin = pin;
    }

    public void TogglePopupEnable(){
		if(purchasePopup.gameObject.activeInHierarchy){
			purchasePopup.gameObject.SetActive(false);
		}else{
			purchasePopup.gameObject.SetActive(true);
		}
	}
	
	void Update () {

		if(Vector2.Distance(todaysPins[0].transform.position,player.transform.position) <5){
			
		}else if(Vector2.Distance(todaysPins[1].transform.position,player.transform.position) <5){

		}else if(Vector2.Distance(todaysPins[2].transform.position,player.transform.position) <5){

		}
	}

	public void ShowThings(Ev_PinBehavior highlightedPin){
		harry.GetComponent<tk2dSpriteAnimator>().Play("talk");
		//priceDisplayText.text = highlightedPin.
		priceDisplay.SetActive(true);
	}

	public void HideThings(){
		harry.GetComponent<tk2dSpriteAnimator>().Play("idle");
		priceDisplay.SetActive(false);
	}

	void SpawnPins(){
		PinDefinition currentPin = null;
		float xPos = 0f;
		float yPos = 0f;
		tk2dSprite pinsSprite;
		GameObject spawnedPin;

        for (int i = 0; i < GlobalVariableManager.Instance.shopPins.Count; ++i)
        {
			if(i == 0){
				currentPin = upgrade1;
				xPos = 8.89f;
				yPos = 10f;
			}else if(i == 1){
                currentPin = upgrade2;
				xPos = 14f;
				yPos = 10.6f;
			}else if(i == 2){
                currentPin = upgrade3;
				xPos = 18.6f;
				yPos = 10f;
			}

            spawnedPin = ObjectPool.Instance.GetPooledObject("Pin", new Vector2(xPos,yPos));

            // populate the pin data
            spawnedPin.name = currentPin.displayName;

            var sprite = spawnedPin.GetComponent<tk2dSprite>();
            sprite.SetSprite(currentPin.sprite);

            var behavior = spawnedPin.GetComponent<Ev_PinBehavior>();
            behavior.SetPinData(currentPin);

            spawnedPin.SetActive(true);
            todaysPins.Add(spawnedPin);
            //spawnedPin.GetComponent<Ev_PinBehavior>().SetPinData(currentUpgradeCheck);
            behavior.SetMySpot(i+1);
		}
	}
}
