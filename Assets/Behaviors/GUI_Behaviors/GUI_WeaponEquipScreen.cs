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
	public List<GameObject> inventoryWeapons = new List<GameObject>();
	public List<GameObject> currentlyEquipped = new List<GameObject>();

	NavigationState currentNavigationArea = NavigationState.TRASH_BAG;
	Hero currentlySelectedHero;

	int previouslyHighlightedArrowPos;

	void Start ()
	{
		//TODO: Spawn all weapons in inventory
		/*foreach(WeaponDefinition weapon in GlobalVariableManager.Instance.WeaponInventory){
			GameObject displayBox = ObjectPool.Instance.GetPooledObject("GUI_weapon_option_display");
			inventoryWeapons.Add(displayBox);
			displayBox.GetComponent<GUI_WeaponDisplayBox>().weaponIcon.sprite = weapon.displaySprite;
			displayBox.transform.parent = trashBagDisplayHolder.transform;
			displayBox.transform.localPosition = new Vector2(-12 +(3*inventoryWeapons.Count), 5.2f - (3* Mathf.Floor(inventoryWeapons.Count/5)));
		}*/

		currentlySelectedHero = GlobalVariableManager.Instance.HeroData[0]; // start with Jim
		maxArrowPos = inventoryWeapons.Count;
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
					inventoryWeapons[arrowPos].GetComponent<GUI_WeaponDisplayBox>().UnHighlight();
				}else{
					currentlyEquipped[arrowPos].GetComponent<GUI_WeaponDisplayBox>().UnHighlight();
				}
            	Navigate("right");
			}else if ((ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVELEFT) 
			|| ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKLEFT)) && arrowPos > 0){
				if(currentNavigationArea == NavigationState.TRASH_BAG){
					inventoryWeapons[arrowPos].GetComponent<GUI_WeaponDisplayBox>().UnHighlight();
				}else{
					currentlyEquipped[arrowPos].GetComponent<GUI_WeaponDisplayBox>().UnHighlight();
				}				
				Navigate("left");
			} else if ((ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVEUP)
	        || ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKUP)) && arrowPos > 4) {
				if(currentNavigationArea == NavigationState.TRASH_BAG){
					inventoryWeapons[arrowPos].GetComponent<GUI_WeaponDisplayBox>().UnHighlight();
				}else{
					currentlyEquipped[arrowPos].GetComponent<GUI_WeaponDisplayBox>().UnHighlight();
				}				
				Navigate("up");
			} else if ((ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVEDOWN)
			|| ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKDOWN))) {
				if(currentNavigationArea == NavigationState.TRASH_BAG && arrowPos+5 < maxArrowPos){
					inventoryWeapons[arrowPos].GetComponent<GUI_WeaponDisplayBox>().UnHighlight();
					Navigate("down");
				}else if(currentNavigationArea == NavigationState.CURRENTLY_EQUIPPED && arrowPos+4 < maxArrowPos){
					currentlyEquipped[arrowPos].GetComponent<GUI_WeaponDisplayBox>().UnHighlight();
					Navigate("down");
				}
	        }else if(ControllerManager.Instance.GetKeyDown(INPUTACTION.CALENDAR)){
	        	//swap between the two sections
	        	if(currentNavigationArea == NavigationState.TRASH_BAG){
					inventoryWeapons[arrowPos].GetComponent<GUI_WeaponDisplayBox>().UnHighlight();
					previouslyHighlightedArrowPos = arrowPos;
	        		currentNavigationArea = NavigationState.CURRENTLY_EQUIPPED;
					arrowPos = 0;
					maxArrowPos = currentlyEquipped.Count;
					currentlyEquipped[arrowPos].GetComponent<GUI_WeaponDisplayBox>().Highlight();

	        	}else{
					currentlyEquipped[arrowPos].GetComponent<GUI_WeaponDisplayBox>().UnHighlight();
	        		currentNavigationArea = NavigationState.TRASH_BAG;
					arrowPos = previouslyHighlightedArrowPos; // return to last highlighted box, not the first box
					maxArrowPos = inventoryWeapons.Count;
					inventoryWeapons[arrowPos].GetComponent<GUI_WeaponDisplayBox>().Highlight();


	        	}
	        }else if(ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT)){
	        Debug.Log("InteractRead");
	        	if(currentNavigationArea == NavigationState.TRASH_BAG){
					Debug.Log("current state is trash bag" + currentlySelectedHero.strength);

	        		//equip currently highlighted weapon if this hero has the strength required
	        		if(currentlySelectedHero.strength >= GlobalVariableManager.Instance.WeaponInventory[arrowPos].weight){
						Debug.Log("Proper Weight available");

						currentlySelectedHero.myEquippedWeapons.Add(GlobalVariableManager.Instance.WeaponInventory[arrowPos]);
						currentlySelectedHero.strength -= GlobalVariableManager.Instance.WeaponInventory[arrowPos].weight;
						currentlyEquipped.Add(inventoryWeapons[arrowPos]);
						inventoryWeapons.RemoveAt(arrowPos);
						GlobalVariableManager.Instance.WeaponInventory.RemoveAt(arrowPos);
						if(arrowPos > 0){
							arrowPos--;
						}
						UpdateInventoryDisplay();
						UpdateEquippedDisplay();
	        		}
	        	}
	        }

		}
	}


	public override void NavigateEffect(){
		//TODO: change description/stats text to currently highlighted weapon text
		weaponImage.sprite = GlobalVariableManager.Instance.WeaponInventory[arrowPos].displaySprite;
		weaponStrTextDisplay.text = GlobalVariableManager.Instance.WeaponInventory[arrowPos].weight.ToString();
		weaponDmgTextDisplay.text = GlobalVariableManager.Instance.WeaponInventory[arrowPos].damage.ToString();
		descriptionText.text = GlobalVariableManager.Instance.WeaponInventory[arrowPos].description;
		if(currentNavigationArea == NavigationState.TRASH_BAG){
			inventoryWeapons[arrowPos].GetComponent<GUI_WeaponDisplayBox>().Highlight();
		}else{
			currentlyEquipped[arrowPos].GetComponent<GUI_WeaponDisplayBox>().Highlight();
		}
	}


	void UpdateInventoryDisplay(){ //Readjusts box positions after taking something out of inventory

		for(int i = 0; i < inventoryWeapons.Count; i++){
			Debug.Log(i%5);
			inventoryWeapons[i].transform.localPosition = new Vector2(-12 +(3*(i%5)), 5.2f - (3* Mathf.Floor(i/5)));
		}
		inventoryWeapons[arrowPos].GetComponent<GUI_WeaponDisplayBox>().Highlight();

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

}

