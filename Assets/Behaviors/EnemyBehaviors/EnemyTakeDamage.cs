using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTakeDamage : MonoBehaviour {

	//4/26/18 - still need to add drawing stuff, but looked as though maybe that should be its own script

	public bool armoredEnemy = false;
	//public bool globalEnemy = false;
	public bool moveWhenHit = true;
	public bool secretHider = false;
	//public bool spawnShadow = true;
	public int currentHp;
	public int meleeDmgBonus = 0;
	//public int myPositionInList = 0;
	//public int myPosInBasicEnemyStr = 0; // should be set by 'populateSelf'
	public tk2dSpriteAnimationClip invincibleAni = null;
	public tk2dSpriteAnimationClip aniToSwitchBackTo = null;
	public tk2dCamera currentCamera; //set in inspector
	//public bool doesNotArcWhenHit = false;
	public bool respawnEnemy = false;
	public AudioClip hitSound;
	public AudioClip hitSqueal;
	public bool bossEnemy;
	//public AudioClip bounce;
	//public AudioSource audioSource;
	//public GameObject deathShadow;
	//public GameObject hitStar;
	//public GameObject smokePuff;
	public GameObject objectPool;
	public GameObject tutPopup;

	public GameObject gar_trash;
	public GameObject gar_rec;
	public GameObject gar_comp;
	public GameObject timeDrop;
	public GameObject healthDrop;
	public GameObject pinDrop;
	//public GameObject scrapDrop;
	//public GameObject hitStarPS;
	//public GameObject landingDustParticle;

	//GameObject deathShadowInstance;
	Ev_MainCamera currentCam;
	//private bool hitPushBack = false;
	//private float bounceCounter = 30f;
	float swingDirectionSide; // uses scale to see if swinging left or right

	bool piercingPin = false;
	int maxHp; //just used for Vitality Vision pin
	int sharingUpgrade = 0;
	int damageOnce = 0;

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
	string mySpawnerID;



	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		roomNum = GlobalVariableManager.Instance.ROOM_NUM;
		currentCam = GameObject.Find("tk2dCamera").GetComponent<Ev_MainCamera>();
		myAnim = this.gameObject.GetComponent<tk2dSpriteAnimator>();


		//----------------Pins----------------//
		if(GlobalVariableManager.Instance.IsPinEquipped(PIN.PIERCINGPIN)) //Piercing Pin
			piercingPin = true;
		if(GlobalVariableManager.Instance.IsPinEquipped(PIN.VITALITYVISION)){ //'Vitality Vision' pin
			maxHp = currentHp;
		}
		if(GlobalVariableManager.Instance.IsPinEquipped(PIN.CURSED)){//Cursed Pin- toughness change
			currentHp--;
		} else if(GlobalVariableManager.Instance.IsPinEquipped(PIN.BLISTERINGBOND))
			currentHp++;

		/*if(GlobalVariableManager.Instance.pinsEquipped[29] == GlobalVariableManager.Instance.ROOM_NUM &&GlobalVariableManager.Instance.ROOM_NUM != 0){
			GameObject waifu =  Instantiate(GameObject.Find("upgradeActor_japWoman"), transform.position, Quaternion.identity) as GameObject;
			Destroy(gameObject);
		}*/
		if(GlobalVariableManager.Instance.IsPinEquipped(PIN.SHARING)){//Sharing Pin
			sharingUpgrade = 2;
		}
		//----------------------------//

		if(armoredEnemy && GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED.Count != 4){
			moveWhenHit = false;
		}

		if(armoredEnemy && (GlobalVariableManager.Instance.TUT_POPUPS_SHOWN & GlobalVariableManager.TUTORIALPOPUPS.ARMOREDENEMIES) != GlobalVariableManager.TUTORIALPOPUPS.ARMOREDENEMIES){
			tutPopup.SetActive(true);
			tutPopup.GetComponent<GUI_TutPopup>().SetData("ArmoredEnemies");
		}

		//if(globalEnemy){
			//int i = int.Parse(GlobalVariableManager.Instance.GLOBAL_ENEMY_HP[myPositionInList]);
				//if(i == 0|| i > 49)
				//	Destroy(gameObject);
		//}/*else if(GlobalVariableManager.Instance.WORLD_ENEMY_LIST[GlobalVariableManager.Instance.ROOM_NUM].Substring(myPosInBasicEnemyStr,myPosInBasicEnemyStr +1 ).CompareTo("o")){
			//if(secretHider){
				//GameObject hole =  Instantiate(GameObject.Find("hiddenHole"), transform.position, Quaternion.identity) as GameObject;
				//Destroy(gameObject);
			//}
		//}*/
	}
	

	void Update () {
		//for when camera shake is activated
		/*if(camShake >0){
			currentCamera.transform.localPosition = Random.insideUnitSphere * 0.7f;
		}*/ //took out because messy when hitting boss and didnt think it was needed because of camera's ScreenShake()

	}
	void OnTriggerEnter2D(Collider2D melee){
		Debug.Log("Collision with weapon");
		if(melee.tag == "Weapon"){
			TakeDamage(melee.gameObject);
			SoundManager.instance.PlaySingle(hitSound);
			SoundManager.instance.PlaySingle(hitSqueal);


		}
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

					//if(GlobalVariableManager.Instance.IsPinEquipped(PIN.STAYBACK) && GlobalVariableManager.Instance.CURRENT_HP == 1){
						//STAY BACK pin
						//meleeDmgBonus++;
					//}
					string dmgBonus = GlobalVariableManager.Instance.characterUpgradeArray[5][0].ToString();
					Debug.Log(dmgBonus);
					meleeDmgBonus = meleeDmgBonus + int.Parse(dmgBonus);

					meleeSwingDirection = melee.GetComponent<tk2dSpriteAnimator>().CurrentClip.name;
					swingDirectionSide = player.transform.localScale.x;
					Debug.Log("MELEE SWING DIRECTION: " + meleeSwingDirection);

					if(secretHider)
						Destroy(melee.gameObject);

					if(GlobalVariableManager.Instance.MASTER_SFX_VOL >0){
						//play hit squeal
					}



					this.gameObject.GetComponent<tk2dSprite>().color = Color.red;
					GameObject damageCounter = objectPool.GetComponent<ObjectPool>().GetPooledObject("HitStars");
					damageCounter.GetComponent<Ev_HitStars>().ShowProperDamage(1 + meleeDmgBonus);
					damageCounter.SetActive(true);
					GameObject littleStars = objectPool.GetComponent<ObjectPool>().GetPooledObject("effect_LittleStars");
					damageCounter.transform.position = new Vector3((transform.position.x), transform.position.y, transform.position.z);
					littleStars.transform.position = new Vector3((transform.position.x), transform.position.y, transform.position.z);
					littleStars.SetActive(true);

						if(gameObject.transform.position.x < player.transform.position.x){
							//hitStarPS.SetActive(true);
							//hitStarPS.transform.localScale = new Vector3(1f,1f,1f);//makes stars burst in right direction

							damageCounter.GetComponent<Rigidbody2D>().AddForce(new Vector2(4f,10f), ForceMode2D.Impulse);
						}else{
							//hitStarPS.SetActive(true);
							//hitStarPS.transform.localScale = new Vector3(-1f,1f,1f);//makes stars burst in right direction
							damageCounter.GetComponent<Rigidbody2D>().AddForce(new Vector2(-4f,10f), ForceMode2D.Impulse);

						}
					
					Debug.Log("GOT THIS FAR- ENEMY TAKE DAMGE ----- 1");
					currentCam.StartCoroutine("ScreenShake",.2f);
				
					if(!moveWhenHit){
						GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
					}

					currentHp = currentHp - 1 - meleeDmgBonus;
					Debug.Log("GOT THIS FAR- ENEMY TAKE DAMGE 2");
					if(bossEnemy){
						gameObject.GetComponent<Boss>().hpDisplay.GetComponent<GUI_BossHpDisplay>().UpdateBossHp(currentHp);
						//TODO: make sure all bosses hp global vars are updated properly at the day's end...
						//GlobalVariableManager.Instance.BOSS_HP_LIST[bossesListPosition] = currentHp;
					}

					

					myAnim.Play("hit");
					if(gameObject.transform.position.x < player.transform.position.x)
						gameObject.transform.localScale = new Vector2(-1f,1f);
					else
						gameObject.transform.localScale = new Vector2(1f,1f);

					//camShake = 1;
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
		//if(aniToSwitchBackTo != null)
		if(currentHp >0 && myAnim.GetClipByName("idle") != null)
			myAnim.Play("idle");
			//else
				//myAnim.Play("IdleR");
		takingDamage = false;

	}

	IEnumerator ContinueHit(){

		//remove effects from self


		Debug.Log("**Continue Hit activation***");
		if(moveWhenHit){
			takingDamage = true;
			if(meleeSwingDirection.CompareTo("plankSwing") == 0||meleeSwingDirection.CompareTo("clawR") == 0||meleeSwingDirection.CompareTo("poleR") == 0){
				Debug.Log(swingDirectionSide);
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

				yield return new WaitForSeconds(.2f);
				this.gameObject.GetComponent<tk2dSprite>().color = Color.white;
				gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
				Debug.Log("**AND HERE!!!!!!!!***");
				yield return new WaitForSeconds(.4f);
				StartCoroutine( "StopKnockback");

		}else{
			yield return new WaitForSeconds(.2f);
			this.gameObject.GetComponent<tk2dSprite>().color = Color.white;
			damageOnce = 0;
			takingDamage = false;
		} // end of movement functions

			if(currentHp >0){
				if(gameObject.GetComponent<FollowPlayer>() != null){
					gameObject.GetComponent<FollowPlayer>().StopSound();
					gameObject.GetComponent<FollowPlayer>().enabled = false;
				}
				//disable follow player after notice behavior if end up having thta in game
				if(gameObject.GetComponent<RandomDirectionMovement>() != null && gameObject.GetComponent<RandomDirectionMovement>().enabled){
					gameObject.GetComponent<RandomDirectionMovement>().enabled = false;
				}

				//****SFX PLAY - 'hit2' on ch4
				yield return new WaitForSeconds(.4f);
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

				if(gameObject.GetComponent<RandomDirectionMovement>() != null && gameObject.GetComponent<RandomDirectionMovement>().enabled){
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
				//gameObject.transform.Rotate(Vector3.right*Time.deltaTime);
				GlobalVariableManager.Instance.ENEMIES_DEFEATED++;

				//string thisRoomsEnemyList = GlobalVariableManager.Instance.WORLD_ENEMY_LIST[roomNum];
				//if(!respawnEnemy){
					//GlobalVariableManager.Instance.WORLD_ENEMY_LIST[roomNum].Substring(myPosInBasicEnemyStr,myPosInBasicEnemyStr+1) = "o";
					//GlobalVariableManager.Instance.WORLD_ENEMY_LIST[roomNum].Remove(myPosInBasicEnemyStr);
					//GlobalVariableManager.Instance.WORLD_ENEMY_LIST[roomNum].Insert(myPosInBasicEnemyStr, "o")
					//GlobalVariableManager.Instance.WORLD_ENEMY_LIST[roomNum] = GlobalVariableManager.Instance.WORLD_ENEMY_LIST[roomNum].Replace(thisRoomsEnemyList[myPosInBasicEnemyStr], 'o');
				//}
				if(secretHider){
					//**SFX PLAY 'secret discovered'
					GameObject hole =  Instantiate(GameObject.Find("hiddenHole"), transform.position, Quaternion.identity) as GameObject;
				}else{
					DropThings();
				}


				yield return new WaitForSeconds(.2f);

				DropScrap();
			
				if(!bossEnemy){
					Death();
				}else{
					gameObject.GetComponent<Boss>().BossDeath();
				}
			}//end of (if hp is not > 0)
		

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
        // 'Waste Warrior' pin ups the scrap cap by 5, due to the additional upgrade.
        int wasteWarriorAdjust = (GlobalVariableManager.Instance.IsPinEquipped(PIN.WASTEWARRIOR) ? 1 : 0) * 5;
        if (GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[1] < (13 + wasteWarriorAdjust)){
			if(!GlobalVariableManager.Instance.IsPinEquipped(PIN.SCRAPCITY)){
				//^ Scrap City - more scrap dropped
				scrapDropped = Random.Range(1,3);
			}else{
				scrapDropped = Random.Range(2,6);
				player.GetComponent<PinFunctionsManager>().StartCoroutine("DisplayEffectsHud",PinManager.Instance.GetPin(PIN.SCRAPCITY).sprite);
			}
			//Debug.Log("dropped scrap:" + scrapDropped);
		    for(int i = 0; i < scrapDropped; i++){
			    GameObject droppedScrap= objectPool.GetComponent<ObjectPool>().GetPooledObject("Scrap",gameObject.transform.position);
			    //droppedScrap = Instantiate(scrapDrop,new Vector3((transform.position.x + Random.Range(0,gameObject.GetComponent<tk2dSprite>().GetBounds().size.x)),(transform.position.y + Random.Range(0,gameObject.GetComponent<tk2dSprite>().GetBounds().size.y)),transform.position.z), Quaternion.identity);
		    }
        }
    }


	public void SetSpawnerID(string id){
		mySpawnerID = id;
	}

	void Death(){
		Debug.Log("Death Activate!!!");

		GameObject deathSmoke = objectPool.GetComponent<ObjectPool>().GetPooledObject("effect_SmokePuff");
		deathSmoke.transform.position = new Vector3((transform.position.x), transform.position.y, transform.position.z);

		GameObject deathGhost = objectPool.GetComponent<ObjectPool>().GetPooledObject("effect_DeathGhost");
		deathGhost.transform.position = new Vector3((transform.position.x), transform.position.y, transform.position.z);
		Debug.Log("My spawner ID: "+mySpawnerID);
		GlobalVariableManager.Instance.BASIC_ENEMY_LIST[this.mySpawnerID].dayOfRevival = GlobalVariableManager.Instance.DAY_NUMBER +3;
		Debug.Log("Day of revival: "+ GlobalVariableManager.Instance.BASIC_ENEMY_LIST[this.mySpawnerID].dayOfRevival);


		this.gameObject.SetActive(false);

	}
}
