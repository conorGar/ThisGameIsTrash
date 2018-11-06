using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class SlabFriend : Friend
{
	public int trashInLoveFund;
	public int trashNeeded = 30;
	public GameObject trashGiveHUD;
	public GameObject eyeBreakPS;
    public GameObject eyeCover;
    //public GameObject slabTotalTrashHUD;

    public GameObject moon;
    public GameObject blockade;
    public GameObject moonShadow;

    bool moonInProperLocation;
    int slabDepartureSequence = 0;

    int currentDisplayedTotalTrash;

	public override void GenerateEventData()
    {
        // These guys show up every day.
        day = CalendarManager.Instance.currentDay;
    }

    public new void OnEnable()
    {
        switch (GetFriendState()) {
            case "START":
                break;
            case "WANTS_TRASH":
				GUIManager.Instance.SlabTrashNeededDisplay.GetComponent<TextMeshProUGUI>().text = trashInLoveFund + "/20";
            	currentDisplayedTotalTrash = trashInLoveFund;
            	break;
            case "END":
				blockade.SetActive(false);
                gameObject.SetActive(false);
          
                break;
        }
    }

    private void Update()
    {
        OnUpdate();
    }

    public override void OnUpdate()
    {
        switch (GetFriendState()) {
            case "START":
                break;
            case "WANTS_TRASH":
                nextDialog = "Slab1";
                GetComponent<ActivateDialogWhenClose>().Execute();
                break;
            case "END":
                break;
        }

        if(moon.activeInHierarchy && !moonInProperLocation){
        	moon.transform.position = Vector2.MoveTowards(moon.transform.position, new Vector2(31,50), (3*Time.deltaTime));
			moonShadow.transform.localPosition = Vector2.MoveTowards(moonShadow.transform.localPosition, new Vector2(0,-4.5f), (.4f*Time.deltaTime));

        	if(Vector2.Distance(moon.transform.position,new Vector2(31,60)) <5){
        		moonInProperLocation = true;
        	}
        }

        if(slabDepartureSequence == 1){
			moon.transform.position = Vector2.MoveTowards(moon.transform.position, this.gameObject.transform.position, (10*Time.deltaTime));

        }else if(slabDepartureSequence == 2){
			moon.transform.position = Vector2.MoveTowards(moon.transform.position, new Vector2(54,92), (5*Time.deltaTime));

        }
    }

    public override IEnumerator OnFinishDialogEnumerator()
    {
        yield return new WaitForSeconds(.3f);

        switch (GetFriendState()) {
            case "START":
            case "WANTS_TRASH":
                // Turn into a non-auto start prompt!
                gameObject.GetComponent<ActivateDialogWhenClose>().autoStart = false;
                GetComponent<ActivateDialogWhenClose>().canTalkTo = true;
                StartCoroutine("TotalSlabTrashDisplay");
				CamManager.Instance.mainCamEffects.ReturnFromCamEffect();
                break;
            case "END":
                gameObject.GetComponent<ActivateDialogWhenClose>().ResetDefaults();
                break;
        }
    }

    public void AddTrashToFund(int trashAdded){
    	if(trashAdded == 0)
			dialogManager.JumpToNewNode("SlabNoTrash1");

    	trashInLoveFund += trashAdded;

        // TODO: Maybe not expose GlobalVariableManager.TODAYS_TRASH_AQUIRED directly and just make some AddTrash and Remove Trash functions.
        // That way the GUI gets updated in that function automatically and we don't have to worry we did it correctly all over the code.
        GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[0] -= trashAdded;
        GUIManager.Instance.TrashCollectedDisplayGameplay.UpdateDisplay(GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[0]);
        UserDataManager.Instance.SetDirty();
        dialogManager.ReturnFromAction();
    }

    public void TrashGiveHUDEnable(){
		dialogManager.currentlySpeakingIcon.GetComponent<DialogIconAnimationManager>().Slide();
    	trashGiveHUD.SetActive(true);
    }
    public void EnoughTrashCheck(){
    	if(trashInLoveFund >= trashNeeded){
            dialogManager.JumpToNewNode("SlabComplete");
            SetFriendState("END");
            dialogManager.ReturnFromActionOnSameNode();
        }
        else{
            dialogManager.ReturnFromAction();
        }
    	
    }
    public void GiveAnythingCheck(){
		dialogManager.currentlySpeakingIcon.GetComponent<DialogIconAnimationManager>().SlideBack();

		/*if(trashInLoveFund >= trashNeeded){
    		dialogManager.JumpToNewNode("SlabFinish");
    	}*/
    	dialogManager.ReturnFromAction();
    }

    public void BreakEyes(){
    	Destroy(eyeCover);
    	eyeBreakPS.SetActive(true);
    }

    public override void GiveData(List<GameObject> neededObjects){
    	trashGiveHUD = neededObjects[0];
    	trashGiveHUD.GetComponent<GUI_SlabTrashGiveHUD>().slabFriend = this;
    }

    public void MoonArrive(){
    	StartCoroutine(MoonArriveSequence());
    }

    IEnumerator MoonArriveSequence(){
    	CamManager.Instance.mainCamPostProcessor.profile = null;
    	moon.SetActive(true);
    	yield return new WaitUntil(() => moonInProperLocation);
    	yield return new WaitForSeconds(.5f);
    	dialogManager.ReturnFromAction();

    }

    public void SlabDeparture(){
		StartCoroutine(SlabDepartureSequence());

    }

    public IEnumerator SlabDepartureSequence(){
    	slabDepartureSequence = 1;
    	blockade.SetActive(false);
    	yield return new WaitUntil(() => moon.transform.position.x >= transform.position.x);
    	yield return new WaitForSeconds(.4f);
    	slabDepartureSequence = 2;
    	transform.parent = moon.transform;
    	yield return new WaitForSeconds(1.5f);
    	slabDepartureSequence=3;
    	moon.SetActive(false);
    	dialogManager.ReturnFromAction();
    }

    IEnumerator TotalSlabTrashDisplay(){
    	GUIManager.Instance.SlabTrashNeededDisplay.SetActive(true);
    	if(currentDisplayedTotalTrash < trashInLoveFund){
    		InvokeRepeating("IncreaseDisplayedTrash",0f,.1f);
    	}
		yield return new WaitUntil(() => currentDisplayedTotalTrash >= trashInLoveFund);
		yield return new WaitForSeconds(2f);
		GUIManager.Instance.SlabTrashNeededDisplay.SetActive(false);
    }

    void IncreaseDisplayedTrash(){
		if(currentDisplayedTotalTrash < trashInLoveFund){
			currentDisplayedTotalTrash++;
			GUIManager.Instance.SlabTrashNeededDisplay.GetComponent<TextMeshProUGUI>().text = currentDisplayedTotalTrash + "/20";
		}else{
			CancelInvoke();
		}
    }


    // User Data implementation
    public override string UserDataKey()
    {
        return "Slab";
    }

    public override SimpleJSON.JSONObject Save()
    {
        var json_data = new SimpleJSON.JSONObject();

        json_data["friendState"] = friendState;
        json_data["trashInLoveFund"] = trashInLoveFund;

        return json_data;
    }

    public override void Load(SimpleJSON.JSONObject json_data)
    {
        friendState = json_data["friendState"].AsInt;
        trashInLoveFund = json_data["trashInLoveFund"].AsInt;
    }
}

