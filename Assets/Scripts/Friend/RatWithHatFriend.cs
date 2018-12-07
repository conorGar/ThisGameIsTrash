using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatWithHatFriend : Friend {
    public int StartDay = 3;

    public override void GenerateEventData()
    {
        // Shows up every day from start day and on.
        if (CalendarManager.Instance.currentDay >= StartDay) {
            day = CalendarManager.Instance.currentDay;
        }
        else {
            day = -1;
        }
    }

    public new void OnEnable()
    {
        switch (GetFriendState()) {
            case "START":
                GUIManager.Instance.Hub_UpgradeStand.enabled = false;
                gameObject.GetComponent<ActivateDialogWhenClose>().autoStart = true;
                break;
            case "ADVERTISED":
                gameObject.GetComponent<ActivateDialogWhenClose>().autoStart = false;
                break;
            case "OPEN_FOR_BUSINESS":
                GUIManager.Instance.Hub_UpgradeStand.enabled = true;
                gameObject.GetComponent<ActivateDialogWhenClose>().autoStart = false;
                break;
        }
    }

    public void Update()
    {
        OnUpdate();
    }

    public override void OnUpdate()
    {
        switch (GetFriendState()) {
            case "START":
                nextDialog = "Start";
                GetComponent<ActivateDialogWhenClose>().Execute();
                break;
            case "ADVERTISED":
                nextDialog = "RatHat1";
                GetComponent<ActivateDialogWhenClose>().Execute();
                break;
        }
    }

    public override IEnumerator OnFinishDialogEnumerator()
    {
        switch (GetFriendState()) {
            case "ADVERTISED":
                gameObject.GetComponent<ActivateDialogWhenClose>().canTalkTo = true;
                gameObject.GetComponent<ActivateDialogWhenClose>().autoStart = false;
                yield return base.OnFinishDialogEnumerator();
                break;
            case "OPEN_FOR_BUSINESS":
                // pop the movie state before we push the shop state.
                yield return base.OnFinishDialogEnumerator();

                // After the shop is open for business it's controlled by the Hub_UpgradeStand!
                GUIManager.Instance.Hub_UpgradeStand.enabled = true;

                // First time the shop opens for business it'll go directly into the shop.
                GUIManager.Instance.BaseStatHUD.SetActive(true);
                GUIManager.Instance.GUI_BaseStatUpgrade.GetComponent<GUI_BaseStatUpgrade>().Navigate("");
                GetComponent<ActivateDialogWhenClose>().speechBubbleIcon.SetActive(false);
                break;
        }
    }

    public void RatWithHatIntro()
    {
        dialogManager.ReturnFromAction();
    }

    // User Data implementation
    public override string UserDataKey()
    {
        return "RatWithHat";
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
