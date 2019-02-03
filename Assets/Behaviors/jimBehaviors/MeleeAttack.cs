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


	public bool cantAttack = false;
	bool isSwinging = false;
	int swingDirection;
	float turningSpeed;
	private float playerMomentum; // a little 'bounce' when swing
	Vector3 startingScale;

	void Start () {
        startingScale = this.gameObject.transform.localScale;
		if(GlobalVariableManager.Instance.IsPinEquipped(PIN.CURSED) || GlobalVariableManager.Instance.CARRYING_SOMETHING == true){
            //Cursed pin- can't attack || carrying large trash- can't attack
            Debug.Log("Cant attack because of here");
			cantAttack = true;
		}
		if(GlobalVariableManager.Instance.IsPinEquipped(PIN.DUCKSFX)){
			//duck sfx pin
			swing = (AudioClip)Resources.Load("sfx_duckSwing", typeof(AudioClip));
			Debug.Log("Cant attack because of HHHHEEEERRREE");
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
        if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState) && !GlobalVariableManager.Instance.CARRYING_SOMETHING) {
            if (isSwinging) {
                if (swingDirection == 1) {
                    //weapon.transform.position = new Vector2(gameObject.transform.position.x + 4f,gameObject.transform.position.y+ 1.4f);
                    transform.Translate(new Vector2(playerMomentum, 0) * Time.deltaTime);
					this.gameObject.transform.localScale = startingScale; //always faces proper way

                }
                else if (swingDirection == 2) {
                    //weapon.transform.position = new Vector2(gameObject.transform.position.x,gameObject.transform.position.y + 1.4f);
                    transform.Translate(new Vector2(playerMomentum * -1, 0) * Time.deltaTime);
					this.gameObject.transform.localScale = new Vector3(startingScale.x * -1, startingScale.y, startingScale.z); //always faces left

                }
                else if (swingDirection == 3) {//swing up
                    transform.Translate(new Vector2(0, playerMomentum) * Time.deltaTime);
                    //weapon.transform.position = new Vector2(gameObject.transform.position.x -.5f,gameObject.transform.position.y + 2f);
                }
                else if (swingDirection == 4) {//swing down
                    transform.Translate(new Vector2(0, playerMomentum * -1) * Time.deltaTime);
                    //weapon.transform.position = new Vector2(gameObject.transform.position.x + 2f,gameObject.transform.position.y + 2f);
                }
                playerMomentum -= .5f;
            }
            if (!cantAttack && Time.timeScale != 0f) {
                //timescale part = if game isnt paused
                if (GlobalVariableManager.Instance.TRASH_TYPE_SELECTED == 3 && !isSwinging && GlobalVariableManager.Instance.PLAYER_CAN_MOVE &&
                 !gameObject.GetComponent<tk2dSpriteAnimator>().IsPlaying("hurtL") && !gameObject.GetComponent<tk2dSpriteAnimator>().IsPlaying("hurtR")) {

                    if (ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKLEFT)) {
                        playerMomentum = 6f;
                        this.gameObject.transform.localScale = new Vector3(startingScale.x * -1, startingScale.y, startingScale.z);
                        StartCoroutine("Swing", 2);
                    }
                    else if (ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKRIGHT)) {
                        this.gameObject.transform.localScale = startingScale;
                        playerMomentum = 6f;
                        StartCoroutine("Swing", 1);
                    }
                    else if (ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKDOWN)) {
                        this.gameObject.transform.localScale = startingScale;
                        playerMomentum = 6f;
                        StartCoroutine("Swing", 4);
                    }
                    else if (ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKUP)) {
                        this.gameObject.transform.localScale = startingScale;
                        playerMomentum = 6f;
                        StartCoroutine("Swing", 3);
                    }

                  
                }
                else {
                    //Debug.Log("something wrong with global var checks");
                    //Debug.Log(GlobalVariableManager.Instance.TRASH_TYPE_SELECTED);
                    //Debug.Log(GlobalVariableManager.Instance.PLAYER_CAN_MOVE);
                    //Debug.Log(GlobalVariableManager.Instance.MENU_SELECT_STAGE);
                }
            }
            else {
                //Debug.Log("Something wrong at MeleeAttack script, prob with pausing");
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

		}/*else if(meleeWeaponRightSwing.GetComponent<tk2dSpriteAnimator>().CurrentClip.name == "poleSwing"){
				meleeWeaponRightSwing.GetComponent<tk2dSpriteAnimator>().Play("broomSwing");
				meleeWeaponRightSwing.transform.localPosition = new Vector2(6.73f,-2.68f);

				meleeWeaponBotSwing.GetComponent<tk2dSpriteAnimator>().Play("broomDown");
				meleeWeaponBotSwing.transform.localPosition = new Vector2(.85f,-3.83f);

				meleeWeaponTopSwing.GetComponent<tk2dSpriteAnimator>().Play("broomUp");
				meleeWeaponTopSwing.transform.localPosition = new Vector2(-.13f,3.31f);

		}*/

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

		}/*else if(meleeWeaponRightSwing.GetComponent<tk2dSpriteAnimator>().CurrentClip.name == "broomSwing"){
				meleeWeaponRightSwing.GetComponent<tk2dSpriteAnimator>().Play("poleSwing");
				meleeWeaponRightSwing.transform.localPosition = new Vector2(4.8f,-2.01f);

				meleeWeaponBotSwing.GetComponent<tk2dSpriteAnimator>().Play("poleDown");
				meleeWeaponBotSwing.transform.localPosition = new Vector2(.6f,-2.9f);

				meleeWeaponTopSwing.GetComponent<tk2dSpriteAnimator>().Play("poleUp");
				meleeWeaponBotSwing.transform.localPosition = new Vector2(-.13f,2.56f);

		}*/

	}

	IEnumerator Swing(int direction){
			SoundManager.instance.RandomizeSfx(swing);
			GlobalVariableManager.Instance.PLAYER_CAN_MOVE = false;
			GameObject meleeDirectionEnabled = null;
			swingDirection = direction;
			if(direction == 1){
				meleeDirectionEnabled = meleeWeaponRightSwing;
				gameObject.GetComponent<JimAnimationManager>().PlayAnimation("ani_jimSwingR",true);
				sideSwoosh.GetComponent<tk2dSpriteAnimator>().Play();
				//sideSwoosh.GetComponent<tk2dSpriteAnimator>().PlayFrom(0);
				//gameObject.GetComponent<tk2dSpriteAnimator>().Play("ani_jimSwingR");
			}else if(direction == 2){
				meleeDirectionEnabled = meleeWeaponLeftSwing;
				gameObject.GetComponent<JimAnimationManager>().PlayAnimation("ani_jimSwingR",true);
				sideSwoosh.GetComponent<tk2dSpriteAnimator>().Play();
				//sideSwoosh.GetComponent<tk2dSpriteAnimator>().PlayFrom(0);
				//gameObject.GetComponent<tk2dSpriteAnimator>().Play("ani_jimSwingR");
			}else if(direction == 3){
				//gameObject.GetComponent<tk2dSpriteAnimator>().Play("ani_jimSwingUp");
				gameObject.GetComponent<JimAnimationManager>().PlayAnimation("ani_jimSwingUp",true);
				meleeDirectionEnabled = meleeWeaponTopSwing;
			
				topSwoosh.GetComponent<tk2dSpriteAnimator>().Play();
				//topSwoosh.GetComponent<tk2dSpriteAnimator>().PlayFrom(0);
			}else if(direction == 4){
				//gameObject.GetComponent<tk2dSpriteAnimator>().Play("ani_jimSwingDown");
				botSwoosh.GetComponent<tk2dSpriteAnimator>().Play();
				//botSwoosh.GetComponent<tk2dSpriteAnimator>().PlayFrom(0);
				gameObject.GetComponent<JimAnimationManager>().PlayAnimation("ani_jimSwingDown",true);
				meleeDirectionEnabled = meleeWeaponBotSwing;

			}
			meleeDirectionEnabled.GetComponent<tk2dSpriteAnimator>().Play();

			//meleeDirectionEnabled.SetActive(true);


			//meleeDirectionEnabled.GetComponent<tk2dSpriteAnimator>().Play();

			isSwinging = true;
			gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,0f);

		    if(GlobalVariableManager.Instance.IsPinEquipped(PIN.HEROOFGRIME) && GlobalVariableManager.Instance.HP_STAT.GetCurrent() == GlobalVariableManager.Instance.HP_STAT.GetMax()){
				gameObject.GetComponent<PinFunctionsManager>().HeroOfGrime(swingDirection,meleeDirectionEnabled.transform.position);
			}

			yield return new WaitForSeconds(.1f);
			meleeDirectionEnabled.SetActive(true);

			meleeDirectionEnabled.transform.GetChild(0).gameObject.SetActive(true);//swoosh
			/*if(GlobalVariableManager.Instance.IsPinEquipped(PIN.LINKTOTRASH) && (
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


				//yield return new WaitForSeconds(.3f);
				isSwinging = false; //prevents momentum from pushin player while twisting
				cantAttack = true;

				if(ControllerManager.Instance.GetKey(currentKey)){
					gameObject.GetComponent<JimAnimationManager>().PlayAnimation("spinAttack",true);
					gameObject.GetComponent<PinFunctionsManager>().StartCoroutine("SpinAttack",currentKey);
					meleeDirectionEnabled.SetActive(false);
				}		    		
                    	
            }else{*/
				if(!GlobalVariableManager.Instance.IsPinEquipped(PIN.SCRAPPYSHINOBI)){
					//Scrappy Shinobi
					yield return new WaitForSeconds(.1f);
					meleeDirectionEnabled.transform.GetChild(0).gameObject.SetActive(false);
					yield return new WaitForSeconds(.1f);
					ReturnFromSwing();
					meleeDirectionEnabled.SetActive(false);
				}else{
					ReturnFromSwing();
					meleeDirectionEnabled.SetActive(false);
				}
			//}
		
	}


	public void SetCanAttack(bool val){
		cantAttack = val;
	}

	public void ReturnFromSwing(){

		GlobalVariableManager.Instance.PLAYER_CAN_MOVE = true;
		//gameObject.GetComponent<JimAnimationManager>().PlayAnimation("ani_jimIdle",false);
		//GetComponent<EightWayMovement>().clipOverride = false;
		gameObject.GetComponent<JimAnimationManager>().PlayAnimation("ani_jimWalk", false);

		//gameObject.GetComponent<tk2dSpriteAnimator>().Play("ani_jimIdle");
		isSwinging = false;

	}
}

