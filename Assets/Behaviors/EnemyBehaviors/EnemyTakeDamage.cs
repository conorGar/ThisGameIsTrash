using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTakeDamage : MonoBehaviour {


	public bool armoredEnemy = false;
	public bool moveWhenHit = true;
    public bool resetFacingAfterHit = false;
	public bool secretHider = false;
	public bool returnToCurrentAniAfterHit = false;
	public tk2dSpriteAnimationClip invincibleAni = null;
	public tk2dSpriteAnimationClip aniToSwitchBackTo = null;
	//public tk2dCamera currentCamera; //set in inspector
	public string myDeadBodyName;
	public bool respawnEnemy = false;
	public AudioClip hitSound;
	public AudioClip hitSqueal;
	public AudioClip bounce;
	public AudioClip armoredEnemyHitSfx;
	public bool bossEnemy;
	public bool IAmParentObj;
	public GameObject childEnemy;

	public GameObject tutPopup;

	public GameObject gar_trash;
	public GameObject gar_rec;
	public GameObject gar_comp;
	public GameObject timeDrop;
	public GameObject healthDrop;
	public GameObject pinDrop;

	public GameObject myShadow;
	//public BoxCollider2D myCollisionBox;
	public string returnAniName = "idle";
	public bool dontSpawnBody;


	public List<MonoBehaviour> behaviorsToDeactivate = new List<MonoBehaviour>();



	//armor knockoff stuff
	public GameObject armorCollisionObj; //object, like a shield or something, to deactivate
	public tk2dSpriteAnimation disarmoredAnimation;


	[HideInInspector]
	public GameObject objectPool;
	[HideInInspector]
public EnemyRespawner myRespawner; //for use with respawning enemies
public int currentHp;
	[HideInInspector]
public int meleeDmgBonus = 0;
	[HideInInspector]
public bool dontStopWhenHit; //usually temporary and set by other behavior, such as 'LungeAtPlayer.cs'

	float swingDirectionSide; // uses scale to see if swinging left or right

	bool piercingPin = false;
	int maxHp; //just used for Vitality Vision pin
	int sharingUpgrade = 0;
	int damageOnce = 0;
	bool hitByThrownObject;

	tk2dSpriteAnimator myAnim;
	int camShake = 0;
	string meleeSwingDirection;
	int dropsPin = 0;
	int dropsTrash = 0;
	int scrapDropped = 0;
	int roomNum;
	//Color lerpColor = Color.white;
	protected bool takingDamage = false;
	GameObject player;
	string mySpawnerID;

	Vector2 shadowStartPos;
	Rigidbody2D myBody;
	bool spinning;
	float t;
	Quaternion startRotation;

	bool startsOffDontMoveWhenHit; // needed for returning proper value when enemy dies(otherwise spawned enemies after starting obj pool amount will always have 'movewhenhit' enabled)

	Vector2 startScale;

	void OnEnable(){
		
		roomNum = GlobalVariableManager.Instance.ROOM_NUM;
		if(!bossEnemy)
			currentHp = gameObject.GetComponent<Enemy>().health;//enemy health reset when enter room again
		if(myBody == null)
			myBody = gameObject.GetComponent<Rigidbody2D>();
		myBody.mass = 2;
		myBody.gravityScale = 0;
		takingDamage = false;
		damageOnce = 0;
		if(gameObject.GetComponent<RandomDirectionMovement>() != null)
			gameObject.GetComponent<RandomDirectionMovement>().enabled = true;

        // Reset scale and rotations if this enemy has already been initialized.
        if (myAnim != null) {
            transform.localScale = startScale;
            transform.rotation = startRotation;
        }
    }


	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		if(IAmParentObj){
			myAnim = childEnemy.GetComponent<tk2dSpriteAnimator>();
		}else{
			myAnim = this.gameObject.GetComponent<tk2dSpriteAnimator>();
		}
		myBody = gameObject.GetComponent<Rigidbody2D>();
		if(!moveWhenHit){
			startsOffDontMoveWhenHit = true;
		}
		startRotation = transform.rotation;
		if(myShadow !=null) //if has shadow
			shadowStartPos = myShadow.transform.localPosition;
		startScale = gameObject.transform.localScale;

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

		if(armoredEnemy && (GlobalVariableManager.Instance.TUT_POPUPS_SHOWN & GlobalVariableManager.TUTORIALPOPUPS.ARMOREDENEMIES) != GlobalVariableManager.TUTORIALPOPUPS.ARMOREDENEMIES && tutPopup != null){
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


		if(spinning){
			if (t<.5f){
				transform.rotation = startRotation * Quaternion.AngleAxis(t/.5f * 360f, Vector3.forward);
				t += Time.deltaTime;
				myShadow.transform.position = new Vector2(gameObject.transform.position.x,myShadow.transform.position.y);
			}else{
				spinning = false;
				t = 0f;
				myShadow.transform.parent = gameObject.transform;//shadow follows again.
				myShadow.transform.localPosition = shadowStartPos;
				myBody.velocity = Vector2.zero;
				transform.rotation = startRotation;

			}
		}


	}

	public void OnTriggerEnter2D(Collider2D melee){

		if(melee.tag == "Weapon"){
			
			TakeDamage(melee.gameObject);

			//Debug.Log("Collision with weapon: ");

		}else if(melee.tag == "pObj_bullet"){
			if(!takingDamage){
				StartCoroutine("NonMeleeHit");
				melee.GetComponent<Ev_FallingProjectile>().Fell();
			}
			//Debug.Log("Collision with nen melee weapon: >>>>>>>>>>> ");
			SoundManager.instance.RandomizeSfx(hitSound,.8f,1.1f);
			SoundManager.instance.PlaySingle(hitSqueal);
		}else if(melee.gameObject.layer == 15){//throwable object
			hitByThrownObject = true;
            // TODO: Get the boss battle to use throwable bodies???
            var body = melee.gameObject.GetComponent<ThrowableBody>();
            if (body)
                melee.gameObject.GetComponent<ThrowableBody>().StartCoroutine("Impact",this.gameObject);
			Debug.Log("Hit by thrown object!");
			ArmorKnockoff();
			TakeDamage(melee.gameObject);
			SoundManager.instance.PlaySingle(hitSound);
			SoundManager.instance.PlaySingle(hitSqueal);
		}
	}

	IEnumerator NonMeleeHit(){
		if(damageOnce == 0 && myAnim.CurrentClip!= invincibleAni &&( armoredEnemy != true || (armoredEnemy && GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED.Count == 4)|| piercingPin)){
			if(!takingDamage){
				takingDamage = true;
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
				if(!moveWhenHit && !dontStopWhenHit){
						GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
				}

				currentHp = currentHp - 1 - meleeDmgBonus;
				Debug.Log("GOT THIS FAR- ENEMY TAKE DAMGE 2");
					if(bossEnemy){
                    gameObject.GetComponent<Boss>().UpdateBossHp(currentHp);
						//TODO: make sure all bosses hp global vars are updated properly at the day's end...
						//GlobalVariableManager.Instance.BOSS_HP_LIST[bossesListPosition] = currentHp;
					}
				myAnim.Play("hit");
				if(moveWhenHit){
                    UpdateFacing();
                }
				yield return new WaitForSeconds(.2f);
				this.gameObject.GetComponent<tk2dSprite>().color = Color.white;
				//gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
				//Debug.Log("**AND HERE!!!!!!!!***");
				yield return new WaitForSeconds(.4f);
				StartCoroutine( "StopKnockback");
				StartCoroutine("AfterHit");

			}
		}else if(armoredEnemy){
			SoundManager.instance.PlaySingle(armoredEnemyHitSfx);
		}
	}

	public void TakeDamage(GameObject melee){ //set public for Stuart
		//Debug.Log("--------TAKE DAMAGE ACTIVATE ----------");
		Debug.Log(damageOnce);
			if(this.enabled && damageOnce == 0 && myAnim.CurrentClip!= invincibleAni &&( armoredEnemy != true || (armoredEnemy && GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED.Count == 4)|| piercingPin)){
				if(!takingDamage){
					takingDamage = true;
					damageOnce = 1;
					meleeDmgBonus = 0;
					SoundManager.instance.PlaySingle(hitSound);
					SoundManager.instance.PlaySingle(hitSqueal);
					if(GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[1] > 12){
						//bonus dmg with pole
						meleeDmgBonus++;
					}
					if(hitByThrownObject)
						meleeDmgBonus+=2;
					//if(GlobalVariableManager.Instance.IsPinEquipped(PIN.STAYBACK) && GlobalVariableManager.Instance.CURRENT_HP == 1){
						//STAY BACK pin
						//meleeDmgBonus++;
					//}

					meleeDmgBonus = meleeDmgBonus;
					if(!hitByThrownObject)
						meleeSwingDirection = melee.GetComponent<tk2dSpriteAnimator>().CurrentClip.name;
					swingDirectionSide = player.transform.localScale.x;
					//Debug.Log("MELEE SWING DIRECTION: " + meleeSwingDirection);

					if(secretHider)
						Destroy(melee.gameObject);
					if(moveWhenHit || hitByThrownObject){
						for(int i = 0; i < behaviorsToDeactivate.Count;i++){
							behaviorsToDeactivate[i].enabled = false;
						}
					}
					if(IAmParentObj){
						childEnemy.GetComponent<tk2dSprite>().color = Color.red;
					}else{
						this.gameObject.GetComponent<tk2dSprite>().color = Color.red;
					}
					GameObject damageCounter = ObjectPool.Instance.GetPooledObject("HitStars"); 
					damageCounter.GetComponent<Ev_HitStars>().ShowProperDamage(1 + meleeDmgBonus);
					damageCounter.SetActive(true);
					GameObject littleStars = ObjectPool.Instance.GetPooledObject("effect_LittleStars");
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


                    //Debug.Log("GOT THIS FAR- ENEMY TAKE DAMGE ----- 1");
                   CamManager.Instance.mainCam.ScreenShake(.2f);

					if(hitByThrownObject){
                        // TODO: Fix the boss battle to use throwable bodies????
                        var body = melee.gameObject.GetComponent<ThrowableBody>();
                        if (body)
                         body.TakeDamage();
						meleeSwingDirection = "plankSwing";
					}
					if(!moveWhenHit && !dontStopWhenHit && !hitByThrownObject ){
						GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
					}

					currentHp = currentHp - 1 - meleeDmgBonus;
					//Debug.Log("GOT THIS FAR- ENEMY TAKE DAMGE 2");
					if(bossEnemy){
						gameObject.GetComponent<Boss>().UpdateBossHp(currentHp);
						//TODO: make sure all bosses hp global vars are updated properly at the day's end...
						//GlobalVariableManager.Instance.BOSS_HP_LIST[bossesListPosition] = currentHp;
					}
					if(!moveWhenHit && currentHp <= 0){
						moveWhenHit = true; // enemy flies back at final hit
					}

					if(returnToCurrentAniAfterHit){
						returnAniName = myAnim.CurrentClip.name;
					}

					myAnim.Play("hit");
					if(moveWhenHit){
                        UpdateFacing();
                    }
					//camShake = 1;
					if(gameObject.GetComponent<FollowPlayer>() != null && moveWhenHit){
						gameObject.GetComponent<FollowPlayer>().StopSound();
						gameObject.GetComponent<FollowPlayer>().enabled = false;
					}
					StartCoroutine("ContinueHit"); // just needed to seperate here for IEnumerator stuff
				}else{
				Debug.Log("taking damange = true?");
				}
			}

	}

    public void UpdateFacing()
    {
        if (gameObject.transform.position.x < player.transform.position.x)
            gameObject.transform.localScale = new Vector2(startScale.x, startScale.y);
        else
            gameObject.transform.localScale = new Vector2(startScale.x * -1, startScale.y);
    }

	IEnumerator StopKnockback(float delay = 0f){
		yield return new WaitForSeconds(delay);

		ObjectPool.Instance.GetPooledObject("effect_enemyLand",gameObject.transform.position);
		spinning = false;
		//myCollisionBox.enabled = true;
		for(int i = 0; i < behaviorsToDeactivate.Count;i++){
						behaviorsToDeactivate[i].enabled = true;
		}

		//Debug.Log("STOP KNOCKBACK ACTIVATE");
		damageOnce = 0;
		SoundManager.instance.PlaySingle(bounce);
		gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
		gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
		//if(aniToSwitchBackTo != null)
	
		if(currentHp >0 && myAnim.GetClipByName(returnAniName) != null)
			myAnim.Play(returnAniName);
			//else
				//myAnim.Play("IdleR");
		yield return new WaitForSeconds(.1f);
		takingDamage = false;

	}

	IEnumerator ContinueHit(){

		//remove effects from self



		if(moveWhenHit || hitByThrownObject){
			takingDamage = true;

			if(meleeSwingDirection.CompareTo("plankSwing") == 0||meleeSwingDirection.CompareTo("clawR") == 0||meleeSwingDirection.CompareTo("poleR") == 0){
				Debug.Log(swingDirectionSide);

				//myCollisionBox.enabled = false;

				if(swingDirectionSide < 0){
					if(currentHp <= 0){
						spinning = true;
						myBody.mass = 1;
						myBody.AddForce(new Vector2(-11f,8f), ForceMode2D.Impulse);
						myBody.gravityScale = 3;
						if(myShadow != null)
							myShadow.transform.parent = null; //shadow doesnt follow Y pos
					}else{
						Debug.Log("**Got here- enemy hit***" + moveWhenHit + currentHp);
						myBody.AddForce(new Vector2(-17f,0f), ForceMode2D.Impulse);
					}
				}else{
					if(currentHp <= 0){
						spinning = true;
						myBody.mass = 1;
						gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(11f,8f), ForceMode2D.Impulse);
						myBody.gravityScale = 3;
						if(myShadow != null)
							myShadow.transform.parent = null; //shadow doesnt follow Y pos	
					}else{
						Debug.Log("**Got here- enemy hit***");
						myBody.velocity = new Vector2(7f,0f);
						gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(17f,0f), ForceMode2D.Impulse);	
					}
				}




			}else if(meleeSwingDirection.CompareTo("stickUp") == 0||meleeSwingDirection.CompareTo("clawUp") == 0||meleeSwingDirection.CompareTo("poleUp") == 0){
				gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f,-17f), ForceMode2D.Impulse);
			}else if(meleeSwingDirection.CompareTo("stickDown") == 0||meleeSwingDirection.CompareTo("clawDown") == 0||meleeSwingDirection.CompareTo("poleDown") == 0){
				gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f,17f), ForceMode2D.Impulse);
			}
				

			yield return new WaitForSeconds(.1f);
			if(IAmParentObj){
				childEnemy.gameObject.GetComponent<tk2dSprite>().color = Color.white;

			}else
				this.gameObject.GetComponent<tk2dSprite>().color = Color.white;
				//gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
				//Debug.Log("**AND HERE!!!!!!!!***");

			if(currentHp <= 0){
				if(!bossEnemy)
					yield return new WaitForSeconds(.4f);
			}else
				yield return new WaitForSeconds(.1f);

			StartCoroutine( "StopKnockback");

		}else{
			yield return new WaitForSeconds(.2f);
			if(IAmParentObj){
				childEnemy.gameObject.GetComponent<tk2dSprite>().color = Color.white;
			}else{
				this.gameObject.GetComponent<tk2dSprite>().color = Color.white;
			}
			damageOnce = 0;
			takingDamage = false;
			if(currentHp >0 && myAnim.GetClipByName(returnAniName) != null)
				myAnim.Play(returnAniName);
		} // end of movement functions
		hitByThrownObject = false;

		StartCoroutine("AfterHit");
			
		

	}

    IEnumerator AfterHit()
    {
        if (currentHp > 0) {

            //disable follow player after notice behavior if end up having thta in game
            if (gameObject.GetComponent<RandomDirectionMovement>() != null && gameObject.GetComponent<RandomDirectionMovement>().enabled) {
                gameObject.GetComponent<RandomDirectionMovement>().enabled = false;
            }

            //****SFX PLAY - 'hit2' on ch4
            yield return new WaitForSeconds(.4f);
            //***grow/shrink scale back to normal on all fronts

            if (gameObject.GetComponent<FollowPlayer>() && this.enabled) { //enabled check for if other things disable follow player for whatever reason
                gameObject.GetComponent<FollowPlayer>().enabled = true;
                //***enable 'follow target after notice' here(ALSO TRIGGER 'notice' method in that script
            }
            else if (gameObject.GetComponent<RandomDirectionMovement>()) {
                gameObject.GetComponent<RandomDirectionMovement>().enabled = true;
            }
            damageOnce = 0;

        }
        else { //if hp is NOT > 0
            Debug.Log("CURRENT HP IS NOT GREATER THAN ZEROOOOOOO!");
            if (GlobalVariableManager.Instance.MASTER_SFX_VOL > 0) {
                //**SFX PLAY- 'hit_final'
            }

            if (gameObject.GetComponent<RandomDirectionMovement>() != null && gameObject.GetComponent<RandomDirectionMovement>().enabled) {
                gameObject.GetComponent<RandomDirectionMovement>().enabled = false;
            }

            if (GlobalVariableManager.Instance.ROOM_NUM != 201) {
                /*if(GlobalVariableManager.Instance.characterUpgradeArray[1].Substring(10,11).CompareTo("o") == 0){
                    //pin perk 1 - enemies have chance to drop pin
                    dropsPin = Random.Range(1,30);
                }*/
                dropsTrash = 99;
                if (dropsPin != 22) {
                    dropsTrash = Random.Range(1, (12 - sharingUpgrade));
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
            if (secretHider) {
                //**SFX PLAY 'secret discovered'
                GameObject hole = Instantiate(GameObject.Find("hiddenHole"), transform.position, Quaternion.identity) as GameObject;
            }
            else if (!bossEnemy) {
                DropThings();
            }


           
			//Debug.Log("Got this far for boss death check");


            if (!bossEnemy) {
				yield return new WaitForSeconds(.2f);
                DropScrap();
                Death();
            }
            else {
                gameObject.GetComponent<Boss>().StartCoroutine("BossDeath");
            }
        }//end of (if hp is not > 0)

        // Some Enemies control their own facing and need their scale reset after being hit (e.g. Questio).
        if (resetFacingAfterHit) {
            gameObject.transform.localScale = startScale;
        }
    }

	public void Clank(AudioClip clankSfx,Vector2 clankPosition){
		if(!takingDamage){
			Debug.Log("Clanking material got here----x-x-x-x--- 2");
			takingDamage = true;
			GameObject clankSpark = ObjectPool.Instance.GetPooledObject("effect_clank",clankPosition);
			SoundManager.instance.PlaySingle(clankSfx);
			/*if(gameObject.GetComponent<FollowPlayer>()){
				gameObject.GetComponent<FollowPlayer>().enabled = false;
			}*/
			for(int i = 0; i < behaviorsToDeactivate.Count;i++){
						behaviorsToDeactivate[i].enabled = false;
			}
			if(gameObject.transform.position.x < player.transform.position.x){
				player.GetComponent<Rigidbody2D>().AddForce(new Vector2(11,0),ForceMode2D.Impulse);
				this.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-11,0),ForceMode2D.Impulse);
			}else{
				player.GetComponent<Rigidbody2D>().AddForce(new Vector2(-11,0),ForceMode2D.Impulse);	
				this.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(11,0),ForceMode2D.Impulse);
			}
			StartCoroutine("StopKnockback",.5f);
		}
	}


	void ArmorKnockoff(){
		armoredEnemy = false;
		armorCollisionObj.SetActive(false); //TODO: best way to enable it when the enemy is spawned(for proper reuse with ObjectPool pull)
		gameObject.GetComponent<tk2dSpriteAnimator>().Library= disarmoredAnimation;
		GameObject armorPiece =ObjectPool.Instance.GetPooledObject("effect_armorPiece",gameObject.transform.position);
		armorPiece.GetComponent<Rigidbody2D>().AddForce(new Vector2(0,6f),ForceMode2D.Impulse);
	}

	void DropThings(){
		//Just took out for now, not sure what im doing as far as possibility to drop trash

		/*GameObject droppedTrash;
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

		}*/
	}

	void DropScrap(){
        // 'Waste Warrior' pin ups the scrap cap by 5, due to the additional upgrade.
        int wasteWarriorAdjust = (GlobalVariableManager.Instance.IsPinEquipped(PIN.WASTEWARRIOR) ? 1 : 0) * 5;
        if (GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[1] < (13 + wasteWarriorAdjust)){
			if(!GlobalVariableManager.Instance.IsPinEquipped(PIN.SCRAPCITY)){
				//^ Scrap City - more scrap dropped
				scrapDropped = Random.Range(1,4);
			}else{
				scrapDropped = Random.Range(2,7);
				player.GetComponent<PinFunctionsManager>().StartCoroutine("DisplayEffectsHud",PinManager.Instance.GetPin(PIN.SCRAPCITY).sprite);
			}
			Debug.Log("dropped scrap:" + scrapDropped);
		    for(int i = 0; i < scrapDropped; i++){
				GameObject droppedScrap=   ObjectPool.Instance.GetPooledObject("Scrap",gameObject.transform.position); 
				droppedScrap.GetComponent<Ev_Scrap>().landingY = gameObject.transform.position.y + Random.Range(-2f,2f);
			    //droppedScrap = Instantiate(scrapDrop,new Vector3((transform.position.x + Random.Range(0,gameObject.GetComponent<tk2dSprite>().GetBounds().size.x)),(transform.position.y + Random.Range(0,gameObject.GetComponent<tk2dSprite>().GetBounds().size.y)),transform.position.z), Quaternion.identity);
		    }
        }
    }


	public void SetSpawnerID(string id){
		mySpawnerID = id;
	}

	void Death(){
		Debug.Log("Death Activate!!!");

		if(respawnEnemy){
			myRespawner.currentEnemies.Remove(this.gameObject);
			//needed to make sure enemy doesnt spawn again functioning as if it was dead
			spinning = false;
			//myCollisionBox.enabled = true;
			//damageOnce = 0;
			gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
			gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
			if(currentHp >0 && myAnim.GetClipByName("idle") != null)
				myAnim.Play("idle");
			takingDamage = false;
			if(gameObject.GetComponent<FollowPlayer>()){
					gameObject.GetComponent<FollowPlayer>().enabled = true;
					//***enable 'follow target after notice' here(ALSO TRIGGER 'notice' method in that script
			}
			currentHp = 2; //TODO: this is just for brownmole, maybe get it from 'Enemy.cs"?

		}else{
			
			GlobalVariableManager.Instance.BASIC_ENEMY_LIST[this.mySpawnerID].dayOfRevival = GlobalVariableManager.Instance.DAY_NUMBER +3;
			Debug.Log("Day of revival: "+ GlobalVariableManager.Instance.BASIC_ENEMY_LIST[this.mySpawnerID].dayOfRevival);

		}

		if(gameObject.GetComponent<FollowPlayerAfterNotice>()){
			gameObject.GetComponent<FollowPlayer>().enabled = true;
		}

		if(gameObject.GetComponent<Animator>()){
			gameObject.GetComponent<Animator>().StopPlayback();
		}

		GameObject deathSmoke = ObjectPool.Instance.GetPooledObject("effect_SmokePuff"); 
		deathSmoke.transform.position = new Vector3((transform.position.x), transform.position.y, transform.position.z);
		AudioClip smokeSFX = deathSmoke.GetComponent<KillSelfAfterTime>().mySound;
		SoundManager.instance.PlaySingle(smokeSFX);

		GameObject deathGhost = ObjectPool.Instance.GetPooledObject("effect_DeathGhost");
		deathGhost.transform.position = new Vector3((transform.position.x), transform.position.y, transform.position.z);
		Debug.Log("My spawner ID: "+mySpawnerID);
		if(!respawnEnemy && !dontSpawnBody){
			GameObject body = ObjectPool.Instance.GetPooledObject("enemyBody",gameObject.transform.position);
			body.GetComponent<tk2dSprite>().SetSprite(myDeadBodyName);
			body.GetComponent<ThrowableBody>().SetSpawnerID(mySpawnerID);
		}
		myAnim.Play("idle");//to fix enemies sometimes spawning in hurt animation
		damageOnce = 0;
		if(startsOffDontMoveWhenHit){
			moveWhenHit = false;
		}
		this.gameObject.SetActive(false);

	}
}
