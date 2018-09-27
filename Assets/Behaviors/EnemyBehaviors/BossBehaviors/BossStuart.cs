using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class BossStuart : Boss
{
	public GameObject player;
	public MultipleDialogIconsManager mdim;
	public GameObject bossTrio;
	int activateEventOnce;
	public GameObject trio;
	public B_Ev_Hash hash;
	EnemyTakeDamage myETD;


	bool canDamage;
	// Use this for initialization
	void OnEnable ()
	{

		myETD = gameObject.GetComponent<EnemyTakeDamage>();
		if(GlobalVariableManager.Instance.BOSS_HP_LIST[bossNumber] <= 10 && activateEventOnce == 0){
			activateEventOnce = 1; // boss event already happened.
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(myETD.currentHp <= 6 && activateEventOnce == 0){
			BossEvent();
			activateEventOnce = 1;
		}

	}


	void OnTriggerEnter2D(Collider2D collider){
		if(collider.gameObject.layer == 15){ //throwable object hit
			//gameObject.GetComponent<EnemyTakeDamage>().meleeDmgBonus + 2;//thrown object causes 3 damage

			hash.KnockOff();
			canDamage = true;
			myETD.enabled = true;
		}

	}


	public override void BossEvent(){
		//activate boss 1 middle dialog
		gameObject.GetComponent<EnemyTakeDamage>().enabled = false;
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
		trio.SetActive(true); //activate dialog
		canDamage = false;
	}

	public override void BossDeathEvent(){
		mdim.SetStartingIcons(new string[]{"Stuart"});
		trio.SetActive(true);
	}
}

