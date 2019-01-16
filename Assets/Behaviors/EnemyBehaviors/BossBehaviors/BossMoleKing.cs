using UnityEngine;
using System.Collections;

public class BossMoleKing : Boss
{
		
	public GameObject myShadow;
	public GameObject[] dirtBallSpawnPoints;
	public GameObject[] smallMoleSpawners;

	public AudioClip land;

	bool inAir;
	Vector2 targetPos;
	bool falling;
	public GameObject player;

	void Start ()
	{
		base.Start();
		StartCoroutine("Leap");
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(inAir){
			myShadow.transform.position = Vector2.MoveTowards(myShadow.transform.position,targetPos,5*Time.deltaTime);
			//Debug.Log("landing Position: " + landingPos + myShadow.transform.position);

			if(!falling)
				//gameObject.transform.position = Vector2.Lerp(gameObject.transform.position,new Vector2(myShadow.transform.position.x,myShadow.transform.position.y +3f),4*Time.deltaTime);
				gameObject.transform.position = new Vector2(myShadow.transform.position.x +.1f, Mathf.Lerp(gameObject.transform.position.y, myShadow.transform.position.y+15f, 9*Time.deltaTime));

		}



		if(falling){
			if(gameObject.transform.position.y < targetPos.y+1){
				Debug.Log("Landed from fall" + gameObject.transform.position.y + targetPos.y);
				falling = false;
				inAir = false;
				gameObject.GetComponent<Renderer>().sortingLayerName = "Layer01";
				SoundManager.instance.PlaySingle(land);
				ObjectPool.Instance.GetPooledObject("effect_enemyLand",new Vector2(gameObject.transform.position.x, gameObject.transform.position.y -1f));
				gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
				gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
				myShadow.transform.parent = gameObject.transform;
				myShadow.transform.localPosition = new Vector2(0f,-1.33f);
				StartCoroutine("LandDelay");
			}
		}
	}

	IEnumerator Leap(){
		Debug.Log("Mole king - LEAP activated");
		gameObject.GetComponent<tk2dSpriteAnimator>().Play("leap");

		//gameObject.GetComponent<Animator>().Play("leapRotation",0,-1f);
		Debug.Log("Got here leap sequence - 1");
		yield return new WaitForSeconds(.5f);
		Debug.Log("Got here leap sequence - 2");
		gameObject.GetComponent<Renderer>().sortingLayerName = "Layer03"; // above other objects when in air

		myShadow.transform.parent = null;

		targetPos = player.transform.position;
		inAir = true;
		Invoke("TossRock",Random.Range(1f,2f));
		yield return new WaitUntil(() => Vector2.Distance(myShadow.transform.position, targetPos)<1);
		Debug.Log("Got here leap sequence - 3");
		CancelInvoke("TossRock"); // cancels rock toss if haven't done so already

		falling = true;
		gameObject.GetComponent<Rigidbody2D>().gravityScale = 2;
	}

	IEnumerator LandDelay(){
		
		gameObject.GetComponent<tk2dSpriteAnimator>().Play("LandingDive");
		SpawnDirtBallRing();
		yield return new WaitForSeconds(.4f);
		gameObject.GetComponent<tk2dSpriteAnimator>().Play("stuck");
		yield return new WaitForSeconds(Random.Range(1.1f,2.1f));
		gameObject.GetComponent<tk2dSpriteAnimator>().Play("completeDive"); //mole goes fully into ground
		StartCoroutine("NewPopup");
	}

	IEnumerator NewPopup(){
		Debug.Log("Mole king - new popup activated");
		yield return new WaitForSeconds(Random.Range(.5f,1.1f));
		targetPos = player.transform.position;
		gameObject.GetComponent<tk2dSpriteAnimator>().Play("Popup");
		yield return new WaitForSeconds(.4f);
		gameObject.GetComponent<tk2dSpriteAnimator>().Play("RiseFromGround");
		StartCoroutine("Leap");
	}

	void SpawnDirtBallRing(){
		Vector2 movementDir;

		for(int i = 0; i <dirtBallSpawnPoints.Length;i++){
			GameObject dirtBall = ObjectPool.Instance.GetPooledObject("projectile_largeRock",gameObject.transform.position);
			movementDir = ( gameObject.transform.position - dirtBallSpawnPoints[i].transform.position).normalized * 5;
			dirtBall.GetComponent<Rigidbody2D>().velocity = new Vector2(movementDir.x,movementDir.y);
		}
	}

	void TossRock(){
		Debug.Log("Toss Rock Activate");
		gameObject.GetComponent<FireTowardPlayer>().FireBullet();
	}

	public void ActivateMoleSpawners(){
		for(int i = 0; i < smallMoleSpawners.Length; i++){
			smallMoleSpawners[i].SetActive(true);
		}
	}
}

