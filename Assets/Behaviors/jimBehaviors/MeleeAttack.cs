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

	int swingDirection;
	float turningSpeed;
	private float playerMomentum; // a little 'bounce' when swing
	Vector3 startingScale;

	void Start () {
        startingScale = this.gameObject.transform.localScale;

		if(GlobalVariableManager.Instance.IsPinEquipped(PIN.DUCKSFX)){
			//duck sfx pin
			swing = (AudioClip)Resources.Load("sfx_duckSwing", typeof(AudioClip));
		}

		//reset values
		meleeWeaponRightSwing.GetComponent<tk2dSpriteAnimator>().Play("plankSwing");
		meleeWeaponRightSwing.transform.localPosition = new Vector2(2.3f,-1.28f);

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
                    playerMomentum -= .5f;
                    break;
                case JimState.IDLE:
                    // Can't swing with the cursed pin.
                    if (!GlobalVariableManager.Instance.IsPinEquipped(PIN.CURSED)) {
                        if (ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKLEFT)) {
                            playerMomentum = 6f;
                            this.gameObject.transform.localScale = new Vector3(startingScale.x * -1, startingScale.y, startingScale.z);
                            StartCoroutine("Swing", 2);
                        } else if (ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKRIGHT)) {
                            this.gameObject.transform.localScale = startingScale;
                            playerMomentum = 6f;
                            StartCoroutine("Swing", 1);
                        } else if (ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKDOWN)) {
                            this.gameObject.transform.localScale = startingScale;
                            playerMomentum = 6f;
                            StartCoroutine("Swing", 4);
                        } else if (ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKUP)) {
                            this.gameObject.transform.localScale = startingScale;
                            playerMomentum = 6f;
                            StartCoroutine("Swing", 3);
                        }
                    }
                    break;
            }
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
				meleeWeaponRightSwing.transform.localPosition = new Vector2(2.3f,-1.28f);

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
			if(GlobalVariableManager.Instance.IsPinEquipped(PIN.LINKTOTRASH) && (
			ControllerManager.Instance.GetKey(INPUTACTION.ATTACKRIGHT) ||ControllerManager.Instance.GetKey(INPUTACTION.ATTACKLEFT) || ControllerManager.Instance.GetKey(INPUTACTION.ATTACKUP) || ControllerManager.Instance.GetKey(INPUTACTION.ATTACKDOWN))){
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

				if(ControllerManager.Instance.GetKey(currentKey)){
					gameObject.GetComponent<JimAnimationManager>().PlayAnimation("spinAttack",true);
					gameObject.GetComponent<PinFunctionsManager>().StartCoroutine("SpinAttack",currentKey);
					meleeDirectionEnabled.SetActive(false);
				}		    		
                    	
            }else{
				if(!GlobalVariableManager.Instance.IsPinEquipped(PIN.SCRAPPYSHINOBI)){
					//Scrappy Shinobi
					yield return new WaitForSeconds(.1f);
					meleeDirectionEnabled.transform.GetChild(0).gameObject.SetActive(false);
					yield return new WaitForSeconds(.1f);
					meleeDirectionEnabled.SetActive(false);
				}else{
					meleeDirectionEnabled.SetActive(false);
				}
			}
		
	}
}