using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ev_LargeTrash : PickupableObject {
	public bool isRewardForBoss;


	//public int position;
	//public int myWorld;
	//public string myString;
	//public char myCharValue; //only used to set up string for use when showing today's collected large trash at day end
    public LargeGarbage garbage = new LargeGarbage();
    public Sprite collectedDisplaySprite;
    public BoxCollider2D myCollisionBox;

	//public GameObject largeShadow;
	//public GameObject cloudEffect;
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

	//float xSpeedWhenCollected;
	//loat pickUpYSpeed;
	//int currentRoomNumber;

	bool returning = false;


	//look up - find object in view


	// Use this for initialization
	void OnEnable () {
		
		player = GameObject.FindGameObjectWithTag("Player");
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
		player.GetComponent<JimAnimationManager>().PlayAnimation("ani_jimPickUp",true);
		gameObject.GetComponent<Renderer>().sortingLayerName = "Layer02";
		gameObject.GetComponent<Animator>().enabled = true;
		base.PickUp();
		myCollisionBox.enabled = false; //doesnt collide with player
		StartCoroutine("PickupDelay");
	}

	IEnumerator PickupDelay(){
		yield return new WaitForSeconds(1f);
		beingCarried = true;
		PickUpEvent();
		SoundManager.instance.PlaySingle(SFXBANK.ITEM_CATCH);
		player.GetComponent<EightWayMovement>().enabled = true;
	}

	public override void PickUpEvent(){
		gameObject.tag = "ActiveLargeTrash";
		player.GetComponent<JimAnimationManager>().PlayAnimation("ani_jimPickUp",true);
		sparkle.SetActive(false);
		//gameObject.GetComponent<Animator>().enabled = true;
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
		player.GetComponent<MeleeAttack>().enabled = true;

	}

	public void Kill(){
		//activated when player dies
		if(phase == 3){
			phase = 0;
			GlobalVariableManager.Instance.CARRYING_SOMETHING = false;
			//add mychar value and room number to large trash locations?!?
		}
	}

	void MyCollectionSetUp(){

	}


	/*void Drop(){
		gameObject.transform.position = new Vector2(transform.position.x + 1f, transform.position.y -1f);
		GlobalVariableManager.Instance.LARGE_TRASH_LOCATIONS[myPositionInList+1] = transform.position;
		myShadow = Instantiate(largeShadow,new Vector2(transform.position.x - .1f,transform.position.y -.8f),Quaternion.identity);
		Debug.Log("DROPPED LARGE TRASH");
		myCollision = Instantiate(collisionBox,transform.position, Quaternion.identity);
		mySparkles = Instantiate(sparkle,transform.position,Quaternion.identity);
		phase = 0;
		gameObject.tag = "LargeTrash";
		gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Layer02";
	}*/





	IEnumerator Fall(){

		gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,20f);
		yield return new WaitForSeconds(1.5f);
		gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,-20f);
		yield return new WaitForSeconds(1.8f);
		gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,0f);

	}

	/*void Bounce(){
		bounce++;
		if(bounce == 1){
			gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,-15f);
			if(GlobalVariableManager.Instance.MASTER_SFX_VOL >0){
				//play drop trash3 on channel 2
			}
		}else if (bounce == 2){
			gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,0f);
			if(GlobalVariableManager.Instance.MASTER_SFX_VOL >0){
				//play drop trash3 on channel 2
			}
		}
	}//end of Bouce()*/





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
		gameObject.GetComponent<Animator>().enabled = false;
		Debug.Log("Return activated - LARGE TRASH");
		phase = 0;
		GlobalVariableManager.Instance.CARRYING_SOMETHING = false;

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
		returning = true;
		player.GetComponent<JimAnimationManager>().PlayAnimation("ani_jimIdle",false);
		player.GetComponent<MeleeAttack>().enabled = true;
		dumpster.GetComponent<SE_GlowWhenClose>().enabled = true;
		//ReturnArc();
		StartCoroutine("ReturnSequence");
		
	}// end of Return()

	IEnumerator ReturnSequence(){
		GameStateManager.Instance.PushState(typeof(MovieState));
		player.GetComponent<JimAnimationManager>().PlayExcitedJump();
		CamManager.Instance.mainCamEffects.CameraPan(player.transform.position," ");
		CamManager.Instance.mainCamEffects.ZoomInOut(2f,1f);
		yield return new WaitForSeconds(.5f);
	
		dumpster.GetComponent<Ev_Dumpster>().largeTrashDiscoveredDisplay.GetComponent<GUI_LargeTrashCollectedDisplay>().indexOfCurrentLargeTrash =  garbage.GarbageIndex();
		dumpster.GetComponent<Ev_Dumpster>().largeTrashDiscoveredDisplay.SetActive(true);
		CamManager.Instance.mainCamEffects.ReturnFromCamEffect();
		player.GetComponent<JimAnimationManager>().StopTweenAnimation();
		Destroy(gameObject);

	}


}
