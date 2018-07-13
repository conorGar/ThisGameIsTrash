using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ev_LargeTrash : MonoBehaviour {

	
	public int myPositionInList;
	public bool isRewardForBoss;
	//public int myRoomNumber;
	//public int position;
	//public int myWorld;
	//public string myString;
	public char myCharValue; //only used to set up string for use when showing today's collected large trash at day end
    public GlobalVariableManager.LARGETRASH trashType = GlobalVariableManager.LARGETRASH.NONE;

	public GameObject collisionBox;
	public GameObject largeShadow;
	public GameObject cloudEffect;
	public GameObject sparkle;
	public GameObject smokePuff;

	int phase = 0;
	int bounce = 0;
	int doOnce = 0;
	float myY;
	float xSpeedWhenCollected;
	float pickUpYSpeed;
	int currentRoomNumber;
	bool falling = false;
	bool stopBouncing = false;
	bool returning = false;
	bool pickingUp = false;
	Rigidbody2D myBody;

	//look up - find object in view


	GameObject myCollision;
	GameObject myShadow;
	GameObject player;
	GameObject mySparkles;
	tk2dSpriteAnimator playerAni;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		playerAni = player.GetComponent<tk2dSpriteAnimator>();
		myBody  = gameObject.GetComponent<Rigidbody2D>();
		currentRoomNumber = GlobalVariableManager.Instance.ROOM_NUM;

		if(GlobalVariableManager.Instance.ROOM_NUM == 101){
			MyCollectionSetUp();
		}else{
			Debug.Log(GlobalVariableManager.Instance.LARGE_TRASH_LOCATIONS.Count);
			if(myPositionInList == 2){ //for now Large Sofa sets up large trash start positions, otherwise populateworld
				GlobalVariableManager.Instance.LARGE_TRASH_LOCATIONS.Add(new Vector2(0f,0f));
				GlobalVariableManager.Instance.LARGE_TRASH_LOCATIONS.Add(new Vector2(26.1f,-26.1f));
				GlobalVariableManager.Instance.LARGE_TRASH_LOCATIONS.Add(new Vector2(-5.4f,1f));
				GlobalVariableManager.Instance.LARGE_TRASH_LOCATIONS.Add(new Vector2(2f,103f));
			}
			Debug.Log(GlobalVariableManager.Instance.LARGE_TRASH_LOCATIONS.Count);
			Debug.Log(GlobalVariableManager.Instance.LARGE_TRASH_LOCATIONS[0].x);
			Debug.Log(GlobalVariableManager.Instance.LARGE_TRASH_LOCATIONS[1].x);
			Debug.Log(GlobalVariableManager.Instance.LARGE_TRASH_LOCATIONS[2].x);
				if(GlobalVariableManager.Instance.LARGE_TRASH_LOCATIONS[myPositionInList + 1].x == 0){ //set to 0 at return
					//set to null when collected
					Debug.Log("Large Trash Destroyed at start");
					Destroy(gameObject);
				}else{
					//-------Check positions for this world ------------//
					transform.position = GlobalVariableManager.Instance.LARGE_TRASH_LOCATIONS[myPositionInList + 1];
					Debug.Log("My Large Trash x "+GlobalVariableManager.Instance.LARGE_TRASH_LOCATIONS[myPositionInList +1].x);
					//------------------------------------------------//
					myY = gameObject.transform.position.y;
					if(isRewardForBoss){
						if(GlobalVariableManager.Instance.MASTER_SFX_VOL > 0){
							//stop sound on chanel 7
					}
					if(currentRoomNumber != 101){
							GameObject myCollision;
							myCollision = Instantiate(collisionBox,transform.position, Quaternion.identity);
					}

					}else{ //if not reward for boss
						phase = 0;
						if(currentRoomNumber != 101){
							if(GlobalVariableManager.Instance.LARGE_TRASH_LOCATIONS[myPositionInList].x != 0){
								if(GlobalVariableManager.Instance.MASTER_SFX_VOL >0 && gameObject.active){
									//loop large trash sparkle on channel 11
								}
							Debug.Log("GOT HERE LARGE TRASH");
							mySparkles = Instantiate(sparkle,transform.position,Quaternion.identity);
							myCollision = Instantiate(collisionBox,transform.position, Quaternion.identity);

							}

						myShadow = Instantiate(largeShadow,new Vector2(transform.position.x - .1f,transform.position.y -.8f),Quaternion.identity);

							//ShowNow();


						}
					}
			}
		}


		// If in starting room check to see if it should be...
		/*if(currentRoomNumber == myRoomNumber){
			for(int i = 0; i< GlobalVariableManager.Instance.LARGE_TRASH_LOCATIONS.Count; i++){

			//for now didnt add because not sure if it will be needed anymore.. 6/2/18

			}
		}*/

		//gameObject.GetComponent<tk2dSprite>().SetSprite(myCharValue);
	}// end of Start()
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)){
			if(phase !=3){
				if(returning != true && GlobalVariableManager.Instance.WORLD_ROOM_DISCOVER.Count < 6) // world room discover part for no pickup during coon. Still needs pause check***
					PickUp();
			}else{
				Drop();
			}
		}

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

	void Sparkle(){

	}

	void Drop(){
		gameObject.transform.position = new Vector2(transform.position.x + 1f, transform.position.y -1f);
		GlobalVariableManager.Instance.LARGE_TRASH_LOCATIONS[myPositionInList+1] = transform.position;
		myShadow = Instantiate(largeShadow,new Vector2(transform.position.x - .1f,transform.position.y -.8f),Quaternion.identity);
		Debug.Log("DROPPED LARGE TRASH");
		myCollision = Instantiate(collisionBox,transform.position, Quaternion.identity);
		mySparkles = Instantiate(sparkle,transform.position,Quaternion.identity);
		phase = 0;
		gameObject.tag = "LargeTrash";
		gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Layer02";
	}

	void ShowNow(){

			xSpeedWhenCollected = 30f;


	}//end of ShowNow()

	IEnumerator Fall(){
		falling = true;

		gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,20f);
		yield return new WaitForSeconds(1.5f);
		gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,-20f);
		yield return new WaitForSeconds(1.8f);
		stopBouncing = true;
		falling = false;
		gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,0f);

	}

	void Bounce(){
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
	}//end of Bouce()

	void PickUp(){
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
	}//end of pickup();

	void QuickPickup(){
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
		
	}//End of QuickPickup()

	void PickUpArc(){
		

		if(pickUpYSpeed > -40){
				
				myBody.velocity = new Vector2((player.transform.position.x - gameObject.transform.position.x),pickUpYSpeed);
				pickUpYSpeed -= 7;
				//playerAni.Play("pickUp");
			}else{
				gameObject.transform.position = player.transform.position;
				/*if(GlobalVariableManager.Instance.LARGE_TRASH_LOCATIONS[0][0].Equals('o') != true){
					//removes self from the dropped trash list containing all the large trash that arent in their start rooms

				}*/
				if(pickingUp){
					GlobalVariableManager.Instance.CARRYING_SOMETHING = true;
					myBody.velocity = new Vector2(0f,0f);
					//playerAni.Play("hold_idleR");
					phase = 3;
					CancelInvoke();
					pickingUp = false;
					Debug.Log("LargeTrashPhaseNum:" + phase);
				}else if(returning){
						/*for(int i = 0; i < 4; i++){
							GameObject tempCloud;
							tempCloud =Instantiate(cloudEffect,transform.position,Quaternion.identity);
							tempCloud.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-5,6),Random.Range(-5,6)),ForceMode2D.Impulse);
							GameObject tempSparkle;
							tempSparkle = Instantiate(sparkle,transform.position, Quaternion.identity);
							//tempSparkle.GetComponent<SpriteRenderer>().sprite = "upgradeSparkle";
						}*/		

						//player.GetComponent<SpecialEffectsBehavior>().StartCoroutine("Squish");
						GameObject deathSmoke;
						deathSmoke = Instantiate(smokePuff,transform.position, Quaternion.identity);
						player.GetComponent<SE_GlowWhenClose>().enabled = true;
						Destroy(gameObject);
				}
			}
	}
	public void Return(){
		//activated by dumpster's 'SE_GlowWhenClose'
		Debug.Log("Return activated - LARGE TRASH");
		phase = 0;
		GlobalVariableManager.Instance.CARRYING_SOMETHING = false;

        // Add this trash item to the large trash list.
        var largeTrashItem = new GlobalVariableManager.LargeTrashItem(trashType);
        GlobalVariableManager.Instance.LARGE_TRASH_LIST.Add(largeTrashItem);

		if(GlobalVariableManager.Instance.MASTER_SFX_VOL > 0){
			//play trash pickup on channel 2
		}
		myBody.velocity = new Vector2(-40f,0f);
		
		pickUpYSpeed = 40f; //just use pickUp YSpeed for return arch movement
		returning = true;
		GlobalVariableManager.Instance.LARGE_TRASH_LOCATIONS[myPositionInList].Set(0,0);
		player = GameObject.Find("Dumpster");
		InvokeRepeating("PickUpArc", 0.1f,0.1f);
		
	}// end of Return()


}
