using UnityEngine;
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


	bool canDamage;

    // Use this for initialization
    protected void Start()
    {
        base.Start();

        canDamage = true;
        spawnPosition = gameObject.transform.position;
        exSpawnPosition = bossEx.transform.position;
        hashSpawnPosition = bossHash.transform.position;
        questioSpawnPosition = bossQuestio.transform.position;
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
        switch (ex.GetFriendState())
        {
            // In phase one, the trio will come back when Stuart's HP dips below 7.
            case "FIGHT_PHASE_1":
                if (myETD.currentHp <= 6)
                {
                    BossEvent();
                }
                break;
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
		if(collider.gameObject.layer == 15){ //throwable object hit
			//gameObject.GetComponent<EnemyTakeDamage>().meleeDmgBonus + 2;//thrown object causes 3 damage

			hash.KnockOff();
			canDamage = true;
			myETD.enabled = true;
		}

	}

    public void PrepPhase2()
    {
        SoundManager.instance.musicSource.Play();
        bossTrio.SetActive(true);
        bossEx.SetActive(true);
        bossHash.SetActive(true);
        bossQuestio.SetActive(true);
        GetComponent<EnemyTakeDamage>().enabled = false;
        canDamage = false;
        GetComponent<FollowPlayer>().enabled = true;
    }

	public override void BossEvent(){
		//activate boss 1 middle dialog
		player.GetComponent<BoxCollider2D>().enabled = false;
		player.GetComponent<EightWayMovement>().enabled = false;
		gameObject.GetComponent<FollowPlayer>().enabled = false;
		/*for(int i = 0; i<bossTrio.transform.childCount;i++){//add ex and questio to bosses. This is done by BossFriendEx at start if in middle of fight
			RoomManager.Instance.currentRoom.bosses.Add(bossTrio.transform.GetChild(i).gameObject);
		}*/
		gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		SoundManager.instance.musicSource.Pause();
		mdim.icons[0].GetComponent<MultipleIcon>().positionOnScreen = 0;//change ex icon position to be on left side
		mdim.SetStartingIcons(new string[]{"Stuart"});
		myETD.currentHp += 10; //regain lost hp
		ex.gameObject.SetActive(true); //activate dialog

        ex.GetComponent<ActivateDialogWhenClose>().canTalkTo = true;
        ex.GetComponent<ActivateDialogWhenClose>().xDistanceThreshold = 42;
        ex.GetComponent<ActivateDialogWhenClose>().yDistanceThreshold = 42;
        ex.SetFriendState("STUART_PEP");
	}

	public override void BossDeathEvent(){
		mdim.SetStartingIcons(new string[]{"Stuart"});

        ex.gameObject.SetActive(true);
        ex.GetComponent<ActivateDialogWhenClose>().canTalkTo = true;
        ex.GetComponent<ActivateDialogWhenClose>().xDistanceThreshold = 42;
        ex.GetComponent<ActivateDialogWhenClose>().yDistanceThreshold = 42;
        ex.SetFriendState("STUART_DEFEATED");
        bossTrio.SetActive(false);
    }
}

