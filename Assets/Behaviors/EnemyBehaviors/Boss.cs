﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour {

	public int bossNumber;
	public int hp;
	public int attkDmg;
	public bool displayHealth;
	public GameObject hpDisplay;
	public GameObject objectPool;
	public bool vanishAtDeath;
	public MonoBehaviour myBossScript;
	public tk2dCamera mainCam;
	public GameObject objectToPanTo;

	[HideInInspector]
	public Room currentRoom; //used to diable other bosses at main bosses' death
	int deathSmokeNumber;


	void Start () {


		if(GlobalVariableManager.Instance.BOSS_HP_LIST[bossNumber] > hp){
			GlobalVariableManager.Instance.BOSS_HP_LIST[bossNumber] = hp;
		}else if(GlobalVariableManager.Instance.BOSS_HP_LIST[bossNumber] < hp && GlobalVariableManager.Instance.BOSS_HP_LIST[bossNumber] != 0 ){
			GlobalVariableManager.Instance.BOSS_HP_LIST[bossNumber]++; //regain hp each day
		}

		//gameObject.SetActive(false);
		ActivateBoss();

	}

	public void ActivateBoss(){
		gameObject.GetComponent<EnemyTakeDamage>().currentHp = GlobalVariableManager.Instance.BOSS_HP_LIST[bossNumber];
		gameObject.GetComponent<EnemyTakeDamage>().bossEnemy = true;
		if(displayHealth){
			hpDisplay.SetActive(true);
			hpDisplay.GetComponent<GUI_BossHpDisplay>().maxHp = hp;
			hpDisplay.GetComponent<GUI_BossHpDisplay>().UpdateBossHp(GlobalVariableManager.Instance.BOSS_HP_LIST[bossNumber]);
		}
	}

	public IEnumerator BossDeath(){
		Debug.Log("BOSS DEATH ACTIVATE ***********");
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		player.GetComponent<EightWayMovement>().enabled = false;
		player.GetComponent<PlayerTakeDamage>().enabled = false;
		InvokeRepeating("DeathSmoke",.1f,.2f);
		if(gameObject.GetComponent<FollowPlayer>() != null){
				gameObject.GetComponent<FollowPlayer>().StopSound();
				gameObject.GetComponent<FollowPlayer>().enabled = false;
		}
	
		GlobalVariableManager.Instance.BOSS_HP_LIST[bossNumber] = 0;
		yield return new WaitForSeconds(1.5f);
		GameObject deathGhost = objectPool.GetComponent<ObjectPool>().GetPooledObject("effect_DeathGhost");
		deathGhost.transform.position = new Vector3((transform.position.x), transform.position.y, transform.position.z);
		if(vanishAtDeath){
			mainCam.GetComponent<Ev_MainCameraEffects>().CameraPan(objectToPanTo.transform.position,"BossItem");
			mainCam.GetComponent<Ev_MainCameraEffects>().objectToSpawn = objectToPanTo;
			GlobalVariableManager.Instance.BOSSES_KILLED |= GlobalVariableManager.BOSSES.ONE; //use this as way to tell if player has upgrade
			for(int i = 0; i < currentRoom.bosses.Count; i++){//disable all other bosses at death
				currentRoom.bosses[i].SetActive(false);
			}
			//this.gameObject.SetActive(false);
		}else{
			gameObject.GetComponent<tk2dSpriteAnimator>().Play("Death");
			myBossScript.enabled = false;
		}
	}

	public virtual void BossEvent(){

		//nothing for basic boss

	}


	void DeathSmoke(){
		if(deathSmokeNumber < 15){
			GameObject deathSmoke = objectPool.GetComponent<ObjectPool>().GetPooledObject("effect_SmokePuff");
			deathSmoke.transform.position = new Vector3((transform.position.x+ Random.Range(-4,4)), transform.position.y+ Random.Range(-4,4), transform.position.z);
			deathSmokeNumber++;
		}else{
			CancelInvoke();
		}
	}

}
