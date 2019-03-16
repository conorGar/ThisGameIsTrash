using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GenericEnemyStateController = EnemyStateController<EnemyState, EnemyTrigger>;

[RequireComponent(typeof(GenericEnemyStateController))]
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

	public AudioClip armoredEnemyHitSfx;
	public bool bossEnemy;
	public bool IAmParentObj;
	public GameObject childEnemy;

    //public SFXBANK hitSound;
    public SFXBANK hitSqueal;
    public SFXBANK bounce;


	public GameObject tutPopup;

	public GameObject gar_trash;
	public GameObject gar_rec;
	public GameObject gar_comp;
	public GameObject timeDrop;
	public GameObject healthDrop;
	public GameObject pinDrop;

	public GameObject myShadow;
	//public BoxCollider2D myCollisionBox;
	public bool dontSpawnBody;
	public bool canKnockoffArmor;

	public List<MonoBehaviour> behaviorsToDeactivate = new List<MonoBehaviour>();
	public bool hasPowerHitEffect;


	//armor knockoff stuff
	public GameObject armorCollisionObj; //object, like a shield or something, to deactivate
	public tk2dSpriteAnimation disarmoredAnimation;


	[HideInInspector]
	public GameObject objectPool;
	[HideInInspector]
public EnemyRespawner myRespawner; //for use with respawning enemies
	[HideInInspector]
public B_Ev_Ex otherRespawner; //for use with other things that spawn enemies, like bosses(see ex behavior for example). SHort term solution. Im a bit drunk when im making this so cant think of anything better for now...
public int currentHp;
	[HideInInspector]
public int meleeDmgBonus = 0;
	//[HideInInspector]
public bool bossSpawnedEnemy;

	float swingDirectionSide; // uses scale to see if swinging left or right

	bool piercingPin = false;
	int maxHp; //just used for Vitality Vision pin
	int sharingUpgrade = 0;
	int damageOnce = 0;
	bool hitByThrownObject;
	float hitPitch = .9f;

	tk2dSpriteAnimator myAnim;
	int camShake = 0;
	public string meleeSwingDirection;
	int dropsPin = 0;
	int dropsTrash = 0;
	int scrapDropped = 0;
	//Color lerpColor = Color.white;
	protected bool takingDamage = false;
	string mySpawnerID;

	Vector2 shadowStartPos;
	Rigidbody2D myBody;
    [SerializeField]
	bool spinning;
	float t;
	Quaternion startRotation;

	public bool startsOffDontMoveWhenHit; // needed for returning proper value when enemy dies(otherwise spawned enemies after starting obj pool amount will always have 'movewhenhit' enabled)

	Vector2 startScale;

    protected GenericEnemyStateController controller;

    void Awake()
    {
        controller = GetComponent<GenericEnemyStateController>();
    }

    void OnEnable(){
		if(bossSpawnedEnemy){
			bossSpawnedEnemy = false;
		}
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

        if (myShadow != null){
            myShadow.SetActive(true);
            myShadow.transform.rotation = Quaternion.identity; //keep shadow from rotating.
        }
    }

    private void OnDisable()
    {
        if (myShadow != null)
            myShadow.SetActive(false);
    }


    protected void Start () {
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
	

	protected void Update () {
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
			if(hitByThrownObject)
				hitByThrownObject = false;
			TakeDamage(melee.gameObject);

			//Debug.Log("Collision with weapon: ");

		}else if(melee.tag == "pObj_bullet" || melee.tag == "BigSwoosh"){
			if(!takingDamage){
				if(melee.tag == "BigSwoosh"){
					meleeDmgBonus = 0;
					if(hasPowerHitEffect){
						PowerHitEffect();
					}
					meleeDmgBonus++;
					meleeSwingDirection = melee.name;
					StartCoroutine("NonMeleeHit",true);
					Debug.Log("BIG SWOOSH HITS ENEMY");
				}else{
					if(melee.GetComponent<Ev_FallingProjectile>() != null)
						melee.GetComponent<Ev_FallingProjectile>().Fell();
					if(melee.GetComponent<Ev_ProjectileChargable>() != null && melee.GetComponent<Ev_ProjectileChargable>().charged){
						meleeSwingDirection = "plankSwing";

						StartCoroutine("NonMeleeHit",true);
					}/*else{
						StartCoroutine("NonMeleeHit",false);
					}*/
				}
			}
			//Debug.Log("Collision with nen melee weapon: >>>>>>>>>>> ");
            SoundManager.instance.RandomizeSfx(SFXBANK.HIT6, .8f, 1.1f);
			SoundManager.instance.PlaySingle(hitSqueal);
		}else if(melee.gameObject.layer == 15){//throwable object
			hitByThrownObject = true;
            // TODO: Get the boss battle to use throwable bodies???
            var body = melee.gameObject.GetComponent<ThrowableBody>();
            if (body){
                //melee.gameObject.GetComponent<ThrowableBody>().StartCoroutine("Impact",this.gameObject);
              	body.StartCoroutine("DeathImpact",this.gameObject);
            }else{
				var destructableObj = melee.gameObject.GetComponent<DestructableThrowingObject>();
				if(destructableObj){
					melee.gameObject.GetComponent<DestructableThrowingObject>().LandingEvent();
				}
            }
			Debug.Log("Hit by thrown object!");
			if(canKnockoffArmor){
				ArmorKnockoff();
			}else if(gameObject.GetComponent<InvincibleEnemy>() == null){
				TakeDamage(melee.gameObject);
			}else if(gameObject.GetComponent<InvincibleEnemy>().enabled == false){
				TakeDamage(melee.gameObject);
			}
			SoundManager.instance.PlaySingle(SFXBANK.HIT7);
			SoundManager.instance.PlaySingle(hitSqueal);
			/*var throwObj = melee.gameObject.GetComponent<ThrowableObject>();
				if(throwObj)
					melee.gameObject.layer = 11; //switched to item obj once hit so doesnt hit anything else*/
		}
	}

	IEnumerator NonMeleeHit(bool knockback){
		if(damageOnce == 0 && myAnim.CurrentClip!= invincibleAni &&( armoredEnemy != true || (armoredEnemy && GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED.Count == 4)|| piercingPin)){
			if(!takingDamage){
				takingDamage = true;
				this.gameObject.GetComponent<tk2dSprite>().color = Color.red;
					GameObject damageCounter = ObjectPool.Instance.GetPooledObject("HitStars");
					damageCounter.GetComponent<Ev_HitStars>().ShowProperDamage(1 + meleeDmgBonus);
					damageCounter.SetActive(true);
					GameObject littleStars = ObjectPool.Instance.GetPooledObject("effect_LittleStars");
					damageCounter.transform.position = new Vector3((transform.position.x), transform.position.y, transform.position.z);
					littleStars.transform.position = new Vector3((transform.position.x), transform.position.y, transform.position.z);
					littleStars.SetActive(true);

						if(gameObject.transform.position.x < PlayerManager.Instance.player.transform.position.x){
							//hitStarPS.SetActive(true);
							//hitStarPS.transform.localScale = new Vector3(1f,1f,1f);//makes stars burst in right direction

							damageCounter.GetComponent<Rigidbody2D>().AddForce(new Vector2(4f,10f), ForceMode2D.Impulse);
						}else{
							//hitStarPS.SetActive(true);
							//hitStarPS.transform.localScale = new Vector3(-1f,1f,1f);//makes stars burst in right direction
							damageCounter.GetComponent<Rigidbody2D>().AddForce(new Vector2(-4f,10f), ForceMode2D.Impulse);

						}
				if(!moveWhenHit && controller.GetCurrentState() != EnemyState.LUNGE){
						GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
				}
				Debug.Log("GOT THIS FAR- ENEMY TAKE DAMGE 2....." + meleeDmgBonus);

				currentHp = currentHp - 1 - meleeDmgBonus;
					if(bossEnemy){
                    gameObject.GetComponent<Boss>().UpdateBossHp(currentHp);
						//TODO: make sure all bosses hp global vars are updated properly at the day's end...
						//GlobalVariableManager.Instance.BOSS_HP_LIST[bossesListPosition] = currentHp;
					}

                GetComponent<GenericEnemyStateController>().SendTrigger(EnemyTrigger.HIT);
                if (moveWhenHit){
                    UpdateFacing();
                }

                if(knockback){
                	Debug.Log("****----- GOT HERE BIG HIT ---------****");
                	if(meleeSwingDirection == "bigShooshR"){
						meleeSwingDirection = "plankSwing";
					}else if(meleeSwingDirection == "bigShooshDown"){
						meleeSwingDirection = "plankDown";

					}else if(meleeSwingDirection == "bigShooshUp"){
						meleeSwingDirection = "plankUp";

                	}
					swingDirectionSide = PlayerManager.Instance.player.transform.localScale.x;
					moveWhenHit = true;
					StartCoroutine("ContinueHit"); 
					yield return new WaitForSeconds(.1f);
                }else{

				yield return new WaitForSeconds(.2f);
				this.gameObject.GetComponent<tk2dSprite>().color = Color.white;
				//gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
				//Debug.Log("**AND HERE!!!!!!!!***");
				yield return new WaitForSeconds(.4f);
				StartCoroutine( "StopKnockback",0f);
				StartCoroutine("AfterHit");
				}
			}
		}else if(armoredEnemy){
			SoundManager.instance.PlaySingle(armoredEnemyHitSfx);
		}
	}

	public void TakeDamage(GameObject melee){ //set public for Stuart
		if(this.enabled && damageOnce == 0 && myAnim.CurrentClip!= invincibleAni &&( armoredEnemy != true || (armoredEnemy && GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED.Count == 4)|| piercingPin)){
			if(!takingDamage){
				takingDamage = true;
				damageOnce = 1;
				meleeDmgBonus = 0;

			
				if(hitByThrownObject){
					meleeDmgBonus+=1;
				}else if(GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[1] > 12){
				//bonus dmg with pole
					meleeDmgBonus++;
				}


				meleeDmgBonus = meleeDmgBonus;
				if(!hitByThrownObject)
					meleeSwingDirection = melee.GetComponent<tk2dSpriteAnimator>().CurrentClip.name;
				swingDirectionSide = PlayerManager.Instance.player.transform.localScale.x;
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

				if(gameObject.transform.position.x < PlayerManager.Instance.player.transform.position.x){
					//hitStarPS.SetActive(true);
					//hitStarPS.transform.localScale = new Vector3(1f,1f,1f);//makes stars burst in right direction

					damageCounter.GetComponent<Rigidbody2D>().AddForce(new Vector2(4f,10f), ForceMode2D.Impulse);
				}else{
					//hitStarPS.SetActive(true);
					//hitStarPS.transform.localScale = new Vector3(-1f,1f,1f);//makes stars burst in right direction
					damageCounter.GetComponent<Rigidbody2D>().AddForce(new Vector2(-4f,10f), ForceMode2D.Impulse);

				}


                //Debug.Log("GOT THIS FAR- ENEMY TAKE DAMGE ----- 1");
//                   CamManager.Instance.mainCam.ScreenShake(.2f);

				if(hitByThrownObject){
                    // TODO: Fix the boss battle to use throwable bodies????
                    /* var body = melee.gameObject.GetComponent<ThrowableBody>();
                    if (body){
                        //body.TakeDamage();
                        body.Death();
                        }*/
					meleeSwingDirection = "plankSwing";
				}
				if(!moveWhenHit && !hitByThrownObject && controller.GetCurrentState() != EnemyState.LUNGE) {
					GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
				}

				currentHp = currentHp - 1 - meleeDmgBonus;

				if(currentHp <= 0){
				    SoundManager.instance.PlaySingle(SFXBANK.HIT7, hitPitch);
                    GetComponent<GenericEnemyStateController>().SendTrigger(EnemyTrigger.DEATH);
                } else{
				    SoundManager.instance.PlaySingle(SFXBANK.HIT6, hitPitch);
				    if(hitPitch < 1.3f)
					    hitPitch += .1f; // pitch goes up as hit enemy

                    GetComponent<GenericEnemyStateController>().SendTrigger(EnemyTrigger.HIT);
                }
				SoundManager.instance.PlaySingle(hitSqueal);

				//Debug.Log("GOT THIS FAR- ENEMY TAKE DAMGE 2");
				if(bossEnemy){
					gameObject.GetComponent<Boss>().UpdateBossHp(currentHp);
					//TODO: make sure all bosses hp global vars are updated properly at the day's end...
					//GlobalVariableManager.Instance.BOSS_HP_LIST[bossesListPosition] = currentHp;
				}
				if(!moveWhenHit && currentHp <= 0){
					moveWhenHit = true; // enemy flies back at final hit
				}

                
                if (moveWhenHit){
                    UpdateFacing();
                }
				//camShake = 1;
				if(gameObject.GetComponent<FollowPlayer>() != null && moveWhenHit){
					gameObject.GetComponent<FollowPlayer>().StopSound();
					gameObject.GetComponent<FollowPlayer>().enabled = false;
				}

				StartCoroutine("ContinueHit"); // just needed to seperate here for IEnumerator stuff
			}
		}

	}

    public void UpdateFacing()
    {
        if (gameObject.transform.position.x < PlayerManager.Instance.player.transform.position.x)
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
	
		yield return new WaitForSeconds(.1f);
		takingDamage = false;

	}

	IEnumerator ContinueHit(){

		//remove effects from self



		if(moveWhenHit || hitByThrownObject){
			takingDamage = true;

			Debug.Log("-----MELEE WEAPON SWING DIRECTION :" + meleeSwingDirection);

			if(meleeSwingDirection.CompareTo("plankSwing") == 0||meleeSwingDirection.CompareTo("clawSwing") == 0||meleeSwingDirection.CompareTo("poleSwing") == 0){
				Debug.Log(swingDirectionSide);

				//myCollisionBox.enabled = false;

				if(swingDirectionSide < 0){
					if(currentHp <= 0 && !bossEnemy){
						spinning = true;
						myBody.mass = 1;
						myBody.AddForce(new Vector2(-11f,10f), ForceMode2D.Impulse);
						myBody.gravityScale = 3.3f;
						if(myShadow != null)
							myShadow.transform.parent = null; //shadow doesnt follow Y pos
					}else{
						Debug.Log("**Got here- enemy hit***" + moveWhenHit + currentHp);
						myBody.AddForce(new Vector2(-17f,0f), ForceMode2D.Impulse);
					}
				}else{
					if(currentHp <= 0 && !bossEnemy){
						spinning = true;
						myBody.mass = 1;
						gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(11f,10f), ForceMode2D.Impulse);
						myBody.gravityScale = 3.3f;
						if(myShadow != null)
							myShadow.transform.parent = null; //shadow doesnt follow Y pos	
					}else{
						Debug.Log("**Got here- enemy hit***");
						myBody.velocity = new Vector2(7f,0f);
						gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(17f,0f), ForceMode2D.Impulse);	
					}
				}




			}else if(meleeSwingDirection.CompareTo("plankUp") == 0||meleeSwingDirection.CompareTo("clawUp") == 0||meleeSwingDirection.CompareTo("poleUp") == 0){
				gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f,17f), ForceMode2D.Impulse);
				Debug.Log("ENEMY WAS HIT UP!!");
			}else if(meleeSwingDirection.CompareTo("plankDown") == 0||meleeSwingDirection.CompareTo("clawDown") == 0||meleeSwingDirection.CompareTo("poleDown") == 0){
				gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f,-17f), ForceMode2D.Impulse);
				Debug.Log("ENEMY WAS HIT down!!");
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

			StartCoroutine( "StopKnockback",0f);

		}else{
			yield return new WaitForSeconds(.2f);
			if(IAmParentObj){
				childEnemy.gameObject.GetComponent<tk2dSprite>().color = Color.white;
			}else{
				this.gameObject.GetComponent<tk2dSprite>().color = Color.white;
			}
			damageOnce = 0;
			takingDamage = false;
		} // end of movement functions
		hitByThrownObject = false;

		StartCoroutine("AfterHit");
			
		

	}

    IEnumerator AfterHit()
    {
        if (currentHp > 0) {

            //disable follow player after notice behavior if end up having thta in game
            if (gameObject.GetComponent<RandomDirectionMovement>() != null && gameObject.GetComponent<RandomDirectionMovement>().enabled) {
                gameObject.GetComponent<RandomDirectionMovement>().StopMoving();
            }

            //****SFX PLAY - 'hit2' on ch4
            yield return new WaitForSeconds(.4f);
            //***grow/shrink scale back to normal on all fronts

            if (gameObject.GetComponent<FollowPlayer>() && this.enabled) { //enabled check for if other things disable follow player for whatever reason
                gameObject.GetComponent<FollowPlayer>().enabled = true;
                //***enable 'follow target after notice' here(ALSO TRIGGER 'notice' method in that script
            }
            else if (gameObject.GetComponent<RandomDirectionMovement>()) {
                gameObject.GetComponent<RandomDirectionMovement>().StartMoving();
            }
            damageOnce = 0;

        }
        else { //if hp is NOT > 0
            Debug.Log("CURRENT HP IS NOT GREATER THAN ZEROOOOOOO!");
            if (GlobalVariableManager.Instance.MASTER_SFX_VOL > 0) {
                //**SFX PLAY- 'hit_final'
            }

            if (gameObject.GetComponent<RandomDirectionMovement>() != null && gameObject.GetComponent<RandomDirectionMovement>().enabled) {
                gameObject.GetComponent<RandomDirectionMovement>().StopMoving();
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

	public void Clank(AudioClip clankSfx,Vector2 clankPosition, bool getsPushedBack){
		if(!takingDamage){
			Debug.Log("Clanking material got here----x-x-x-x--- 2");
			takingDamage = true;
			GameObject clankSpark = ObjectPool.Instance.GetPooledObject("effect_clank",clankPosition);
			SoundManager.instance.PlaySingle(clankSfx);
			/*if(gameObject.GetComponent<FollowPlayer>()){
				gameObject.GetComponent<FollowPlayer>().enabled = false;
			}*/
			Vector2 pushBackDir = (gameObject.transform.position- PlayerManager.Instance.player.transform.position).normalized * 9;

			if(getsPushedBack){
				for(int i = 0; i < behaviorsToDeactivate.Count;i++){
							behaviorsToDeactivate[i].enabled = false;
				}
				/*if(gameObject.transform.position.x < player.transform.position.x){
					player.GetComponent<Rigidbody2D>().AddForce(new Vector2(11,0),ForceMode2D.Impulse);
					this.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-11,0),ForceMode2D.Impulse);
				}else{
					player.GetComponent<Rigidbody2D>().AddForce(new Vector2(-11,0),ForceMode2D.Impulse);	
					this.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(11,0),ForceMode2D.Impulse);
				}*/
				this.gameObject.GetComponent<Rigidbody2D>().AddForce(pushBackDir,ForceMode2D.Impulse);
                PlayerManager.Instance.player.gameObject.GetComponent<Rigidbody2D>().AddForce(pushBackDir*-1,ForceMode2D.Impulse);

				StartCoroutine("StopKnockback",.5f);
			}else{
                /*if(gameObject.transform.position.x < player.transform.position.x){
					player.GetComponent<Rigidbody2D>().AddForce(new Vector2(11,0),ForceMode2D.Impulse);
				}else{
					player.GetComponent<Rigidbody2D>().AddForce(new Vector2(-11,0),ForceMode2D.Impulse);	
				}*/
                PlayerManager.Instance.player.gameObject.GetComponent<Rigidbody2D>().AddForce(pushBackDir*-1,ForceMode2D.Impulse);

				Invoke("ClankReturnDelay",.2f);
			}
		}
	}
	void ClankReturnDelay(){
		takingDamage = false;

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
                PlayerManager.Instance.player.GetComponent<PinFunctionsManager>().StartCoroutine("DisplayEffectsHud",PinManager.Instance.GetPin(PIN.SCRAPCITY).sprite);
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

		if(respawnEnemy || bossSpawnedEnemy){
			if(myRespawner != null)
				myRespawner.currentEnemies.Remove(this.gameObject);
			else{
				otherRespawner.currentBlobs.Remove(this.gameObject);
			}
			//needed to make sure enemy doesnt spawn again functioning as if it was dead
			spinning = false;
			//myCollisionBox.enabled = true;
			//damageOnce = 0;
			gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
			gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
            GetComponent<GenericEnemyStateController>().SendTrigger(EnemyTrigger.HIT);

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

		GameObject deathGhost = ObjectPool.Instance.GetPooledObject("effect_DeathGhost",new Vector3((transform.position.x), transform.position.y, transform.position.z));
		deathGhost.GetComponent<Ev_DeathGhost>().OnSpawn();
	
		Debug.Log("My spawner ID: "+mySpawnerID);
		if(!respawnEnemy && !dontSpawnBody){
			GameObject body = ObjectPool.Instance.GetPooledObject("enemyBody",gameObject.transform.position);
			body.GetComponent<tk2dSprite>().SetSprite(myDeadBodyName);
			body.GetComponent<ThrowableBody>().SetSpawnerID(mySpawnerID);
		}

		damageOnce = 0;
		if(startsOffDontMoveWhenHit){
			moveWhenHit = false;
		}
		this.gameObject.SetActive(false);

	}

	public virtual void PowerHitEffect(){
		//nothing for base
	}
}
