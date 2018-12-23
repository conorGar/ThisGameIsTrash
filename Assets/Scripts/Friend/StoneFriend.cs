using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;


public class StoneFriend : Friend
{

	public List<GameObject> handsDelivered = new List<GameObject>();//hands should be children, so that there positions are saved
	public GameObject eyeBreakPS;
    public GameObject eyeCover;
    public GameObject rocket;
    public GameObject fader;
    public GameObject blockade;
    public GameObject stoneHand;
    public GameObject secondStoneHand;
    public GameObject rightHand;
    public GameObject leftHand;
    public GameObject rock;
    public GameObject slab;

    bool rocketIsLaunching;
    Vector2 rocketDestination;

    public new void OnEnable()
    {
        switch (GetFriendState()) {
            case "START":
                break;
            case "WANTS_HANDS":
                // Eyes open.
                BreakEyes();
                break;
            case "ONE_MORE_HAND":
				BreakEyes();
				nextDialog = "StoneRequest";
				rightHand.SetActive(true);
                break;
            case "END":
				blockade.SetActive(false);
                gameObject.SetActive(false);
          
                break;
        }
    }

    void OnDisable()
    {
        GUIManager.Instance.StoneHandNeededDisplay.SetActive(false);
    }

    public override IEnumerator OnFinishDialogEnumerator(bool panToPlayer = true)
    {
        yield return new WaitForSeconds(.3f);

        switch (GetFriendState()) {
            case "START":
            case "WANTS_HANDS":
                // Turn into a non-auto start prompt!
                gameObject.GetComponent<ActivateDialogWhenClose>().autoStart = false;
                GetComponent<ActivateDialogWhenClose>().canTalkTo = true;
                StartCoroutine(TotalStoneHandDisplay());
                break;
            case "END":
				CamManager.Instance.mainCamEffects.CameraPan(slab.transform.position,"");
            	yield return new WaitForSeconds(.5f);
            	slab.GetComponent<SlabFriend>().Sleep();
				yield return new WaitForSeconds(.5f);
            	CamManager.Instance.mainCamEffects.CameraPan(rock.transform.position,"");
            	yield return new WaitForSeconds(.5f);
            	rock.GetComponent<RockFriend>().Sleep();
            	yield return new WaitForSeconds(.5f);
                gameObject.GetComponent<ActivateDialogWhenClose>().ResetDefaults();
                break;
        }

        yield return base.OnFinishDialogEnumerator(panToPlayer);

    }

    private void Update(){
    	if(rocketIsLaunching){
    		gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position,rocketDestination,5*Time.deltaTime);
    	}

    	OnUpdate();
    }
	public override void OnUpdate()
    {
        switch (GetFriendState()) {
            case "START":
                break;
            case "WANTS_HANDS":
                nextDialog = "StoneRequest";
                GetComponent<ActivateDialogWhenClose>().Execute();
                break;
            case "END":
                break;
        }

    }
    public override void GenerateEventData()
    {
        // These guys show up every day.
        day = CalendarManager.Instance.currentDay;
    }

	public void DeliverObject(GameObject obj)
    {
    	handsDelivered.Add(obj);
    }
	IEnumerator TotalStoneHandDisplay(){
    	GUIManager.Instance.StoneHandNeededDisplay.SetActive(true);
		GUIManager.Instance.StoneHandNeededDisplay.GetComponent<TextMeshProUGUI>().text = handsDelivered.Count + "/2";

		yield return new WaitForSeconds(2f);
		GUIManager.Instance.StoneHandNeededDisplay.SetActive(false);
    }
    public void BuildRocket(){
		CamManager.Instance.mainCamPostProcessor.profile = null;
		fader.SetActive(true);
    	StartCoroutine(BuildRocketSequence());
    }
    IEnumerator BuildRocketSequence(){
		yield return new WaitForSeconds(2f);
		fader.GetComponent<Animator>().Play("Fade");
		gameObject.GetComponent<Renderer>().sortingLayerName = "Layer03";
    	rocket.SetActive(true);
    	yield return new WaitForSeconds(1f);
		dialogManager.ReturnFromAction();

    }

    public void StoneEnding(){
		CamManager.Instance.mainCamPostProcessor.profile = null;
		SetFriendState("END");
    	StartCoroutine(StoneEndingSequence());

    }

    IEnumerator StoneEndingSequence(){
		CamManager.Instance.mainCamEffects.CameraPan(gameObject.transform.position,null);
		CamManager.Instance.mainCam.ScreenShake(3f);
    	yield return new WaitForSeconds(1f);
    	rocketDestination = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y +20f);
    	rocketIsLaunching = true;
    	yield return new WaitForSeconds(2f);
		blockade.SetActive(false);
		dialogManager.ReturnFromAction();


		
    }

    public void ShowRockHand(){
		CamManager.Instance.mainCamPostProcessor.profile = null;
		fader.SetActive(true);
		fader.GetComponent<Animator>().Play("FadeOut");
		SetFriendState("ONE_MORE_HAND");
		StartCoroutine(NewHandSequence());
    }

    IEnumerator NewHandSequence(){
    	yield return new WaitForSeconds(1f);
    	rightHand.SetActive(true);
		fader.GetComponent<Animator>().Play("Fade");
		yield return new WaitForSeconds(1f);
		dialogManager.ReturnFromAction();
    }

    public void BreakEyes(){
    	eyeCover.SetActive(false);
    	eyeBreakPS.SetActive(true);
    }

    public void Sleep(){
    	eyeCover.SetActive(true);
    	SetFriendState("END");
    }

    // User Data implementation
    public override string UserDataKey()
    {
        return "Stone";
    }

    public override SimpleJSON.JSONObject Save()
    {
        var json_data = new SimpleJSON.JSONObject();

        json_data["friendState"] = friendState;

        return json_data;
    }

    public override void Load(SimpleJSON.JSONObject json_data)
    {
        friendState = json_data["friendState"].AsInt;
    }
}

