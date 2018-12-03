using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ev_ProjectileTowrdPlayer : MonoBehaviour {

	

	Vector2 playerPos;
	Vector2 movementDir;
	public ParticleSystem throwPS;
	[HideInInspector]
	public float speed = 5;
	[HideInInspector]
	public GameObject player; //given usually by whatever is spawning this.
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
		throwPS.Play();
		if(player == null){

		//player = GameObject.FindGameObjectWithTag("Player");
		}
		StartCoroutine("Delay");//Delay needed because this enable() was being ativated BEFORE the rock's new postion(back to thrower) was being set

	}
	// Update is called once per frame
	void Update () {
		
	}
	IEnumerator Delay(){
		yield return new WaitForSeconds(.1f);
		movementDir = (player.transform.position - gameObject.transform.position).normalized * 5;
		gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(movementDir.x,movementDir.y);
	}
}
