using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Ev_Hub : MonoBehaviour {

	
	// Use this for initialization
	void Start () {
		GlobalVariableManager.Instance.MENU_SELECT_STAGE = 2;
		GlobalVariableManager.Instance.ARROW_POSITION = 0;
		//create jim
		GlobalVariableManager.Instance.ROOM_NUM = 101;
		GlobalVariableManager.Instance.PLAYER_CAN_MOVE = true;

        GlobalVariableManager.Instance.LARGE_TRASH_LIST.Clear();

		if(GlobalVariableManager.Instance.characterUpgradeArray[1][13] == '-'){//**was originall '\' but wouldnt work with code so had to be switched
		//resets pin button at shop(pin perk 3)
		GlobalVariableManager.Instance.characterUpgradeArray[1].Replace("-","z");
		}

		//cursed pin health effects
		if(GlobalVariableManager.Instance.pinsEquipped[4] == 3){
			GlobalVariableManager.Instance.characterUpgradeArray[3] = (int.Parse(GlobalVariableManager.Instance.characterUpgradeArray[3][0].ToString()) - 1).ToString();
		}else if (GlobalVariableManager.Instance.pinsEquipped[4] ==2){
			GlobalVariableManager.Instance.characterUpgradeArray[3] =	(int.Parse(GlobalVariableManager.Instance.characterUpgradeArray[3][0].ToString()) + 1).ToString();
		}

		//disable melee swing at hub
		GameObject.Find("Jim").GetComponent<MeleeAttack>().enabled = false;

		LargeTrashSpawn();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void LargeTrashSpawn(){

	//all large trash is already placed in hub but deactivated. Maybe each large trash in hub(hub large trash
	//should  maybe be its own object..) has a number as its name. Com runs through all positions in
	//unlocked string and if = 'o' then searches scene for gameobject with a name = the index it's currently
	//at and enables it...
        
        for (int i=0; i < sizeof(GlobalVariableManager.LARGETRASH); ++i)
        {
            GlobalVariableManager.LARGETRASH largeTrashType = GlobalVariableManager.Instance.LargeTrashByIndex(i);
            if ((GlobalVariableManager.Instance.LARGE_TRASH_DISCOVERY_LIST & largeTrashType) == largeTrashType)
            {
                GameObject.Find(i.ToString()).SetActive(true);
            }
        }
	}
}
