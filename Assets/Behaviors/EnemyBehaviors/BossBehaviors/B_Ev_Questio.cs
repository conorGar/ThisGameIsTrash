using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_Ev_Questio : MonoBehaviour {

	public GameObject mySlashR;
	public GameObject mySlashL;
	public GameObject player;

	tk2dSpriteAnimator myAnim;
	FollowPlayer fp;
	int facingDirection = 0; //0 = left, 1 = right
	int swingOnce;

	void Start () {
		fp = gameObject.GetComponent<FollowPlayer>();
		myAnim = gameObject.GetComponent<tk2dSpriteAnimator>();
	}
	
	void Update () {

		if(player.transform.position.x < gameObject.transform.position.x && facingDirection != 0){
			facingDirection = 0;
		}else if(player.transform.position.x > gameObject.transform.position.x && facingDirection != 1){
			facingDirection = 1;
		}

		if(fp.enabled == true){
			if(facingDirection == 0 && myAnim.CurrentClip.name != "walkL"){
				myAnim.Play("walkL");
			}else if(facingDirection == 1 && myAnim.CurrentClip.name != "walkR"){
				myAnim.Play("walkR");
			}
			float distance = Vector3.Distance(transform.position, player.transform.position);
			if(distance < 5 && swingOnce == 0){
				Debug.Log("QUESTIO SWING ACTIVATE");
				StartCoroutine("Swing");
				swingOnce = 1;
			}
		}
	}

	IEnumerator Swing(){
		fp.enabled = false;
		if(facingDirection == 0){
			myAnim.Play("swingL");
		}else{
			myAnim.Play("swingR");
		}
		yield return new WaitForSeconds(.3f);
		gameObject.GetComponent<Rigidbody2D>().velocity = (player.transform.position -gameObject.transform.position).normalized *15;
		yield return new WaitForSeconds(.5f);
		if(facingDirection == 0){
			mySlashL.SetActive(true);
		}else{
			mySlashL.SetActive(true);
		}
		gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,0f);
		yield return new WaitForSeconds(.5f);
		//check for directionFacing
		mySlashL.SetActive(false);
		mySlashR.SetActive(false);

		swingOnce = 0;
		myAnim.Play("idleL");
		fp.enabled = true;
	}
}
