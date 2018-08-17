using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ev_SpecialItem : MonoBehaviour {

	// Use this for initialization
	public tk2dCamera mainCamera;
	public GameObject player;
	Vector3 smoothVelocity = Vector3.zero;
	bool playerIsMovingToward;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(playerIsMovingToward){
			transform.position = Vector3.SmoothDamp(player.transform.position, gameobject.transform.position, ref smoothVelocity, 5f);

				if(player.transform.position.x < transform.position.x){
					transform.localScale = new Vector3(-1,1,1);
				} else{
					//if(!anim.IsPlaying("chaseL"))
						//anim.Play("chaseL");
					transform.localScale = new Vector3(1,1,1);
				}
		}
	}

	void OnTriggerEnter2D(Collider2D collider){
		if(collider.tag == "Player"){
			player.GetComponent<tk2dSpriteAnimator>().Play("anim_jimPickUp");
			gameObject.GetComponent<SpecialEffectsBehavior>().SmoothMovementToPoint(transform.position.x, player.transform.position.y,2f);


		}
	}

	void PlayerMoveToward(){
		player.GetComponent<EightWayMovement>().enabled = false;
		player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		mainCamera.GetComponent<Ev_MainCameraEffects>().CameraPan(gameObject.transform.position);
		player.GetComponent<tk2dSpriteAnimator>().Play("anim_jimWalkR");
			if(player.transform.position.x < transform.position.x){
					player.transform.localScale = new Vector3(1,1,1);
			} else{
				player.transform.localScale = new Vector3(-1,1,1);
			}
	}
}
