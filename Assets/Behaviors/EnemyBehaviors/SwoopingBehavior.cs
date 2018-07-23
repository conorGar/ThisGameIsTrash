using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwoopingBehavior : MonoBehaviour {

	bool swoopin;
	float swoopingYSpeed = 15f;
	string swoopingDirection;
	public float distanceTilSwoop;

	GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if(swoopin){
			if(swoopingDirection == "left"){
				transform.Translate(new Vector2(-1f,swoopingYSpeed)*Time.deltaTime);
			}else if(swoopingDirection == "right"){
				transform.Translate(new Vector2(1f,swoopingYSpeed)*Time.deltaTime);
			}

			if(swoopingYSpeed > -15f){
				swoopingYSpeed -= .1f;
			}else{
				swoopin = false;
				swoopingYSpeed = 15f;
				gameObject.GetComponent<FollowPlayer>().enabled = true;

			}
		}else if((Mathf.Abs(transform.position.x - player.transform.position.x) < distanceTilSwoop) && Mathf.Abs(transform.position.y - player.transform.position.y) < distanceTilSwoop){
			Swoop();
		}
	}

	void Swoop(){
		swoopin = true;
		gameObject.GetComponent<FollowPlayer>().enabled = false;
		if(player.transform.position.x < gameObject.transform.position.x){
			swoopingDirection = "left";
		}else{
			swoopingDirection = "right";
		}
	}
}
