using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class GUI_ConsumablePopupMenu : MonoBehaviour
{
	public GUI_WeaponEquipScreen weaponEquipScreen;
	public List<TextMeshProUGUI> options = new List<TextMeshProUGUI>();
	public int arrowPos;

	ConsumableItemOption thisItem;
	//Need a list of current heroes so it sends the proper hero 

	//THis actually uses the item



	// Use this for initialization
	void Start ()
	{
	
	}

	void OnEnable(){
		AddOptions();
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
					UseItem();
					Close();
				}else if(options[arrowPos].gameObject.name == "drop"){
					weaponEquipScreen.Drop();
				}else if(options[arrowPos].gameObject.name == "back"){
					Close();
				}
			}

		}
	}

	public void SetItem(ConsumableItemOption item){ //Called by GUI_WeaponEquipScreen
		thisItem = item;
	}

	void UseItem(){
		Hero targetHero;

		targetHero = FindHero(options[arrowPos].text);
		

		thisItem.Use(targetHero);
	}

	Hero FindHero(string heroName){
		for(int i = 0; i < GlobalVariableManager.Instance.HeroData.Count; i++){
			if(heroName == GlobalVariableManager.Instance.HeroData[i].heroName){
				Debug.Log("Found Hero - HeroAttacker");
				return GlobalVariableManager.Instance.HeroData[i];
			}
		}

		Debug.LogError("Could not find HERO with name:" + heroName);
		return null;
	}

	void Close(){
		options[arrowPos].color = new Color(255,255,255,.2f); // unhighlight current option
		GameStateManager.Instance.PopState();
		gameObject.SetActive(false);
	}

	void AddOptions(){
		//Add all hero options 
		//TODO: only amount of options as there are partners

		for(int i = 0; i < GlobalVariableManager.Instance.partners.Count;i++){
			options[i+1].text = GlobalVariableManager.Instance.partners[i].myHeroName;
		}

	}
}

