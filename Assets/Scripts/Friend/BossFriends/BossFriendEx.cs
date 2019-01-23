using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossFriendEx : Friend
{

    //*** For now using for all 3 of the trio, change if needed at any point
    public List<ParticleSystem> trioParticleSystems;
	public BossStuart stuart;
	public GameObject player;
    public GameObject ex;
	public GameObject hash;
	public GameObject questio;
	public GameObject slideTrash;
    public int FieldRoomNum;
    public AudioClip trioTheme;
    public AudioClip shieldSound;
    public GameObject toxicBarrier;
    public ParticleSystem toxicBarrierPS;
    public GameObject goWithItDisplay;
    public ParticleSystem toxicPipeFountain;

    public int ToxicFieldRoomNum;
	//public GameObject stuartIcon;
	public AudioClip smokePuff;
	public AudioClip bossMusic;
    public AudioClip windSFX;

	// Update is called once per frame
	void Update ()
	{
        OnUpdate();
	}

    private void OnEnable()
    {
        base.OnEnable();
        player = GameObject.Find("Jim");

        ex.SetActive(true);
        hash.SetActive(true);
        questio.SetActive(true);

		switch (GetFriendState())
        {
			
           
            case "END":
            	
            	toxicBarrier.SetActive(false);
            	toxicBarrierPS.gameObject.SetActive(false);
            	break;
        }
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
               
				GetComponent<ActivateDialogWhenClose>().Execute("Ex", "Questio", "Hash");
                break;
            case "IN_TOXIC_FIELD":
                nextDialog = "Boss1Start";
				GetComponent<ActivateDialogWhenClose>().Execute("Stuart");
                break;
            case "STUART_PEP":
                nextDialog = "Boss1Middle";
                GetComponent<ActivateDialogWhenClose>().Execute("Stuart");
                break;
            case "STUART_DEFEATED":
                nextDialog = "Boss1Death";
                GetComponent<ActivateDialogWhenClose>().Execute("Stuart");
                break;
            
        }
    }

    public override void OnWorldStart(World world)
    {
        // Make sure the toxicBarrier is down if Stuart was defeated previously and we are entering World One.
        if (world.type == WORLD.ONE) {
            switch (GetFriendState()) {
                case "END":
                    // spawn the slideTrash if it wasn't delivered already.
                    if (slideTrash != null) {
                        if (!GlobalVariableManager.Instance.IsLargeTrashDiscovered(slideTrash.GetComponent<Ev_LargeTrash>().garbage.type)) {
                            slideTrash.SetActive(true);
                        }
                        else {
                            slideTrash.SetActive(false);
                        }
                    }

                    toxicPipeFountain.Stop();
                    toxicBarrier.SetActive(false);
                    toxicBarrierPS.Stop();
                    break;
                default:
                    // Any other state the slide is not spawned.
                    if (slideTrash != null)
                        slideTrash.SetActive(false);
                    break;
            }
        }
        else {
            if (slideTrash != null)
                slideTrash.SetActive(false);
        }
    }

    public override IEnumerator OnFinishDialogEnumerator(bool panToPlayer = true)
    {
        yield return new WaitForSeconds(.5f);

        switch (GetFriendState())
        {
            case "IN_TOXIC_FIELD":
            	
                yield return TrioDisappears();
				SoundManager.instance.backupMusicSource.Stop();
            	SoundManager.instance.musicSource.Play();
                break;

            case "PREP_FIGHT_PHASE_1":
                yield return TrioDisappears();
                CamManager.Instance.mainCamEffects.ZoomInOut(1.5f,2f);
                CamManager.Instance.mainCamEffects.CameraPan(stuart.gameObject,true);
                stuart.ActivateHpDisplay();
                yield return new WaitForSeconds(2f);

                SoundManager.instance.TransitionMusic(bossMusic);
                stuart.PrepPhase1();
                panToPlayer = false;
                SetFriendState("FIGHT_PHASE_1");

                break;
            case "PREP_FIGHT_PHASE_2":
                yield return HashShieldShow();

                ex.SetActive(false);
                hash.SetActive(false);
                questio.SetActive(false);

                SetFriendState("FIGHT_PHASE_2");
                break;
            case "END":
                yield return TrioDisappears();
                stuart.gameObject.SetActive(false);
                CamManager.Instance.mainCamEffects.CameraPan(slideTrash.transform.position,"");
                yield return new WaitUntil(() => Vector2.Distance(CamManager.Instance.mainCam.transform.position, slideTrash.transform.position) < 2);
                slideTrash.SetActive(true);
                yield return new WaitForSeconds(1f);
				CamManager.Instance.mainCamEffects.CameraPan(toxicPipeFountain.transform.position,"");
				yield return new WaitForSeconds(1f);
				toxicPipeFountain.Stop();
				yield return new WaitForSeconds(1f);
				CamManager.Instance.mainCamEffects.CameraPan(toxicBarrier.transform.position,"");
				yield return new WaitForSeconds(1f);
				toxicBarrier.SetActive(false);
				goWithItDisplay.SetActive(true);
				toxicBarrierPS.Stop();
				yield return new WaitForSeconds(4f);
				goWithItDisplay.SetActive(false);
                SetFriendState("END");
                break;
        }

        yield return base.OnFinishDialogEnumerator();
        gameObject.GetComponent<ActivateDialogWhenClose>().canTalkTo = true;
        gameObject.SetActive(false);
    }

    // Helper Enumerators.
    IEnumerator TrioDisappears()
    {
        // Poof.
        SoundManager.instance.PlaySingle(smokePuff);
        for (int i = 0; i < trioParticleSystems.Count; i++) {
            trioParticleSystems[i].gameObject.SetActive(true);
            trioParticleSystems[i].Play();
        }
        yield return new WaitForSeconds(.5f);

        ex.SetActive(false);
        hash.SetActive(false);
        questio.SetActive(false);
        yield return new WaitForSeconds(1.5f);
    }

    public override void OnActivateRoom()
    {
        switch (GetFriendState())
        {
            case "IN_TOXIC_FIELD":
                stuart.GetComponent<FollowPlayer>().enabled = true;
                gameObject.GetComponent<ActivateDialogWhenClose>().canTalkTo = true;
                gameObject.GetComponent<ActivateDialogWhenClose>().distanceThreshold = 22;
                stuart.GetComponent<FollowPlayer>().enabled = false;
                break;
            case "STUART_PEP":
                stuart.GetComponent<FollowPlayer>().enabled = true;
                gameObject.GetComponent<ActivateDialogWhenClose>().canTalkTo = true;
                gameObject.GetComponent<ActivateDialogWhenClose>().distanceThreshold = 22;
                break;
            case "STUART_DEFEATED":
                stuart.GetComponent<FollowPlayer>().enabled = false;
                gameObject.GetComponent<ActivateDialogWhenClose>().canTalkTo = true;
                gameObject.GetComponent<ActivateDialogWhenClose>().distanceThreshold = 22;
                break;
            case "FIGHT_PHASE_1":
            case "LEFT_FIGHT_PHASE_1":
                ex.SetActive(false);
                hash.SetActive(false);
                questio.SetActive(false);

                SoundManager.instance.TransitionMusic(bossMusic);
                stuart.PrepPhase1();
                SetFriendState("FIGHT_PHASE_1");
                gameObject.SetActive(false);
                break;

            case "FIGHT_PHASE_2":
            case "LEFT_FIGHT_PHASE_2":
                ex.SetActive(false);
                hash.SetActive(false);
                questio.SetActive(false);

                SoundManager.instance.TransitionMusic(bossMusic);
                stuart.PrepPhase2();
                SetFriendState("FIGHT_PHASE_2");
				gameObject.SetActive(false);
                break;

        }
    }

    public override void OnActivateDialog(){
		switch (GetFriendState())
        {
			case "IN_FIELD":
				SoundManager.instance.backupMusicSource.clip = trioTheme;
				SoundManager.instance.backupMusicSource.volume = GlobalVariableManager.Instance.MASTER_MUSIC_VOL;
				SoundManager.instance.backupMusicSource.Play();
				SoundManager.instance.musicSource.Stop();
        	break;
           
        }
    }

    public override void OnDeactivateRoom()
    {
        switch (GetFriendState())
        {
            case "FIGHT_PHASE_1":
                stuart.ResetBossPositions();
                SoundManager.instance.TransitionMusic(windSFX);

                stuart.DeactivateHpDisplay();
                SetFriendState("LEFT_FIGHT_PHASE_1");
                break;
            case "FIGHT_PHASE_2":
                stuart.bossTrio.SetActive(false);
                stuart.bossEx.SetActive(false);
                stuart.bossHash.SetActive(false);
                stuart.bossQuestio.SetActive(false);
                stuart.ResetBossPositions();
                SoundManager.instance.TransitionMusic(windSFX);

                stuart.DeactivateHpDisplay();
                SetFriendState("LEFT_FIGHT_PHASE_2");
                break;
        }

        stuart.GetComponent<FollowPlayer>().enabled = false;
    }

    IEnumerator HashShieldShow(){
        // Hash runs at stuart.
        CamManager.Instance.mainCamEffects.CameraPan(hash, true);
        while (Vector2.Distance(hash.transform.position, stuart.transform.position) > 5f) {
            hash.transform.position = Vector2.MoveTowards(hash.transform.position, stuart.transform.position, 10 * Time.deltaTime);
            yield return null;
        }

        // Hash jumps on stuart;
        hash.GetComponent<tk2dSpriteAnimator>().Play("cast");
    	if(hash.transform.position.x < stuart.transform.position.x)
    		hash.GetComponent<Rigidbody2D>().AddForce(new Vector2(3,5f),ForceMode2D.Impulse); //hash jumps on Stuart
    	else
			hash.GetComponent<Rigidbody2D>().AddForce(new Vector2(-3,5f),ForceMode2D.Impulse); //hash jumps on Stuart

    	hash.GetComponent<Rigidbody2D>().gravityScale = 1;
    	yield return new WaitForSeconds(1f);
		hash.GetComponent<Rigidbody2D>().gravityScale = 0;
		SoundManager.instance.PlaySingle(shieldSound);
		stuart.transform.Find("shield").gameObject.SetActive(true);
		hash.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		yield return new WaitForSeconds(1f);
		GameObject healIcon = ObjectPool.Instance.GetPooledObject("effect_HealMarker",hash.transform.position);
		healIcon.transform.GetChild(0).GetComponent<tk2dTextMesh>().text = "10";
		healIcon.GetComponent<Rigidbody2D>().velocity = Vector2.up;
		yield return new WaitForSeconds(1f);

        //Questio Shows gloves
        CamManager.Instance.mainCamEffects.CameraPan(questio.gameObject,true);

		questio.GetComponent<tk2dSpriteAnimator>().Play("ShowGloves");
		questio.transform.Find("throwingGloves").gameObject.SetActive(true);

		yield return new WaitForSeconds(2f);
		questio.transform.Find("throwingGloves").gameObject.SetActive(false);

        stuart.PrepPhase2();
    }

	public override void GiveData(List<GameObject> neededObjs){
    	toxicBarrier = neededObjs[0];
    	toxicBarrierPS = neededObjs[1].GetComponent<ParticleSystem>();
    	toxicPipeFountain = neededObjs[2].GetComponent<ParticleSystem>();
    	goWithItDisplay = neededObjs[3];
    }


    // User Data implementation
    public override string UserDataKey()
    {
        return "BossFriendEx";
    }
	public void Upset()
    {
        DialogManager.Instance.currentlySpeakingIcon.SetAnimBool("IsUpset", true);
        DialogManager.Instance.ReturnFromAction();
    }
    public void NotUpset()
    {
        DialogManager.Instance.currentlySpeakingIcon.SetAnimBool("IsUpset", false);
        DialogManager.Instance.ReturnFromAction();
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