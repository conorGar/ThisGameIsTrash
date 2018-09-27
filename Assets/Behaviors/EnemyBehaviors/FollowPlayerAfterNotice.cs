using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerAfterNotice : MonoBehaviour {

	public float noticeThreshold;
	public bool sleepingEnemy;
	public GameObject sleepingPS;
	GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		this.gameObject.GetComponent<FollowPlayer>().enabled = false;

	}
	
	// Update is called once per frame
	void Update () {
		if((Mathf.Abs(transform.position.x - player.transform.position.x) < noticeThreshold) && Mathf.Abs(transform.position.y - player.transform.position.y) < noticeThreshold){
			if((player.transform.position.x < gameObject.transform.position.x && gameObject.transform.localScale.x < 0) || (player.transform.position.x > gameObject.transform.position.x && gameObject.transform.localScale.x > 0)){//make sure is facing the direction of the player..
				if(sleepingEnemy){
					gameObject.GetComponent<Animator>().enabled = false;
					gameObject.transform.localScale = Vector3.one;//set to proper scale from sleeping
					sleepingPS.SetActive(false);
				}
				if(this.gameObject.GetComponent<RandomDirectionMovement>() != null)
					this.gameObject.GetComponent<RandomDirectionMovement>().enabled = false;
				this.gameObject.GetComponent<FollowPlayer>().enabled = true;
				this.enabled = false;
			}
		}
	}
}
