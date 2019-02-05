using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RatNearACatFriend : Friend
{


	public GameObject timeBack;
	public Hub_TimeUpgradeStand timeStand;
	public GameObject hubParent; //switches to this after 1st interaction. TODO: not sure if this is the best solution for switching friends to a new 'World"/Scene

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
			gameObject.transform.parent = hubParent.transform;
			timeStand.gameObject.SetActive(true);
			timeStand.enabled = false;
			break;
        case "END":
			gameObject.transform.parent = hubParent.transform;
			timeStand.enabled = true;
			timeStand.gameObject.SetActive(true);
            break;
         }
	}

	public void TimeBackShow(){
		Debug.Log("Time Back show activate ************");
		StartCoroutine("TimeBackShowSequence");
	}

	public IEnumerator TimeBackShowSequence(){
		CamManager.Instance.mainCamPostProcessor.profile = null;
		CamManager.Instance.mainCamEffects.CameraPan(new Vector3(20,29,-10),"");
		CamManager.Instance.mainCamEffects.ZoomInOut(.7f,1f);
		timeBack.SetActive(true);
		yield return new WaitForSeconds(3f);
		dialogManager.ReturnFromAction();
	}


	private void Update()
    {
        OnUpdate();
    }

	public override void OnUpdate()
    {
        switch (GetFriendState()) {
            case "INTRO":
                nextDialog = "RatCatIntro";
                GetComponent<ActivateDialogWhenClose>().Execute();
                break;
            case "HUB_INTRO":
                nextDialog = "RatCat1";
                GetComponent<ActivateDialogWhenClose>().Execute();
                break;
        }
    }

	public override IEnumerator OnFinishDialogEnumerator(bool panToPlayer = true)
    {
        yield return new WaitForSeconds(.3f);

        switch (GetFriendState()) {
            case "INTRO":
                IsVisiting = false;
                gameObject.transform.parent = hubParent.transform;
                SetFriendState("HUB_INTRO");
				yield return base.OnFinishDialogEnumerator();
                gameObject.SetActive(false);
                break;
			case "HUB_INTRO":
				SetFriendState("END");
				// pop the movie state before we push the shop state (no panning).
                yield return base.OnFinishDialogEnumerator(false);

                // After the shop is open for business it's controlled by the Hub_UpgradeStand!
               timeStand.enabled = true;
				GameStateManager.Instance.PushState(typeof(ShopState));
                // First time the shop opens for business it'll go directly into the shop.
                GUIManager.Instance.GUI_TimeUpgrade.gameObject.SetActive(true);
                GUIManager.Instance.GUI_TimeUpgrade.Navigate("");
                GetComponent<ActivateDialogWhenClose>().speechBubbleIcon.SetActive(false);
				break;
            case "END":
                break;
        }


        yield return base.OnFinishDialogEnumerator();


    }

    public override void GiveData(List<GameObject> neededObjs){
    	timeBack = neededObjs[0];
    	timeStand = neededObjs[1].GetComponent<Hub_TimeUpgradeStand>();
    }
}

