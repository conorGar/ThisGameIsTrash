using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUI_LevelUpDisplay : MonoBehaviour
{

	public List<GameObject> levelUpOptions = new List<GameObject>();
	Hero targetHero; //set by BattleManager

	public int arrowPos = 1; //public for debuggin'

	void OnEnable(){
		GameStateManager.Instance.PushState(typeof(LevelSelectState));

	}
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (GameStateManager.Instance.GetCurrentState() == typeof(LevelSelectState)) {
			if ((ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVERIGHT)
			|| ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKRIGHT)) && arrowPos < (levelUpOptions.Count -1)) {
				UnHighlight(levelUpOptions[arrowPos]);
				arrowPos++;
				Highlight(levelUpOptions[arrowPos]);
			}else if ((ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVELEFT) 
			|| ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKLEFT)) && arrowPos > 0){
				UnHighlight(levelUpOptions[arrowPos]);
				arrowPos--;
				Highlight(levelUpOptions[arrowPos]);
			}else if(ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT)){
				LevelUpHero(arrowPos);
				Close();
			}
		}
	}

	public void SetHero(Hero hero){ //Activated by BattleManager
		targetHero = hero;
	}

	public void SetSpawnPosition(Vector2 pos){ //Set by BattleManager
		gameObject.transform.position = pos;

	}

	void Highlight(GameObject go){
		go.GetComponent<tk2dSprite>().color = new Color(255,255,255,1f);
	}

	void UnHighlight(GameObject go){
		go.GetComponent<tk2dSprite>().color = new Color(255,255,255,.5f);
	}

	void LevelUpHero(int choice){
		if(choice == 0){//health
			Debug.Log("Level up health for:" + targetHero);
			targetHero.maxHP += 5;
			targetHero.currentHP = targetHero.maxHP;
		}else if(choice == 1){//speed
			Debug.Log("Level up speed for:" + targetHero);
			targetHero.speed += 3;
		}else if(choice == 2){//strength
			Debug.Log("Level up strength for:" + targetHero);
			targetHero.maxStrength += 2;
		}
		int leftoverXP = targetHero.xp - 100;
		targetHero.xp = leftoverXP; //reset xp back
	}

	void Close(){
		GameStateManager.Instance.PopState();
		BattleManager.Instance.EndBattle();
		gameObject.SetActive(false);
	}


}

