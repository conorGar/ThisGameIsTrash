﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerAfterNotice : MonoBehaviour {

	public float noticeThreshold;
	public float noticeThresholdY;
	public bool sleepingEnemy;
	public GameObject sleepingPS;
	public AudioClip noticeSfx;
	GameObject player;

	bool noticedPlayer = false;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		this.gameObject.GetComponent<FollowPlayer>().enabled = false;
		if(noticeThresholdY == 0){
			noticeThresholdY = noticeThreshold; //so doesnt interfere with the given data to all the enemies i coded before adding this for the spear moles
		}
	}

	void OnEnable(){
		noticedPlayer = false;
		if(sleepingEnemy){
			gameObject.GetComponent<tk2dSpriteAnimator>().Play("sleep");
			sleepingPS.SetActive(true);
		}
		if(gameObject.GetComponent<FollowPlayer>().enabled){
			gameObject.GetComponent<FollowPlayer>().enabled = false;
		}
	}
	
	// Update is called once per frame
	public void Update () {
		if(!noticedPlayer){
			if((Mathf.Abs(transform.position.x - player.transform.position.x) < noticeThreshold) && Mathf.Abs(transform.position.y - player.transform.position.y) < noticeThresholdY){
				if((player.transform.position.x < gameObject.transform.position.x && gameObject.transform.localScale.x < 0) || (player.transform.position.x > gameObject.transform.position.x && gameObject.transform.localScale.x > 0)){//make sure is facing the direction of the player..
					if(!GlobalVariableManager.Instance.IS_HIDDEN){
						if(sleepingEnemy){
							gameObject.GetComponent<Animator>().enabled = false;
							gameObject.transform.localScale = Vector3.one;//set to proper scale from sleeping
							sleepingPS.SetActive(false);
						}
						if(this.gameObject.GetComponent<RandomDirectionMovement>() != null){
							this.gameObject.GetComponent<RandomDirectionMovement>().enabled = false;
							this.gameObject.GetComponent<RandomDirectionMovement>().StopAllCoroutines();
						}
						this.gameObject.GetComponent<FollowPlayer>().enabled = true;
						SoundManager.instance.PlaySingle(noticeSfx);
						GameObject notice = ObjectPool.Instance.GetPooledObject("effect_notice",gameObject.transform.position);
						notice.transform.parent = this.transform;
						NoticePlayerEvent();
					}
				}
			}
		}
	}

	protected virtual void NoticePlayerEvent(){
		noticedPlayer = true;
	}
}
