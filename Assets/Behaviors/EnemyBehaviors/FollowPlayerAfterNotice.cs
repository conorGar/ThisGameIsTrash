﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GenericEnemyStateController = EnemyStateController<EnemyState, EnemyTrigger>;

[RequireComponent(typeof(GenericEnemyStateController))]
public class FollowPlayerAfterNotice : MonoBehaviour {

	public float noticeThreshold;
	public float noticeThresholdY;
	public bool sleepingEnemy;
	public GameObject sleepingPS;

    protected GenericEnemyStateController controller;

    void Awake()
    {
        controller = GetComponent<GenericEnemyStateController>();
    }

    void Start () {
		if(noticeThresholdY == 0){
			noticeThresholdY = noticeThreshold; //so doesnt interfere with the given data to all the enemies i coded before adding this for the spear moles
		}
	}

	protected void OnEnable(){
		if(sleepingEnemy){
			//gameObject.GetComponent<tk2dSpriteAnimator>().Play("sleep");
			sleepingPS.SetActive(true);
		}
	}

    public void Update()
    {
        // An idling enemy, when in range of a player or target, will go into notice and then chase!
        if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
            if (controller.currentState.GetState() == EnemyState.IDLE) {
                if ((Mathf.Abs(transform.position.x - PlayerManager.Instance.player.transform.position.x) < noticeThreshold) && Mathf.Abs(transform.position.y - PlayerManager.Instance.player.transform.position.y) < noticeThresholdY) {
                    if ((PlayerManager.Instance.player.transform.position.x < gameObject.transform.position.x && gameObject.transform.localScale.x < 0) || (PlayerManager.Instance.player.transform.position.x > gameObject.transform.position.x && gameObject.transform.localScale.x > 0)) {//make sure is facing the direction of the player..
                        if (!GlobalVariableManager.Instance.IS_HIDDEN) {
                            if (sleepingEnemy) {
                                gameObject.GetComponent<Animator>().enabled = false;
                                gameObject.transform.localScale = Vector3.one;//set to proper scale from sleeping
                                sleepingPS.SetActive(false);
                            }

                            SoundManager.instance.PlaySingle(SFXBANK.NOTICE);
                            GameObject notice = ObjectPool.Instance.GetPooledObject("effect_notice", gameObject.transform.position + new Vector3(-1f, gameObject.GetComponent<BoxCollider2D>().size.y/2 + 1f, 0f));
                            notice.transform.parent = this.transform;
                            NoticePlayerEvent();

                            controller.SendTrigger(EnemyTrigger.NOTICE);
                        }
                    }
                }
            }
        }
    }

    public bool IsChasing()
    {
        return controller.IsFlag((int)EnemyFlag.CHASING);
    }

	protected virtual void NoticePlayerEvent(){
        // Nothing for base.
	}

	public virtual void LostSightOfPlayer(){
        // Nothing for base.
	}
}
