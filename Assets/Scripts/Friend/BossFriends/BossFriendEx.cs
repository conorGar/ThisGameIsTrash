using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(ActivateDialogWhenClose))]
public class BossFriendEx : Friend
{

	//*** For now using for all 3 of the trio, change if needed at any point
	
	public ParticleSystem myTeleportPS;
	public BossStuart stuart;
	public GameObject player;
    public int FieldRoomNum;
    public int ToxicFieldRoomNum;
	//public GameObject stuartIcon;
	public AudioClip smokePuff;
	public AudioClip bossMusic;
    public AudioClip windSFX;
	int enterIconPhase;
	
	// Update is called once per frame
	void Update ()
	{
        OnUpdate();
	}

    private void OnEnable()
    {
        base.OnEnable();
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
                GetComponent<ActivateDialogWhenClose>().Execute("Ex", "Questio", "Hash");
                break;
            case "IN_TOXIC_FIELD":
                nextDialog = "Boss1Start";
                GetComponent<ActivateDialogWhenClose>().Execute();
                break;
            case "STUART_PEP":
                nextDialog = "Boss1Middle";
                GetComponent<ActivateDialogWhenClose>().Execute();
                break;
            case "STUART_DEFEATED":
                nextDialog = "Boss1Death";
                GetComponent<ActivateDialogWhenClose>().Execute();
                break;
            case "PREP_FIGHT_PHASE_1":
                player.GetComponent<PlayerTakeDamage>().enabled = true;
                stuart.GetComponent<FollowPlayer>().enabled = true;

                SoundManager.instance.TransitionMusic(bossMusic);

                gameObject.SetActive(false);
                SetFriendState("FIGHT_PHASE_1");
                break;
            case "PREP_FIGHT_PHASE_2":
                stuart.PrepPhase2();
                player.GetComponent<BoxCollider2D>().enabled = true;
                gameObject.SetActive(false);
                SetFriendState("FIGHT_PHASE_2");
                break;
        }
    }

    public override IEnumerator OnFinishDialogEnumerator()
    {
        yield return new WaitForSeconds(.3f);

        //gameObject.GetComponent<MeshRenderer>().enabled =false;//hide sprite
        myTeleportPS.gameObject.SetActive(true);
        myTeleportPS.Play();

        switch (GetFriendState())
        {
            case "IN_TOXIC_FIELD":
                yield return new WaitForSeconds(.1f);
                gameObject.GetComponent<MeshRenderer>().enabled = true;
                GlobalVariableManager.Instance.PLAYER_CAN_MOVE = true;
                CamManager.Instance.mainCamEffects.ReturnFromCamEffect();
                gameObject.GetComponent<ActivateDialogWhenClose>().canTalkTo = true;
                gameObject.SetActive(false);
                break;
            case "PREP_FIGHT_PHASE_1":
                gameObject.GetComponent<MeshRenderer>().enabled = false;
                SoundManager.instance.PlaySingle(smokePuff);

                yield return new WaitForSeconds(1f);
                gameObject.GetComponent<MeshRenderer>().enabled = true;
                GlobalVariableManager.Instance.PLAYER_CAN_MOVE = true;
                CamManager.Instance.mainCamEffects.ReturnFromCamEffect();
                gameObject.GetComponent<ActivateDialogWhenClose>().canTalkTo = true;
                break;
            case "PREP_FIGHT_PHASE_2":
                yield return new WaitForSeconds(1f);
                gameObject.GetComponent<MeshRenderer>().enabled = true;
                GlobalVariableManager.Instance.PLAYER_CAN_MOVE = true;
                CamManager.Instance.mainCamEffects.ReturnFromCamEffect();
                gameObject.GetComponent<ActivateDialogWhenClose>().canTalkTo = true;
                break;
            case "END":
                yield return new WaitForSeconds(1f);
                gameObject.GetComponent<MeshRenderer>().enabled = false;
                GlobalVariableManager.Instance.PLAYER_CAN_MOVE = true;
                CamManager.Instance.mainCamEffects.ReturnFromCamEffect();
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
                gameObject.GetComponent<ActivateDialogWhenClose>().canTalkTo = true;
                gameObject.GetComponent<ActivateDialogWhenClose>().yDistanceThreshold = 15;
                gameObject.GetComponent<ActivateDialogWhenClose>().yDistanceThreshold = 22;
                break;
            case "STUART_PEP":
                gameObject.GetComponent<ActivateDialogWhenClose>().canTalkTo = true;
                gameObject.GetComponent<ActivateDialogWhenClose>().yDistanceThreshold = 15;
                gameObject.GetComponent<ActivateDialogWhenClose>().yDistanceThreshold = 22;
                break;
            case "STUART_DEFEATED":
                gameObject.GetComponent<ActivateDialogWhenClose>().canTalkTo = true;
                gameObject.GetComponent<ActivateDialogWhenClose>().yDistanceThreshold = 15;
                gameObject.GetComponent<ActivateDialogWhenClose>().yDistanceThreshold = 22;
                break;
            case "FIGHT_PHASE_1":
            case "LEFT_FIGHT_PHASE_1":
                SetFriendState("PREP_FIGHT_PHASE_1");
                break;
            case "FIGHT_PHASE_2":
            case "LEFT_FIGHT_PHASE_2":
                SoundManager.instance.TransitionMusic(bossMusic);
                SetFriendState("PREP_FIGHT_PHASE_2");
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

                SoundManager.instance.TransitionMusic(windSFX);

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

                SoundManager.instance.TransitionMusic(windSFX);

                SetFriendState("LEFT_FIGHT_PHASE_2");
                break;
        }
    }

    // Actions from the dialog script!
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
}

