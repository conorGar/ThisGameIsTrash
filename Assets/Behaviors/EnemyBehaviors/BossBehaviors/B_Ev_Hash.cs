using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_Ev_Hash : MonoBehaviour {

	tk2dSpriteAnimator myAnim;
	public GameObject stuart;
	public GameObject stuartShield;
	public AudioClip castSound;
	public GameObject player;

	bool isRunningAway;

	//Protects Stuart Until Hash is hit
	//^Then Hash runs away



	void Start () {
		myAnim = gameObject.GetComponent<tk2dSpriteAnimator>();
	}
	
	void Update () {
		float distance = Vector3.Distance(transform.position, player.transform.position);

		if(stuartShield.activeInHierarchy == true){//if hit while shielding Stuart, stops shield and starts runaway
			if(myAnim.CurrentClip.name == "hurt"){
				stuartShield.SetActive(false);
				isRunningAway = true;
			}
		}else if(!isRunningAway && distance <10){ //if not shielding stuart, will start running away if the player gets close
			isRunningAway = true;
		}
		if(isRunningAway){
			transform.position = Vector2.MoveTowards(transform.position, player.transform.position, -1*8*Time.deltaTime);
			if(myAnim.CurrentClip.name != "walk"){
				myAnim.Play("walk");
			}
			if(distance > 10){
				isRunningAway = false;
				StartCoroutine("Shield");
			}
		}
	}




	IEnumerator Shield(){
	Debug.Log("HASH SHIELD ACTIVATED");
		myAnim.Play("idle");
		yield return new WaitForSeconds(Random.Range(3f,6f));
		if(!isRunningAway){ //if runningAway wasn't activated in the time between coroutine activate and wait till cast....
			myAnim.Play("cast");

			yield return new WaitForSeconds(1f);
			stuartShield.SetActive(true);
			stuart.GetComponent<EnemyTakeDamage>().enabled = false;
			stuart.GetComponent<FollowPlayer>().enabled = false;
		}
	}


}
