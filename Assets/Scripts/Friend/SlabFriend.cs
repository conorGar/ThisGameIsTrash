using UnityEngine;
using System.Collections;

public class SlabFriend : Friend
{
	public int trashInLoveFund;
	public int trashNeeded = 30;
	public GameObject trashGiveHUD;
	public DialogManager dialogManager; //TODO: Remove if add a dialogManager variable to all friends through 'Friend' class


	public override void GenerateEventData()
    {
        // These guys show up every day.
        day = CalendarManager.Instance.currentDay;
    }

    public void AddTrashToFund(int trashAdded){
    	trashInLoveFund += trashAdded;
		GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[0] -= trashAdded;
    	dialogManager.ReturnFromAction();
    }

    public void TrashGiveHUDEnable(){
    	trashGiveHUD.SetActive(true);
    }
    public void EnoughTrashCheck(){
    	if(trashInLoveFund >= trashNeeded){
    		dialogManager.JumpToNewNode("SlabFinish");
    	}
    	dialogManager.ReturnFromAction();
    	
    }
}

