using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SlabFriend : Friend
{
	public int trashInLoveFund;
	public int trashNeeded = 30;
	public GameObject trashGiveHUD;
	public GameObject eyeBreakPS;
    public GameObject eyeCover;

	public override void GenerateEventData()
    {
        // These guys show up every day.
        day = CalendarManager.Instance.currentDay;
    }

    public void AddTrashToFund(int trashAdded){
    	if(trashAdded == 0)
			dialogManager.JumpToNewNode("SlabNoTrash1");

    	trashInLoveFund += trashAdded;
		GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[0] -= trashAdded;
    	dialogManager.ReturnFromAction();
    }

    public void TrashGiveHUDEnable(){
		dialogManager.currentlySpeakingIcon.GetComponent<DialogIconAnimationManager>().SwitchAni("IconSlide");
    	trashGiveHUD.SetActive(true);
    }
    public void EnoughTrashCheck(){
    	if(trashInLoveFund >= trashNeeded){
    		dialogManager.JumpToNewNode("SlabFinish");
    	}
    	dialogManager.ReturnFromAction();
    	
    }
    public void GiveAnythingCheck(){
		dialogManager.currentlySpeakingIcon.GetComponent<DialogIconAnimationManager>().SwitchAni("IconSlideBack");

		/*if(trashInLoveFund >= trashNeeded){
    		dialogManager.JumpToNewNode("SlabFinish");
    	}
    	dialogManager.ReturnFromAction();*/
    }

    public void BreakEyes(){
    	Destroy(eyeCover);
    	eyeBreakPS.SetActive(true);
    }

    public override void GiveData(List<GameObject> neededObjects){
    	trashGiveHUD = neededObjects[0];
    	trashGiveHUD.GetComponent<GUI_SlabTrashGiveHUD>().slabFriend = this;
    }
}

