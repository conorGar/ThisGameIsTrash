using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CooneliusFriend : Friend
{

	public List<CoonItemSpawner> possibleLocations = new List<CoonItemSpawner>();
	//public CoonScavengerHuntItem[] items = new CoonScavengerHuntItem[3];
	List<CoonScavengerHuntItem> pickedUpItems = new List<CoonScavengerHuntItem>();

	string riddle1Text;
	string riddle2Text;
	string riddle3Text;
	int riddleGetCounter;



	public override void GenerateEventData()
    {
		switch (GetFriendState()) {
			case "INTRO":
				day = CalendarManager.Instance.currentDay;
				break;
			case "HUB_INTRO":
				day = CalendarManager.Instance.currentDay;
				//
				break;
			case "END":
				day = CalendarManager.Instance.currentDay;
				//timeStand.enabled = true;
				break;
		}
        
    }

	void Start ()
	{
		switch (GetFriendState()) {
		case "INTRO":
         	break;
		case "HUB_INTRO":
			
			break;
        case "END":
			
            break;
         }
	}
	
	// Update is called once per frame
	void Update ()
	{
		OnUpdate();
	}

	public override void OnUpdate(){
		switch (GetFriendState()) {
            case "INTRO":
                nextDialog = "CoonIntro";
                GetComponent<ActivateDialogWhenClose>().Execute();
                break;
            case "IN_COMPETITION":
            	if(pickedUpItems.Count >2){
                	nextDialog = "CoonLost";
					GetComponent<ActivateDialogWhenClose>().Execute();
                }else{
					nextDialog = "RiddleStart";
                }
                //GetComponent<ActivateDialogWhenClose>().Execute();
                break;
			case "INTRO_LOST":
                nextDialog = "CoonLost";
                GetComponent<ActivateDialogWhenClose>().Execute();
                break;
            case "INTRO_WON":
				nextDialog = "CoonWon";
           	    GetComponent<ActivateDialogWhenClose>().Execute();
            	break;
            case "END":
                break;
        }
	}
	public override IEnumerator OnFinishDialogEnumerator(bool panToPlayer = true)
    {
        yield return new WaitForSeconds(.3f);

        switch (GetFriendState()) {
            case "INTRO":
             
                SetFriendState("INTRO_NOTPLAYED");
                break;
			case "INTRO_LOST":
             
                SetFriendState("INTRO_NOTPLAYED");
                break;
			case "INTRO_WON":
             
                SetFriendState("INTRO_END");
                break;
            case "END":
                break;
        }


        yield return base.OnFinishDialogEnumerator();


    }
    public override bool DayEndEventCheck(){
		switch (GetFriendState()) {
            case "IN_COMPETITION":
            	StartCoroutine("EndDaySetup");
                return true;
                break;
        }

        return false;
    }

	public void RiddleSetup(){
		StartCoroutine("RiddleSetupSequence");
	}

	IEnumerator RiddleSetupSequence(){
		//fade to black and position coons properly, then return to riddleStart Dialog
		Ev_FadeHelper.Instance.StartCoroutine("Fade");
		yield return new WaitForSeconds(1f);
		Ev_FadeHelper.Instance.FadeIn();
		dialogManager.JumpToNewNode("RiddleStart");
		dialogManager.ReturnFromAction();
	}



	public void CoonsTurn(){
		StartCoroutine("CoonsTurnSequence");

	}

	IEnumerator CoonsTurnSequence(){
		//show coons turning around to face player
		CamManager.Instance.mainCamPostProcessor.profile = null;
		yield return new WaitForSeconds(1f);
		dialogManager.ReturnFromAction();
	}


	public void ChooseLocations(){

		Debug.Log("Choose Locations activated");
		int pos1 = Random.Range(0,possibleLocations.Count);
		int pos2 = Random.Range(0,possibleLocations.Count);
		int pos3 = Random.Range(0,possibleLocations.Count);

		while(pos2 == pos1){
			pos2 = Random.Range(0,possibleLocations.Count);
		}

		while(pos3 == pos1 || pos3 == pos2){
			pos3 = Random.Range(0,possibleLocations.Count);
		}

		possibleLocations[pos1].gameObject.SetActive(true);
		GameObject item1 = ObjectPool.Instance.GetPooledObject("ScavengerHuntItem");
		possibleLocations[pos1].SetItem(item1);
		possibleLocations[pos2].gameObject.SetActive(true);
		GameObject item2 = ObjectPool.Instance.GetPooledObject("ScavengerHuntItem");
		possibleLocations[pos2].SetItem(item2);
		possibleLocations[pos3].gameObject.SetActive(true);
		GameObject item3 = ObjectPool.Instance.GetPooledObject("ScavengerHuntItem");
		possibleLocations[pos3].SetItem(item3);
		riddle1Text = possibleLocations[pos1].myRiddleText;
		riddle2Text = possibleLocations[pos2].myRiddleText;
		riddle3Text = possibleLocations[pos3].myRiddleText;
		dialogManager.ReturnFromAction();



	}

	public IEnumerator EndDaySetup(){
		//move the player to the raccoon room and initiate either the lost or win dialog
		Ev_FadeHelper.Instance.StartCoroutine("Fade");
		yield return new WaitForSeconds(1.5f);
		//TODO: Move player to d2 pos

		Room myRoom = null;

		//TODO: better way to get coon room value?!?
		for(int i = 0; i < RoomManager.Instance.rooms.Count; i++){
			if(RoomManager.Instance.rooms[i].name == "d2"){
				myRoom = RoomManager.Instance.rooms[i];
			}
		}
	
		RoomManager.Instance.currentRoom  = myRoom;
		Ev_FadeHelper.Instance.FadeIn();
		yield return new WaitForSeconds(1.5f);
		if(pickedUpItems.Count >2){
			SetFriendState("INTRO_LOST");
		}else{
			SetFriendState("INTRO_WON");
		}
	}


	public override string GetVariableText(string varKey)
    {
    	Debug.Log("var key = " +varKey);
        switch (varKey) {
            case "myRiddle":
            	Debug.Log("Read it as riddle");
            	if(riddleGetCounter == 0){
            		Debug.Log("Riddle sending back r1: " +riddle1Text);
                	return riddle1Text;
				}else if(riddleGetCounter == 1){
					Debug.Log("Riddle sending back r2: " +riddle2Text);

					return riddle2Text;
				}else
					return riddle3Text;

				riddleGetCounter++;
        }

        return base.GetVariableText(varKey);
    }

	public void Smug()
    {
        DialogManager.Instance.currentlySpeakingIcon.SetAnimBool("IsSmug", true);
        DialogManager.Instance.ReturnFromAction();
    }

    public void Calm()
    {
        DialogManager.Instance.currentlySpeakingIcon.SetAnimBool("IsSmug", false);
        DialogManager.Instance.ReturnFromAction();
    }


	public override void PickUpObject(CoonScavengerHuntItem item){
		//TODO:show coon HUD and fill in need spots like with Rock's HUD.
		pickedUpItems.Add(item);
		GUIManager.Instance.coonScavengerHUD.UpdateItemsCollected(item.GetComponent<SpriteRenderer>().sprite);
    	StartCoroutine("TotalProductsDisplay");
	}

	IEnumerator TotalProductsDisplay(){
    	GUIManager.Instance.coonScavengerHUD.gameObject.SetActive(true);

		yield return new WaitForSeconds(2f);
		GUIManager.Instance.coonScavengerHUD.gameObject.SetActive(false);
    }

	public override void GiveData(List<GameObject> neededObjs){ //given by 'DialogActivators' in tempManager

		for(int i = 0 ; i < neededObjs.Count;i++){
			possibleLocations.Add(neededObjs[i].GetComponent<CoonItemSpawner>());
		}
    }

	public override string GetEventDescription(){
    	return "The great Coonelius wants to challenge you to a trash hunt.";
    }

}

