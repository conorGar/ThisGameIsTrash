﻿using UnityEngine;
using System.Collections;

public class BossFriendEx : Friend
{

	//*** For now using for all 3 of the trio, change if needed at any point
	
	public tk2dCamera mainCam;
	public ParticleSystem myTeleportPS;
	public GameObject stuart;
	public GameObject player;
	public GameObject bossTrio;
	//public GameObject stuartIcon;
	public AudioClip smokePuff;
	public AudioClip bossMusic;
	int phase;
	int enterIconPhase;


	// Use this for initialization
	void OnStart ()
	{
		//dtermine set up at start of day depending on where you are in the boss 1 fight
		if(nextDialog == "Boss1Start"){
			gameObject.transform.parent.position = new Vector2(-29f,149f);
			gameObject.GetComponent<ActivateDialogWhenClose>().yDistanceThreshold = 22;
			phase = 1;
		}else if(nextDialog == "Boss1Death" ||nextDialog == "Boss1Middle"  ){
			gameObject.transform.parent.position = new Vector2(-29f,149f);
			transform.parent.gameObject.SetActive(false);
		}else if(nextDialog == "end"){
			Destroy(gameObject);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}


	public override void FinishDialogEvent(){
		Debug.Log("Ec finish dialog event activate");
		StartCoroutine("ReturnCam");
	}

	IEnumerator ReturnCam(){
		yield return new WaitForSeconds(.3f);

		//gameObject.GetComponent<MeshRenderer>().enabled =false;//hide sprite
		myTeleportPS.gameObject.SetActive(true);
		myTeleportPS.Play();
		if(phase == 0){//after intro
			yield return new WaitForSeconds(.1f);
			gameObject.transform.parent.position = new Vector2(-29f,149f);
			gameObject.GetComponent<ActivateDialogWhenClose>().yDistanceThreshold = 22;
		}else if(phase ==1){
			gameObject.GetComponent<MeshRenderer>().enabled = false;
			SoundManager.instance.PlaySingle(smokePuff);
			player.GetComponent<PlayerTakeDamage>().enabled = true;
		}
		yield return new WaitForSeconds(1f);
		gameObject.GetComponent<MeshRenderer>().enabled =true;
		GlobalVariableManager.Instance.PLAYER_CAN_MOVE = true;
		mainCam.GetComponent<Ev_MainCameraEffects>().ReturnFromCamEffect();
		gameObject.GetComponent<ActivateDialogWhenClose>().canTalkTo = true;
		if(phase == 1){//boss start
			stuart.GetComponent<FollowPlayer>().enabled = true;
			SoundManager.instance.musicSource.PlayOneShot(bossMusic);
			transform.parent.gameObject.SetActive(false);
			gameObject.GetComponent<ActivateDialogWhenClose>().xDistanceThreshold = 42;

		}else if(phase ==2){
			SoundManager.instance.musicSource.Play();
			bossTrio.SetActive(true);
			stuart.GetComponent<FollowPlayer>().enabled = true;
			player.GetComponent<BoxCollider2D>().enabled = true;
			transform.parent.gameObject.SetActive(false);
		}
		phase++;
		//gameObject.SetActive(false);

	}

}
