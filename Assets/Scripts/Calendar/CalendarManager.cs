using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalendarManager : MonoBehaviour {
    public static CalendarManager Instance;

    public int maxDays = 30;
    public int currentDay = 0;
    private bool IsDayStarted = false;
    public List<FriendEvent> friendEvents = new List<FriendEvent>();

    void Awake()
    {
        Instance = this;
    }

    public void StartDay()
    {
        currentDay = GlobalVariableManager.Instance.DAY_NUMBER - 1;
        FriendManager.Instance.GenerateEvents();
        for (int i = 0; i < friendEvents.Count; ++i)
        {
            if (friendEvents[i].day == currentDay)
            {
                friendEvents[i].Execute();
            }
        }

        IsDayStarted = true;
    }

    void EndDay ()
    {
        for (int i=0; i < friendEvents.Count; ++i)
        {
            if (friendEvents[i].day == currentDay)
            {
               // friendEvents[i].Execute();

                // remove and update the iterator as to not mess things up. MUST BE THE LAST OPERATION.
                friendEvents[i].friend.MissedEvent();
                friendEvents.RemoveAt(i--);
            }
        }
    }

    public void AddFriendEvent(FriendEvent friendEvent)
    {
        if (friendEvent.friend == null)
        {
            Debug.Log("No Friend defined for Friend Event!");
            return;
        }

        if (friendEvent.day < 0 || friendEvent.day > maxDays)
        {
            Debug.Log("Day is out of range for Friend Event!");
            return;
        }

        friendEvents.Add(friendEvent);
    }

    public List<FriendEvent> GetFriendEvents(){
    	return friendEvents;
    }

    public void ClearFriendEvents()
    {
        friendEvents.Clear();
    }

    /*public void MissFriendEventCheck(){ //checked at end of day
    	int currentDayNumber = GlobalVariableManager.Instance.DAY_NUMBER;
    	for(int i = 0; i < friendEvents.Count; i++){
			if(friendEvents[i].day == currentDayNumber){

			}
    	}
    }*/
}
