﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableObject : PickupableObject {

	float landingY;
	bool beingThrown;
	protected bool canThrow;
	public AudioClip carrySound;
	public GameObject myShadow;
	Room currentRoom;//for staying in bounds
	public BoxCollider2D physicalCollision;

	public List<MonoBehaviour> behaviorsToStop = new List<MonoBehaviour>();
	// Use this for initialization
	void Start(){
		base.Start();
		myBody.drag = 0.5f;
	}


	// Update is called once per frame
	protected override void Update () {
		base.Update();
		if(ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT) && canThrow && !GlobalVariableManager.Instance.TUT_POPUP_ISSHOWING){
			Invoke("Throw",.1f);
		}

		if(beingThrown){
			if(transform.position.y < landingY){
				ObjectPool.Instance.GetPooledObject("effect_enemyLand",gameObject.transform.position);
				Debug.Log("Reached Landing");
				beingThrown = false;
				myBody.gravityScale = 0f;
				myBody.velocity = new Vector2(0,0f);
				myBody.AddForce(new Vector2(-4f*(Mathf.Sign(gameObject.transform.lossyScale.x)),0f),ForceMode2D.Impulse);//slide
				beingCarried= false;
				canThrow = false;
				myShadow.SetActive(true);
				myShadow.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,.75f);

			//	physicalCollision.enabled = false;
				gameObject.layer = 11; //switch to item layer.
				for(int i = 0; i < behaviorsToStop.Count; i++){
					behaviorsToStop[i].enabled = true;
				}
				gameObject.GetComponent<Renderer>().sortingLayerName = "Layer01";
				gameObject.GetComponent<IsometricSorting>().enabled = true;
				gameObject.GetComponent<CannotExitScene>().enabled = false;
				if(physicalCollision != null)
					physicalCollision.enabled = true;
			}
		}

		Debug.Log(physicalCollision.enabled);

	}

	public override void PickUp(){
		base.PickUp();
		gameObject.GetComponent<IsometricSorting>().enabled = false;
		gameObject.GetComponent<Renderer>().sortingLayerName = "Layer02";
		//if(physicalCollision != null)
			physicalCollision.enabled = false;
		myShadow.SetActive(false);
	}

	void Throw(){
		//beingCarried = false;
		gameObject.GetComponent<Animator>().enabled = false;

		player.GetComponent<PlayerTakeDamage>().currentlyCarriedObject = null;
		if(currentRoom != RoomManager.Instance.currentRoom){
			gameObject.GetComponent<CannotExitScene>().SetLimits(RoomManager.Instance.currentRoom);
		}
	//	physicalCollision.enabled = false;
		//gameObject.GetComponent<CannotExitScene>().enabled = true;
		Debug.Log("Thrown");
		myShadow.GetComponent<SpriteRenderer>().sortingLayerName = "Layer01";
		GlobalVariableManager.Instance.CARRYING_SOMETHING = false;
		player.GetComponent<MeleeAttack>().enabled = true;
		if(gameObject.GetComponent<BoxCollider2D>()!=null){
			gameObject.GetComponent<BoxCollider2D>().enabled = true;
		}
		player.GetComponent<JimAnimationManager>().PlayAnimation("ani_jimThrowR",true);
		spinning = true;
		landingY = transform.position.y -3f;
		beingThrown = true;
		gameObject.transform.parent = null;
		myBody.simulated = true;
		gameObject.transform.position = new Vector2(transform.position.x,transform.position.y-1); //move more directly in front of player
		myBody.velocity = new Vector2(22f*(Mathf.Sign(player.transform.lossyScale.x)),2f);
		player.GetComponent<EightWayMovement>().myLegs.SetActive(true);
		myBody.gravityScale = 1f;
	
	}

	public override void PickUpEvent(){
		Debug.Log("*******Pickup Event activate********");
		player.GetComponent<EightWayMovement>().myLegs.SetActive(false);
		player.GetComponent<EightWayMovement>().clipOverride = false;
		//gameObject.GetComponent<Animator>().enabled = true;
		canThrow = true;
		myShadow.SetActive(true);
		myShadow.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,.5f);
		myShadow.GetComponent<SpriteRenderer>().sortingLayerName = "Layer02";
		SoundManager.instance.PlaySingle(carrySound);
		gameObject.layer = 15; //switch to thrownTrash layer.
		for(int i = 0; i < behaviorsToStop.Count; i++){
			behaviorsToStop[i].enabled = false;
		}
	}

	public override void DropEvent(){
				Debug.Log("THORWABLE OBJECT DROP() ACTIVATED----");
				landingY = transform.position.y -3f;

				beingThrown = false;
				myBody.gravityScale = 0f;
				myBody.velocity = new Vector2(0,0f);
				beingCarried= false;
				canThrow = false;
				myShadow.SetActive(true);
				myShadow.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,.75f);

				gameObject.layer = 11; //switch to item layer.
				for(int i = 0; i < behaviorsToStop.Count; i++){
					behaviorsToStop[i].enabled = true;
				}
				gameObject.GetComponent<Renderer>().sortingLayerName = "Layer01";
				gameObject.GetComponent<IsometricSorting>().enabled = true;
				gameObject.GetComponent<CannotExitScene>().enabled = false;
	}




}