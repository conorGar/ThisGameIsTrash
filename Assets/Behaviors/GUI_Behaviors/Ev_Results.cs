using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Ev_Results : MonoBehaviour {

	public Text trashCollected;
	public Text largeTrashCollected;
	public Text enemiesDefeated;
	public Text nextUnlock;
	public GameObject largeTrashTextDisplay;
	public GameObject treasureCollectedDisplay;


	GameObject backPaper;
	int trashCollectedValue;
	int phase = 0;
	int spawnLargeTrashOnce = 0;
	int displayIndex = 0;


	void Start () {
		/*for(int i = 0; i<GlobalVariableManager.Instance.WORLD_ROOM_DISCOVER[4].Length; i++){
			//determines how many new locations were discovered by looking at
			//the 'temp string' (created/defined in populate worls'
		}*/
		//gameObject.transform.parent = GameObject.Find("BackPaper").transform;
		for(int i = 0; i<GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED.Count; i++){
			if(i != 1){
				//^ doesnt count scrap
				trashCollectedValue += GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[i];
			}
		}

		if(GlobalVariableManager.Instance.WORLD_ROOM_DISCOVER.Count > 5){
			//in case player dies during race
			GlobalVariableManager.Instance.WORLD_ROOM_DISCOVER.RemoveAt(5);
		}

		StartCoroutine("InteractDelay");
	}


	void Update () {

		

		if(ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT)){
			if(phase == 2){
				if(GlobalVariableManager.Instance.LARGE_TRASH_LIST.Count > displayIndex){

					if(spawnLargeTrashOnce == 0){
						//spawn large trash collected display
						largeTrashTextDisplay.SetActive(true);
						spawnLargeTrashOnce = 1;
					}else{
						treasureCollectedDisplay.GetComponent<GUIEffects>().Start();
					}

					//change sprite of the large trash display
					treasureCollectedDisplay.GetComponent<Image>().sprite = (GlobalVariableManager.Instance.LARGE_TRASH_LIST[displayIndex].collectedDisplaySprite);


					//add to large trash discovery list
					GlobalVariableManager.Instance.LARGE_GARBAGE_DISCOVERED |= GlobalVariableManager.Instance.LARGE_TRASH_LIST[displayIndex].type;


					displayIndex++;


				}else{
					GlobalVariableManager.Instance.ENEMIES_DEFEATED = 0;
					for(int i = 0; i<GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED.Count; i++){
						GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[i] = 0;
					}
					//GlobalVariableManager.Instance.CURRENT_HP = 0;
					if(GlobalVariableManager.Instance.WORLD_ROOM_DISCOVER.Count > 4)
						GlobalVariableManager.Instance.WORLD_ROOM_DISCOVER.RemoveAt(4); // remove temporary room discover string
					GlobalVariableManager.Instance.MENU_SELECT_STAGE = 1;
					if(GlobalVariableManager.Instance.DEJAVUCOUNT - 3 <= 0){
						//Deja Vu pin
						GlobalVariableManager.Instance.DAY_NUMBER++;
					}else{
						GlobalVariableManager.Instance.DEJAVUCOUNT--;
					}

                    // TODO: Review and figure this out
                    /*if(GlobalVariableManager.Instance.pinsEquipped[10] ==2){
						//Mo' Garbage Mo' Problems
						//if(GlobalVariableManager.Instance.CURRENT_HP > int.Parse(GlobalVariableManager.Instance.characterUpgradeArray[3][0].ToString()) -2){
						//	GlobalVariableManager.Instance.CURRENT_HP = int.Parse(GlobalVariableManager.Instance.characterUpgradeArray[3][0].ToString()) -2;
						//}
						GlobalVariableManager.Instance.characterUpgradeArray[3] = (int.Parse(GlobalVariableManager.Instance.characterUpgradeArray[3][0].ToString()) -2).ToString();
						GlobalVariableManager.Instance.pinsEquipped[10] = 1;
					}*/

                    //-------Reset today's trash collected---------//
                    if (GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED.Count > 3){
						if(GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED.Count > 4){
							//resets hp after cassie gives you bonus
							GlobalVariableManager.Instance.Max_HP -=1;
							GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED.RemoveAt(4);
						}
						GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED.RemoveAt(3);
					}
					//---------------------------------------------//
					GlobalVariableManager.Instance.ARROW_POSITION = 1;
					if(GlobalVariableManager.Instance.DAY_NUMBER == 2){
						GameObject.Find("fadeHelper").GetComponent<Ev_FadeHelper>().FadeToScene("Hub");//supposed to be intro credits scene, changed for testing
					}else if(GlobalVariableManager.Instance.DAY_NUMBER == 3){
						StartCoroutine("HomelessHarry");
					}else{
						GameObject.Find("fadeHelper").GetComponent<Ev_FadeHelper>().FadeToScene("Hub");

					}
				}
			}// end of phase = 2 check

		}
	}//end of update

	public void SetBackPaper(GameObject go){
		backPaper = go;
	}

	IEnumerator InteractDelay(){
		yield return new WaitForSeconds(1f);
		phase = 2;
	}


}
