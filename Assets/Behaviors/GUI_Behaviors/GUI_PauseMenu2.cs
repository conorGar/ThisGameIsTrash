using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GUI_PauseMenu2 : GUI_PauseMenu
{
	int weaponSelectArrowPos = 1;
	int weaponsAvailable = 1;

	public List<GameObject> weaponBoxes = new List<GameObject>();
	//public GameObject mopBox;
	public GameObject highlightBox;
	public GameObject selectArrow;

	// Use this for initialization
	void Start ()
	{
	
	}

	void OnEnable(){
		weaponsAvailable = 1;
		/*if(GlobalVariableManager.Instance.IsSuitAvailable(GlobalVariableManager.WEAPONS.MOP)){
			weaponsAvailable++;
			weaponBoxes[1].SetActive(true);
			if(GlobalVariableManager.Instance.IsSuitEquipped(GlobalVariableManager.WEAPONS.MOP)){
				highlightBox.transform.position = weaponBoxes[1].transform.position;
			}
		}*/
	}
	
	void Update () {
        if (GameStateManager.Instance.GetCurrentState() == typeof(PauseMenuState)) {
            if (ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVERIGHT)
            || ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKRIGHT)) {
            	if(arrowpos == 1 && weaponSelectArrowPos < weaponsAvailable){
            		weaponSelectArrowPos++;
            		selectArrow.transform.position = weaponBoxes[weaponSelectArrowPos].transform.position;
            	}

             
            }
            else if (ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVELEFT)
                 || ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKLEFT)) {
                if (arrowpos == 1 && weaponSelectArrowPos > 1 && !forHub) {
                    arrowpos--;
                    SoundManager.instance.PlaySingle(selectSound);
               
                }
            }
			else if (ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKDOWN) ||ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVEDOWN)) {
				if (arrowpos < 3 && !forHub) {
                    arrowpos++;
					Debug.Log(GameStateManager.Instance.GetCurrentState());
                    SoundManager.instance.PlaySingle(selectSound);
                    if(arrowpos == 3){
                    	enddayOption.GetComponent<Image>().sprite = endayHLspr;               
                    	optionsOption.GetComponent<Image>().sprite = optionStartSpr;
                    }else{
						enddayOption.GetComponent<Image>().sprite = endDayStartSpr;               
                    	optionsOption.GetComponent<Image>().sprite = optionsHLspr;
                    }
                }
			}else if (ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKUP) ||ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVEUP)) {
				if (arrowpos > 0 && !forHub) {
                    arrowpos--;
					Debug.Log(GameStateManager.Instance.GetCurrentState());
                    SoundManager.instance.PlaySingle(selectSound);
                    if(arrowpos == 2){
						enddayOption.GetComponent<Image>().sprite = endDayStartSpr;   
						optionsOption.GetComponent<Image>().sprite = optionsHLspr;           
              
                    }else{
						enddayOption.GetComponent<Image>().sprite = endDayStartSpr;               
						optionsOption.GetComponent<Image>().sprite = optionStartSpr;
                    }
                }
			}else if (ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT)) {
				if (arrowpos > 0 && !forHub) {
                    arrowpos--;
					Debug.Log(GameStateManager.Instance.GetCurrentState());
                    SoundManager.instance.PlaySingle(selectSound);
                    if(arrowpos == 2){
						enddayOption.GetComponent<Image>().sprite = endDayStartSpr;   
						optionsOption.GetComponent<Image>().sprite = optionsHLspr;           
              
                    }else{
						enddayOption.GetComponent<Image>().sprite = endDayStartSpr;               
						optionsOption.GetComponent<Image>().sprite = optionStartSpr;
                    }
                }
            }
            if (ControllerManager.Instance.GetKeyDown(INPUTACTION.PAUSE)) {
                arrowpos = 1;
                gameObject.transform.localPosition = startPos;
                GameStateManager.Instance.PopState();
                GameStateManager.Instance.PushState(typeof(GameState));
            }
        }


		//gameObject.transform.localPosition = Vector2.Lerp(gameObject.transform.localPosition,Vector2.zero,.1f*(Time.realtimeSinceStartup - Time.deltaTime));
	}
}

