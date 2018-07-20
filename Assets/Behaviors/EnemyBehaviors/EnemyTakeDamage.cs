using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTakeDamage : MonoBehaviour {

	//4/26/18 - still need to add drawing stuff, but looked as though maybe that should be its own script

	public bool armoredEnemy = false;
	public bool globalEnemy = false;
	public bool moveWhenHit = true;
	public bool secretHider = false;
	public bool spawnShadow = true;
	public int currentHp;
	public int meleeDmgBonus = 0;
	public int myPositionInList = 0;
	public int myPosInBasicEnemyStr = 0; // should be set by 'populateSelf'
	public tk2dSpriteAnimationClip invincibleAni = null;
	public tk2dSpriteAnimationClip aniToSwitchBackTo = null;
	public tk2dCamera currentCamera; //set in inspector
	public bool doesNotArcWhenHit = false;
	public bool respawnEnemy = false;
	public AudioClip hitSqueal;
	//public AudioClip bounce;
	public AudioSource audioSource;
	//public GameObject deathShadow;
	//public GameObject hitStar;
	//public GameObject smokePuff;
	public GameObject objectPool;

	public GameObject gar_trash;
	public GameObject gar_rec;
	public GameObject gar_comp;
	public GameObject timeDrop;
	public GameObject healthDrop;
	public GameObject pinDrop;
	public GameObject scrapDrop;
	public GameObject hitStarPS;
	//public GameObject landingDustParticle;

	//GameObject deathShadowInstance;
	Ev_MainCamera currentCam;
	//private bool hitPushBack = false;
	//private float bounceCounter = 30f;
	float swingDirectionSide; // uses scale to see if swinging left or right
	//private int numberOfBounces = 0;
	int changeAliveOrDeadCharValueAtPos = 0;
	bool piercingPin = false;
	int maxHp; //just used for Vitality Vision pin
	bool showHealth = false;
	int sharingUpgrade = 0;
	int damageOnce = 0;
	//private float landY;
	private float ySpeed = 0;//gameObject.GetComponent<Rigidbody2D>().velocity.y;
	private float xSpeed = 0f;
	tk2dSpriteAnimator myAnim;
	int camShake = 0;
	string meleeSwingDirection;
	int dropsPin = 0;
	int dropsTrash = 0;
	int scrapDropped = 0;
	int roomNum;
	//Color lerpColor = Color.white;
	bool takingDamage = false;
	GameObject player;

	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		roomNum = GlobalVariableManager.Instance.ROOM_NUM;
		currentCam = GameObject.Find("tk2dCamera").GetComponent<Ev_MainCamera>();
		myAnim = this.gameObject.GetComponent<tk2dSpriteAnimator>();
		if(changeAliveOrDeadCharValueAtPos != 0){
			if(GlobalVariableManager.Instance.WORLD_NUM == 3){
			//onsen towel oni
				if(GlobalVariableManager.Instance.GLOBAL_ENEMY_HP[18].Substring(changeAliveOrDeadCharValueAtPos,changeAliveOrDeadCharValueAtPos + 1) == "o"){
					Destroy(gameObject);
				}
			}else if(GlobalVariableManager.Instance.WORLD_NUM == 2){
			//w2 pelican guards
				if(GlobalVariableManager.Instance.GLOBAL_ENEMY_HP[19].Substring(changeAliveOrDeadCharValueAtPos,changeAliveOrDeadCharValueAtPos + 1) == "o"){
					Destroy(gameObject);
				}
			}
		}
		//----------------Pins----------------//
		if(GlobalVariableManager.Instance.pinsEquipped[31] == 1) //Piercing Pin
			piercingPin = true;
		if(GlobalVariableManager.Instance.pinsEquipped[28] == 1){ //'Vitality Vision' pin
			maxHp = currentHp;
			showHealth = true;
		}
		if(GlobalVariableManager.Instance.pinsEquipped[4] == 5){//Cursed Pin- toughness change
			currentHp--;
		} else if(GlobalVariableManager.Instance.pinsEquipped[4] == 4)
			currentHp++;

		if(GlobalVariableManager.Instance.pinsEquipped[29] == GlobalVariableManager.Instance.ROOM_NUM &&GlobalVariableManager.Instance.ROOM_NUM != 0){
			GameObject waifu =  Instantiate(GameObject.Find("upgradeActor_japWoman"), transform.position, Quaternion.identity) as GameObject;
			Destroy(gameObject);
		}
		if(GlobalVariableManager.Instance.pinsEquipped[9] == 1){//Sharing Pin
			sharingUpgrade = 2;
		}
		//----------------------------//

		if(armoredEnemy && GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED.Count != 4){
			moveWhenHit = false;
		}

		if(globalEnemy){
			int i = int.Parse(GlobalVariableManager.Instance.GLOBAL_ENEMY_HP[myPositionInList]);
				if(i == 0|| i > 49)
					Destroy(gameObject);
		}/*else if(GlobalVariableManager.Instance.WORLD_ENEMY_LIST[GlobalVariableManager.Instance.ROOM_NUM].Substring(myPosInBasicEnemyStr,myPosInBasicEnemyStr +1 ).CompareTo("o")){
			if(secretHider){
				GameObject hole =  Instantiate(GameObject.Find("hiddenHole"), transform.position, Quaternion.identity) as GameObject;
				Destroy(gameObject);
			}
		}*/
	}
	

	void Update () {
		//for when camera shake is activated
		if(camShake >0){
			currentCamera.transform.localPosition = Random.insideUnitSphere * 0.7f;
		}

		if(takingDamage && !doesNotArcWhenHit){

		


			//Debug.Log("Arc movement active");
			//Debug.Log("Number of bounces:" + numberOfBounces);
			//Debug.Log("Yspeed" + ySpeed);
			//Debug.Log("BOUNCE COUNTER: " + bounceCounter);
			//if(numberOfBounces != 0){
				//gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(xSpeed, bounceCounter);
				//Debug.Log("********Applying force here 2*******");
				//gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-7f,31f), ForceMode2D.Impulse);

				//}
			//else{
				//if(currentHp > 0){
				///	ySpeed = ySpeed + 2;
					//gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(xSpeed, ySpeed);
				//}else{
					//myAnim.Play("hit");
					//gameObject.transform.Rotate(Vector3.right*Time.deltaTime);
				//}

			//}
			//if(-30 < bounceCounter){
			//bounceCounter = bounceCounter - 1;
			//}


		/*if(landY > gameObject.transform.position.y && currentHp > 0){
			if(numberOfBounces != 2){
				Debug.Log("********Applying force here*******");

				gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-2f,41f), ForceMode2D.Impulse);

				//gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(xSpeed, 32f);
				bounceCounter = 30f;
				numberOfBounces++;
				//Debug.Log("Number of bounces:" + numberOfBounces);
				//Instantiate(landingDustParticle, transform.position,Quaternion.identity);
				arcMovement = false;
				if(currentHp > 0){
					StartCoroutine("ArcStop");
				}
			}
			/*else{
				gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
					if(currentHp > 0){
					StartCoroutine("ArcStop");
					}
				arcMovement = false;
			}*/
			/*if(GlobalVariableManager.Instance.MASTER_SFX_VOL > 0 && audioSource != null){
				audioSource.pitch = Random.Range(1,3);
				AudioSource.PlayClipAtPoint(bounce, transform.position);
			}
			/*if(deathShadowInstance){
				GameObject landingSmoke;
				landingSmoke = Instantiate(GameObject.Find("smoke_land"),transform.position, Quaternion.identity);
			}*/
		//}
			
		}//end of arch movement

		/*if(deathShadowInstance) // shadow follows enemy flying back on x axis
				deathShadowInstance.transform.position = new Vector2(gameObject.transform.position.x,deathShadowInstance.transform.position.y );
				*/

		

	}
	void OnTriggerEnter2D(Collider2D melee){
		Debug.Log("Collision with weapon");
		if(melee.tag == "Weapon")
			TakeDamage(melee.gameObject);
	}

	void TakeDamage(GameObject melee){
		Debug.Log("--------TAKE DAMAGE ACTIVATE ----------");
		Debug.Log(damageOnce);
			if(damageOnce == 0 && myAnim.CurrentClip!= invincibleAni &&( armoredEnemy != true || (armoredEnemy && GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED.Count == 4)|| piercingPin)){
				if(!takingDamage){
					takingDamage = true;
					damageOnce = 1;
					meleeDmgBonus = 0;
					if(GlobalVariableManager.Instance.characterUpgradeArray[1].Substring(3,4).CompareTo("o") == 0 && GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[1] > 6 && GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[1] <= 12){
						//3rd aggressive/warrior perk:second level melee weapon does 2 dmg
						meleeDmgBonus++;
					}
					if(GlobalVariableManager.Instance.characterUpgradeArray[1].Substring(9,10).CompareTo("o") == 0){
						//5th agressive perk: increase dmg by 1
						meleeDmgBonus++;
					}
					if(GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[1] > 12){
						//bonus dmg with pole
						meleeDmgBonus++;
					}
					if(GlobalVariableManager.Instance.pinsEquipped[21] == 1 && GlobalVariableManager.Instance.CURRENT_HP == 1){
						//STAY BACK pin
						meleeDmgBonus++;
					}
					string dmgBonus = GlobalVariableManager.Instance.characterUpgradeArray[5][0].ToString();
					Debug.Log(dmgBonus);
					meleeDmgBonus = meleeDmgBonus + int.Parse(dmgBonus);

					meleeSwingDirection = melee.GetComponent<tk2dSpriteAnimator>().CurrentClip.name;
					swingDirectionSide = melee.transform.localScale.x;
					Debug.Log("MELEE SWING DIRECTION: " + meleeSwingDirection);

					if(secretHider)
						Destroy(melee.gameObject);

					if(GlobalVariableManager.Instance.MASTER_SFX_VOL >0){
						//play hit squeal
					}



					this.gameObject.GetComponent<tk2dSprite>().color = Color.red;
					GameObject damageCounter = objectPool.GetComponent<ObjectPool>().GetPooledObject("HitStars");
                    damageCounter.SetActive(true);
					damageCounter.transform.position = new Vector3((transform.position.x + 2), transform.position.y - 1, transform.position.z);
					if(gameObject.transform.position.x < player.transform.position.x){
						hitStarPS.SetActive(true);
						hitStarPS.transform.localScale = new Vector3(1f,1f,1f);//makes stars burst in right direction

						damageCounter.GetComponent<Rigidbody2D>().AddForce(new Vector2(4f,10f), ForceMode2D.Impulse);
					}else{
						hitStarPS.SetActive(true);
						hitStarPS.transform.localScale = new Vector3(-1f,1f,1f);//makes stars burst in right direction
						damageCounter.GetComponent<Rigidbody2D>().AddForce(new Vector2(-4f,10f), ForceMode2D.Impulse);

					}

					Debug.Log("current cam shake activate here ******* VVVV");
					currentCam.StartCoroutine("ScreenShake",.2f);

					if(!moveWhenHit){
						GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
					}
					if(!globalEnemy){
						currentHp = currentHp - 1 - meleeDmgBonus;
					}else{
					GlobalVariableManager.Instance.GLOBAL_ENEMY_HP[myPositionInList] = GlobalVariableManager.Instance.GLOBAL_ENEMY_HP[myPositionInList].Replace(GlobalVariableManager.Instance.GLOBAL_ENEMY_HP[myPositionInList],(int.Parse(GlobalVariableManager.Instance.GLOBAL_ENEMY_HP[myPositionInList]) -1 -meleeDmgBonus).ToString());
						//GlobalVariableManager.Instance.GLOBAL_ENEMY_HP = GlobalVariableManager.Instance.GLOBAL_ENEMY_HP[myPositionInList].Remove
					}

					myAnim.Play("hit");
					if(gameObject.transform.position.x < player.transform.position.x)
						gameObject.transform.localScale = new Vector2(-1f,1f);
					else
						gameObject.transform.localScale = new Vector2(1f,1f);

					if(spawnShadow && moveWhenHit){
						
						//deathShadowInstance = Instantiate(deathShadow,new Vector2(transform.position.x, (transform.position.y - transform.lossyScale.y)),Quaternion.identity);
					}
					camShake = 1;
					StartCoroutine("ContinueHit"); // just needed to seperate here for IEnumerator stuff
				}
			}

	}
	IEnumerator StopKnockback(){
		
		yield return new WaitForSeconds(.3f);
		Debug.Log("STOP KNOCKBACK ACTIVATE");
		damageOnce = 0;
		gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
		gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
		if(aniToSwitchBackTo != null)
				myAnim.Play("idleR");
			else
				myAnim.Play("IdleR");
		takingDamage = false;

	}

	IEnumerator ContinueHit(){
		yield return new WaitForSeconds(.1f);
		//remove effects from self
		this.gameObject.GetComponent<tk2dSprite>().color = Color.white;

		Debug.Log("**Continue Hit activation***");
		if(moveWhenHit){
			takingDamage = true;
			if(meleeSwingDirection.CompareTo("plankSwing") == 0||meleeSwingDirection.CompareTo("clawR") == 0||meleeSwingDirection.CompareTo("poleR") == 0){
				if(swingDirectionSide < 0){
						gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-17f,0f), ForceMode2D.Impulse);
				}else{
						gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(17f,0f), ForceMode2D.Impulse);	
				}
			}else if(meleeSwingDirection.CompareTo("stickUp") == 0||meleeSwingDirection.CompareTo("clawUp") == 0||meleeSwingDirection.CompareTo("poleUp") == 0){
				gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f,-17f), ForceMode2D.Impulse);
			}else if(meleeSwingDirection.CompareTo("stickDown") == 0||meleeSwingDirection.CompareTo("clawDown") == 0||meleeSwingDirection.CompareTo("poleDown") == 0){
				gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f,17f), ForceMode2D.Impulse);
			}
			Debug.Log("**GOT THIS FAR***");

				yield return new WaitForSeconds(.2f);
				gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
				Debug.Log("**AND HERE!!!!!!!!***");
				yield return new WaitForSeconds(.4f);
				StartCoroutine( "StopKnockback");

		} // end of movement functions
		if(!globalEnemy){
			if(currentHp >0){
				if(gameObject.GetComponent<FollowPlayer>() != null){
					gameObject.GetComponent<FollowPlayer>().StopSound();
					gameObject.GetComponent<FollowPlayer>().enabled = false;
				}
				//disable follow player after notice behavior if end up having thta in game
				if(gameObject.GetComponent<RandomDirectionMovement>().enabled){
					gameObject.GetComponent<RandomDirectionMovement>().enabled = false;
				}

				//****SFX PLAY - 'hit2' on ch4
				yield return new WaitForSeconds(.5f);
				//***grow/shrink scale back to normal on all fronts

				if(gameObject.GetComponent<FollowPlayer>()){
					gameObject.GetComponent<FollowPlayer>().enabled = true;
					//***enable 'follow target after notice' here(ALSO TRIGGER 'notice' method in that script
				}else if(gameObject.GetComponent<RandomDirectionMovement>()){
					gameObject.GetComponent<RandomDirectionMovement>().enabled = true;
				}
				damageOnce = 0;
			
			}else{ //if hp is NOT > 0
				if(GlobalVariableManager.Instance.MASTER_SFX_VOL > 0){
				//**SFX PLAY- 'hit_final'
				}

				if(gameObject.GetComponent<RandomDirectionMovement>().enabled){
					gameObject.GetComponent<RandomDirectionMovement>().enabled = false;
				}

				if(GlobalVariableManager.Instance.ROOM_NUM != 201){
					if(GlobalVariableManager.Instance.characterUpgradeArray[1].Substring(10,11).CompareTo("o") == 0){
						//pin perk 1 - enemies have chance to drop pin
						dropsPin = Random.Range(1,30);
					}
					dropsTrash = 99;
					if(dropsPin !=22){
						dropsTrash = Random.Range(1, (12-sharingUpgrade));
					}
				}
				gameObject.transform.Rotate(Vector3.right*Time.deltaTime);
				GlobalVariableManager.Instance.ENEMIES_DEFEATED++;

				string thisRoomsEnemyList = GlobalVariableManager.Instance.WORLD_ENEMY_LIST[roomNum];
				if(!respawnEnemy){
					//GlobalVariableManager.Instance.WORLD_ENEMY_LIST[roomNum].Substring(myPosInBasicEnemyStr,myPosInBasicEnemyStr+1) = "o";
					//GlobalVariableManager.Instance.WORLD_ENEMY_LIST[roomNum].Remove(myPosInBasicEnemyStr);
					//GlobalVariableManager.Instance.WORLD_ENEMY_LIST[roomNum].Insert(myPosInBasicEnemyStr, "o")
					GlobalVariableManager.Instance.WORLD_ENEMY_LIST[roomNum] = GlobalVariableManager.Instance.WORLD_ENEMY_LIST[roomNum].Replace(thisRoomsEnemyList[myPosInBasicEnemyStr], 'o');
				}
				if(secretHider){
					//**SFX PLAY 'secret discovered'
					GameObject hole =  Instantiate(GameObject.Find("hiddenHole"), transform.position, Quaternion.identity) as GameObject;
				}else{
					DropThings();
				}


				yield return new WaitForSeconds(.4f);

				DropScrap();
			

				GameObject deathSmoke;
				//deathSmoke = Instantiate(smokePuff,transform.position, Quaternion.identity);

				if(changeAliveOrDeadCharValueAtPos != 0){
					if(GlobalVariableManager.Instance.WORLD_NUM == 3){
						//Onsen Towel Oni
						GlobalVariableManager.Instance.GLOBAL_ENEMY_HP[18] = GlobalVariableManager.Instance.GLOBAL_ENEMY_HP[18].Replace(GlobalVariableManager.Instance.GLOBAL_ENEMY_HP[18][changeAliveOrDeadCharValueAtPos],'o');
					}else if(GlobalVariableManager.Instance.WORLD_NUM == 2){
						//w2 Pelican Guards
						GlobalVariableManager.Instance.GLOBAL_ENEMY_HP[19] = GlobalVariableManager.Instance.GLOBAL_ENEMY_HP[19].Replace(GlobalVariableManager.Instance.GLOBAL_ENEMY_HP[19][changeAliveOrDeadCharValueAtPos],'o');
					}else if(GlobalVariableManager.Instance.WORLD_NUM == 5){
						//GameObject.Find("sceneManager").GetComponent<sEv_kakedaRoom>().kill();
					}
				}

				gameObject.GetComponent<Rigidbody2D>().MoveRotation(0f);
				Destroy(gameObject);

			}//end of (if hp is not > 0)
		}else{ //end of(if! globalEnemy)
			if(int.Parse(GlobalVariableManager.Instance.GLOBAL_ENEMY_HP[myPositionInList]) > 0){
				if(gameObject.GetComponent<FollowPlayer>().enabled){
					//gameObject.GetComponent<FollowPlayer>().stopSound();
					gameObject.GetComponent<FollowPlayer>().enabled = false;
				}
				//**SFX PLAY - HIT2 ch 4
				gameObject.GetComponent<Renderer>().sortingLayerName = "front";
				yield return new WaitForSeconds(1.4f);
				if(moveWhenHit){
					gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
				}
				if(gameObject.GetComponent<FollowPlayer>()){
					gameObject.GetComponent<FollowPlayer>().enabled = true;
				}
			}else{ //if hp = 0/dead
				GlobalVariableManager.Instance.ENEMIES_DEFEATED++;
				GlobalVariableManager.Instance.MY_NUM_IN_ROOM = 3;
				DropThings();
				DropScrap();

				if(GlobalVariableManager.Instance.WORLD_NUM == 3){
				//maskedOni
					GameObject droppedMask;
					droppedMask = Instantiate(GameObject.Find("drop_enemyMask"), transform.position, Quaternion.identity);
				}else if(GlobalVariableManager.Instance.WORLD_NUM == 4){
					//Quiet Mound (west rooms)
					//GameObject.Find("sceneManager").GetComponent<sEv_quietMoundRooms>().kill();
				}
				GameObject deathSmoke;
				//deathSmoke = Instantiate(smokePuff,transform.position, Quaternion.identity);
				if(GlobalVariableManager.Instance.MASTER_SFX_VOL >0){
					if(hitSqueal != null){
						//play hitSqueal at ch 5
					}
				}
				//if(deathShadowInstance)
				//	Destroy(deathShadowInstance);
				Destroy(gameObject);
		}

			yield return new WaitForSeconds(.2f);
		if(currentHp > 0 || (globalEnemy && int.Parse(GlobalVariableManager.Instance.GLOBAL_ENEMY_HP[myPositionInList]) > 1)){
			StartCoroutine( "StopKnockback");
			//if(deathShadowInstance){
			//	Destroy(deathShadowInstance);
			//}
			//if(!doesNotArcWhenHit && arcMovement)
				//gameObject.transform.position = new Vector2(transform.position.x,landY);
			//hitPushBack = -30;
			//Debug.Log("REACHED THIS PART");
			damageOnce = 0;
		}

		}

	}


	void DropThings(){
		GameObject droppedTrash;
		if(dropsPin !=22){
			if(dropsTrash <=2){
					GlobalVariableManager.Instance.MY_NUM_IN_ROOM = 3;
					if(GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED.Count == 3)
							droppedTrash = Instantiate(gar_trash, transform.position, Quaternion.identity);
					else if(GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED.Count == 4)
							droppedTrash = Instantiate(gar_rec, transform.position, Quaternion.identity);
					else if(GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED.Count == 5)
								droppedTrash = Instantiate(gar_comp, transform.position, Quaternion.identity);
					}else if(dropsTrash == 3 && timeDrop != null){
							droppedTrash = Instantiate(timeDrop, transform.position, Quaternion.identity);
					}else if(dropsTrash >= 4 && dropsTrash <= (4+sharingUpgrade)&& healthDrop != null){
							droppedTrash = Instantiate(healthDrop, transform.position, Quaternion.identity);

					}
		}else if(pinDrop != null){
			droppedTrash = Instantiate(pinDrop, transform.position, Quaternion.identity);
		}
		if(GlobalVariableManager.Instance.WORLD_NUM ==2){
		//DJKK/Lil' Krill 2nd missions
			if(gameObject.name.CompareTo("enemy_w2_moleCrab") == 0){
				if(GlobalVariableManager.Instance.FRIEND_LIST[8].Substring(3,4).CompareTo("j") == 0 &&GlobalVariableManager.Instance.FRIEND_LIST[8].Substring(6,7).CompareTo("f") == 0){
						GlobalVariableManager.Instance.FRIEND_LIST[8] = GlobalVariableManager.Instance.FRIEND_LIST[8].Replace(GlobalVariableManager.Instance.FRIEND_LIST[8][0],(char)(int.Parse(GlobalVariableManager.Instance.FRIEND_LIST[8].Substring(0,1)) +1));
				}
			}else if(gameObject.name.CompareTo("hermitCrab") == 0){
				if(GlobalVariableManager.Instance.FRIEND_LIST[8].Substring(3,4).CompareTo("k") == 0 && GlobalVariableManager.Instance.FRIEND_LIST[9].Substring(5,6).CompareTo("g") == 0){
					GlobalVariableManager.Instance.FRIEND_LIST[9] = GlobalVariableManager.Instance.FRIEND_LIST[9].Replace(GlobalVariableManager.Instance.FRIEND_LIST[9][0],(char)(int.Parse(GlobalVariableManager.Instance.FRIEND_LIST[9].Substring(0,1)) +1));
				}
			}else if(gameObject.name.CompareTo("boss_Dudley") == 0){
							//GameObject.FindGameObjectWithTag("boss").GetComponent<Ev_boss_dudleyV2>().minionDied();
			}else if(roomNum == 48 ||roomNum == 46 || roomNum == 44 || roomNum == 58){
				//blocked whale rooms/ (58) one blocked dock room
				//GameObject.Find("sceneManager").GetComponent<sEv_blockedRooms>().kill();
			}

		}
	}

	void DropScrap(){
		if(GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[1] < (13 + (GlobalVariableManager.Instance.pinsEquipped[20]*5))){
		// get #20*5 thing is for 'Waste Warrior' adjustment
			if(GlobalVariableManager.Instance.pinsEquipped[22] != 1){
				//^ Scrap City - more scrap dropped
				scrapDropped = Random.Range(1,3);
			}else{
				scrapDropped = Random.Range(2,5);
			}
		for(int i = 0; i < scrapDropped; i++){
			GameObject droppedScrap;
			droppedScrap = Instantiate(scrapDrop,new Vector3((transform.position.x + Random.Range(0,gameObject.GetComponent<tk2dSprite>().GetBounds().size.x)),(transform.position.y + Random.Range(0,gameObject.GetComponent<tk2dSprite>().GetBounds().size.y)),transform.position.z), Quaternion.identity);
		}
		}
	}
}
