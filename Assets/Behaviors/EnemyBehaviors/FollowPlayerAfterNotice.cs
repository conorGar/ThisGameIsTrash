using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerAfterNotice : MonoBehaviour {

	public float noticeThreshold;
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
				this.gameObject.GetComponent<RandomDirectionMovement>().enabled = false;
				this.gameObject.GetComponent<FollowPlayer>().enabled = true;
				this.enabled = false;
			}
		}
	}
}
