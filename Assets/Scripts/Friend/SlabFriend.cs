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

    public new void OnEnable()
    {
        switch (GetFriendState()) {
            case "START":
                break;
            case "WANTS_TRASH":
            case "END":
                // Eyes open.
                BreakEyes();
                break;
        }
    }

    private void Update()
    {
        OnUpdate();
    }

    public override void OnUpdate()
    {
        switch (GetFriendState()) {
            case "START":
                break;
            case "WANTS_TRASH":
                nextDialog = "Slab1";
                GetComponent<ActivateDialogWhenClose>().Execute();
                break;
            case "END":
                break;
        }
    }

    public override IEnumerator OnFinishDialogEnumerator()
    {
        yield return new WaitForSeconds(.3f);

        switch (GetFriendState()) {
            case "START":
            case "WANTS_TRASH":
                // Turn into a non-auto start prompt!
                gameObject.GetComponent<ActivateDialogWhenClose>().autoStart = false;
                GetComponent<ActivateDialogWhenClose>().canTalkTo = true;
                break;
            case "END":
                gameObject.GetComponent<ActivateDialogWhenClose>().ResetDefaults();
                break;
        }
    }

    public void AddTrashToFund(int trashAdded){
    	if(trashAdded == 0)
			dialogManager.JumpToNewNode("SlabNoTrash1");

    	trashInLoveFund += trashAdded;

        // TODO: Maybe not expose GlobalVariableManager.TODAYS_TRASH_AQUIRED directly and just make some AddTrash and Remove Trash functions.
        // That way the GUI gets updated in that function automatically and we don't have to worry we did it correctly all over the code.
        GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[0] -= trashAdded;
        GUIManager.Instance.TrashCollectedDisplayGameplay.UpdateDisplay(GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[0]);
        UserDataManager.Instance.SetDirty();
        dialogManager.ReturnFromAction();
    }

    public void TrashGiveHUDEnable(){
		dialogManager.currentlySpeakingIcon.GetComponent<DialogIconAnimationManager>().SwitchAni("IconSlide");
    	trashGiveHUD.SetActive(true);
    }
    public void EnoughTrashCheck(){
    	if(trashInLoveFund >= trashNeeded){
            dialogManager.JumpToNewNode("SlabComplete");
            SetFriendState("END");
            dialogManager.ReturnFromActionOnSameNode();
        }
        else{
            dialogManager.ReturnFromAction();
        }
    	
    }
    public void GiveAnythingCheck(){
		dialogManager.currentlySpeakingIcon.GetComponent<DialogIconAnimationManager>().SwitchAni("IconSlideBack");

		/*if(trashInLoveFund >= trashNeeded){
    		dialogManager.JumpToNewNode("SlabFinish");
    	}*/
    	dialogManager.ReturnFromAction();
    }

    public void BreakEyes(){
    	Destroy(eyeCover);
    	eyeBreakPS.SetActive(true);
    }

    public override void GiveData(List<GameObject> neededObjects){
    	trashGiveHUD = neededObjects[0];
    	trashGiveHUD.GetComponent<GUI_SlabTrashGiveHUD>().slabFriend = this;
    }

    // User Data implementation
    public override string UserDataKey()
    {
        return "Slab";
    }

    public override SimpleJSON.JSONObject Save()
    {
        var json_data = new SimpleJSON.JSONObject();

        json_data["friendState"] = friendState;
        json_data["trashInLoveFund"] = trashInLoveFund;

        return json_data;
    }

    public override void Load(SimpleJSON.JSONObject json_data)
    {
        friendState = json_data["friendState"].AsInt;
        trashInLoveFund = json_data["trashInLoveFund"].AsInt;
    }
}

