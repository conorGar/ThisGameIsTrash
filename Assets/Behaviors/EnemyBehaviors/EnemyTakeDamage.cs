using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTakeDamage : MonoBehaviour {


	public int armorRating = 0;
	public bool moveWhenHit = true;
 
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



	public GameObject myShadow;
	public bool dontSpawnBody;

	public List<MonoBehaviour> behaviorsToDeactivate = new List<MonoBehaviour>();
	public bool hasPowerHitEffect;


	//armor knockoff stuff
	public GameObject armorCollisionObj; //object, like a shield or something, to deactivate
	public tk2dSpriteAnimation disarmoredAnimation;

	[HideInInspector]
	public float knockbackForce = 11; 
	bool powerHit;
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

	//bool piercingPin = false;
	int maxHp; //just used for Vitality Vision pin
	int sharingUpgrade = 0;
	int damageOnce = 0;
	//bool hitByThrownObject;
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

	Vector3 startScale;

    protected EnemyStateController controller;

    void Awake()
    {
        controller = GetComponent<EnemyStateController>();
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

	
		if(GlobalVariableManager.Instance.IsPinEquipped(PIN.SHARING)){//Sharing Pin
			sharingUpgrade = 2;
		}
		//----------------------------//


		if(armorRating >0 && (GlobalVariableManager.Instance.TUT_POPUPS_SHOWN & GlobalVariableManager.TUTORIALPOPUPS.ARMOREDENEMIES) != GlobalVariableManager.TUTORIALPOPUPS.ARMOREDENEMIES && tutPopup != null){
			tutPopup.SetActive(true);
			tutPopup.GetComponent<GUI_TutPopup>().SetData("ArmoredEnemies");
		}


	}
	

	protected void Update () {


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
        if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
            if (controller != null && !controller.IsHittable()) // ignore enemies in an un hittable state
                return;

            if (melee.tag == "Weapon") {
              
                TakeDamage(melee.gameObject);

                Debug.Log("Collision with weapon: ");
			} else if (melee.GetComponent<Ev_ProjectileBasic>() !=null || melee.tag == "pObj_bullet_large" || melee.tag == "BigSwoosh") { //player-created projectiles or big swoosh
                if (!takingDamage) {
					if (melee.tag == "BigSwoosh" || melee.tag == "pObj_bullet_large") {
                        meleeDmgBonus = 0;
                       powerHit = true;
                        meleeDmgBonus++;
                        meleeSwingDirection = melee.name;
                        StartCoroutine("NonMeleeHit");
                     
						if (hasPowerHitEffect) {
                            PowerHitEffect();
                        }
                        Debug.Log("BIG SWOOSH HITS ENEMY");
                    } else {
						meleeDmgBonus = 0;

                    	meleeDmgBonus += melee.GetComponent<Ev_ProjectileBasic>().additionalDamage;
						Debug.Log("HIT ENEMY");

                        if (melee.GetComponent<Ev_FallingProjectile>() != null)
                            melee.GetComponent<Ev_FallingProjectile>().Fell();
                        if (melee.GetComponent<Ev_ProjectileChargable>() != null && melee.GetComponent<Ev_ProjectileChargable>().charged) {
                            meleeSwingDirection = "plankSwing";
							if (hasPowerHitEffect) {
                            	PowerHitEffect();
                        	}
                            StartCoroutine("NonMeleeHit");
                        }else{
						StartCoroutine("NonMeleeHit");
						}
                    }
                }
                Debug.Log("Collision with nen melee weapon: >>>>>>>>>>> ");
                SoundManager.instance.RandomizeSfx(SFXBANK.HIT6, .8f, 1.1f);
                SoundManager.instance.PlaySingle(hitSqueal);
            } else if (melee.gameObject.layer == 15) {//throwable object

                var body = melee.gameObject.GetComponent<ThrowableBody>();
                if (body) {
                    //melee.gameObject.GetComponent<ThrowableBody>().StartCoroutine("Impact",this.gameObject);
                    body.StartCoroutine("DeathImpact", this.gameObject);
                } else {
                    var destructableObj = melee.gameObject.GetComponent<DestructableThrowingObject>();
                    if (destructableObj) {
                        melee.gameObject.GetComponent<DestructableThrowingObject>().LandingEvent();
                    }
                }
                Debug.Log("Hit by thrown object!");
                if (gameObject.GetComponent<InvincibleEnemy>() == null) { // no invincible component
                    TakeDamage(melee.gameObject);
                } else if (!gameObject.GetComponent<InvincibleEnemy>().IsInvulnerable()) { // has an invincible component but they aren't currently invincible
                    TakeDamage(melee.gameObject);
                }
                SoundManager.instance.PlaySingle(SFXBANK.HIT7);
                SoundManager.instance.PlaySingle(hitSqueal);
            }
        }
	}

	IEnumerator NonMeleeHit(){
		if(damageOnce == 0 && armorRating <= meleeDmgBonus){
			if(!takingDamage){
				takingDamage = true;
				this.gameObject.GetComponent<tk2dSprite>().color = Color.red;
					GameObject damageCounter = ObjectPool.Instance.GetPooledObject("HitStars");
					damageCounter.GetComponent<Ev_HitStars>().ShowProperDamage(1 + meleeDmgBonus - armorRating);
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
                // no moving on hit if requested or this is an enemy that is lunging.
				if(!moveWhenHit || (controller != null && controller.GetCurrentState() == EnemyState.LUNGE)){
						GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
				}
				Debug.Log("GOT THIS FAR- ENEMY TAKE DAMGE 2....." + meleeDmgBonus);

				currentHp = currentHp - 1 - meleeDmgBonus + armorRating;
					if(bossEnemy){
                    gameObject.GetComponent<Boss>().UpdateBossHp(currentHp);
					}

                if (controller != null)
                    controller.SendTrigger(EnemyTrigger.HIT);

                if (moveWhenHit){
                    UpdateFacing();
                }

                if(powerHit){
					controller.SendTrigger(EnemyTrigger.POWER_HIT);

                	Debug.Log("****----- GOT HERE BIG HIT ---------****");
					if(meleeSwingDirection == "bigShooshR" || meleeSwingDirection == "Proj_hazmat_big(Clone)"|| meleeSwingDirection == "Proj_hazmat_big"){
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
					controller.SendTrigger(EnemyTrigger.HIT);

				yield return new WaitForSeconds(.2f);
				this.gameObject.GetComponent<tk2dSprite>().color = Color.white;
				//gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
				//Debug.Log("**AND HERE!!!!!!!!***");
				yield return new WaitForSeconds(.4f);
				StartCoroutine( "StopKnockback",0f);
				StartCoroutine("AfterHit");
				}
			}
		}
	}

	public void TakeDamage(GameObject melee){ //set public for Stuart
		if(this.enabled && damageOnce == 0 && armorRating <= meleeDmgBonus){
			if(!takingDamage){
				takingDamage = true;
				damageOnce = 1;


				meleeSwingDirection = melee.GetComponent<tk2dSpriteAnimator>().CurrentClip.name;
				swingDirectionSide = PlayerManager.Instance.player.transform.localScale.x;
				//Debug.Log("MELEE SWING DIRECTION: " + meleeSwingDirection);

			
				if(moveWhenHit){
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
					
					damageCounter.GetComponent<Rigidbody2D>().AddForce(new Vector2(4f,10f), ForceMode2D.Impulse);
				}else{

					damageCounter.GetComponent<Rigidbody2D>().AddForce(new Vector2(-4f,10f), ForceMode2D.Impulse);

				}


				if(!moveWhenHit && controller.GetCurrentState() != EnemyState.LUNGE) {
					GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
				}

                // no moving on hit if requested, it's not hit by a thrown object or this is an enemy that is lunging.
                if (!moveWhenHit || (controller != null && controller.GetCurrentState() == EnemyState.LUNGE)) {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                }

                currentHp = currentHp - 1 - meleeDmgBonus  +armorRating;

				if(currentHp <= 0){
					CamManager.Instance.mainCam.ScreenShake(.2f,.4f);
					ObjectPool.Instance.GetPooledObject("effect_landingSmoke",gameObject.transform.position);
				    SoundManager.instance.PlaySingle(SFXBANK.HIT7, hitPitch);
                    if (controller != null)
                        controller.SendTrigger(EnemyTrigger.DEATH);
                } else{
					CamManager.Instance.mainCam.ScreenShake(.2f,.2f);
			
				    SoundManager.instance.PlaySingle(SFXBANK.HIT6, hitPitch);
				    if(hitPitch < 1.3f)
					    hitPitch += .1f; // pitch goes up as hit enemy

                    if (controller != null)
                        controller.SendTrigger(EnemyTrigger.HIT);
                }
				SoundManager.instance.PlaySingle(hitSqueal);

				//Debug.Log("GOT THIS FAR- ENEMY TAKE DAMGE 2");
				if(bossEnemy){
					gameObject.GetComponent<Boss>().UpdateBossHp(currentHp);
			
				}
				if(!moveWhenHit && currentHp <= 0){
					moveWhenHit = true; // enemy flies back at final hit
				}

                
                if (moveWhenHit){
                    UpdateFacing();
                }

				StartCoroutine("ContinueHit"); // just needed to seperate here for IEnumerator stuff
			}
		}

	}

    public void UpdateFacing()
    {
        if (gameObject.transform.position.x < PlayerManager.Instance.player.transform.position.x)
            gameObject.transform.localScale = new Vector3(startScale.x, startScale.y, startScale.z);
        else
            gameObject.transform.localScale = new Vector3(startScale.x * -1, startScale.y, startScale.z);
    }

	IEnumerator StopKnockback(float delay = 0f){
		yield return new WaitForSeconds(delay);
		if(delay > 1){ //if hit by strong force produce land cloud
		ObjectPool.Instance.GetPooledObject("effect_enemyLand",gameObject.transform.position);
			SoundManager.instance.PlaySingle(bounce);
		}
		spinning = false;
		//myCollisionBox.enabled = true;
		for(int i = 0; i < behaviorsToDeactivate.Count;i++){
						behaviorsToDeactivate[i].enabled = true;
		}

		Debug.Log("STOP KNOCKBACK ACTIVATE");
		damageOnce = 0;

		gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
		gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
		//if(aniToSwitchBackTo != null)
		if(powerHit){
			powerHit = false;
		}
		yield return new WaitForSeconds(.1f);

		takingDamage = false;

	}

	IEnumerator ContinueHit(){

		//remove effects from self



		if(moveWhenHit || powerHit){
			takingDamage = true;

			Debug.Log("-----MELEE WEAPON SWING DIRECTION :" + meleeSwingDirection);

			if(meleeSwingDirection.CompareTo("plankSwing") == 0||meleeSwingDirection.CompareTo("clawSwing") == 0||meleeSwingDirection.CompareTo("poleSwing") == 0){
				Debug.Log(swingDirectionSide);

				//myCollisionBox.enabled = false;

				if(swingDirectionSide < 0){
					if(currentHp <= 0 && !bossEnemy){
						spinning = true;
						myBody.mass = 1;
						myBody.AddForce(new Vector2(knockbackForce*-1,10f), ForceMode2D.Impulse);
						myBody.gravityScale = 3.3f;
						if(myShadow != null)
							myShadow.transform.parent = null; //shadow doesnt follow Y pos
					}else{
						Debug.Log("**Got here- enemy hit***" + moveWhenHit + currentHp);
						myBody.AddForce(new Vector2(knockbackForce*-1,0f), ForceMode2D.Impulse);
					}
				}else{
					if(currentHp <= 0 && !bossEnemy){
						spinning = true;
						myBody.mass = 1;
						gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(knockbackForce,10f), ForceMode2D.Impulse);
						myBody.gravityScale = 3.3f;
						if(myShadow != null)
							myShadow.transform.parent = null; //shadow doesnt follow Y pos	
					}else{
						Debug.Log("**Got here- enemy hit***");
						myBody.velocity = new Vector2(7f,0f);
						gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(knockbackForce,0f), ForceMode2D.Impulse);	
					}
				}




			}else if(meleeSwingDirection.CompareTo("plankUp") == 0||meleeSwingDirection.CompareTo("clawUp") == 0||meleeSwingDirection.CompareTo("poleUp") == 0){
				gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f,knockbackForce), ForceMode2D.Impulse);
				Debug.Log("ENEMY WAS HIT UP!!");
			}else if(meleeSwingDirection.CompareTo("plankDown") == 0||meleeSwingDirection.CompareTo("clawDown") == 0||meleeSwingDirection.CompareTo("poleDown") == 0){
				gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f,knockbackForce*-1), ForceMode2D.Impulse);
				Debug.Log("ENEMY WAS HIT down!!");
			}
				

			yield return new WaitForSeconds(.1f);
			if(IAmParentObj){
				childEnemy.gameObject.GetComponent<tk2dSprite>().color = Color.white;

			}else
				this.gameObject.GetComponent<tk2dSprite>().color = Color.white;

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

		StartCoroutine("AfterHit");
			
		

	}

    IEnumerator AfterHit()
    {
        if (currentHp > 0) {

            //disable follow player after notice behavior if end up having thta in game
            if (gameObject.GetComponent<RandomDirectionMovement>() != null && gameObject.GetComponent<RandomDirectionMovement>().enabled) {
                gameObject.GetComponent<RandomDirectionMovement>().StopMoving();
            }

            yield return new WaitForSeconds(.4f);

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


              
                dropsTrash = 99;
                if (dropsPin != 22) {
                    dropsTrash = Random.Range(1, (12 - sharingUpgrade));
                }
            
            GlobalVariableManager.Instance.ENEMIES_DEFEATED++;

            if (!bossEnemy) {
				yield return new WaitForSeconds(.2f);
                DropScrap();
                Death();
            }
            else {
                gameObject.GetComponent<Boss>().StartCoroutine("BossDeath");
            }

        }//end of (if hp is not > 0)


    }

	public void Clank(AudioClip clankSfx,Vector2 clankPosition, bool getsPushedBack){
		if(!takingDamage){
			Debug.Log("Clanking material got here----x-x-x-x--- 2");
			takingDamage = true;
			ObjectPool.Instance.GetPooledObject("effect_clank",clankPosition);
			SoundManager.instance.PlaySingle(clankSfx);

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

			spinning = false;
		
			gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
			gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
            GetComponent<EnemyStateController>().SendTrigger(EnemyTrigger.HIT);

            takingDamage = false;

			currentHp = 2; //TODO: this is just for brownmole, maybe get it from 'Enemy.cs"?

		}else{
			
			GlobalVariableManager.Instance.BASIC_ENEMY_LIST[this.mySpawnerID].dayOfRevival = GlobalVariableManager.Instance.DAY_NUMBER +3;
			Debug.Log("Day of revival: "+ GlobalVariableManager.Instance.BASIC_ENEMY_LIST[this.mySpawnerID].dayOfRevival);

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
