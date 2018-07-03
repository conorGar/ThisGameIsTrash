using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomDirectionMovement : MonoBehaviour {

	public float movementSpeed = 0;
	public float minMoveTime = 0;
	public float maxMoveTime = 0;
	public GameObject walkCloud;
	public float walkCloudYadjust = 0.8f;

	private Vector3 direction;
	private bool moving = false;
	private tk2dSpriteAnimator anim;

	// Use this for initialization
	void Start () {
		//walkCloud  = GameObject.Find("effect_WalkCloud");
		anim = GetComponent<tk2dSpriteAnimator>();
		GoAgain();
	}
	
	// Update is called once per frame
	void Update () {
		if(moving && !anim.IsPlaying("hit")){
			transform.position += direction*movementSpeed*Time.deltaTime;
			if(direction.x > 0 ){
				if(!anim.IsPlaying("runR")){
					anim.Play("runR");
					}
			}else{
				if(!anim.IsPlaying("runL"))
					anim.Play("runL");
			}
		}
	}

	IEnumerator Pause(){
		yield return new WaitForSeconds(Random.Range(minMoveTime,maxMoveTime));
		CancelInvoke("SpawnClouds");
		moving = false;
		if(anim.IsPlaying("runR")){
			anim.Play("idleR");
		} else if(anim.IsPlaying("runL"))
			anim.Play("idleL");
		yield return new WaitForSeconds(2);
		GoAgain();
	}

	void OnCollisionEnter2D(Collision2D collision){
		print("Collided");
	}
	void SpawnClouds(){
		GameObject newestCloud;
		newestCloud = Instantiate(walkCloud, new Vector3(transform.position.x,transform.position.y - walkCloudYadjust, transform.position.z), Quaternion.identity) as GameObject;
		if(direction.x <0){
			newestCloud.GetComponent<Ev_WalkCloud>().MoveRight();
		}else {
			newestCloud.GetComponent<Ev_WalkCloud>().MoveLeft();
			}
	}

	void GoAgain(){
		moving = true;
		InvokeRepeating("SpawnClouds",.2f, .2f);
		direction = (new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0.0f)).normalized;
		StartCoroutine("Pause");
	}


}
