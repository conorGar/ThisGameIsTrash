using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class GUI_WeaponEquipPopupMenu : MonoBehaviour
{
	public GUI_WeaponEquipScreen weaponEquipScreen;

	public List<TextMeshProUGUI> options = new List<TextMeshProUGUI>();

	public int arrowPos;


	void OnEnable(){
		//change to 'Equip'/'Unequip' depending on what section player is in
		if(weaponEquipScreen.currentNavigationArea == GUI_WeaponEquipScreen.NavigationState.TRASH_BAG){
			for(int i = 0; i < options.Count; i++){
				if(options[i].gameObject.name == "equip"){
					options[i].text = "Equip";
				}
			}
		}else{
			for(int i = 0; i < options.Count; i++){
				if(options[i].gameObject.name == "equip"){
					options[i].text = "Unequip";
				}
			}
		}
		options[arrowPos].color = new Color(255,255,255,1f); // unhighlight current option

	}
	
	// Update is called once per frame
	void Update ()
	{
		if(GameStateManager.Instance.GetCurrentState() == typeof(OptionsState)){
			if ((ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVEDOWN)
			|| ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKDOWN)) && arrowPos < options.Count) {
				options[arrowPos].color = new Color(255,255,255,.2f); // unhighlight current option
				arrowPos++;
				options[arrowPos].color = new Color(255,255,255,1f); // unhighlight current option

			}else if((ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVEUP)
		    || ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKUP)) && arrowPos > 0){
				options[arrowPos].color = new Color(255,255,255,.2f); // unhighlight current option
				arrowPos--;
				options[arrowPos].color = new Color(255,255,255,1f); // highlight current option
			}else if(ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT)){
				if(options[arrowPos].gameObject.name == "equip"){
					if(weaponEquipScreen.currentNavigationArea == GUI_WeaponEquipScreen.NavigationState.TRASH_BAG){
						weaponEquipScreen.Equip();
					}else{
						weaponEquipScreen.Unequip();
					}
					Close();
				}else if(options[arrowPos].gameObject.name == "drop"){
					weaponEquipScreen.Drop();
				}else if(options[arrowPos].gameObject.name == "back"){
					Close();
				}
			}

		}
	}


	void Close(){
		options[arrowPos].color = new Color(255,255,255,.2f); // unhighlight current option
		GameStateManager.Instance.PopState();
		gameObject.SetActive(false);
	}

	public void GreyOutEquip(){ //For if hero does not have the strength to carry this weapon
		options[0].color = new Color(255,75,75,.5f);
		arrowPos = 1;
		options[arrowPos].color = new Color(255,255,255,1f); // highlight current option

	}
}

