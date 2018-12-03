using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class S_Ev_DayDisplay : MonoBehaviour {

	//************************** OBSOLETE! ******************************************************//

	public Text numberDisplay;
	public Text curseDisplay;

	void Start () {
		GlobalVariableManager.Instance.PLAYER_CAN_MOVE = false;

		GlobalVariableManager.Instance.ROOM_NUM = 112;
		//GlobalVariableManager.Instance.CURRENT_HP = 0; //needs to be set for player to spawn
		GlobalVariableManager.Instance.MENU_SELECT_STAGE =1;
		GlobalVariableManager.Instance.ENEMIES_DEFEATED = 0;
		GlobalVariableManager.Instance.TIME_IN_DAY = -90;
		GlobalVariableManager.Instance.TRASH_TYPE_SELECTED = 3;

		//-------Accountant's Day time upgrades-------------//
		/*if(GlobalVariableManager.Instance.FRIEND_LIST[10][7] == 'o'){
			GlobalVariableManager.Instance.TIME_IN_DAY = -180;
		}else if(GlobalVariableManager.Instance.FRIEND_LIST[10][6] == 'o'){
			GlobalVariableManager.Instance.TIME_IN_DAY = -150;
		}else if(GlobalVariableManager.Instance.FRIEND_LIST[10][5] == 'o'){
			GlobalVariableManager.Instance.TIME_IN_DAY = -130;
		}else if(GlobalVariableManager.Instance.FRIEND_LIST[10][4] == 'o'){
			GlobalVariableManager.Instance.TIME_IN_DAY = -110;
		}*/
		//-------------------------------------------------//

		if(GlobalVariableManager.Instance.IsPinEquipped(PIN.CURSED)){
			CursePin();
		}

        //replace trash 'newly discovered' values.  Anything that has been discovered will be considered as viewed.
        GlobalVariableManager.Instance.STANDARD_GARBAGE_DISCOVERED = GlobalVariableManager.Instance.STANDARD_GARBAGE_VIEWED;
        GlobalVariableManager.Instance.COMPOST_GARBAGE_DISCOVERED = GlobalVariableManager.Instance.COMPOST_GARBAGE_VIEWED;
        GlobalVariableManager.Instance.RECYCLABLE_GARBAGE_DISCOVERED = GlobalVariableManager.Instance.RECYCLABLE_GARBAGE_VIEWED;
        GlobalVariableManager.Instance.LARGE_GARBAGE_DISCOVERED = GlobalVariableManager.Instance.LARGE_GARBAGE_VIEWED;

		numberDisplay.text = GlobalVariableManager.Instance.DAY_NUMBER.ToString();
		/*if(GlobalVariableManager.Instance.FRIEND_LIST[14][0] == 1 && GlobalVariableManager.Instance.FRIEND_LIST[14][1] != 'x'){
			SueSpawn();
		}else{
			if(GlobalVariableManager.Instance.WORLD_NUM == 1){
				if(GlobalVariableManager.Instance.FRIEND_LIST[27][1] != 'o'){
					// ^ didn't buy world truck stop 
					gameObject.GetComponent<Ev_FadeHelper>().FadeToScene("1_1");
				}
			}else if(GlobalVariableManager.Instance.WORLD_NUM == 2){
				gameObject.GetComponent<Ev_FadeHelper>().FadeToScene("World2");

			}
		}*/
	}
	
	void Update () {
		
	}

	void SueSpawn(){
		GlobalVariableManager.Instance.FRIEND_LIST[14].Replace(GlobalVariableManager.Instance.FRIEND_LIST[14][0],'0');
	}//end of SueSpawn()

	void CursePin(){
		int whichCurse = Random.Range(2,10);
		curseDisplay.enabled = true;

		if(whichCurse == 2){
			curseDisplay.text = "-1 HP";
		}else if(whichCurse == 3){
			curseDisplay.text = "+1 HP";
		}else if(whichCurse == 4){
			curseDisplay.text = "TOUGHER ENEMIES";
		}else if(whichCurse == 5){
			curseDisplay.text = "WEAKER ENEMIES";
		}else if(whichCurse == 6){
			curseDisplay.text = "SCARCE TRASH";
		}else if(whichCurse == 7){
			curseDisplay.text = "EXTRA TRASH";
		}else if(whichCurse == 8){
			curseDisplay.text = "SPEEDY";
		}else if(whichCurse == 9){
			curseDisplay.text = "SLOW PROJECTILES";
		}
		GlobalVariableManager.Instance.CURSEVALUE = whichCurse;


	}
}
