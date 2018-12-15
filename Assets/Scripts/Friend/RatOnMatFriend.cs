using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RatOnMatFriend : Friend
{
	public ParticleSystem vanishPS;
	GameObject openMapPrompt;

	bool actionCheck_moleTown;
	bool actionCheck_dog;
	bool actionCheck_bugZone;

	bool treasureCheck_dogTreasure;
	bool treasureCheck_porcupines;
	bool treasureCheck_moleTown;

    private void Update(){
        OnUpdate();
    }

    void OnEnable(){
		base.OnEnable();
		if(openMapPrompt == null)
			openMapPrompt = GameObject.Find("promptText");
    }

    public override void GenerateEventData()
    {
        // Tutorial is every day.
		switch (GetFriendState()) {
            case "TUTORIAL":
				day = CalendarManager.Instance.currentDay;
                break;
            case "GUIDE":
            	if(GlobalVariableManager.Instance.DAY_NUMBER > 4){
				    day = CalendarManager.Instance.currentDay;
            	}
            	break;
            case "END":
                gameObject.SetActive(false);
                break;
        }
    }

    public override void OnActivateRoom()
    {
        switch (GetFriendState()) {
            case "TUTORIAL":
                gameObject.SetActive(true);
                break;
            case "GUIDE":
            	if(GlobalVariableManager.Instance.DAY_NUMBER < day){
				    gameObject.SetActive(false);
            	}
            	break;
            case "END":
                gameObject.SetActive(false);
                break;
        }
    }

    public override void OnUpdate()
    {
        switch (GetFriendState()) {
            case "TUTORIAL":
                nextDialog = "RatMat1";
                GetComponent<ActivateDialogWhenClose>().Execute();
                break;
			case "GUIDE":
                nextDialog = "RatMat_2_1";
                GetComponent<ActivateDialogWhenClose>().Execute();
                break;
            case "END":
                break;
        }
    }

    public override IEnumerator OnFinishDialogEnumerator(bool panToPlayer = true)
    {
        StartCoroutine("ReturnCam");
        if(openMapPrompt != null)
        	openMapPrompt.SetActive(true);
		switch (GetFriendState()) {
            case "TUTORIAL":
                nextDialog = "RatMat_2_1";
				SetFriendState("GUIDE");
                day = 4;
                gameObject.GetComponent<ActivateDialogWhenClose>().distanceThreshold = 5;
                gameObject.GetComponent<ActivateDialogWhenClose>().autoStart = false;
                break;
            case "END":
                break;
        }
        yield return base.OnFinishDialogEnumerator();
    }

	IEnumerator ReturnCam(){
        yield return new WaitForSeconds(.3f);


		vanishPS.Play();

	
		yield return new WaitForSeconds(.5f);
		gameObject.GetComponent<MeshRenderer>().enabled =true;
		GlobalVariableManager.Instance.PLAYER_CAN_MOVE = true;
        CamManager.Instance.mainCamEffects.ReturnFromCamEffect();
		gameObject.SetActive(false);

	}

	void CheckDanger(){
		 bool foundAction = false;
		 int whichAction = Random.Range(1,4); //chooses randomly from given dialogs
		 for(int i = 0; i < 3; i++){
		 	if(whichAction == 1 && !actionCheck_bugZone){
		 		//return call to proper node
		 		dialogManager.JumpToNewNode("RatMat2_danger3_1");
		 		actionCheck_bugZone = true;
		 		foundAction = true;
		 		break;
		 	}else if(whichAction == 2 && !actionCheck_dog){
				dialogManager.JumpToNewNode("RatMat2_danger4_1");
		 		actionCheck_dog = true;
		 		foundAction = true;
		 		break;
		 	}else if(whichAction == 3 && !actionCheck_moleTown){
				dialogManager.JumpToNewNode("RatMat2_danger2_1");
		 		actionCheck_moleTown = true;
		 		foundAction = true;
		 		break;
		 	}

		 	if(whichAction <3){
		 		whichAction++;
		 	}else{
		 		whichAction = 0;
		 	}

		 }

		 if(!foundAction){
			dialogManager.JumpToNewNode("RatMat2_danger_none");
		 }

		 dialogManager.ReturnFromAction(true);
	}

	void CheckTreasure(){
		 bool foundAction = false;
		 int whichAction = Random.Range(1,4); //chooses randomly from given dialogs
		 for(int i = 0; i < 3; i++){
		 	if(whichAction == 1 && !actionCheck_bugZone){
		 		//return call to proper node
		 		dialogManager.JumpToNewNode("RatMat2_treasure2_1");
		 		treasureCheck_porcupines = true;
		 		foundAction = true;
		 		break;
		 	}else if(whichAction == 2 && !actionCheck_dog){
				dialogManager.JumpToNewNode("RatMat2_treasure3_1");
		 		treasureCheck_dogTreasure = true;
		 		foundAction = true;
		 		break;
		 	}else if(whichAction == 3 && !actionCheck_moleTown){
				dialogManager.JumpToNewNode("RatMat2_treasure4_1");
		 		treasureCheck_moleTown = true;
		 		foundAction = true;
		 		break;
		 	}

		 	if(whichAction <3){
		 		whichAction++;
		 	}else{
		 		whichAction = 0;
		 	}

		 }

		 if(!foundAction){
			dialogManager.JumpToNewNode("RatMat2_treasure_none");
		 }

		 dialogManager.ReturnFromAction(true);
	}

    // User Data implementation
    public override string UserDataKey()
    {
        return "RatOnAMat";
    }

    public override SimpleJSON.JSONObject Save()
    {
        var json_data = new SimpleJSON.JSONObject();

        json_data["friendState"] = friendState;

        return json_data;
    }

    public override void Load(SimpleJSON.JSONObject json_data)
    {
        friendState = json_data["friendState"].AsInt;
    }


}

