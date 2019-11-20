using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using TMPro;
using UnityEngine.UI;

public class GUI_WeaponEquipScreen : GUI_MenuBase
{
	public enum NavigationState {
		TRASH_BAG,
		CURRENTLY_EQUIPPED
	}

	public TextMeshProUGUI descriptionText;
	public TextMeshProUGUI weaponStrTextDisplay;
	public TextMeshProUGUI weaponDmgTextDisplay;
	public Image weaponImage;
	public GameObject trashBagDisplayHolder;
	public GameObject equippedSectionHolder;
	public List<GameObject> inventory = new List<GameObject>();
	public List<GameObject> currentlyEquipped = new List<GameObject>();
	public GUI_WeaponEquipPopupMenu popupMenu;
	public GUI_ConsumablePopupMenu  itemPopupMenu;

	public NavigationState currentNavigationArea = NavigationState.TRASH_BAG;
	Hero currentlySelectedHero;

	int previouslyHighlightedArrowPos;

	void Start ()
	{
		//Spawn all weapons in inventory
		foreach(WeaponDefinition weapon in GlobalVariableManager.Instance.WeaponInventory){
			GameObject displayBox = ObjectPool.Instance.GetPooledObject("GUI_weapon_option_display");
			inventory.Add(displayBox);
			displayBox.GetComponent<GUI_WeaponDisplayBox>().weaponIcon.sprite = weapon.displaySprite;
			displayBox.transform.parent = trashBagDisplayHolder.transform;
			displayBox.transform.localPosition = new Vector2(-12 +(3*((inventory.Count-1)%5)), 5.2f - (3* Mathf.Floor((inventory.Count-1)/5)));
		}

		//Spawn all consumables in inventory
		foreach(ConsumableItemOption item in GlobalVariableManager.Instance.CONSUMABLE_INVENTORY){
			GameObject displayBox = ObjectPool.Instance.GetPooledObject("GUI_item_option_display");
			inventory.Add(displayBox);
			displayBox.GetComponent<GUI_WeaponDisplayBox>().weaponIcon.sprite = item.myItemDefinition.displaySprite;
			displayBox.transform.parent = trashBagDisplayHolder.transform;
			displayBox.transform.localPosition = new Vector2(-12 +(3*((inventory.Count-1)%5)), 5.2f - (3* Mathf.Floor((inventory.Count-1)/5)));
		}

		currentlySelectedHero = GlobalVariableManager.Instance.HeroData[0]; // start with Jim
		maxArrowPos = inventory.Count;
		GameStateManager.Instance.PushState(typeof(PauseMenuState));
	}

	void OnEnable(){
		
	}


	// Update is called once per frame
	void Update ()
	{
		if (GameStateManager.Instance.GetCurrentState() == typeof(PauseMenuState)) {
			if ((ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVERIGHT)
			|| ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKRIGHT)) && arrowPos < maxArrowPos -1) {
				if(currentNavigationArea == NavigationState.TRASH_BAG){
					inventory[arrowPos].GetComponent<GUI_WeaponDisplayBox>().UnHighlight();
				}else{
					currentlyEquipped[arrowPos].GetComponent<GUI_WeaponDisplayBox>().UnHighlight();
				}
            	Navigate("right");
			}else if ((ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVELEFT) 
			|| ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKLEFT)) && arrowPos > 0){
				if(currentNavigationArea == NavigationState.TRASH_BAG){
					inventory[arrowPos].GetComponent<GUI_WeaponDisplayBox>().UnHighlight();
				}else{
					currentlyEquipped[arrowPos].GetComponent<GUI_WeaponDisplayBox>().UnHighlight();
				}				
				Navigate("left");
			} else if ((ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVEUP)
	        || ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKUP)) && arrowPos > 4) {
				if(currentNavigationArea == NavigationState.TRASH_BAG){
					inventory[arrowPos].GetComponent<GUI_WeaponDisplayBox>().UnHighlight();
				}else{
					currentlyEquipped[arrowPos].GetComponent<GUI_WeaponDisplayBox>().UnHighlight();
				}				
				Navigate("up");
			} else if ((ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVEDOWN)
			|| ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKDOWN))) {
				if(currentNavigationArea == NavigationState.TRASH_BAG && arrowPos+5 < maxArrowPos){
					inventory[arrowPos].GetComponent<GUI_WeaponDisplayBox>().UnHighlight();
					Navigate("down");
				}else if(currentNavigationArea == NavigationState.CURRENTLY_EQUIPPED && arrowPos+4 < maxArrowPos){
					currentlyEquipped[arrowPos].GetComponent<GUI_WeaponDisplayBox>().UnHighlight();
					Navigate("down");
				}
	        }else if(ControllerManager.Instance.GetKeyDown(INPUTACTION.CALENDAR)){
	        	//swap between the two sections
	        	if(currentNavigationArea == NavigationState.TRASH_BAG){
					inventory[arrowPos].GetComponent<GUI_WeaponDisplayBox>().UnHighlight();
					previouslyHighlightedArrowPos = arrowPos;
	        		currentNavigationArea = NavigationState.CURRENTLY_EQUIPPED;
					arrowPos = 0;
					maxArrowPos = currentlyEquipped.Count;
					currentlyEquipped[arrowPos].GetComponent<GUI_WeaponDisplayBox>().Highlight();

	        	}else{
					currentlyEquipped[arrowPos].GetComponent<GUI_WeaponDisplayBox>().UnHighlight();
	        		currentNavigationArea = NavigationState.TRASH_BAG;
					arrowPos = previouslyHighlightedArrowPos; // return to last highlighted box, not the first box
					maxArrowPos = inventory.Count;
					inventory[arrowPos].GetComponent<GUI_WeaponDisplayBox>().Highlight();


	        	}
	        }else if(ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT)){
	        	Debug.Log("InteractRead");
				Debug.Log("current state is trash bag" + currentlySelectedHero.strength);

				//POPUP MENU APPEAR

				if(inventory[arrowPos].GetComponent<GUI_WeaponDisplayBox>().type == GUI_WeaponDisplayBox.Type.WEAPON){
					if(currentlySelectedHero.strength < GlobalVariableManager.Instance.WeaponInventory[arrowPos].weight){
						popupMenu.GreyOutEquip();
					}else{
						popupMenu.arrowPos = 0;
					}
					popupMenu.gameObject.SetActive(true);
				}else if(inventory[arrowPos].GetComponent<GUI_WeaponDisplayBox>().type == GUI_WeaponDisplayBox.Type.CONSUMABLE){
					itemPopupMenu.SetItem(inventory[arrowPos].GetComponent<ConsumableItemOption>());
					itemPopupMenu.gameObject.SetActive(true);

				}

				GameStateManager.Instance.PushState(typeof(OptionsState));
	        	
	        }

		}
	}


	public override void NavigateEffect(){
		//show consumable item/ Weapon info depending on type

		if(inventory[arrowPos].GetComponent<GUI_WeaponDisplayBox>().type == GUI_WeaponDisplayBox.Type.WEAPON){
			weaponImage.sprite = GlobalVariableManager.Instance.WeaponInventory[arrowPos].displaySprite;
			weaponStrTextDisplay.text = GlobalVariableManager.Instance.WeaponInventory[arrowPos].weight.ToString();
			weaponDmgTextDisplay.text = GlobalVariableManager.Instance.WeaponInventory[arrowPos].damage.ToString();
			descriptionText.text = GlobalVariableManager.Instance.WeaponInventory[arrowPos].description;
		}else{ //because there are two seperate global vars for each type on inventory, need to check seperate var in globalVarManager
			int numOfWeapons = GlobalVariableManager.Instance.WeaponInventory.Count;
			Debug.Log("Num of weapon options:" + numOfWeapons);
			weaponImage.sprite = GlobalVariableManager.Instance.CONSUMABLE_INVENTORY[arrowPos - numOfWeapons].myItemDefinition.displaySprite;
			weaponStrTextDisplay.text = " ";
			weaponDmgTextDisplay.text = " ";
			descriptionText.text = GlobalVariableManager.Instance.CONSUMABLE_INVENTORY[arrowPos-numOfWeapons].myItemDefinition.description;
		}
		if(currentNavigationArea == NavigationState.TRASH_BAG){
			inventory[arrowPos].GetComponent<GUI_WeaponDisplayBox>().Highlight();
		}else{
			currentlyEquipped[arrowPos].GetComponent<GUI_WeaponDisplayBox>().Highlight();
		}
	}


	void UpdateInventoryDisplay(){ //Readjusts box positions after taking something out of inventory

		for(int i = 0; i < inventory.Count; i++){
			Debug.Log(i%5);
			if(inventory[i].transform.parent != trashBagDisplayHolder.transform){
				inventory[i].transform.parent = trashBagDisplayHolder.transform;
				inventory[i].transform.localScale = new Vector2(1f,1f);
			}
			inventory[i].transform.localPosition = new Vector2(-12 +(3*(i%5)), 5.2f - (3* Mathf.Floor(i/5)));
		}
		inventory[arrowPos].GetComponent<GUI_WeaponDisplayBox>().Highlight();

	}

	void UpdateEquippedDisplay(){
		for(int i = 0; i < currentlyEquipped.Count; i++){
			if(currentlyEquipped[i].transform.parent != equippedSectionHolder.transform){
				currentlyEquipped[i].transform.parent = equippedSectionHolder.transform;
				currentlyEquipped[i].transform.localScale = new Vector2(.7f,.7f);
			}
			currentlyEquipped[i].transform.localPosition = new Vector2(-3.5f +(2*(i%4)), -2f - (2* Mathf.Floor(i/4)));

		}

		currentlyEquipped[arrowPos].GetComponent<GUI_WeaponDisplayBox>().Highlight();

	}

	public void Equip(){ //Activated by GUI_WeaponPopup
		if(currentlySelectedHero.strength >= GlobalVariableManager.Instance.WeaponInventory[arrowPos].weight){
			Debug.Log("Proper Weight available");

			currentlySelectedHero.myEquippedWeapons.Add(GlobalVariableManager.Instance.WeaponInventory[arrowPos]);
			currentlySelectedHero.strength -= GlobalVariableManager.Instance.WeaponInventory[arrowPos].weight;
			currentlyEquipped.Add(inventory[arrowPos]);
			currentlyEquipped[currentlyEquipped.Count-1].GetComponent<GUI_WeaponDisplayBox>().UnHighlight();
			inventory.RemoveAt(arrowPos);
			GlobalVariableManager.Instance.WeaponInventory.RemoveAt(arrowPos);
			if(arrowPos > 0){
				arrowPos--;
			}
			UpdateInventoryDisplay();
			UpdateEquippedDisplay();
	    }
	}

	public void Unequip(){
		GlobalVariableManager.Instance.WeaponInventory.Add(currentlySelectedHero.myEquippedWeapons[arrowPos]);
		currentlySelectedHero.myEquippedWeapons.RemoveAt(arrowPos);
		inventory.Add(currentlyEquipped[arrowPos]);
		inventory[inventory.Count-1].GetComponent<GUI_WeaponDisplayBox>().UnHighlight();

		currentlyEquipped.RemoveAt(arrowPos);
		if(arrowPos > 0){
				arrowPos--;
		}
		maxArrowPos = currentlyEquipped.Count;

		UpdateInventoryDisplay();
		UpdateEquippedDisplay();
	}

	public void Drop(){
		//TODO: Are you sure? Popup
		if(currentNavigationArea == NavigationState.TRASH_BAG){
			inventory.RemoveAt(arrowPos);
			GlobalVariableManager.Instance.WeaponInventory.RemoveAt(arrowPos);
			UpdateInventoryDisplay();

		}else{
			currentlyEquipped.RemoveAt(arrowPos);
			currentlySelectedHero.myEquippedWeapons.RemoveAt(arrowPos);
			UpdateEquippedDisplay();
		}
	}



}

