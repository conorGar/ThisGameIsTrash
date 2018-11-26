using UnityEngine;
using System.Collections;

public class Ev_Enemy_Porcupine : FollowPlayer
{


	//turns around to spikey backside and leaps toward player.

	public GameObject myShadow;
	public BoxCollider2D mySpikeHitBox;
	public BoxCollider2D myHitBox;
	public AudioClip land;


	bool leaping;
	bool inAir;
	Vector2 landingPos;
	bool falling;


	
	// Update is called once per frame
	void Update ()
	{
		if(!leaping){
				base.Update();
			if(Vector2.Distance(gameObject.transform.position,player.transform.position) < 7){
				leaping = true;
				Leap();
			}
		}else if(inAir){
			myShadow.transform.position = Vector2.MoveTowards(myShadow.transform.position,landingPos,4*Time.deltaTime);
			Debug.Log("landing Position: " + landingPos + myShadow.transform.position);

			if(!falling)
				gameObject.transform.position = Vector2.Lerp(gameObject.transform.position,new Vector2(myShadow.transform.position.x,myShadow.transform.position.y +3f),4*Time.deltaTime);

		}



		if(falling){
			if(gameObject.transform.position.y < landingPos.y){
				Debug.Log("Landed from fall" + gameObject.transform.position.y + landingPos.y);
				falling = false;
				inAir = false;
				SoundManager.instance.PlaySingle(land);
				ObjectPool.Instance.GetPooledObject("effect_enemyLand",gameObject.transform.position);
				gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
				gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
				myShadow.transform.parent = gameObject.transform;
				StartCoroutine("LandDelay");
			}
		}

	}

	void Leap(){
		gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x*-1,gameObject.transform.localScale.y); // turn around

		//TODO: switch to leap animation
		StartCoroutine("LeapSequence");

	}


	IEnumerator LeapSequence(){
		yield return new WaitForSeconds(.5f);
		gameObject.GetComponent<EnemyTakeDamage>().moveWhenHit = false;
		myShadow.transform.parent = null;
		myHitBox.enabled = false;
		mySpikeHitBox.enabled = false;
		landingPos = player.transform.position;
		inAir = true;
		yield return new WaitUntil(() => Vector2.Distance(myShadow.transform.position, landingPos)<1);
		myHitBox.enabled = true;
		mySpikeHitBox.enabled = true;
		falling = true;
		gameObject.GetComponent<Rigidbody2D>().gravityScale = 2;
	}

	IEnumerator LandDelay(){
		//TODO: switch to confused/ looking around animation
		yield return new WaitForSeconds(1f);
		gameObject.GetComponent<FollowPlayerAfterNotice>().enabled = true;
		gameObject.GetComponent<RandomDirectionMovement>().enabled = true;
		gameObject.GetComponent<EnemyTakeDamage>().moveWhenHit = true;
		leaping = false;
	
		this.enabled = false;
	}


}

