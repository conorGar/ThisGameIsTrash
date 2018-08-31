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

    // Use this for initialization
    void Start () {
        // TESTING EVENTS!  Go through the list of friends and let them generate any friend events they'd like to have.
        //FriendManager.Instance.GenerateEvents();
       // StartDay();

        IsDayStarted = true;
    }
	
	// Update is called once per frame
	void Update () {
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

    /*public void MissFriendEventCheck(){ //checked at end of day
    	int currentDayNumber = GlobalVariableManager.Instance.DAY_NUMBER;
    	for(int i = 0; i < friendEvents.Count; i++){
			if(friendEvents[i].day == currentDayNumber){

			}
    	}
    }*/
}
