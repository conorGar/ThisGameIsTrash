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
    public LargeGarbage garbage;
    public Sprite collectedDisplaySprite;


	//public GameObject largeShadow;
	//public GameObject cloudEffect;
	public GameObject sparkle;
	public GameObject smokePuff;
	public GameObject mainCamera;
	public Room myCurrentRoom;//for now only needed for tutorial popup proper function.
	public GameObject myLTMManager; //large trash manager subdivision(w1,w2,etc.)

	//^
	//large trash is aware f the current room it is in. This room is given by roomManger.currentoom at start and when large trash
	//is dropped. If myCurrentRoom = RoomManager.currentRoom, deactivate self. (In update method?) check if the roomManager.room =
	//current room and if so, activate the large trash


	int phase = 0;

	int doOnce = 0;

	//float xSpeedWhenCollected;
	f//loat pickUpYSpeed;
	//int currentRoomNumber;

	bool returning = false;


	//look up - find object in view


	// Use this for initialization
	void Start () {
		
		player = GameObject.FindGameObjectWithTag("Player");
		//playerAni = player.GetComponent<tk2dSpriteAnimator>();
		currentRoomNumber = GlobalVariableManager.Instance.ROOM_NUM;

		if(GlobalVariableManager.Instance.ROOM_NUM == 101){
			MyCollectionSetUp();
		}else{


					//myY = gameObject.transform.position.y;
					phase = 0;
					if(currentRoomNumber != 101){
							if(GlobalVariableManager.Instance.MASTER_SFX_VOL >0 && gameObject.active){
									//loop large trash sparkle on channel 11
								}
							Debug.Log("GOT HERE LARGE TRASH");
							//mySparkles = Instantiate(sparkle,transform.position,Quaternion.identity);
							//m//yCollision = Instantiate(collisionBox,transform.position, Quaternion.identity);



						//myShadow = Instantiate(largeShadow,new Vector2(transform.position.x - .1f,transform.position.y -.8f),Quaternion.identity);

							//ShowNow();


						}
					
			
		}


		if(RoomManager.Instance.currentRoom == myCurrentRoom && (GlobalVariableManager.Instance.TUT_POPUPS_SHOWN & GlobalVariableManager.TUTORIALPOPUPS.LARGETRASH ) != GlobalVariableManager.TUTORIALPOPUPS.LARGETRASH){
				ActivateTutorial();//TODO:Test properly
		}


	}// end of Start()
	
	// Update is called once per frame
	void Update () {
		/*if(ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT))
        {
			if(phase !=3){
				if(returning != true && GlobalVariableManager.Instance.WORLD_ROOM_DISCOVER.Count < 6) // world room discover part for no pickup during coon. Still needs pause check***
					PickUp();
			}else{
				Drop();
			}
		}*/

		if(phase ==3){
			gameObject.transform.position = player.transform.position;

		}
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

	void ActivateTutorial(){
		Debug.Log("Large Trash tutorial activated xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
		mainCamera.GetComponent<Ev_MainCameraEffects>().CameraPan(gameObject.transform.position,"tutorial");

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

	public void DropEvent(){
		gameObject.transform.parent = myLTMManager.gameObject.transform; //goes back to largeTrashManager when drop
	}

	void ShowNow(){

			xSpeedWhenCollected = 30f;


	}//end of ShowNow()

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

	/*void PickUp(){
		if(GlobalVariableManager.Instance.PLAYER_CAN_MOVE){
			//player can move check otherwise could pick up at dialog instances
			if(currentRoomNumber != 101){
				if(phase == 0 && gameObject.GetComponent<SpriteRenderer>().Equals("hidden") ==  false){
				GameObject player = GameObject.FindGameObjectWithTag("Player");
					if(Mathf.Abs(transform.position.x - player.transform.position.x) < 3f &&Mathf.Abs(transform.position.y - player.transform.position.y) < 3f){
						if(pickingUp == false){
							player.GetComponent<MeleeAttack>().SetCanAttack(false);
							if(GlobalVariableManager.Instance.MASTER_SFX_VOL > 0){
								//play large trash pickup on channel 4
							}
							QuickPickup();
							Debug.Log("LAGRE TRASH-  quick pickup activated");
						}
					}

				}//end of phase =0 check
			}
		}
	}//end of pickup();*/

	/*void QuickPickup(){
		pickUpYSpeed = 30f;
		pickingUp = true;
		//GameObject player = GameObject.FindGameObjectWithTag("Player");
		if(myCollision != null){
			Destroy(myCollision);
		}
		if(myShadow != null){
			Destroy(myShadow);
		}
		if(mySparkles !=null){
			Destroy(mySparkles);
		}
		gameObject.tag = "ActiveLargeTrash";
		gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Layer04";
		Debug.Log("Picking up y speed:" + pickUpYSpeed);
		InvokeRepeating("PickUpArc", 0.1f,0.1f);
		
	}//End of QuickPickup()*/

	void PickUpArc(){
		
		 if(returning){
						
			GameObject deathSmoke;
			deathSmoke = Instantiate(smokePuff,transform.position, Quaternion.identity);
			player.GetComponent<SE_GlowWhenClose>().enabled = true;
			Destroy(gameObject); //removes from large trash holder
		}
			
	}
	public void Return(){
		//activated by dumpster's 'SE_GlowWhenClose'
		Debug.Log("Return activated - LARGE TRASH");
		phase = 0;
		GlobalVariableManager.Instance.CARRYING_SOMETHING = false;

        // Add this trash item to the large trash list.
        var largeTrashItem = new GlobalVariableManager.LargeTrashItem(garbage.type);
        largeTrashItem.spriteIndex = myPositionInList;
        largeTrashItem.collectedDisplaySprite = collectedDisplaySprite;
        GlobalVariableManager.Instance.LARGE_TRASH_LIST.Add(largeTrashItem);

		if(GlobalVariableManager.Instance.MASTER_SFX_VOL > 0){
			//play trash pickup on channel 2
		}

		pickUpYSpeed = 40f; //just use pickUp YSpeed for return arch movement
		returning = true;
		player = GameObject.Find("Dumpster");
		InvokeRepeating("PickUpArc", 0.1f,0.1f);
		
	}// end of Return()


}
