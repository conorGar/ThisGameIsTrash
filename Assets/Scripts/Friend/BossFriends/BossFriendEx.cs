using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(ActivateDialogWhenClose))]
public class BossFriendEx : Friend
{

	//*** For now using for all 3 of the trio, change if needed at any point
	
	public tk2dCamera mainCam;
	public ParticleSystem myTeleportPS;
	public BossStuart stuart;
	public GameObject player;
	public GameObject hash;
	public GameObject questio;
    public int FieldRoomNum;
    public int ToxicFieldRoomNum;
	//public GameObject stuartIcon;
	public AudioClip smokePuff;
	public AudioClip bossMusic;
	int enterIconPhase;
	int followingHash;
	// Update is called once per frame
	void Update ()
	{
        OnUpdate();
        if(followingHash == 1){
         if(Vector2.Distance(hash.transform.position,stuart.transform.position) <5){
         	StartCoroutine("HashShieldShow");
         	followingHash = 2;
         }else{
			hash.transform.position = Vector2.MoveTowards(hash.transform.position,stuart.transform.position,10*Time.deltaTime);

         }
        }
	}

    private void OnEnable()
    {
        // TODO: Not really into this but doing it for the demo (might cause a hiccup actually).
        // A lot of these camera variables should be part of a singleton Manager so we don't have to find all these references for every object that uses them.
        base.OnEnable();
        mainCam = GameObject.Find("tk2dCamera").GetComponent<tk2dCamera>();
        player = GameObject.Find("Jim");
    }

    public override void GenerateEventData()
    {
        // These guys show up every day.
        day = CalendarManager.Instance.currentDay;
    }

    // The trio is in the field at the beginning, but then migrate to the toxic field room.
    public override bool IsCurrentRoom(Room room)
    {
        switch (GetFriendState())
        {
            case "IN_FIELD":
                return room.roomNum == FieldRoomNum;
            case "END":
                return false;
        }

        return room.roomNum == ToxicFieldRoomNum;
    }

    // moving down here so all this logic is together
    public override void OnUpdate()
    {
        switch (GetFriendState())
        {
            case "IN_FIELD":
                nextDialog = "Boss1Intro";
                GetComponent<ActivateDialogWhenClose>().Execute();
                break;
            case "IN_TOXIC_FIELD":
                nextDialog = "Boss1Start";
                GetComponent<ActivateDialogWhenClose>().Execute();
                break;
            case "STUART_PEP":
                nextDialog = "Boss1Middle";
				stuart.GetComponent<FollowPlayer>().enabled = false;
				stuart.ActivateHpDisplay();
                GetComponent<ActivateDialogWhenClose>().Execute();
				player.GetComponent<EightWayMovement>().enabled = false;
				player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                break;
            case "STUART_DEFEATED":
                nextDialog = "Boss1Death";
                GetComponent<ActivateDialogWhenClose>().Execute();
                break;
            case "PREP_FIGHT_PHASE_1":
                //player.GetComponent<PlayerTakeDamage>().enabled = true;
               // stuart.GetComponent<FollowPlayer>().enabled = true;
               // SoundManager.instance.musicSource.PlayOneShot(bossMusic);
               // gameObject.SetActive(false);
               // SetFriendState("FIGHT_PHASE_1");
                //StartCoroutine("OnFinishDialogEnumerator");
                break;
            case "PREP_FIGHT_PHASE_2":
                
                player.GetComponent<BoxCollider2D>().enabled = true;
               // gameObject.SetActive(false);
                SetFriendState("FIGHT_PHASE_2");
                break;
        }
    }

    public override IEnumerator OnFinishDialogEnumerator()
    {
        yield return new WaitForSeconds(.3f);
        Debug.Log("$$$$$$$$$$$$$$$$$$$$$$$$$--Finish Dialog Enumerator started--$$$$$$$$$$$$$$$$$$$$$");
        //gameObject.GetComponent<MeshRenderer>().enabled =false;//hide sprite
        myTeleportPS.gameObject.SetActive(true);
        myTeleportPS.Play();

        switch (GetFriendState())
        {
            case "IN_TOXIC_FIELD":
            	//Debug.Log("In toxic field..................x.x.");
                yield return new WaitForSeconds(.8f);
                gameObject.GetComponent<MeshRenderer>().enabled = true;
                GlobalVariableManager.Instance.PLAYER_CAN_MOVE = true;
                mainCam.GetComponent<Ev_MainCameraEffects>().ReturnFromCamEffect();
                gameObject.GetComponent<ActivateDialogWhenClose>().canTalkTo = true;
                gameObject.SetActive(false);
                break;
            case "PREP_FIGHT_PHASE_1":
				//Debug.Log("In prep fight phase 1..................x.x.");
                gameObject.GetComponent<MeshRenderer>().enabled = false;
                hash.GetComponent<MeshRenderer>().enabled = false;
                questio.GetComponent<MeshRenderer>().enabled = false;
                SoundManager.instance.PlaySingle(smokePuff);
                mainCam.GetComponent<Ev_MainCameraEffects>().ZoomInOut(1.5f,2f);
                mainCam.GetComponent<Ev_MainCameraEffects>().CameraPan(stuart.gameObject,true);
                stuart.ActivateHpDisplay();
                yield return new WaitForSeconds(2f);
                Debug.Log("*******^^^^^^CAMERA SHOULDVE GONE BACK TO PLAYER^^^^^^^^^^^^^^^^****");
                gameObject.GetComponent<MeshRenderer>().enabled = true;
				gameObject.SetActive(false);
	
                GlobalVariableManager.Instance.PLAYER_CAN_MOVE = true;
				stuart.GetComponent<FollowPlayer>().enabled = true;
                mainCam.GetComponent<Ev_MainCameraEffects>().ReturnFromCamEffect();
                gameObject.GetComponent<ActivateDialogWhenClose>().canTalkTo = true;
				SetFriendState("FIGHT_PHASE_1");
                break;
            case "FIGHT_PHASE_2":
            	hash.GetComponent<MeshRenderer>().enabled = true;
            	questio.GetComponent<MeshRenderer>().enabled = true;
			Debug.Log("*******^^^^^^PREP FIGHT PHASE 2^^^^^^^^^^^^^^^^****");
				mainCam.GetComponent<Ev_MainCameraEffects>().CameraPan(hash.gameObject,true);
				followingHash =1;
              yield return new WaitForSeconds(.4f);
              /*
                gameObject.GetComponent<MeshRenderer>().enabled = true;

                GlobalVariableManager.Instance.PLAYER_CAN_MOVE = true;
                mainCam.GetComponent<Ev_MainCameraEffects>().ReturnFromCamEffect();
                gameObject.GetComponent<ActivateDialogWhenClose>().canTalkTo = true;
				gameObject.SetActive(false);*/
                break;
			
            case "END":
                yield return new WaitForSeconds(1f);
                gameObject.GetComponent<MeshRenderer>().enabled = false;
                GlobalVariableManager.Instance.PLAYER_CAN_MOVE = true;
                mainCam.GetComponent<Ev_MainCameraEffects>().ReturnFromCamEffect();
                gameObject.GetComponent<ActivateDialogWhenClose>().canTalkTo = true;
                SetFriendState("END");
                stuart.gameObject.SetActive(false);
                gameObject.SetActive(false);
                break;
        }
    }

    public override void OnActivateRoom()
    {
        switch (GetFriendState())
        {
            case "IN_TOXIC_FIELD":
            case "STUART_PEP":
            case "STUART_DEFEATED":
                gameObject.GetComponent<ActivateDialogWhenClose>().canTalkTo = true;
                gameObject.GetComponent<ActivateDialogWhenClose>().yDistanceThreshold = 15;
                gameObject.GetComponent<ActivateDialogWhenClose>().yDistanceThreshold = 22;
                break;
            case "FIGHT_PHASE_1":
            case "LEFT_FIGHT_PHASE_1":
                SetFriendState("PREP_FIGHT_PHASE_1");
                break;
           
            case "LEFT_FIGHT_PHASE_2":
            	Debug.Log("LEFT FIGHT PHASE 2 Check -x-x-x-");
                SetFriendState("PREP_FIGHT_PHASE_2");
             	if(stuart == null){
             		StartCoroutine(ValueSetDelay());// stuart value wasnt being set in time for this check(by BossStuart script)
             	}else{
					stuart.PrepPhase2();
					gameObject.SetActive(false);
				}
                break;

        }
    }

    public override void OnDeactivateRoom()
    {
        switch (GetFriendState())
        {
            case "FIGHT_PHASE_1":
                stuart.GetComponent<FollowPlayer>().enabled = false;
                stuart.ResetBossPositions();
                // TODO: Play normal music level music.

                SetFriendState("LEFT_FIGHT_PHASE_1");
                break;
            case "FIGHT_PHASE_2":
                stuart.bossTrio.SetActive(false);
                stuart.bossEx.SetActive(false);
                stuart.bossHash.SetActive(false);
                stuart.bossQuestio.SetActive(false);
                stuart.GetComponent<FollowPlayer>().enabled = false;
                transform.gameObject.SetActive(false);
                stuart.ResetBossPositions();
                // TODO: Play normal music level music.

                SetFriendState("LEFT_FIGHT_PHASE_2");
                break;
        }
        stuart.GetComponent<BossStuart>().hpDisplay.SetActive(false);
    }
    IEnumerator HashShieldShow(){
    	Debug.Log("Hash shield show activate");
    	hash.GetComponent<tk2dSpriteAnimator>().Play("cast");
    	if(hash.transform.position.x < stuart.transform.position.x)
    		hash.GetComponent<Rigidbody2D>().AddForce(new Vector2(3,5f),ForceMode2D.Impulse); //hash jumps on Stuart
    	else
			hash.GetComponent<Rigidbody2D>().AddForce(new Vector2(-3,5f),ForceMode2D.Impulse); //hash jumps on Stuart

    	hash.GetComponent<Rigidbody2D>().gravityScale = 1;
    	yield return new WaitForSeconds(1f);
		hash.GetComponent<Rigidbody2D>().gravityScale = 0;
		stuart.transform.Find("shield").gameObject.SetActive(true);
		hash.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		yield return new WaitForSeconds(1f);
		GameObject healIcon = ObjectPool.Instance.GetPooledObject("effect_HealMarker",hash.transform.position);
		healIcon.transform.GetChild(0).GetComponent<tk2dTextMesh>().text = "10";
		healIcon.GetComponent<Rigidbody2D>().velocity = Vector2.up;
		yield return new WaitForSeconds(1f);

		//Questio Shows gloves
		mainCam.GetComponent<Ev_MainCameraEffects>().CameraPan(questio.gameObject,true);

		questio.GetComponent<tk2dSpriteAnimator>().Play("ShowGloves");
		questio.transform.Find("throwingGloves").gameObject.SetActive(true);

		yield return new WaitForSeconds(2f);
		questio.transform.Find("throwingGloves").gameObject.SetActive(false);
        GlobalVariableManager.Instance.PLAYER_CAN_MOVE = true;
        mainCam.GetComponent<Ev_MainCameraEffects>().ReturnFromCamEffect();
        stuart.PrepPhase2();
		gameObject.SetActive(false);

    }

    IEnumerator ValueSetDelay(){
    	yield return new WaitUntil(() => stuart != null);
		stuart.PrepPhase2();
		gameObject.SetActive(false);
    }
    // User Data implementation
    public override string UserDataKey()
    {
        return "BossFriendEx";
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

    public void StuartZoom(){
    	dialogManager.ChangeIcon("iconZoom");
    }
}

