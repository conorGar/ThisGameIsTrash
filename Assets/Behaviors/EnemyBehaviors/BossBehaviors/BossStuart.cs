﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class BossStuart : Boss
{
	public GameObject player;
    public Vector3 spawnPosition;
    public Vector3 exSpawnPosition, hashSpawnPosition, questioSpawnPosition;

    public MultipleDialogIconsManager mdim;

    public BossFriendEx ex;
    public GameObject bossTrio;
    public GameObject bossEx;
    public GameObject bossHash;
    public GameObject bossQuestio;
    public B_Ev_Hash hash;
	EnemyTakeDamage myETD;

	[HideInInspector]
	public bool canDamage;

    // Use this for initialization
    protected void Start()
    {
        base.Start();

        canDamage = true;
        spawnPosition = gameObject.transform.position;
        exSpawnPosition = bossEx.transform.position;
        hashSpawnPosition = bossHash.transform.position;
        questioSpawnPosition = bossQuestio.transform.position;

        gameObject.SetActive(false);
    }
    void OnEnable ()
	{
		myETD = gameObject.GetComponent<EnemyTakeDamage>();
        ex = FriendManager.Instance.GetFriend("Ex") as BossFriendEx;
        ex.stuart = this;
    }
	
	// Update is called once per frame
	void Update ()
	{
        if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
            switch (ex.GetFriendState()) {
                // In phase one, the trio will come back when Stuart's HP dips below 7.
                case "FIGHT_PHASE_1":
                    if (myETD.currentHp <= 6) {
                        BossEvent();
                    }
                    break;
            }
        }
	}

    public override void ActivateBoss()
    {
        ex = FriendManager.Instance.GetFriend("Ex") as BossFriendEx;
        if (ex != null && ex.GetFriendState() != "END")
        {
            base.ActivateBoss();
        }
    }

    public void ResetBossPositions()
    {
        gameObject.transform.position = spawnPosition;
        bossEx.transform.position = exSpawnPosition;
        bossHash.transform.position = hashSpawnPosition;
        bossQuestio.transform.position = questioSpawnPosition;
    }

	void OnTriggerEnter2D(Collider2D collider){
		if(collider.gameObject.layer == 15 && !canDamage){ //throwable object hit

			collider.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;


			hash.KnockOff();
			GetComponent<InvincibleEnemy>().enabled = false;
			canDamage = true;
			collider.gameObject.layer = 11; //switched to item obj once hit so doesnt hit anything else
			collider.GetComponent<B_Ev_Questio>().StartCoroutine("UndazeCheck");
			myETD.enabled = true;

		}

	}



    public void PrepPhase1()
    {
        Debug.Log("Prep Phase 1");

        SoundManager.instance.musicSource.Play();
        bossTrio.SetActive(false);
        bossEx.SetActive(false);
        bossHash.SetActive(false);
        bossQuestio.SetActive(false);
        GetComponent<InvincibleEnemy>().enabled = false;
        GetComponent<EnemyTakeDamage>().enabled = true;
        canDamage = true;
        GetComponent<FollowPlayer>().enabled = true;
        ActivateHpDisplay();
    }

    public void PrepPhase2()
    {
		Debug.Log("Prep Phase 2");

        SoundManager.instance.musicSource.Play();
        bossTrio.SetActive(true);
       	bossEx.SetActive(true);
       	bossHash.SetActive(true);
        bossQuestio.SetActive(true);
        GetComponent<InvincibleEnemy>().enabled = true;
        GetComponent<EnemyTakeDamage>().enabled = false;
        canDamage = false;
        GetComponent<FollowPlayer>().enabled = true;
        ActivateHpDisplay();
    }

	public override void BossEvent(){
        //activate boss 1 middle dialog
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        /*for(int i = 0; i<bossTrio.transform.childCount;i++){//add ex and questio to bosses. This is done by BossFriendEx at start if in middle of fight
			RoomManager.Instance.currentRoom.bosses.Add(bossTrio.transform.GetChild(i).gameObject);
		}*/
        Debug.Log("Boss Event Activate");
		SoundManager.instance.musicSource.Pause();
		mdim.icons[0].GetComponent<MultipleIcon>().positionOnScreen = 0;//change ex icon position to be on left side
        mdim.icons[1].GetComponent<MultipleIcon>().positionOnScreen = 1;//change questio icon position to be center

        myETD.currentHp += 10; //regain lost hp
		ex.gameObject.SetActive(true); //activate dialog

        ex.GetComponent<ActivateDialogWhenClose>().canTalkTo = true;
        ex.GetComponent<ActivateDialogWhenClose>().distanceThreshold = 42;
        //ex.GetComponent<ActivateDialogWhenClose>().yDistanceThreshold = 42;
        
        ex.SetFriendState("STUART_PEP");
        DeactivateHpDisplay();
	}

	public override void BossDeathEvent(){
        ex.gameObject.SetActive(true);
        ex.GetComponent<ActivateDialogWhenClose>().canTalkTo = true;
        ex.GetComponent<ActivateDialogWhenClose>().distanceThreshold = 42;
        ex.SetFriendState("STUART_DEFEATED");
        bossTrio.SetActive(false);
        bossHash.SetActive(false);
        bossEx.GetComponent<B_Ev_Ex>().KillSlimes();
        bossEx.SetActive(false);
        bossQuestio.SetActive(false);
        DeactivateHpDisplay();
        SoundManager.instance.musicSource.Stop();
    }

	public override void BossDeactivateEvent(){
		Debug.Log("Stuart Boss Deactivate Event activated");

		//return bosses that are being carried properly when player leaves a room while carrying them
		if(bossQuestio.GetComponent<ThrowableObject>().enabled && bossQuestio.GetComponent<ThrowableObject>().onGround == false){
			bossQuestio.GetComponent<ThrowableObject>().Drop();
		}
		if(bossHash.GetComponent<ThrowableObject>().enabled && bossHash.GetComponent<ThrowableObject>().onGround == false){
			bossHash.GetComponent<ThrowableObject>().Drop();
		}
		if(bossEx.GetComponent<ThrowableObject>().enabled&& bossEx.GetComponent<ThrowableObject>().onGround == false){
			bossEx.GetComponent<ThrowableObject>().Drop();
		}
		bossEx.GetComponent<B_Ev_Ex>().KillSlimes();

	}
}

