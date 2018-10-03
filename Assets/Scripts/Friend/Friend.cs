using System.Collections;
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
	protected FriendEvent newestAddedEvent;
	public GameObject calendar;

	[HideInInspector]
	public string missedDialog;

	[HideInInspector]
	public DialogManager dialogManager;// needed for returning from events. Given by dialogManager when activate friend event.

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
	public virtual void GiveData(List<GameObject> neededObjs){

    	// nothing to do for a basic friend.
    }
    public virtual void Execute()
    {
        IsVisiting = true;
    }

    public virtual void FinishDialogEvent(){

    	//TODO: DEFINATELY change this...
    	Debug.Log("FINISH DIALOG EVENT ACTIVATE");
		dialogManager.GetComponent<DialogManager>().mainCam.GetComponent<Ev_MainCameraEffects>().ReturnFromCamEffect();
		GlobalVariableManager.Instance.PLAYER_CAN_MOVE = true;
    	//nothing to do for basic friend

    }

    public void MissedEvent(){
    	nextDialog = missedDialog;
    }
	void DayAsStringSet(){
		dialogManager.variableText = newestAddedEvent.day.ToString();
		Debug.Log(">>>>>DAY AS STRING SET TO: " + newestAddedEvent.day.ToString());
		dialogManager.Invoke("ReturnFromAction",.1f);

	}


	public void CalendarMark(){
		dialogManager.textBox.SetActive(false);
		dialogManager.currentlySpeakingIcon.SetActive(false);
		//Debug.Log(friend.name);
		//Debug.Log(newestAddedEvent.day);
		calendar.SetActive(true);
		calendar.GetComponent<HUD_Calendar>().NewMarkSequence(newestAddedEvent.day,name);
		calendar.GetComponent<HUD_Calendar>().Invoke("LeaveScreen",4f);
		dialogManager.Invoke("ReturnFromAction",5f);

	}
 

   
}
