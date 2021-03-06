﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ev_LargeTrashNEW : PickupableObject {
	public bool isRewardForBoss;



    public LargeGarbage garbage = new LargeGarbage();
    public Sprite collectedDisplaySprite;
    public BoxCollider2D myCollisionBox;


	public GameObject sparkle;
	public GameObject smokePuff;
	public GameObject ltmManager;//only used for drop function
	public AudioClip returnSound;
    
	//^
	//large trash is aware f the current room it is in. This room is given by roomManger.currentoom at start and when large trash
	//is dropped. If myCurrentRoom = RoomManager.currentRoom, deactivate self. (In update method?) check if the roomManager.room =
	//current room and if so, activate the large trash
	public string trashTitle;
	public Room myCurrentRoom; // used by map Star icons
	[HideInInspector]
	//public GameObject dumpster; //used for return

	int phase = 0;

	int doOnce = 0;


	// Use this for initialization
	void OnEnable () {
		
		dumpster = GameObject.Find("Dumpster");
        requiresGrabbyGloves = false;

		//playerAni = player.GetComponent<tk2dSpriteAnimator>();
		//currentRoomNumber = GlobalVariableManager.Instance.ROOM_NUM;

		if(GlobalVariableManager.Instance.ROOM_NUM == 101){
			MyCollectionSetUp();
		}else{


					//myY = gameObject.transform.position.y;
					phase = 0;
					/*if(currentRoomNumber != 101){
							if(GlobalVariableManager.Instance.MASTER_SFX_VOL >0 && gameObject.active){
									//loop large trash sparkle on channel 11
								}
							Debug.Log("GOT HERE LARGE TRASH");
							//mySparkles = Instantiate(sparkle,transform.position,Quaternion.identity);
							//m//yCollision = Instantiate(collisionBox,transform.position, Quaternion.identity);



						//myShadow = Instantiate(largeShadow,new Vector2(transform.position.x - .1f,transform.position.y -.8f),Quaternion.identity);

							//ShowNow();


						}*/
					
			
		}
	}// end of Start()

	public override void PickUp(){
        PlayerManager.Instance.controller.SendTrigger(JimTrigger.PICK_UP_DROPPABLE);
		gameObject.GetComponent<Renderer>().sortingLayerName = "Layer02";
		gameObject.GetComponent<Animator>().enabled = true;
		base.PickUp();
		myCollisionBox.enabled = false; //doesnt collide with player
		StartCoroutine("PickupDelay");
	}

	IEnumerator PickupDelay(){
		yield return new WaitForSeconds(1f);
		PickUpEvent();
		SoundManager.instance.PlaySingle(SFXBANK.ITEM_CATCH);
	}

	public override void PickUpEvent(){
		gameObject.tag = "ActiveLargeTrash";
		sparkle.SetActive(false);
	}

	public override void DropEvent(){
		gameObject.tag = "LargeTrash";
		sparkle.SetActive(true);
		myCollisionBox.enabled = true; //doesnt collide with player
		gameObject.transform.parent = ltmManager.transform; //goes back to largeTrashManager when drop
		//gameObject.GetComponent<Animator>().SetTrigger("Drop");
		gameObject.GetComponent<Renderer>().sortingLayerName = "Layer02";
		gameObject.GetComponent<Animator>().enabled = false;
		myCurrentRoom = RoomManager.Instance.currentRoom;
	}

	public void Kill(){
		//activated when player dies
		if(phase == 3){
			phase = 0;
			//add mychar value and room number to large trash locations?!?
		}
	}

	void MyCollectionSetUp(){

	}

	IEnumerator Fall(){

		gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,20f);
		yield return new WaitForSeconds(1.5f);
		gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,-20f);
		yield return new WaitForSeconds(1.8f);
		gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,0f);

	}

	void ReturnArc(){
		

						
			//GameObject deathSmoke;
			//deathSmoke = Instantiate(smokePuff,transform.position, Quaternion.identity);
			Debug.Log("GOT HERE LARGE TRASH RETURN");
			/*this.gameObject.transform.parent = null; //removes from large trash holder
			this.gameObject.SetActive(false); // sets unactive and just waits for scene to change to destroy. For whatever reason Destroy() wasn't working
			Debug.Log("GOT PAST DESTROY?" + this.gameObject.name);
			*/
			Destroy(gameObject);
			
	}
	public void Return(){
        //activated by dumpster's 'SE_GlowWhenClose'
        Debug.Log("Return activated - LARGE TRASH");
        PlayerManager.Instance.controller.SendTrigger(JimTrigger.DELIVER_BIG);
        gameObject.GetComponent<Animator>().enabled = false;
		phase = 0;

		SoundManager.instance.PlaySingle(returnSound);
        // Add this trash item to the large trash list.
        var largeTrashItem = new GlobalVariableManager.LargeTrashItem(garbage.type);
        largeTrashItem.collectedDisplaySprite = collectedDisplaySprite;
        largeTrashItem.collectedTitle = trashTitle;
        GlobalVariableManager.Instance.LARGE_TRASH_LIST.Add(largeTrashItem);
        GlobalVariableManager.Instance.LARGE_GARBAGE_DISCOVERED |= garbage.type;
        GlobalVariableManager.Instance.LARGE_TRASH_COLLECTED++;
		if(GlobalVariableManager.Instance.MASTER_SFX_VOL > 0){
			//play trash pickup on channel 2
		}
		myBody.simulated = true;
		gameObject.transform.parent = null;
		myBody.AddForce(new Vector2(0,10),ForceMode2D.Impulse);
		myBody.gravityScale = 2;
		ObjectPool.Instance.GetPooledObject("effect_pickUpSmoke",gameObject.transform.position);
		dumpster.GetComponent<SE_GlowWhenClose>().enabled = true;
		StartCoroutine("ReturnSequence");
		
	}// end of Return()

	IEnumerator ReturnSequence(){
		GameStateManager.Instance.PushState(typeof(MovieState));
		CamManager.Instance.mainCamEffects.CameraPan(PlayerManager.Instance.player.transform.position," ");
		CamManager.Instance.mainCamEffects.ZoomInOut(2f,1f);
		yield return new WaitForSeconds(.5f);
	
		dumpster.GetComponent<Ev_Dumpster>().largeTrashDiscoveredDisplay.GetComponent<GUI_LargeTrashCollectedDisplay>().indexOfCurrentLargeTrash =  garbage.GarbageIndex();
		dumpster.GetComponent<Ev_Dumpster>().largeTrashDiscoveredDisplay.SetActive(true);
		CamManager.Instance.mainCamEffects.ReturnFromCamEffect();
		Destroy(gameObject);
	}


}
