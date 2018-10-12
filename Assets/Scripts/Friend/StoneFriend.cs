using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StoneFriend : Friend
{

	List<GameObject> handsDelivered = new List<GameObject>();//hands should be children, so that there positions are saved
	public GameObject eyeBreakPS;
    public GameObject eyeCover;

    public new void OnEnable()
    {
        switch (GetFriendState()) {
            case "START":
                break;
            case "WANTS_HANDS":
                // Eyes open.
                BreakEyes();
                break;
            case "END":
                break;
        }
    }

    public override void GenerateEventData()
    {
        // These guys show up every day.
        day = CalendarManager.Instance.currentDay;
    }

	public void DeliverObject(GameObject obj)
    {
    	handsDelivered.Add(obj);
    }

    public void StoneEnding(){

    }

    public void ShowRockHand(){

    }

    public void BreakEyes(){
    	Destroy(eyeCover);
    	eyeBreakPS.SetActive(true);
    }

    // User Data implementation
    public override string UserDataKey()
    {
        return "Stone";
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

