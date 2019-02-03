using UnityEngine;
using System.Collections;

public class RatNearACatFriend : Friend
{


	public GameObject timeBack;
	public Hub_TimeUpgradeStand timeStand;

	public override void GenerateEventData()
    {
		switch (GetFriendState()) {
			case "INTRO":
				day = -1;
				break;
			case "HUB_INTRO":
				day = CalendarManager.Instance.currentDay;
				timeStand.enabled = false;
				break;
			case "END":
				day = CalendarManager.Instance.currentDay;
				timeStand.enabled = true;
				break;
		}
        
    }

	void Start ()
	{
	
	}

	public IEnumerator TimeBackShow(){
		timeBack.SetActive(true);
		yield return new WaitForSeconds(3f);
		dialogManager.ReturnFromAction();
	}
	
	public override void OnUpdate()
    {
        switch (GetFriendState()) {
            case "INTRO":
                nextDialog = "RatCatIntro";
                GetComponent<ActivateDialogWhenClose>().Execute();
                break;
            case "HUB_INTRO":
                nextDialog = "RatCat1";
                GetComponent<ActivateDialogWhenClose>().Execute();
                break;
        }
    }
}

