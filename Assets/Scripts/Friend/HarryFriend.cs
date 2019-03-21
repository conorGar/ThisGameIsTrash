using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HarryFriend : Friend
{
	public GameObject hubParent; //switches to this after 1st interaction. TODO: not sure if this is the best solution for switching friends to a new 'World"/Scene
	public GameObject shopBlockade;

	public override void GenerateEventData()
    {
		switch (GetFriendState()) {
			case "INTRO":
				if(GlobalVariableManager.Instance.DAY_NUMBER < 11){
					day = CalendarManager.Instance.currentDay;
				}
				break;
			case "END":
			gameObject.transform.parent = hubParent.transform;
				day = CalendarManager.Instance.currentDay;
				shopBlockade.SetActive(false);
				//
				break;
		}
        
    }
	void Start ()
	{
		switch (GetFriendState()) {
		case "INTRO":
         	break;
        case "END":
			gameObject.transform.parent = hubParent.transform;
			shopBlockade.SetActive(false);
            break;
         }
	}
	void OnEnable(){
		switch (GetFriendState()) {
		case "INTRO":
         	break;
        case "END":
			gameObject.transform.parent = hubParent.transform;
			shopBlockade.SetActive(false);
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
            case "INTRO":
				GetComponent<ActivateDialogWhenClose>().autoStart = false;
                GetComponent<ActivateDialogWhenClose>().Execute();
                break;
            case "END":       
                break;
        }
    }

	public override IEnumerator OnFinishDialogEnumerator(bool panToPlayer = true)
    {

        switch (GetFriendState()) {
            case "INTRO":
                IsVisiting = false;
                SetFriendState("END");
				gameObject.transform.parent = hubParent.transform;
                break;
            case "END":
                break;
        }


        yield return base.OnFinishDialogEnumerator();
		gameObject.SetActive(false);

    }

    public void HarryWakeUp(){
    	StartCoroutine("WakeUpSequence");
    }

    IEnumerator WakeUpSequence(){
    	yield return new WaitForSeconds(.2f);
		CamManager.Instance.mainCamPostProcessor.profile = null;//TODO: returns to NO effect, not sure if you want this, future Conor

    	gameObject.GetComponent<tk2dSpriteAnimator>().Play("getUp");
    	yield return new WaitForSeconds(.5f);
    	dialogManager.ReturnFromAction();
    }

	public override void GiveData(List<GameObject> neededObjs){
    	shopBlockade = neededObjs[0];
    }


    public override SimpleJSON.JSONObject Save()
    {
        var json_data = new SimpleJSON.JSONObject();

        json_data["friendState"] = friendState;
        json_data["day"] = day;

       

        return json_data;
    }

    public override void Load(SimpleJSON.JSONObject json_data)
    {
    	Debug.Log("Harry load activate-*-*-*-*-*-*-*-*-*");
        friendState = json_data["friendState"].AsInt;
        day = json_data["day"].AsInt;

    }

}

