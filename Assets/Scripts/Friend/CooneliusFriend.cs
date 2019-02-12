using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CooneliusFriend : Friend
{

	public List<CoonItemSpawner> possibleLocations = new List<CoonItemSpawner>();
	//public CoonScavengerHuntItem[] items = new CoonScavengerHuntItem[3];
	List<CoonScavengerHuntItem> pickedUpItems = new List<CoonScavengerHuntItem>();

	string riddle1Text;// given by CoonItemSpawner
	string riddle2Text;// given by CoonItemSpawner
	string riddle3Text;// given by CoonItemSpawner
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
			case "INVITE_TO_THIRD_SCREENING":
                nextDialog = "Jumbo3";
                GetComponent<ActivateDialogWhenClose>().Execute();
                break;
            case "MISSED_SCREENING":
				nextDialog = "JumboMissed";
           	    GetComponent<ActivateDialogWhenClose>().Execute();
            	break;
           	case "MISSED_SECOND_SCREENING":
				nextDialog = "JumboMissed2_1";
                GetComponent<ActivateDialogWhenClose>().Execute();
           		break;
            case "END":
                break;
        }
	}

	public void RiddleSetup(){
		//fade to black and position coons properly, then return to riddleStart Dialog
	}


	public void CoonsTurn(){


	}

	void ChooseLocations(){
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


	}
	public override string GetVariableText(string varKey)
    {
        switch (varKey) {
            case "myRiddle":
            	if(riddleGetCounter == 0)
                	return riddle1Text;
				else if(riddleGetCounter == 0)
					return riddle2Text;
				else
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

