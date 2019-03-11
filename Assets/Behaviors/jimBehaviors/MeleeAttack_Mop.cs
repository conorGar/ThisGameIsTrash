using UnityEngine;
using System.Collections;

public class MeleeAttack_Mop : MeleeAttack
{

	void Start(){
		startingScale = this.gameObject.transform.localScale;

	}

	void Update () {
        if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
            switch (GetComponent<JimStateController>().GetCurrentState()) {
                case JimState.ATTACKING:
                    if (swingDirection == 1) {
                        transform.Translate(new Vector2(playerMomentum+3f, 0) * Time.deltaTime); // +2 to PM because side swipes didn't seem to go as far as top/bot ones...
				        this.gameObject.transform.localScale = startingScale; //always faces proper way

                    }
                    else if (swingDirection == 2) {
                        transform.Translate(new Vector2(playerMomentum+3f * -1, 0) * Time.deltaTime);
				        this.gameObject.transform.localScale = new Vector3(startingScale.x * -1, startingScale.y, startingScale.z); //always faces left

                    }
                    else if (swingDirection == 3) {//swing up
                        transform.Translate(new Vector2(0, playerMomentum) * Time.deltaTime);

                    }
                    else if (swingDirection == 4) {//swing down
                        transform.Translate(new Vector2(0, playerMomentum * -1) * Time.deltaTime);

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

          

			yield return new WaitForSeconds(.3f);
			InvokeRepeating("TrailSpawn",.01f,.1f);

			if (direction == 1){
                PlayerManager.Instance.controller.SendTrigger(JimTrigger.SWING_RIGHT);
                meleeDirectionEnabled = meleeWeaponRightSwing;
				//sideSwoosh.GetComponent<tk2dSpriteAnimator>().Play();
			}else if(direction == 2){
                PlayerManager.Instance.controller.SendTrigger(JimTrigger.SWING_LEFT);
                meleeDirectionEnabled = meleeWeaponLeftSwing;
				//sideSwoosh.GetComponent<tk2dSpriteAnimator>().Play();
			}else if(direction == 3){
                PlayerManager.Instance.controller.SendTrigger(JimTrigger.SWING_UP);
                meleeDirectionEnabled = meleeWeaponTopSwing;
				//topSwoosh.GetComponent<tk2dSpriteAnimator>().Play();
			}else if(direction == 4){
                PlayerManager.Instance.controller.SendTrigger(JimTrigger.SWING_DOWN);
                meleeDirectionEnabled = meleeWeaponBotSwing;
               //botSwoosh.GetComponent<tk2dSpriteAnimator>().Play();
			}

			//meleeDirectionEnabled.GetComponent<tk2dSpriteAnimator>().Play();
			gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,0f);
			playerMomentum = 10f;
			meleeDirectionEnabled.SetActive(true);

			//meleeDirectionEnabled.transform.GetChild(0).gameObject.SetActive(true);//swoosh


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
					meleeDirectionEnabled.transform.GetChild(0).gameObject.SetActive(false);
					yield return new WaitForSeconds(.1f);
					meleeDirectionEnabled.SetActive(false);
				}else{
					
					//meleeDirectionEnabled.transform.GetChild(0).gameObject.SetActive(false);
					yield return new WaitForSeconds(.1f);

					meleeDirectionEnabled.SetActive(false);
				}
				
			
	}

	void TrailSpawn(){
		if(gameObject.activeInHierarchy){
			GameObject trail = ObjectPool.Instance.GetPooledObject("mopTrail",new Vector2( this.gameObject.transform.position.x, gameObject.transform.position.y -1f));
			trail.GetComponent<Animator>().Play("generalFadeOut",-1,0f);
		}else{
			CancelInvoke();
		}
	}
}

