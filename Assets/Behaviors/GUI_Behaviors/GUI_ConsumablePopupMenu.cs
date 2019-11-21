using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class GUI_ConsumablePopupMenu : MonoBehaviour
{
	public GUI_WeaponEquipScreen weaponEquipScreen;
	public List<TextMeshProUGUI> options = new List<TextMeshProUGUI>();
	public List<TextMeshProUGUI> healthDisplays = new List<TextMeshProUGUI>();

	public int arrowPos;

	ConsumableItemOption thisItem;




	// Use this for initialization
	void Start ()
	{
		AddOptions();

	}

	void OnEnable(){
		AddOptions();
		ShowHealth();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(GameStateManager.Instance.GetCurrentState() == typeof(OptionsState)){
			if ((ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVEDOWN)
			|| ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKDOWN)) && arrowPos < options.Count-1) {
				options[arrowPos].color = new Color(255,255,255,.2f); // unhighlight current option
				arrowPos++;
				while(!options[arrowPos].IsActive()){ //keep going if this option isnt available(dont have 3 partners with you)
					arrowPos++;
				}
				options[arrowPos].color = new Color(255,255,255,1f); // unhighlight current option

			}else if((ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVEUP)
		    || ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKUP)) && arrowPos > 0){
				options[arrowPos].color = new Color(255,255,255,.2f); // unhighlight current option
				arrowPos--;
				while(!options[arrowPos].IsActive()){//keep going if this option isnt available(dont have 3 partners with you)
					arrowPos--;
				}
				options[arrowPos].color = new Color(255,255,255,1f); // highlight current option
			}else if(ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT)){
				if(options[arrowPos].gameObject.name == "drop"){
					weaponEquipScreen.Drop();
				}else if(options[arrowPos].gameObject.name == "back"){
					Close();
				}else{ //If item
					//TODO: cannot use if healer item and player is at max hp
					UseItem();
					Close();
				}
			}

		}
	}




	void UseItem(){
		Debug.Log("Use item activated");
		Hero targetHero;

		targetHero = FindHero(options[arrowPos].text);
		
		weaponEquipScreen.UseItem(targetHero);
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


		for(int i = 0; i < GlobalVariableManager.Instance.partners.Count;i++){
			options[i+1].gameObject.SetActive(true);
			Debug.Log("FOUND PARTNER WITH NAME:" + GlobalVariableManager.Instance.partners[i].myHeroName);
			options[i+1].text = GlobalVariableManager.Instance.partners[i].myHeroName;
			healthDisplays[i+1].gameObject.SetActive(true);
		}


	}

	void ShowHealth(){
		for(int i = 0; i < healthDisplays.Count;i++){
			Hero targetHero;
			targetHero = FindHero(options[i].text);
			healthDisplays[i].text =  targetHero.currentHP + "/" + targetHero.maxHP;
			ChangeHealthColor(healthDisplays[i], targetHero.currentHP, targetHero.maxHP);
		}
	}

	void ChangeHealthColor(TextMeshProUGUI thisText, int currentHP, int maxHP){
		//Change color of text depending on the percentage of the current HP compared to the hero's max HP
		if(currentHP/maxHP > .7f){
			Debug.Log("Got here - change health color" + thisText.color);
			thisText.color =  new Color(144/255,1,156/255);
			Debug.Log("Got here 2 - change health color"  + thisText.color);

		}else if(currentHP/maxHP > .45f){
			thisText.color =  new Color(1,240/255,158/255);
		}else{
			thisText.color =  new Color(1,109/255,109/255);

		}
	}
}

