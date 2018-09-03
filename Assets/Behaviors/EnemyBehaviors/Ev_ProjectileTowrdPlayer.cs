using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ev_ProjectileTowrdPlayer : MonoBehaviour {

	GameObject player;
	Vector2 playerPos;
	Vector2 movementDir;
	public ParticleSystem throwPS;
	[HideInInspector]
	public float speed = 5;
	// Use this for initialization
	void Start () {
		throwPS.Play();
		if(player == null){
			player = GameObject.FindGameObjectWithTag("Player");
		}
		movementDir = (player.transform.position - gameObject.transform.position).normalized * 5;
		gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(movementDir.x,movementDir.y);
	}


	void OnEnable(){
		
	}
	// Update is called once per frame
	void Update () {
		
	}
}
