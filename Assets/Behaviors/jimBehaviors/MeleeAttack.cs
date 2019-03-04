using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour {


	public AudioClip swing;
	//public AudioSource audioSource;
	public GameObject meleeWeaponRightSwing;
	public GameObject meleeWeaponLeftSwing;
	public GameObject meleeWeaponTopSwing;
	public GameObject meleeWeaponBotSwing;
	public GameObject sideSwoosh;
	public GameObject topSwoosh;
	public GameObject botSwoosh;
	public GameObject topBigSwoosh;
	public GameObject botBigSwoosh;
	public GameObject sideBigSwoosh;
	public ParticleSystem chargePS;
	public ParticleSystem chargeReadyPS;
	public GameObject chargeReadyGlow;

	int swingDirection;
	float turningSpeed;
	private float playerMomentum; // a little 'bounce' when swing
	Vector3 startingScale;


	INPUTACTION heldKey;
	bool chargeReady;

	void Start () {
        startingScale = this.gameObject.transform.localScale;

		if(GlobalVariableManager.Instance.IsPinEquipped(PIN.DUCKSFX)){
			//duck sfx pin
			swing = (AudioClip)Resources.Load("sfx_duckSwing", typeof(AudioClip));
		}

		//reset values
		meleeWeaponRightSwing.GetComponent<tk2dSpriteAnimator>().Play("plankSwing");
		meleeWeaponRightSwing.transform.localPosition = new Vector2(3.5f,-1.28f);

		meleeWeaponBotSwing.GetComponent<tk2dSpriteAnimator>().Play("plankDown");
		meleeWeaponBotSwing.transform.localPosition = new Vector2(-1.28f,.2f);

		meleeWeaponTopSwing.GetComponent<tk2dSpriteAnimator>().Play("plankUp");
		meleeWeaponTopSwing.transform.localPosition = new Vector2(-.13f,.94f);

    }

    void Update () {
        if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
            switch (GetComponent<JimStateController>().GetCurrentState()) {
                case JimState.ATTACKING:
                    if (swingDirection == 1) {
                        transform.Translate(new Vector2(playerMomentum, 0) * Time.deltaTime);
				        this.gameObject.transform.localScale = startingScale; //always faces proper way

                    }
                    else if (swingDirection == 2) {
                        transform.Translate(new Vector2(playerMomentum * -1, 0) * Time.deltaTime);
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

	public void UpdateWeapon(){//activated by Ev_currentWeapon
		meleeWeaponRightSwing.transform.localPosition = new Vector2(meleeWeaponRightSwing.transform.localPosition.x+1.1f,meleeWeaponRightSwing.transform.localPosition.y);

		meleeWeaponLeftSwing.transform.localPosition = new Vector2(meleeWeaponLeftSwing.transform.localPosition.x-1.1f,meleeWeaponLeftSwing.transform.localPosition.y);

		meleeWeaponTopSwing.transform.localPosition = new Vector2(meleeWeaponTopSwing.transform.localPosition.x,meleeWeaponTopSwing.transform.localPosition.y+1.1f);

		meleeWeaponBotSwing.transform.localPosition = new Vector2(meleeWeaponBotSwing.transform.localPosition.x,meleeWeaponBotSwing.transform.localPosition.y-1.1f);

		if(meleeWeaponRightSwing.GetComponent<tk2dSpriteAnimator>().CurrentClip.name == "plankSwing"){ //check what animation to change to based on current ani, just check one of the directions

				meleeWeaponBotSwing.GetComponent<tk2dSpriteAnimator>().Play("clawDown");
				meleeWeaponBotSwing.transform.localPosition = new Vector2(-.47f,-1.46f);

				meleeWeaponTopSwing.GetComponent<tk2dSpriteAnimator>().Play("clawUp");
				meleeWeaponTopSwing.transform.localPosition = new Vector2(-.13f,1.81f);

				meleeWeaponRightSwing.GetComponent<tk2dSpriteAnimator>().Play("clawSwing");
				meleeWeaponRightSwing.transform.localPosition = new Vector2(4f,-2.08f);

		}else if(meleeWeaponRightSwing.GetComponent<tk2dSpriteAnimator>().CurrentClip.name == "clawSwing"){
				meleeWeaponRightSwing.GetComponent<tk2dSpriteAnimator>().Play("poleSwing");
				meleeWeaponRightSwing.transform.localPosition = new Vector2(4.8f,-2.06f);

				meleeWeaponBotSwing.GetComponent<tk2dSpriteAnimator>().Play("poleDown");
				meleeWeaponBotSwing.transform.localPosition = new Vector2(-1.28f,.2f);

				meleeWeaponTopSwing.GetComponent<tk2dSpriteAnimator>().Play("poleUp");
				meleeWeaponTopSwing.transform.localPosition = new Vector2(-.13f,.94f);

		}
	}
	public void DemoteWeapon(){//activated by PlayerTakeDamage
		meleeWeaponRightSwing.transform.localPosition = new Vector2(meleeWeaponRightSwing.transform.localPosition.x-1.1f,meleeWeaponRightSwing.transform.localPosition.y);

		meleeWeaponLeftSwing.transform.localPosition = new Vector2(meleeWeaponLeftSwing.transform.localPosition.x+1.1f,meleeWeaponLeftSwing.transform.localPosition.y);

		meleeWeaponTopSwing.transform.localPosition = new Vector2(meleeWeaponTopSwing.transform.localPosition.x,meleeWeaponTopSwing.transform.localPosition.y-1.1f);

		meleeWeaponBotSwing.transform.localPosition = new Vector2(meleeWeaponBotSwing.transform.localPosition.x,meleeWeaponBotSwing.transform.localPosition.y+1.1f);

		if(meleeWeaponRightSwing.GetComponent<tk2dSpriteAnimator>().CurrentClip.name == "poleSwing"){ //check what animation to change to based on current ani, just check one of the directions
				meleeWeaponRightSwing.GetComponent<tk2dSpriteAnimator>().Play("clawSwing");
				meleeWeaponRightSwing.transform.localPosition = new Vector2(4f,-1.28f);

				meleeWeaponBotSwing.GetComponent<tk2dSpriteAnimator>().Play("clawDown");
				meleeWeaponBotSwing.transform.localPosition = new Vector2(-.47f,-1.46f);

				meleeWeaponTopSwing.GetComponent<tk2dSpriteAnimator>().Play("clawUp");
				meleeWeaponTopSwing.transform.localPosition = new Vector2(-.13f,1.81f);

		}else if(meleeWeaponRightSwing.GetComponent<tk2dSpriteAnimator>().CurrentClip.name == "clawSwing"){
				meleeWeaponRightSwing.GetComponent<tk2dSpriteAnimator>().Play("plankSwing");
				meleeWeaponRightSwing.transform.localPosition = new Vector2(3.5f,-1.28f);

				meleeWeaponBotSwing.GetComponent<tk2dSpriteAnimator>().Play("plankDown");
				meleeWeaponBotSwing.transform.localPosition = new Vector2(-1.28f,.2f);

				meleeWeaponTopSwing.GetComponent<tk2dSpriteAnimator>().Play("plankUp");
				meleeWeaponTopSwing.transform.localPosition = new Vector2(-.13f,.94f);

		}
	}

	IEnumerator Swing(int direction){
			SoundManager.instance.RandomizeSfx(swing);
			GameObject meleeDirectionEnabled = null;
			swingDirection = direction;

            if (direction == 1){
                PlayerManager.Instance.controller.SendTrigger(JimTrigger.SWING_RIGHT);
                meleeDirectionEnabled = meleeWeaponRightSwing;
				sideSwoosh.GetComponent<tk2dSpriteAnimator>().Play();
			}else if(direction == 2){
                PlayerManager.Instance.controller.SendTrigger(JimTrigger.SWING_LEFT);
                meleeDirectionEnabled = meleeWeaponLeftSwing;
				sideSwoosh.GetComponent<tk2dSpriteAnimator>().Play();
			}else if(direction == 3){
                PlayerManager.Instance.controller.SendTrigger(JimTrigger.SWING_UP);
                meleeDirectionEnabled = meleeWeaponTopSwing;
				topSwoosh.GetComponent<tk2dSpriteAnimator>().Play();
			}else if(direction == 4){
                PlayerManager.Instance.controller.SendTrigger(JimTrigger.SWING_DOWN);
                meleeDirectionEnabled = meleeWeaponBotSwing;
                botSwoosh.GetComponent<tk2dSpriteAnimator>().Play();
			}

			meleeDirectionEnabled.GetComponent<tk2dSpriteAnimator>().Play();
			gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,0f);

		    if(GlobalVariableManager.Instance.IsPinEquipped(PIN.HEROOFGRIME) && GlobalVariableManager.Instance.HP_STAT.GetCurrent() == GlobalVariableManager.Instance.HP_STAT.GetMax()){
				gameObject.GetComponent<PinFunctionsManager>().HeroOfGrime(swingDirection,meleeDirectionEnabled.transform.position);
			}

			yield return new WaitForSeconds(.1f);
			meleeDirectionEnabled.SetActive(true);

			meleeDirectionEnabled.transform.GetChild(0).gameObject.SetActive(true);//swoosh


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
				yield return new WaitForSeconds(.1f);
				if(ControllerManager.Instance.GetKey(currentKey)){
					StartCoroutine("StrongSwingCharge",currentKey);
					meleeDirectionEnabled.transform.GetChild(0).gameObject.SetActive(false);
					yield return new WaitForSeconds(.1f);
					meleeDirectionEnabled.SetActive(false);
				}else{
					
					meleeDirectionEnabled.transform.GetChild(0).gameObject.SetActive(false);
					yield return new WaitForSeconds(.1f);

					meleeDirectionEnabled.SetActive(false);
				}
				
			
		
	}

	IEnumerator StrongSwingCharge(INPUTACTION givenKey){
		CamManager.Instance.mainCamEffects.ZoomInOut(1.3f,1f);

		gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		heldKey = givenKey;
		//chargingAttack = true;
		if (ControllerManager.Instance.GetKey(INPUTACTION.ATTACKLEFT)) {
	
			PlayerManager.Instance.controller.SendTrigger(JimTrigger.CHARGE_LEFT);
                       
	    } else if (ControllerManager.Instance.GetKey(INPUTACTION.ATTACKRIGHT)) {
			PlayerManager.Instance.controller.SendTrigger(JimTrigger.CHARGE_RIGHT);
                 
	    } else if (ControllerManager.Instance.GetKey(INPUTACTION.ATTACKDOWN)) {

			PlayerManager.Instance.controller.SendTrigger(JimTrigger.CHARGE_DOWN);

	    } else if (ControllerManager.Instance.GetKey(INPUTACTION.ATTACKUP)) {

			PlayerManager.Instance.controller.SendTrigger(JimTrigger.CHARGE_UP);

	    }
	    chargePS.gameObject.SetActive(true);
		yield return new WaitForSeconds(.4f);
		chargeReady = true;
		chargeReadyPS.Play();
		chargeReadyGlow.SetActive(true);
		Debug.Log("chargeReady");
		//ReturnFromSwing();
	
	}

	IEnumerator StrongSwing(){
		Debug.Log("Strong Swing Ienum activated -!-!-!-!-!-!-!-!-!-!");
		GameObject meleeDirectionEnabled = null;
		GameObject bigSwooshDirection = null;
		chargeReadyGlow.SetActive(false);

		if (heldKey == INPUTACTION.ATTACKLEFT) {
	    	playerMomentum = 6f;
			PlayerManager.Instance.controller.SendTrigger(JimTrigger.SWING_LEFT);
        	meleeDirectionEnabled = meleeWeaponLeftSwing;
        	sideBigSwoosh.SetActive(true);
        	bigSwooshDirection = sideBigSwoosh;
		                  
	    } else if (heldKey == INPUTACTION.ATTACKRIGHT) {
			PlayerManager.Instance.controller.SendTrigger(JimTrigger.SWING_RIGHT);
            meleeDirectionEnabled = meleeWeaponRightSwing;
		
	        playerMomentum = 6f;
	        bigSwooshDirection = sideBigSwoosh;
	        sideBigSwoosh.SetActive(true);                   
	    } else if (heldKey == INPUTACTION.ATTACKDOWN) {
	        playerMomentum = 6f;
			PlayerManager.Instance.controller.SendTrigger(JimTrigger.SWING_DOWN);
	        botBigSwoosh.SetActive(true);
	        bigSwooshDirection = botBigSwoosh;
            meleeDirectionEnabled = meleeWeaponBotSwing;
	    } else if (heldKey == INPUTACTION.ATTACKUP) {
	        playerMomentum = 6f;
	        topBigSwoosh.SetActive(true);
			PlayerManager.Instance.controller.SendTrigger(JimTrigger.SWING_UP);
			bigSwooshDirection = topBigSwoosh;
            meleeDirectionEnabled = meleeWeaponTopSwing;
		
	    }

		meleeDirectionEnabled.GetComponent<tk2dSpriteAnimator>().Play();
		bigSwooshDirection.GetComponent<tk2dSpriteAnimator>().PlayFromFrame(0);

		meleeDirectionEnabled.SetActive(true);

		//meleeDirectionEnabled.transform.GetChild(0).gameObject.SetActive(true);//swoosh
		yield return new WaitForSeconds(.2f);
		meleeDirectionEnabled.transform.GetChild(0).gameObject.SetActive(false);
		meleeDirectionEnabled.SetActive(false);
		//chargingAttack = false;
		CamManager.Instance.mainCamEffects.ReturnFromCamEffect();
		chargeReady = false;
		PlayerManager.Instance.controller.SendTrigger(JimTrigger.IDLE);
		bigSwooshDirection.SetActive(false);
	}


}