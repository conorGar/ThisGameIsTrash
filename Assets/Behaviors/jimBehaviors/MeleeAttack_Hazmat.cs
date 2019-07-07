using UnityEngine;
using System.Collections;

public class MeleeAttack_Hazmat : MeleeAttack
{
	public GameObject projectile;
	public GameObject bigProjectile;
	public Vector2 projectileBaseSpeed;
	public tk2dSpriteCollectionData hazmatSpriteCollection;
	public tk2dSpriteAnimation hazmatSpriteAnimation;

	Vector2 projectileSpeed;
	// Use this for initialization
	void Start ()
	{
		startingScale = this.gameObject.transform.localScale;
		//change visuals
		gameObject.GetComponent<tk2dBaseSprite>().SetSprite(hazmatSpriteCollection,0);
		gameObject.GetComponent<tk2dSpriteAnimator>().Library = hazmatSpriteAnimation;
		gameObject.GetComponent<tk2dSpriteAnimator>().Play("ani_jimIdle");

	}
	
	void Update () {
        if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
            switch (GetComponent<JimStateController>().GetCurrentState()) {
                case JimState.ATTACKING:
                    if (swingDirection == 1) {
				        this.gameObject.transform.localScale = startingScale; //always faces proper way

                    }
                    else if (swingDirection == 2) {
				        this.gameObject.transform.localScale = new Vector3(startingScale.x * -1, startingScale.y, startingScale.z); //always faces left

                    }
                   
                    break;
                case JimState.IDLE:
                    
                        if (ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKLEFT)) {
                            //playerMomentum = 6f;
                            this.gameObject.transform.localScale = new Vector3(startingScale.x * -1, startingScale.y, startingScale.z);
                            StartCoroutine("Swing", 2);
                        } else if (ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKRIGHT)) {
                            this.gameObject.transform.localScale = startingScale;
                            //playerMomentum = 6f;
                            StartCoroutine("Swing", 1);
                        } else if (ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKDOWN)) {
                            this.gameObject.transform.localScale = startingScale;
                            //playerMomentum = 6f;
                            StartCoroutine("Swing", 4);
                        } else if (ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKUP)) {
                            this.gameObject.transform.localScale = startingScale;
                            //playerMomentum = 6f;
                            StartCoroutine("Swing", 3);
                        }
                    
                    break;
                case JimState.CHARGING:
					if(ControllerManager.Instance.GetKeyUp(heldKey)){
						// charge attack unleashed at release of key
						chargePS.gameObject.SetActive(false);

						if(chargeReady){
							StartCoroutine("StrongSwing");
						}else{
                    		Debug.Log("ChargingAttack cancel");
                    		StopCoroutine("StrongSwingCharge");
							CamManager.Instance.mainCamEffects.ReturnFromCamEffect();
							PlayerManager.Instance.controller.SendTrigger(JimTrigger.IDLE);
						}
                    }
                	break;
            }
        }
        if(playerMomentum >0){
			playerMomentum -= .5f;
        }else{
			playerMomentum  = 0f;
        }

	}//end of update method

	protected override IEnumerator Swing(int direction){
		SoundManager.instance.RandomizeSfx(swing);
			GameObject meleeDirectionEnabled = null;
			swingDirection = direction;

			GameObject bullet = ObjectPool.Instance.GetPooledObject(projectile.tag,gameObject.transform.position);

			if(bullet.GetComponent<Ev_ProjectileBasic>() != null){
				if(direction == 1){
				projectileSpeed = new Vector2(projectileBaseSpeed.x,0);
			}else if(direction==2){
				projectileSpeed = new Vector2(projectileBaseSpeed.x *-1,0);
			}else if(direction==3){
				projectileSpeed = new Vector2(0,projectileBaseSpeed.y);
			}else{
				projectileSpeed = new Vector2(0,projectileBaseSpeed.y*-1);

			}
			bullet.GetComponent<Ev_ProjectileBasic>().speedX = projectileSpeed.x;
			bullet.GetComponent<Ev_ProjectileBasic>().speedY = projectileSpeed.y;
			}
			bullet.GetComponent<Rigidbody2D>().gravityScale = 0;

	

			gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,0f);



			INPUTACTION currentKey = INPUTACTION.ATTACKRIGHT;
				if(direction ==1){
					currentKey = INPUTACTION.ATTACKRIGHT;
				}else if(direction == 2){
					currentKey = INPUTACTION.ATTACKLEFT;
				}else if(direction == 3){
					currentKey = INPUTACTION.ATTACKUP;
				}else if(direction == 4){
					currentKey = INPUTACTION.ATTACKDOWN;
				}
				yield return new WaitForSeconds(.4f);
				CancelInvoke();
				if(ControllerManager.Instance.GetKey(currentKey)){
				StartCoroutine("StrongSwingCharge",currentKey);

					yield return new WaitForSeconds(.1f);
				}else{
					
					//meleeDirectionEnabled.transform.GetChild(0).gameObject.SetActive(false);
					yield return new WaitForSeconds(.1f);

				}
				
			
	}




	protected override IEnumerator StrongSwing(){
		chargeReadyGlow.SetActive(false);
		GameObject bullet = ObjectPool.Instance.GetPooledObject(bigProjectile.tag,gameObject.transform.position);

		if (heldKey == INPUTACTION.ATTACKLEFT) {
	
			PlayerManager.Instance.controller.SendTrigger(JimTrigger.SWING_LEFT);
			projectileSpeed = new Vector2(projectileBaseSpeed.x*-1,0);

		                  
	    } else if (heldKey == INPUTACTION.ATTACKRIGHT) {
			PlayerManager.Instance.controller.SendTrigger(JimTrigger.SWING_RIGHT);
			projectileSpeed = new Vector2(projectileBaseSpeed.x,0);                 
	    } else if (heldKey == INPUTACTION.ATTACKDOWN) {
	 
			PlayerManager.Instance.controller.SendTrigger(JimTrigger.SWING_DOWN);
			projectileSpeed = new Vector2(0,projectileBaseSpeed.y);
	    } else if (heldKey == INPUTACTION.ATTACKUP) {
	      
			PlayerManager.Instance.controller.SendTrigger(JimTrigger.SWING_UP);
			projectileSpeed = new Vector2(0,projectileBaseSpeed.y*-1);
		
	    }

		bullet.GetComponent<Ev_ProjectileBasic>().speedX = projectileSpeed.x;
		bullet.GetComponent<Ev_ProjectileBasic>().speedY = projectileSpeed.y;
		bullet.GetComponent<Rigidbody2D>().gravityScale = 0;

	
		
		CamManager.Instance.mainCamEffects.ReturnFromCamEffect();
		chargeReady = false;
		PlayerManager.Instance.controller.SendTrigger(JimTrigger.IDLE);
		yield return null;
	}

}

