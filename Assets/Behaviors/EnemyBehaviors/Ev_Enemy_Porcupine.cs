using UnityEngine;
using System.Collections;

public class Ev_Enemy_Porcupine : FollowPlayer
{


	//turns around to spikey backside and leaps toward player.

	public GameObject myShadow;
	public GameObject shellShield;
	public BoxCollider2D myHitBox;
	public AudioClip land;
	public GameObject[] dirtBallSpawnPoints;

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
			myShadow.transform.position = Vector2.MoveTowards(myShadow.transform.position,landingPos,15*Time.deltaTime);
			//Debug.Log("landing Position: " + landingPos + myShadow.transform.position);

			if(!falling)
				//gameObject.transform.position = Vector2.Lerp(gameObject.transform.position,new Vector2(myShadow.transform.position.x,myShadow.transform.position.y +3f),4*Time.deltaTime);
				gameObject.transform.position = new Vector2(myShadow.transform.position.x +.1f, Mathf.Lerp(gameObject.transform.position.y, myShadow.transform.position.y+3f, 9*Time.deltaTime));

		}



		if(falling){
			if(gameObject.transform.position.y < landingPos.y+1){
				Debug.Log("Landed from fall" + gameObject.transform.position.y + landingPos.y);
				falling = false;
				inAir = false;
				gameObject.GetComponent<Renderer>().sortingLayerName = "Layer01";
				SoundManager.instance.PlaySingle(land);
				SpawnDirtBallRing();
				ObjectPool.Instance.GetPooledObject("effect_enemyLand",new Vector2(gameObject.transform.position.x, gameObject.transform.position.y -1f));
				gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
				gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
				myShadow.transform.parent = gameObject.transform;
				myShadow.transform.localPosition = new Vector2(0f,-1.33f);
				shellShield.SetActive(false);
				myHitBox.enabled = true;
				StartCoroutine("LandDelay");
			}
		}

	}

	void Leap(){
		//gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x*-1,gameObject.transform.localScale.y); // turn around

		//TODO: switch to leap animation
		StartCoroutine("LeapSequence");

	}


	IEnumerator LeapSequence(){
		gameObject.GetComponent<tk2dSpriteAnimator>().Play("turnAround");
		shellShield.SetActive(true);
		myHitBox.enabled = false;
		gameObject.GetComponent<Animator>().Play("leapRotation",0,-1f);
		Debug.Log("Got here leap sequence - 1");
		yield return new WaitForSeconds(.5f);
		Debug.Log("Got here leap sequence - 2");
		gameObject.GetComponent<Renderer>().sortingLayerName = "Layer03"; // above other objects when in air
		gameObject.GetComponent<EnemyTakeDamage>().moveWhenHit = false;
		myShadow.transform.parent = null;

		landingPos = player.transform.position;
		inAir = true;
		yield return new WaitUntil(() => Vector2.Distance(myShadow.transform.position, landingPos)<1);
		Debug.Log("Got here leap sequence - 3");
	

		falling = true;
		gameObject.GetComponent<Rigidbody2D>().gravityScale = 2;
	}

	IEnumerator LandDelay(){
		
		gameObject.GetComponent<tk2dSpriteAnimator>().Play("LandConfusion");
		yield return new WaitForSeconds(1f);

		gameObject.GetComponent<FollowPlayerAfterNotice>().enabled = true;
		gameObject.GetComponent<RandomDirectionMovement>().StartMoving();
		gameObject.GetComponent<EnemyTakeDamage>().moveWhenHit = true;
		leaping = false;
		gameObject.GetComponent<tk2dSpriteAnimator>().Play("run");
		//this.enabled = false;
	}

	void SpawnDirtBallRing(){
		Vector2 movementDir;

		for(int i = 0; i <dirtBallSpawnPoints.Length;i++){
			GameObject dirtBall = ObjectPool.Instance.GetPooledObject("projectile_fallingRock",gameObject.transform.position);
			movementDir = ( gameObject.transform.position - dirtBallSpawnPoints[i].transform.position).normalized * 5;
			dirtBall.GetComponent<Rigidbody2D>().velocity = new Vector2(movementDir.x,movementDir.y);
		}
	}


}

