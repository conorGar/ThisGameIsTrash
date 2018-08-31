using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StoneFriend : Friend
{

	List<GameObject> handsDelivered = new List<GameObject>();//hands should be children, so that there positions are saved
	public DialogManager dialogManager;

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


}

