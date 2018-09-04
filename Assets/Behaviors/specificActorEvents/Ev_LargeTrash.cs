using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ev_LargeTrash : PickupableObject {

	
	public int myPositionInList;
	public bool isRewardForBoss;

	//public int myRoomNumber;
	//public int position;
	//public int myWorld;
	//public string myString;
	//public char myCharValue; //only used to set up string for use when showing today's collected large trash at day end
    public LargeGarbage garbage = new LargeGarbage();
    public Sprite collectedDisplaySprite;


	//public GameObject largeShadow;
	//public GameObject cloudEffect;
	public GameObject sparkle;
	public GameObject smokePuff;
	public GameObject mainCamera;
	public GameObject ltmManager;//only used for drop function
	public AudioClip returnSound;
	//^
	//large trash is aware f the current room it is in. This room is given by roomManger.currentoom at start and when large trash
	//is dropped. If myCurrentRoom = RoomManager.currentRoom, deactivate self. (In update method?) check if the roomManager.room =
	//current room and if so, activate the large trash
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
		garbage.type = LargeGarbage.ByIndex(myPositionInList);


	}// end of Start()
	
	public override void PickUpEvent(){
		gameObject.tag = "ActiveLargeTrash";
	}

	public override void DropEvent(){
		gameObject.tag = "LargeTrash";
		gameObject.transform.parent = ltmManager.transform; //goes back to largeTrashManager when drop

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
		Debug.Log("Return activated - LARGE TRASH");
		phase = 0;
		GlobalVariableManager.Instance.CARRYING_SOMETHING = false;

		SoundManager.instance.PlaySingle(returnSound);
        // Add this trash item to the large trash list.
        var largeTrashItem = new GlobalVariableManager.LargeTrashItem(garbage.type);
        largeTrashItem.spriteIndex = myPositionInList;
        largeTrashItem.collectedDisplaySprite = collectedDisplaySprite;
        GlobalVariableManager.Instance.LARGE_TRASH_LIST.Add(largeTrashItem);

		if(GlobalVariableManager.Instance.MASTER_SFX_VOL > 0){
			//play trash pickup on channel 2
		}

		gameObject.transform.parent = null;
		myBody.AddForce(new Vector2(0,10),ForceMode2D.Impulse);
		myBody.gravityScale = 2;
		ObjectPool.Instance.GetPooledObject("effect_pickUpSmoke",gameObject.transform.position);
		returning = true;
		player.GetComponent<MeleeAttack>().enabled = true;
		dumpster.GetComponent<SE_GlowWhenClose>().enabled = true;
		ReturnArc();
		
	}// end of Return()


}
