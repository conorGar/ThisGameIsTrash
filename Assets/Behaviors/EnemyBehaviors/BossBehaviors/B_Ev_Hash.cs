using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_Ev_Hash : MonoBehaviour {

	tk2dSpriteAnimator myAnim;
	public GameObject stuart;
	public GameObject stuartShield;
	public AudioClip castSound;
	public GameObject player;

	//bool isRunningAway;
	float landY;
	Rigidbody2D myBody;
	bool falling;
	bool onStuart;
	bool returnAfterThrow;
	//Protects Stuart Until Hash is hit
	//^Then Hash runs away



	void Start () {
		myAnim = gameObject.GetComponent<tk2dSpriteAnimator>();
		gameObject.transform.parent = stuart.transform;
		gameObject.transform.localPosition = new Vector2(0f,3f);//place hash on top of stuart
		gameObject.GetComponent<Renderer>().sortingLayerName = "Layer02";
		onStuart = true;
		myBody = gameObject.GetComponent<Rigidbody2D>();
		Shield();
	}

	void OnEnable(){
		if(returnAfterThrow){
			Revive();
		}
	}

	void Update () {
		/*float distance = Vector3.Distance(transform.position, player.transform.position);

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
		}*/
		if(onStuart){
			gameObject.transform.localPosition = new Vector2(0f,3f);//place hash on top of stuart
		}

		if(falling){
			if(gameObject.transform.position.y < landY){
				Dazed();
				myBody.gravityScale = 0f;
				myBody.velocity = new Vector2(0,0f);
				myBody.AddForce(new Vector2(4f*(Mathf.Sign(gameObject.transform.lossyScale.x)),0f),ForceMode2D.Impulse);//slide
				falling = false;
			}
		}

	}




	void Shield(){
	Debug.Log("HASH SHIELD ACTIVATED");
		//myAnim.Play("idle");
		//yield return new WaitForSeconds(Random.Range(3f,6f));
		if(!falling){ //if runningAway wasn't activated in the time between coroutine activate and wait till cast....
			myAnim.Play("cast");

			//yield return new WaitForSeconds(1f);
			stuartShield.SetActive(true);
			stuart.GetComponent<EnemyTakeDamage>().enabled = false;
			//stuart.GetComponent<FollowPlayer>().enabled = false;
		}
	}

	public void KnockOff(){
		Debug.Log("HASH KNOCKOFF ACTIVATE*****");
		stuartShield.SetActive(false);
		onStuart = false;
		gameObject.transform.parent = null;
		landY = gameObject.transform.position.y - 4;
		myBody.AddForce(new Vector2(4f*(Mathf.Sign(gameObject.transform.lossyScale.x)),0f),ForceMode2D.Impulse);//slide
		myBody.gravityScale = 1;
		gameObject.GetComponent<Renderer>().sortingLayerName = "Layer01";
		falling = true;
	}

	void Dazed(){
		gameObject.GetComponent<EnemyTakeDamage>().enabled = true;
		gameObject.layer = 15; //switch to thrownTrash layer.
		gameObject.GetComponent<ThrowableObject>().enabled = true;
		myAnim.Play("dazed");
		Invoke("Revive",10f);
	}

	void Revive(){
		if(this.enabled){
			gameObject.GetComponent<EnemyTakeDamage>().enabled = false;//cant attack while he is riding Stuart
			gameObject.transform.parent = stuart.transform;
			gameObject.transform.localPosition = new Vector2(0f,3f);//place hash on top of stuart
			onStuart = true;
			gameObject.layer = 9; //switch to enemy layer.
			gameObject.GetComponent<ThrowableObject>().enabled = true;
			Shield();
		}else{
			returnAfterThrow = true;
		}
	}


}
