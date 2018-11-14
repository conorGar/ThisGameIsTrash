using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RatOnMatFriend : Friend
{
	public ParticleSystem vanishPS;
	GameObject openMapPrompt;



    private void Update(){
        OnUpdate();
    }

    void OnEnable(){
		base.OnEnable();
		if(openMapPrompt == null)
			openMapPrompt = GameObject.Find("promptText");
        StartCoroutine("DayDisplayDelay");
    }

    public override void GenerateEventData()
    {
        // Tutorial is every day.
        day = CalendarManager.Instance.currentDay;
    }

    public override void OnActivateRoom()
    {
        switch (GetFriendState()) {
            case "TUTORIAL":
                gameObject.SetActive(true);
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

                break;
            case "END":
                break;
        }
    }

    public override IEnumerator OnFinishDialogEnumerator(){
        StartCoroutine("ReturnCam");
        if(openMapPrompt != null)
        	openMapPrompt.SetActive(true);
        yield return null;
	}
	IEnumerator DayDisplayDelay(){

	    yield return new WaitForSeconds(2f);
		gameObject.GetComponent<ActivateDialogWhenClose>().enabled = true; // needed to fix glitch where if player spammed continue button dialog would start again
		GetComponent<ActivateDialogWhenClose>().Execute();
	}

	IEnumerator ReturnCam(){
        yield return new WaitForSeconds(.3f);


		vanishPS.Play();

	
		yield return new WaitForSeconds(.5f);
		gameObject.GetComponent<MeshRenderer>().enabled =true;
		GlobalVariableManager.Instance.PLAYER_CAN_MOVE = true;
        CamManager.Instance.mainCamEffects.ReturnFromCamEffect();
		GameStateManager.Instance.PushState(typeof(GameplayState));


		gameObject.SetActive(false);

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

