using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class GUI_TimeUpgrade : GUI_MenuBase
{

	public int currentUpgradeCost;
	public int amntToIncreaseBy = 20;
	public GameObject timeUpgradeHUD;
	public TextMeshProUGUI costDisplay;
	public List<GameObject> arrowPositionLocations = new List<GameObject>();
	public GUI_TotalTrashDisplay totalTrashDisplay;
	public GameObject timeBack;

	void OnEnable(){
       
        arrowPos = 0;
        CamManager.Instance.mainCamPostProcessor.profile = blur;
       	costDisplay.text = "x" + currentUpgradeCost.ToString();

       	DetermineCost();
	}


	void Start(){
		gameObject.SetActive(false);
	}

	void OnDisable()
    {
        CamManager.Instance.mainCamPostProcessor.profile = null;
        CamManager.Instance.mainCamEffects.ReturnFromCamEffect();
        timeBack.SetActive(false);
        GameStateManager.Instance.PopState();
    }
	
	// Update is called once per frame
	void Update ()
	{
		if (GameStateManager.Instance.GetCurrentState() == typeof(ShopState)) {

			if (ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT)) {
                if(arrowPos == 0){
                	if(GlobalVariableManager.Instance.TOTAL_TRASH >= currentUpgradeCost){
                		GlobalVariableManager.Instance.TOTAL_DAYTIME_INSECONDS += amntToIncreaseBy;
                		totalTrashDisplay.UpdateDisplay();
						GlobalVariableManager.Instance.TIME_UPGRADE_LEVEL++;
                	}
                
                }else{
					SoundManager.instance.backupMusicSource.Stop();
					SoundManager.instance.musicSource.volume = GlobalVariableManager.Instance.MASTER_MUSIC_VOL;
					timeUpgradeHUD.SetActive(false);
                }
            }

			if (ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVEUP) && upDownNav && arrowPos > 0) {
	                SoundManager.instance.PlaySingle(SFXBANK.MENU_NAV1);
	                optionIcons[arrowPos].GetComponent<GUI_Ev_buttonPopUp>().UnhighlightButton();
	                Navigate("up");
	        }
	        else if (ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVEDOWN) && upDownNav && arrowPos < maxArrowPos) {
	                SoundManager.instance.PlaySingle(SFXBANK.MENU_NAV2);
	                optionIcons[arrowPos].GetComponent<GUI_Ev_buttonPopUp>().UnhighlightButton();
	                Navigate("down");
	        }

			selectionArrow.transform.position = arrowPositionLocations[arrowPos].transform.position;

        }
	}

	public override void NavigateEffect(){
		optionIcons[arrowPos].GetComponent<GUI_Ev_buttonPopUp>().HighlightButton();
	}

	void DetermineCost(){ 
		int currentLevel = GlobalVariableManager.Instance.TIME_UPGRADE_LEVEL;
		if(GlobalVariableManager.Instance.TIME_UPGRADE_LEVEL < 3){
			currentUpgradeCost = 10;
		}else if(currentLevel >=3 && currentLevel <5){
			currentUpgradeCost = 20;
		}else if(currentLevel >=5 && currentLevel <7){
			currentUpgradeCost = 30;
		}else if (currentLevel >= 7){
			currentUpgradeCost = 35;
		}
	}

}

