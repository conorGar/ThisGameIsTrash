using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ev_Projectile_Boulder : MonoBehaviour {

	//Actually attatched to the shadow(parent) of the boulder, so that the boulder can do its arch movement and such

	public GameObject myBoulder;
	public ParticleSystem landingPS;
	GameObject player;
	Vector2 targetPos;
	bool falling;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		myBoulder.GetComponent<CircleCollider2D>().enabled = false;//boulder cant damage player while in the sky
		myBoulder.transform.localPosition = new Vector2(0f,4f);
		myBoulder.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		myBoulder.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f,8f),ForceMode2D.Impulse);
	}
	
	// Update is called once per frame

	void OnEnable(){
		falling = false;
		myBoulder.GetComponent<CircleCollider2D>().enabled = false;//boulder cant damage player while in the sky
		player = GameObject.FindGameObjectWithTag("Player");
		myBoulder.transform.localPosition = new Vector2(0f,4f);
		myBoulder.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		myBoulder.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f,8f),ForceMode2D.Impulse);
		Debug.Log("added force!");

		targetPos = player.transform.position;
	}
	void Update () {
		if((Vector2)gameObject.transform.position != targetPos && !falling){
		gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position,targetPos,(5*Time.deltaTime));//TODO: figure out time it takes for gravity to bring rock down, and have it always move there in thaat time

		myBoulder.transform.position = new Vector2(this.gameObject.transform.position.x,myBoulder.transform.position.y);
		if(myBoulder.GetComponent<Rigidbody2D>().velocity.y < 0){
				myBoulder.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
				myBoulder.GetComponent<Rigidbody2D>().gravityScale = 0;
		}


		}else{
			myBoulder.GetComponent<Rigidbody2D>().gravityScale = 2;
			myBoulder.GetComponent<CircleCollider2D>().enabled = true;
			falling = true;
			//check if boulder i >=shadow y and if so trigger explode and fragment
			if(myBoulder.transform.position.y < gameObject.transform.position.y){
				ObjectPool.Instance.GetPooledObject("effect_largeLand",gameObject.transform.position);
				for(int i = 0; i<4;i++){
					GameObject debris = ObjectPool.Instance.GetPooledObject("projectile_largeRock",gameObject.transform.position);
					if(i == 0){
						debris.GetComponent<Rigidbody2D>().velocity = new Vector2(-5,5);
					}else if(i == 1){
						debris.GetComponent<Rigidbody2D>().velocity = new Vector2(5,5);
					}else if(i == 2){
						debris.GetComponent<Rigidbody2D>().velocity = new Vector2(5,-5);
					}else if(i == 3){
						debris.GetComponent<Rigidbody2D>().velocity = new Vector2(-5,-5);
					}

				}
				myBoulder.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
				gameObject.SetActive(false);

			}
		}

		if(GlobalVariableManager.Instance.TUT_POPUP_ISSHOWING){
			gameObject.SetActive(false);
		}

	}


}
