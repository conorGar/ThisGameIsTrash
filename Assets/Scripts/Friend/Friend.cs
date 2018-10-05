﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friend : UserDataItem {
    public string friendName = "Unknown Friend";
    public int day = 0;
    public bool activateDialogWhenClose = true;
    // Is the friend visiting the level on this day.
    public bool IsVisiting = false;
    public string nextDialog;
	public DialogDefinition myDialogDefiniton;
	public bool tempFriend; //used for guys who arent really friends, but have dialogs(ex;bosses)

	[HideInInspector]
	public string missedDialog;

    // Use this for initialization
    protected void OnEnable() {

    	int currentDayNumber = GlobalVariableManager.Instance.DAY_NUMBER;

    	//Enable ability to enter dialog on proper day
    	Debug.Log("Next Dialog: " + nextDialog);
    	if(nextDialog != "Start" && nextDialog != missedDialog && !tempFriend){
    		gameObject.GetComponent<ActivateDialogWhenClose>().enabled = false;
    		for(int i = 0; i < CalendarManager.Instance.friendEvents.Count; i++){
				if(CalendarManager.Instance.friendEvents[i].day == currentDayNumber ){//*********MAYBE MAKE THIS DETERMINE IF FRIEND SPAWNS RATHER THAN IF YOU CAN INTERACT...
					if(CalendarManager.Instance.friendEvents[i].friend.name == friendName){
						gameObject.GetComponent<ActivateDialogWhenClose>().enabled = true;
						if(activateDialogWhenClose){
	    					gameObject.GetComponent<ActivateDialogWhenClose>().autoStart = true;
	    				}
						gameObject.GetComponent<ActivateDialogWhenClose>().SetDialog(myDialogDefiniton);
						gameObject.GetComponent<ActivateDialogWhenClose>().dialogName = nextDialog;
						break;
					}
				}
    		}
    	}else{
	    	if(activateDialogWhenClose){
	    		gameObject.GetComponent<ActivateDialogWhenClose>().autoStart = true;
	    	}
			gameObject.GetComponent<ActivateDialogWhenClose>().SetDialog(myDialogDefiniton);
			gameObject.GetComponent<ActivateDialogWhenClose>().dialogName = nextDialog;
			//Debug.Log("Dialog Definition Name:"+ gameObject.GetComponent<ActivateDialogWhenClose>().dialogDefiniton.name);
		}

		StartingEvents();
    }

    // Update is called once per frame
    void Update() {

    }

    // virtual function to generate an event for this friend so it can be added to the calendar.  Let the friend can decide when they want to show up.
    public virtual FriendEvent GenerateEvent()
    {
        var the_event = new FriendEvent();
        GenerateEventData();
        the_event.friend = this;
        the_event.day = day;

        Debug.Log("New Friend Event Generated:\n" +
                  "Friend: " + the_event.friend.name + "\n" +
                  "Day: " + the_event.day);

        return the_event;
    }

    public virtual void GenerateEventData()
    {
        // nothing to do for a basic friend.
    }

    public virtual void StartingEvents(){

    	// nothing to do for a basic friend.
    }

    public virtual void Execute()
    {
        IsVisiting = true;
    }

    public void MissedEvent(){
    	nextDialog = missedDialog;
    }

    // Useful for checking where a friend is if they like to visit different rooms.
    public virtual bool IsCurrentRoom(Room room)
    {
        return true;
    }

    // Configure Friend States In the Editor Inspector for the friend!
    public List<string> friendStates;
    public int friendState = 0;

    public string GetFriendState()
    {
        return friendStates[friendState];
    }

    public int GetFriendStatePosition(string state_str)
    {
        for (int i = 0; i < friendStates.Count; i++)
        {
            if (friendStates[i] == state_str)
            {
                return i;
            }
        }

        Debug.Log("STATE NOT FOUND!  FIX THIS: " + state_str);
        return 0;
    }

    public void SetFriendState(string state_str)
    {
        for (int i = 0; i < friendStates.Count; i++)
        {
            if (friendStates[i] == state_str)
            {
                friendState = i;

                // TODO: Maybe too aggressive saving here???
                UserDataManager.Instance.SetDirty();
                return;
            }
        }

        Debug.Log("STATE NOT FOUND!  FIX THIS: " + state_str);
    }

    public virtual void OnUpdate()
    {

    }

    public virtual void OnFinishDialog()
    {

        //TODO: DEFINATELY change this...
        gameObject.GetComponent<ActivateDialogWhenClose>().dialogManager.GetComponent<DialogManager>().mainCam.GetComponent<Ev_MainCameraEffects>().ReturnFromCamEffect();
        GlobalVariableManager.Instance.PLAYER_CAN_MOVE = true;
        //nothing to do for basic friend

        StartCoroutine(OnFinishDialogEnumerator());
    }

    public virtual IEnumerator OnFinishDialogEnumerator()
    {
        yield return null;
    }

    public virtual void OnActivateRoom()
    {

    }

    public virtual void OnDeactivateRoom()
    {

    }

    // User Data implementation
    public override string UserDataKey()
    {
        return "FRIEND_DATA";
    }

    public override SimpleJSON.JSONObject Save()
    {
        var json_data = new SimpleJSON.JSONObject();

        return json_data;
    }

    public override void Load(SimpleJSON.JSONObject json_data)
    {

    }
}
