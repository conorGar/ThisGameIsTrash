using System.Collections;
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
	protected FriendEvent newestAddedEvent;
    [HideInInspector]
	public string missedDialog;

	[HideInInspector]
	public DialogManager dialogManager;// needed for returning from events. Given by dialogManager when activate friend event.

    // Use this for initialization
    protected void OnEnable() {
    	Debug.Log("Next Dialog: " + nextDialog);
        gameObject.GetComponent<ActivateDialogWhenClose>().SetDialog(myDialogDefiniton);
        gameObject.GetComponent<ActivateDialogWhenClose>().startNodeName = nextDialog;
        gameObject.GetComponent<ActivateDialogWhenClose>().autoStart = true;

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
        IsVisiting = false;
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

    public virtual void MissedEvent(){
    	//nextDialog = missedDialog; Commented this out because didn't work well with new friend state system. For now everything is handeld by specific friend class(JumboFriend). As always, don't yell at me. Ok thanks.
    	//Nothing to do for basic friend

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

    // Runs when the world starts (hub, world 1, world 2, etc..)  useful for setting friend state quest stuff and making sure everything in the world is set up properly.
    public virtual void OnWorldStart(World world)
    {

    }

    public virtual void OnFinishDialog()
    {
        //CamManager.Instance.mainCamEffects.ReturnFromCamEffect();
        DialogManager.Instance.currentlySpeakingIcon.ResetIconPositionsOnScreen();
        DialogManager.Instance.currentlySpeakingIcon.gameObject.SetActive(false);
        //nothing to do for basic friend

        StartCoroutine(OnFinishDialogEnumerator());
    }

    public virtual IEnumerator OnFinishDialogEnumerator(bool panToPlayer = true)
    {

        // Pop Movie State.
        GameStateManager.Instance.PopState();

        CamManager.Instance.mainCamEffects.ReturnFromCamEffect();

        // Slower pan back to the player.  This is probably a sucky way to do this. (Turns out it is!  Like transitioning into stuff after a dialog)
        if (panToPlayer) {
            CamManager.Instance.mainCam.SetSlowCameraSpeed();
            yield return new WaitForSeconds(2f);
            CamManager.Instance.mainCam.SetNormalCameraSpeed();
        }
    }
	void DayAsStringSet(){
		dialogManager.variableText = newestAddedEvent.day.ToString();
		Debug.Log(">>>>>DAY AS STRING SET TO: " + newestAddedEvent.day.ToString());
		dialogManager.Invoke("ReturnFromAction",.1f);

	}


    public void CalendarMark()
    {
        dialogManager.textBox.SetActive(false);
        dialogManager.currentlySpeakingIcon.gameObject.SetActive(false);

        GUIManager.Instance.CalendarHUD.gameObject.SetActive(true);
        GUIManager.Instance.CalendarHUD.NewMarkSequence(newestAddedEvent.day, name);
        GUIManager.Instance.CalendarHUD.Invoke("LeaveScreen", 4f);
        dialogManager.Invoke("ReturnFromAction", 5f);
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
