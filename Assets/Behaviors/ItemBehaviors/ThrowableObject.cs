﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CannotExitScene))]
public class ThrowableObject : PickupableObject {

	float landingY;
	protected bool beingThrown;
	protected bool canThrow;
	public AudioClip carrySound;
	public GameObject myShadow;
	Room currentRoom;//for staying in bounds
	public BoxCollider2D physicalCollision;
	public AudioClip throwObject;
	public AudioClip landSfx;
	public List<MonoBehaviour> behaviorsToStop = new List<MonoBehaviour>();
	public bool livingBody;
	public Transform roomToReattatchTo;
	public bool onGround = true; //used for living bodies that revive after a bit, to make sure they dont do so while being carried
	GameObject panicSweat;
	// Use this for initialization
	void Start(){
		base.Start();
		myBody.drag = 0.5f;
	}


	// Update is called once per frame
	protected override void Update () {
		base.Update();
        if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
            if (ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT) && canThrow) {
                Invoke("Throw", .1f);
                canThrow = false; 
            }

            if (beingThrown) {
                if (transform.position.y < landingY) {
                    SoundManager.instance.PlaySingle(landSfx);
                    ObjectPool.Instance.GetPooledObject("effect_enemyLand", gameObject.transform.position);
                    Debug.Log("Reached Landing");
                    beingThrown = false;
                    myBody.gravityScale = 0f;
                    myBody.velocity = new Vector2(0, 0f);
                    myBody.AddForce(new Vector2(-4f * (Mathf.Sign(gameObject.transform.lossyScale.x)), 0f), ForceMode2D.Impulse);//slide
                    spinning = false;
                    canThrow = false;

                    if (myShadow != null) {
                        myShadow.transform.parent = this.gameObject.transform;
                        myShadow.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, .75f);
                        myShadow.transform.rotation = Quaternion.identity;
						myShadow.GetComponent<Renderer>().sortingLayerName = "Layer01";

                    }

                    //	physicalCollision.enabled = false;
                    gameObject.layer = 11; //switch to item layer.
                    for (int i = 0; i < behaviorsToStop.Count; i++) {
                        behaviorsToStop[i].enabled = true;
                    }
                    gameObject.transform.localScale = Vector2.one;
                    gameObject.GetComponent<Renderer>().sortingLayerName = "Layer01";
                    gameObject.GetComponent<IsometricSorting>().enabled = true;
                    gameObject.GetComponent<CannotExitScene>().enabled = false;
                    if (physicalCollision != null)
                        physicalCollision.enabled = true;

					onGround = true;
					LandingEvent();
                }else{
                	if(myShadow !=null){
                		myShadow.transform.position = new Vector2(gameObject.transform.position.x,landingY); // shadow follows body
                	}
                }
            }
        }
	}

	public override void PickUp(){
        PlayerManager.Instance.controller.SendTrigger(JimTrigger.PICK_UP_THROWABLE);
        onGround = false;
		gameObject.GetComponent<Renderer>().sortingLayerName = "Layer02";
		gameObject.GetComponent<Animator>().enabled = true;
		base.PickUp();
		physicalCollision.enabled = false;


		StartCoroutine("PickupDelay");

        if (myShadow != null)
		    myShadow.SetActive(false);
	}

	IEnumerator PickupDelay(){
		yield return new WaitForSeconds(1f);

		if(livingBody){
            StartSweat();
		}
        if (myShadow != null)
		    myShadow.SetActive(true);
		SoundManager.instance.PlaySingle(SFXBANK.ITEM_CATCH);
		canThrow = true;
	}

	protected virtual void Throw(){
        var animator = gameObject.GetComponent<Animator>();
            if (animator != null) animator.enabled = false;

		SoundManager.instance.PlaySingle(throwObject);
        PlayerManager.Instance.player.GetComponent<PlayerTakeDamage>().currentlyCarriedObject = null;
		if(currentRoom != RoomManager.Instance.currentRoom){
			gameObject.GetComponent<CannotExitScene>().SetLimits(RoomManager.Instance.currentRoom);
		}
	//	physicalCollision.enabled = false;
		//gameObject.GetComponent<CannotExitScene>().enabled = true;
		Debug.Log("Thrown");

        gameObject.layer = 15; //becomes 'ThrowableObject'
		if(gameObject.GetComponent<BoxCollider2D>()!=null){
			gameObject.GetComponent<BoxCollider2D>().enabled = true;
		}
		spinning = true;
		landingY = transform.position.y -3f;
		if (myShadow != null){
            myShadow.GetComponent<SpriteRenderer>().sortingLayerName = "Layer01";
			myShadow.GetComponent<SpriteRenderer>().sortingOrder= GetComponent<Renderer>().sortingOrder -1;
            myShadow.transform.parent = null;
            myShadow.transform.position = new Vector2(myShadow.transform.position.x, landingY);
        }
		beingThrown = true;
		gameObject.transform.parent = null;
		myBody.simulated = true;
		gameObject.transform.position = new Vector2(transform.position.x,transform.position.y-1); //move more directly in front of player
		myBody.velocity = new Vector2(32f*(Mathf.Sign(PlayerManager.Instance.player.transform.lossyScale.x)),2f);

        if (livingBody)
            StopSweat();

        PlayerManager.Instance.controller.SendTrigger(JimTrigger.THROW);

		myBody.gravityScale = 1.5f;
	
	}

	public override void PickUpEvent(){
		Debug.Log("*******Pickup Event activate********");
		canThrow = true;

        if (myShadow != null){
            myShadow.SetActive(true);
            myShadow.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, .5f);
            myShadow.GetComponent<SpriteRenderer>().sortingLayerName = "Layer02";
        }
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
				canThrow = false;
                if (myShadow != null){
                    myShadow.SetActive(true);
                    myShadow.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, .75f);
                }
                if(livingBody){
                	
                	if(roomToReattatchTo !=null){
                		gameObject.transform.parent = roomToReattatchTo; // return body to proper parent, in the case of trio in stuart fight, it was a room, not sure if applicable to later things...
                	}
					gameObject.GetComponent<Animator>().enabled = false;
					gameObject.transform.localScale = Vector2.one;
                }
				gameObject.layer = 11; //switch to item layer.
				for(int i = 0; i < behaviorsToStop.Count; i++){
					behaviorsToStop[i].enabled = true;
				}
				gameObject.GetComponent<Renderer>().sortingLayerName = "Layer01";
				gameObject.GetComponent<IsometricSorting>().enabled = true;
				gameObject.GetComponent<CannotExitScene>().enabled = false;
	}

	public virtual void LandingEvent(){
		//nothing for base
	}

    public void StartSweat()
    {
        if (panicSweat != null)
            ObjectPool.Instance.ReturnPooledObject(panicSweat);

        panicSweat = ObjectPool.Instance.GetPooledObject("effect_carrySweat", gameObject.transform.position);
        panicSweat.transform.parent = gameObject.transform;
    }

	public void StopSweat(){
		if(panicSweat != null){
			ObjectPool.Instance.ReturnPooledObject(panicSweat);
            panicSweat = null;
		}
	}


}
