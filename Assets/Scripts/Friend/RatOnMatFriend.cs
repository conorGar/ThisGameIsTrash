using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RatOnMatFriend : Friend
{
	public ParticleSystem vanishPS;
	GameObject openMapPrompt;
	public S_Ev_DemoEnd demoEndManager;

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
		if(openMapPrompt == null){
			openMapPrompt = GameObject.Find("HUD").GetComponent<Ev_HUD>().promptText;
		}
		base.OnEnable();

		switch (GetFriendState()) {
            case "TUTORIAL":
                break;
           	case "GUIDE":
                gameObject.GetComponent<ActivateDialogWhenClose>().autoStart = false;
				gameObject.GetComponent<ActivateDialogWhenClose>().canTalkTo = true;
				gameObject.GetComponent<ActivateDialogWhenClose>().distanceThreshold = 5;
           		break;
            case "END":
                break;
        }
    }

    public override void GenerateEventData()
    {
        // Tutorial is every day.
		switch (GetFriendState()) {
            case "TUTORIAL":
				day = CalendarManager.Instance.currentDay;
                break;
            case "GUIDE_INTRO":
            case "GUIDE":
                // Shows up on day 4 and beyond.
                day = Mathf.Max(3, CalendarManager.Instance.currentDay);
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
            case "GUIDE_INTRO":
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
			case "GUIDE_INTRO":
                nextDialog = "RatMat_2_1";
                GetComponent<ActivateDialogWhenClose>().Execute();
                break;
            case "GUIDE":
				nextDialog = "RatMat_3";
                GetComponent<ActivateDialogWhenClose>().Execute();
            	break;
            case "END":
                break;
        }
    }

    public override IEnumerator OnFinishDialogEnumerator(bool panToPlayer = true)
    {
       
       		Debug.Log("Rat on Mat end activate");
		switch (GetFriendState()) {
			
            case "TUTORIAL":
				Debug.Log("Rat on Mat end activate ---- tutorial----");
                nextDialog = "RatMat_2_1";
                SetFriendState("GUIDE_INTRO");
                IsVisiting = false;
				if(openMapPrompt != null)
        			openMapPrompt.SetActive(true);

                gameObject.GetComponent<ActivateDialogWhenClose>().distanceThreshold = 5;
                gameObject.GetComponent<ActivateDialogWhenClose>().autoStart = false;
				StartCoroutine("ReturnCam");
                break;
            case "GUIDE_INTRO":
				nextDialog = "RatMat_3";
				SetFriendState("GUIDE");
				gameObject.GetComponent<ActivateDialogWhenClose>().autoStart = false;
				gameObject.GetComponent<ActivateDialogWhenClose>().canTalkTo = true;

            	break;
            case "GUIDE":
				gameObject.GetComponent<ActivateDialogWhenClose>().canTalkTo = true;
            	break;
            case "END":
				Debug.Log("Rat on Mat end activate ---- end----");
				StartCoroutine("ReturnCam");

                break;
            case "DEMO_END":
            	demoEndManager.StartCoroutine("End");
            	break;
        }

        yield return base.OnFinishDialogEnumerator();
    }

	IEnumerator ReturnCam(){
        yield return new WaitForSeconds(.3f);


		vanishPS.Play();

	
		yield return new WaitForSeconds(1f);
		gameObject.GetComponent<MeshRenderer>().enabled =true;
		GlobalVariableManager.Instance.PLAYER_CAN_MOVE = true;
		CamManager.Instance.mainCam.SetNormalCameraSpeed();
       // CamManager.Instance.mainCamEffects.ReturnFromCamEffect();
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
	}

	void CheckTreasure(){
		 bool foundAction = false;
		 int whichAction = Random.Range(1,4); //chooses randomly from given dialogs
		 for(int i = 0; i < 3; i++){
		 	Debug.Log("Check Treasure ---" + whichAction);
		 	if(whichAction == 1 && !treasureCheck_porcupines){
		 		//return call to proper node
		 		dialogManager.JumpToNewNode("RatMat2_treasure2_1");
		 		treasureCheck_porcupines = true;
		 		foundAction = true;
		 		break;
		 	}else if(whichAction == 2 && !treasureCheck_dogTreasure){
				dialogManager.JumpToNewNode("RatMat2_treasure3_1");
		 		treasureCheck_dogTreasure = true;
		 		foundAction = true;
		 		break;
		 	}else if(whichAction == 3 && !treasureCheck_moleTown){
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

