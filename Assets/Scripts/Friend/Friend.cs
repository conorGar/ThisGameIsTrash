﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friend : MonoBehaviour {
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
    void OnEnable() {

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

    public virtual void FinishDialogEvent(){

    	//TODO: DEFINATELY change this...
		gameObject.GetComponent<ActivateDialogWhenClose>().dialogManager.GetComponent<DialogManager>().mainCam.GetComponent<Ev_MainCameraEffects>().ReturnFromCamEffect();
		GlobalVariableManager.Instance.PLAYER_CAN_MOVE = true;
    	//nothing to do for basic friend

    }

    public void MissedEvent(){
    	nextDialog = missedDialog;
    }

 

   
}
