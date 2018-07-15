﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EightWayMovement : MonoBehaviour {

	private tk2dSpriteAnimator anim;
    public float speed = 8f;
    private Vector2 movement;

 
    bool isDiagonal = false;
    bool noDelayStarted = false;
    public float delay = 0.05f;
    private int directionFacing = 2; //1 = right, 2 = left, 3 = up, 4 = down
    private float momentum = 0f;
 
	//public GameObject legs;
	//public GameObject shadow;
	public GameObject walkCloud;

	public GameObject myLegs;
	private tk2dSpriteAnimator legAnim;
	private int setOnce = 0;
	Vector3 transformScale; // used for facing different directions
 
    // Use this for initialization
    void Start () {


        legAnim = myLegs.GetComponent<tk2dSpriteAnimator>();
        anim = GetComponent<tk2dSpriteAnimator>();


        transformScale = gameObject.transform.localScale;

		//GameObject myShadow;
		//myShadow = Instantiate(shadow, transform.position, Quaternion.identity) as GameObject;
		//myShadow.GetComponent<AttatchSelfToObject>().objectToSnapTo = this.gameObject;

		
        //GetComponent<Rigidbody2D>().velocity = movement;
    }
 
    // Update is called once per frame
    void Update () {
    	
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");
        movement = new Vector2(inputX, inputY);
        //Debug.Log(inputX);
 
        //Diagonals
 
        if (inputX != 0 && inputY != 0) {
        	if (momentum != 4){
        		momentum = 4f;
				InvokeRepeating("SpawnClouds",.2f, .2f); //just have this here so it only happens once
        	}
            isDiagonal = true;
            if (movement.y == 1 && movement.x == -1) {
            	if(directionFacing == 1){
				 	anim.Play("ani_jimTurn");
					StartCoroutine ("TurnDelay");
				 }else{
					anim.Play("ani_jimWalk");
				 }
            }
 
            if (movement.y == 1 && movement.x == 1) {
				if(directionFacing == 2){
				 	anim.Play("ani_jimTurn");
					StartCoroutine ("TurnDelay");
				 }else{
					anim.Play("ani_jimWalk");
				 }
            }
 
            if (movement.y == -1 && movement.x == -1) {
				anim.Play("ani_jimWalkUp");
            }
 
            if (movement.y == -1 && movement.x == 1) {
                anim.Play("ani_jimWalkDown");
            }
		} else {   if (isDiagonal && !noDelayStarted) {
                StartCoroutine (NoMoreDiagonal ());
                noDelayStarted = true;
            } else if(anim != null) {
 
                //left/right/up/down
                if (movement.x == -1) {
				if(directionFacing == 1){
				 	anim.Play("ani_jimTurn");
					StartCoroutine (TurnDelay());

				 }else{
					anim.Play("ani_jimWalk");
				 }
                }
 
                if (movement.x == 1) {
				if(directionFacing == 1){
				 	anim.Play("ani_jimTurn");
					StartCoroutine ("TurnDelay");

				 }else{
					anim.Play("ani_jimWalk");
				 }
                }
 
 
                if (movement.y == 1) {
                    anim.Play("ani_jimWalkUp");
                }
 
 
                if (movement.y == -1) {
                    anim.Play("ani_jimWalkDown");
                }

                if(movement.x == 0 && movement.y == 0){
					if(anim.CurrentClip.name == "ani_jimWalk"){
						anim.Play("ani_jimIdle");
					}else if(anim.CurrentClip.name == "ani_jimWalkDown"){
						anim.Play("ani_jimIdleDown");
					}else if(anim.CurrentClip.name == "ani_jimWalkUp"){
						anim.Play("ani_jimIdleUp");
					}

					//not instant stop
				if(momentum > 0)
						momentum = momentum - .2f;
				}else if(momentum != 4){
					momentum = 4f;
				InvokeRepeating("SpawnClouds",.2f, .2f); //just have this here so it only happens once
                }
            }
        }
		if(movement.x == 0 && movement.y == 0){

			CancelInvoke("SpawnClouds");




			//not instant stop
			if(anim.CurrentClip.name == "ani_jimIdleUp"){
				transform.Translate(new Vector2(0,momentum)*Time.deltaTime);
			}else if(anim.CurrentClip.name == "ani_jimIdleDown"){
				transform.Translate(new Vector2(0,momentum*-1)*Time.deltaTime);
			}else if (gameObject.transform.localScale.x > 0 ){
				transform.Translate(new Vector2(momentum,0)*Time.deltaTime);
			}else if(gameObject.transform.localScale.x < 0 ){
				transform.Translate(new Vector2(momentum*-1,0)*Time.deltaTime);
			}
		}
		if(GlobalVariableManager.Instance.PLAYER_CAN_MOVE){

	        	transform.Translate(movement * speed * Time.deltaTime);
	        	//show legs and change to current animation of Jim
	        	legAnim.Play(anim.CurrentClip.name);
				
			
        }
    }
 
    IEnumerator NoMoreDiagonal () {
        yield return new WaitForSeconds (delay);
        isDiagonal = false;
        noDelayStarted = false;
    }
    void SpawnClouds(){
		GameObject newestCloud;
		newestCloud = Instantiate(walkCloud, new Vector3(transform.position.x,transform.position.y - 1.5f, transform.position.z), Quaternion.identity) as GameObject;
		if(movement.x <0){
			newestCloud.GetComponent<Ev_WalkCloud>().MoveRight();
		}else {
			newestCloud.GetComponent<Ev_WalkCloud>().MoveLeft();
			}

    }
	IEnumerator TurnDelay() {
        yield return new WaitForSeconds (.3f);
		if(directionFacing == 1 || directionFacing == 2){
			anim.Play("ani_jimWalk");
			gameObject.transform.localScale = new Vector3(transformScale.x*-1,transformScale.y,transformScale.z);
				if(directionFacing == 1){
					directionFacing = 2;
				}else{
					directionFacing = 1;
				}
		}else if(directionFacing == 3){
			anim.Play("ani_jimWalkDown");
			directionFacing = 4;
        }else{
			anim.Play("ani_jimWalkUp");
			directionFacing = 4;
        }
	
    }
}
