using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomDirectionMovement : MonoBehaviour {

	public float movementSpeed = 0;
	public float minMoveTime = 0;
	public float maxMoveTime = 0;
	public float stopTime = 2;
	//public GameObject walkCloud;
	public ParticleSystem walkPS;
	//public float walkCloudYadjust = 0.8f;


	private Vector3 direction;
	protected bool moving = false;
	protected tk2dSpriteAnimator anim;
	int bounceOffObject;
	protected Vector3 startingScale;
	int turnOnce = 0;

	// Use this for initialization

	void Start(){
		startingScale = gameObject.transform.localScale;

	}

	protected void OnEnable () {
		//walkCloud  = GameObject.Find("effect_WalkCloud");
		anim = GetComponent<tk2dSpriteAnimator>();
		GoAgain();
	}
	
	// Update is called once per frame
	protected void Update () {
        if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
            if (moving && !anim.IsPlaying("hit")) {
                transform.position += direction * movementSpeed * Time.deltaTime;
                if (direction.x > 0) {
                    if (gameObject.transform.localScale.x < 0) {
                        if (turnOnce == 0) {
                            Turn();
                        }
                    }
                    else if (!anim.IsPlaying("run")) {
                        anim.Play("run");
						Debug.Log("RDM sets animation" + anim.CurrentClip.name);

                    }
                }
                else {
                    if (gameObject.transform.localScale.x > 0) {
                        if (turnOnce == 0) {
                            Turn();
                        }
                    }
                    else {
                        if (!anim.IsPlaying("run")) {
                            anim.Play("run");
							Debug.Log("RDM sets animation" + anim.CurrentClip.name);

                        }
                    }
                }
            }
        }
	}
	void Turn(){
		Debug.Log("Turn activated:" + gameObject.name);
		turnOnce = 1;
		/*if(anim.GetClipByName("turn") != null && !anim.IsPlaying("turn")){
			anim.Play("turn");
		}

		yield return new WaitForSeconds(.2f);
		anim.Play("run");
		*/
		gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x*-1,startingScale.y,startingScale.z);
		//Debug.Log("Turn activated");
	}

	public void TurnToNewDirection(){
		//activated by things like "Wander within bounds" when enemy reaches end of the bounds so it doesnt just keep walking into it.
		Debug.Log("Turn to new direction activated:" + gameObject.name);
		turnOnce = 1;
		gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x*-1,startingScale.y,startingScale.z);
		direction = (new Vector3(direction.x*-1, direction.y*-1, 0.0f)).normalized;

	}
	IEnumerator Pause(){
		turnOnce = 0;

		yield return new WaitForSeconds(Random.Range(minMoveTime,maxMoveTime));
		if(moving){
		bounceOffObject = 0;
		//CancelInvoke("SpawnClouds");
		walkPS.Stop();
		moving = false;
		if(anim.GetClipByName("idle") != null){
			anim.Play("idle");
			Debug.Log("RDM sets animation" + anim.CurrentClip.name);

		}
		//if(anim.IsPlaying("run")){
			//anim.Play("idleR");
		//} else if(anim.IsPlaying("runL"))
			//anim.Play("idleL");
		yield return new WaitForSeconds(stopTime);
		GoAgain();
		}
	}

	void OnCollisionEnter2D(Collision2D collision){
		//go a different direction when bump into something
		//if(collision.gameObject.transform.position.y> this.gameObject.transform.position.y);
		if(bounceOffObject == 0 && moving){
			Debug.Log("Collided with something and GoAgain() called");

			GoAgain();
			bounceOffObject = 1;
		}
		//print("Collided");
	}
	void SpawnClouds(){
		/*if(this.gameObject.activeInHierarchy == true){
			GameObject newestCloud;
			newestCloud = Instantiate(walkCloud, new Vector3(transform.position.x,transform.position.y - walkCloudYadjust, transform.position.z), Quaternion.identity) as GameObject;
			if(direction.x <0){
				newestCloud.GetComponent<Ev_WalkCloud>().MoveRight();
			}else {
				newestCloud.GetComponent<Ev_WalkCloud>().MoveLeft();
				}
		}else{
			CancelInvoke();
		}*/
	}

	public virtual void GoAgain(){
		Debug.Log("Go again activated");
		moving = true;
		//InvokeRepeating("SpawnClouds",.2f, .2f);
		walkPS.Play();
		direction = (new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0.0f)).normalized;
		StartCoroutine("Pause");
	}

	public virtual void StopMoving(){
		walkPS.Stop();
		StopAllCoroutines();
		gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		moving = false;
	}


}
