using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RatWhoFatFriend : Friend
{

	public GameObject garbageTruck;
	public GameObject pinCase;
	public GameObject sweatPS;
	public AudioClip footStep;
	int walkCounter;
	bool isWalking;
	Vector2 targetPos;
	// Use this for initialization

	public override void GenerateEventData()
    {
        // shows up every day.
        day = CalendarManager.Instance.currentDay;
    }
	void Start ()
	{
		targetPos = new Vector2(15f,8f);
	}
	public new void OnEnable()
    {

        switch (GetFriendState()) {
        	
            case "START":
                gameObject.GetComponent<ActivateDialogWhenClose>().autoStart = true;
				gameObject.GetComponent<ActivateDialogWhenClose>().canTalkTo = true;
				gameObject.GetComponent<ActivateDialogWhenClose>().dialogDefiniton = myDialogDefiniton;
				Debug.Log("GOT HERE FAT RAT");
                break;
            case "INTRODUCED":
                gameObject.GetComponent<ActivateDialogWhenClose>().autoStart = false;
				gameObject.GetComponent<ActivateDialogWhenClose>().enabled = false;
                break;
          
        }
    }
	// Update is called once per frame
	void Update ()
	{
		if(isWalking){
			gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, targetPos, .3f*Time.deltaTime);
		}
		OnUpdate();
	}

    public override void OnUpdate()
    {
        switch (GetFriendState()) {
            case "START":
                nextDialog = "RatFatIntro";
                GetComponent<ActivateDialogWhenClose>().Execute();

                break;
            /*case "ADVERTISED":
                nextDialog = "RatHat1";
                GetComponent<ActivateDialogWhenClose>().Execute();
                break;*/
        }
    }
	public override IEnumerator OnFinishDialogEnumerator()
    {
        switch (GetFriendState()) {
            case "START":
			    SetFriendState("INTRODUCED");

                gameObject.GetComponent<ActivateDialogWhenClose>().canTalkTo = false;
                gameObject.GetComponent<ActivateDialogWhenClose>().autoStart = false;
                break;
        }

        yield return base.OnFinishDialogEnumerator();
    }
	public void FatRatWalk(){
		Debug.Log("Fat Rat Walk activate");
		sweatPS.SetActive(true);
		CamManager.Instance.mainCamEffects.ZoomInOut(1.6f,1);
	
		//this.gameObject.GetComponent<tk2dSpriteAnimator>().enabled = true;

		this.gameObject.GetComponent<tk2dSpriteAnimator>().Play("fatRatWalk");
		//this.gameObject.GetComponent<tk2dSpriteAnimator>().enabled = false;
		Debug.Log(this.gameObject.GetComponent<tk2dSpriteAnimator>().CurrentClip.name);
		walkCounter++;
		if(walkCounter == 3){
			targetPos = new Vector2(12f,6f);
		}
		isWalking = true;
		InvokeRepeating("PlayFootstep",0f,.33f);
		StartCoroutine("WalkSequence");
	}

	IEnumerator WalkSequence(){
		Debug.Log("Walk Sequence Start");
		CamManager.Instance.mainCamPostProcessor.profile = null;
		yield return new WaitForSeconds(3f);
		isWalking = false;
		//this.gameObject.GetComponent<tk2dSpriteAnimator>().enabled = true;
		CancelInvoke();
		gameObject.GetComponent<tk2dSpriteAnimator>().Play("fatRatIdle");
		//this.gameObject.GetComponent<tk2dSpriteAnimator>().enabled = false;

		Debug.Log(this.gameObject.GetComponent<tk2dSpriteAnimator>().CurrentClip);
		Debug.Log(this.gameObject.GetComponent<tk2dSpriteAnimator>().CurrentClip.name);
		CamManager.Instance.mainCamEffects.ZoomInOut(1.15f,1);
		sweatPS.SetActive(false);
		dialogManager.ReturnFromAction();
		Debug.Log("Walk Sequence End");

		StopCoroutine("WalkSequence");

	}

	public void TruckPan(){
		//dialogManager.currentlySpeakingIcon.gameObject.layer = 0; // turn icon invisible
		CamManager.Instance.mainCamPostProcessor.profile = null;
		CamManager.Instance.mainCamEffects.CameraPan(garbageTruck.transform.position, "");
		dialogManager.ReturnFromAction();

	}

	public void PinCasePan(){
		CamManager.Instance.mainCamPostProcessor.profile = null;
		CamManager.Instance.mainCamEffects.CameraPan(garbageTruck.transform.position, "");
		dialogManager.ReturnFromAction();

	}

	public void PanReturn(){
		//dialogManager.currentlySpeakingIcon.gameObject.layer = 5; // turn icon visible again
		CamManager.Instance.mainCamEffects.CameraPan(gameObject.transform.position, "");

		dialogManager.ReturnFromAction();

	}


	void PlayFootstep(){
		SoundManager.instance.RandomizeSfx(footStep,.9f,1.1f);
	}

	public override void GiveData(List<GameObject> neededObjs){
		garbageTruck = neededObjs[0];
		pinCase= neededObjs[1];

	}

	// User Data implementation
    public override string UserDataKey()
    {
        return "RatWhoFat";
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

