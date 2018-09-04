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

	bool cantAttack = false;
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

	}

	void Update () {
		if(isSwinging){
			if(swingDirection ==1){
				//weapon.transform.position = new Vector2(gameObject.transform.position.x + 4f,gameObject.transform.position.y+ 1.4f);
				transform.Translate(new Vector2(playerMomentum,0)*Time.deltaTime);
			}else if(swingDirection ==2){
				//weapon.transform.position = new Vector2(gameObject.transform.position.x,gameObject.transform.position.y + 1.4f);
				transform.Translate(new Vector2(playerMomentum*-1,0)*Time.deltaTime);
			}else if(swingDirection == 3){//swing up
				transform.Translate(new Vector2(0,playerMomentum)*Time.deltaTime);
				//weapon.transform.position = new Vector2(gameObject.transform.position.x -.5f,gameObject.transform.position.y + 2f);
			}else if(swingDirection == 4){//swing down
				transform.Translate(new Vector2(0,playerMomentum*-1)*Time.deltaTime);
				//weapon.transform.position = new Vector2(gameObject.transform.position.x + 2f,gameObject.transform.position.y + 2f);
			}
			playerMomentum -= .5f;
		}
		if(!cantAttack && Time.timeScale != 0f){
			//timescale part = if game isnt paused
			if(GlobalVariableManager.Instance.TRASH_TYPE_SELECTED == 3 && !isSwinging && GlobalVariableManager.Instance.PLAYER_CAN_MOVE && 
			 !gameObject.GetComponent<tk2dSpriteAnimator>().IsPlaying("hurtL") && !gameObject.GetComponent<tk2dSpriteAnimator>().IsPlaying("hurtR")){

				if(ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKLEFT)){
					playerMomentum = 6f;
					this.gameObject.transform.localScale = new Vector3(startingScale.x*-1,startingScale.y, startingScale.z);
					StartCoroutine("Swing",2);
				}else if(ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKRIGHT)){
					this.gameObject.transform.localScale = startingScale;
					playerMomentum = 6f;
					StartCoroutine("Swing",1);
				}else if(ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKDOWN)){
					this.gameObject.transform.localScale = startingScale;
					playerMomentum = 6f;
					StartCoroutine("Swing",4);
				}else if(ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKUP)){
					this.gameObject.transform.localScale = startingScale;
					playerMomentum = 6f;
					StartCoroutine("Swing",3);
				}
			}else{
				//Debug.Log("something wrong with global var checks");
				//Debug.Log(GlobalVariableManager.Instance.TRASH_TYPE_SELECTED);
				//Debug.Log(GlobalVariableManager.Instance.PLAYER_CAN_MOVE);
				//Debug.Log(GlobalVariableManager.Instance.MENU_SELECT_STAGE);
			}
		}else{
			//Debug.Log("Something wrong at MeleeAttack script, prob with pausing");
		}



	}//end of update method

	public void UpdateWeapon(){//activated by Ev_currentWeapon
		meleeWeaponRightSwing.transform.localPosition = new Vector2(meleeWeaponRightSwing.transform.localPosition.x+1.1f,meleeWeaponRightSwing.transform.localPosition.y);

		meleeWeaponLeftSwing.transform.localPosition = new Vector2(meleeWeaponLeftSwing.transform.localPosition.x-1.1f,meleeWeaponLeftSwing.transform.localPosition.y);

		meleeWeaponTopSwing.transform.localPosition = new Vector2(meleeWeaponTopSwing.transform.localPosition.x,meleeWeaponTopSwing.transform.localPosition.y+1.1f);

		meleeWeaponBotSwing.transform.localPosition = new Vector2(meleeWeaponBotSwing.transform.localPosition.x,meleeWeaponBotSwing.transform.localPosition.y-1.1f);

		if(meleeWeaponRightSwing.GetComponent<tk2dSpriteAnimator>().CurrentClip.name == "plankSwing"){ //check what animation to change to based on current ani, just check one of the directions
				meleeWeaponRightSwing.GetComponent<tk2dSpriteAnimator>().Play("clawSwing");
				meleeWeaponBotSwing.GetComponent<tk2dSpriteAnimator>().Play("clawDown");
				meleeWeaponBotSwing.GetComponent<tk2dSpriteAnimator>().Play("clawUp");
		}else if(meleeWeaponRightSwing.GetComponent<tk2dSpriteAnimator>().CurrentClip.name == "clawSwing"){
				meleeWeaponRightSwing.GetComponent<tk2dSpriteAnimator>().Play("poleSwing");
				meleeWeaponBotSwing.GetComponent<tk2dSpriteAnimator>().Play("poleDown");
				meleeWeaponBotSwing.GetComponent<tk2dSpriteAnimator>().Play("poleUp");
		}else if(meleeWeaponRightSwing.GetComponent<tk2dSpriteAnimator>().CurrentClip.name == "poleSwing"){
				meleeWeaponRightSwing.GetComponent<tk2dSpriteAnimator>().Play("broomSwing");
				meleeWeaponBotSwing.GetComponent<tk2dSpriteAnimator>().Play("broomDown");
				meleeWeaponBotSwing.GetComponent<tk2dSpriteAnimator>().Play("broomUp");
		}

	}
	public void DemoteWeapon(){//activated by PlayerTakeDamage
		meleeWeaponRightSwing.transform.localPosition = new Vector2(meleeWeaponRightSwing.transform.localPosition.x-1.1f,meleeWeaponRightSwing.transform.localPosition.y);

		meleeWeaponLeftSwing.transform.localPosition = new Vector2(meleeWeaponLeftSwing.transform.localPosition.x+1.1f,meleeWeaponLeftSwing.transform.localPosition.y);

		meleeWeaponTopSwing.transform.localPosition = new Vector2(meleeWeaponTopSwing.transform.localPosition.x,meleeWeaponTopSwing.transform.localPosition.y-1.1f);

		meleeWeaponBotSwing.transform.localPosition = new Vector2(meleeWeaponBotSwing.transform.localPosition.x,meleeWeaponBotSwing.transform.localPosition.y+1.1f);

		if(meleeWeaponRightSwing.GetComponent<tk2dSpriteAnimator>().CurrentClip.name == "poleSwing"){ //check what animation to change to based on current ani, just check one of the directions
				meleeWeaponRightSwing.GetComponent<tk2dSpriteAnimator>().Play("clawSwing");
				meleeWeaponBotSwing.GetComponent<tk2dSpriteAnimator>().Play("clawDown");
				meleeWeaponBotSwing.GetComponent<tk2dSpriteAnimator>().Play("clawUp");
		}else if(meleeWeaponRightSwing.GetComponent<tk2dSpriteAnimator>().CurrentClip.name == "clawSwing"){
				meleeWeaponRightSwing.GetComponent<tk2dSpriteAnimator>().Play("plankSwing");
				meleeWeaponBotSwing.GetComponent<tk2dSpriteAnimator>().Play("plankDown");
				meleeWeaponBotSwing.GetComponent<tk2dSpriteAnimator>().Play("plankUp");
		}else if(meleeWeaponRightSwing.GetComponent<tk2dSpriteAnimator>().CurrentClip.name == "broomSwing"){
				meleeWeaponRightSwing.GetComponent<tk2dSpriteAnimator>().Play("poleSwing");
				meleeWeaponBotSwing.GetComponent<tk2dSpriteAnimator>().Play("poleDown");
				meleeWeaponBotSwing.GetComponent<tk2dSpriteAnimator>().Play("poleUp");
		}

	}

	IEnumerator Swing(int direction){
		if(!(Input.GetKey(KeyCode.LeftShift) && gameObject.GetComponent<ThrowTrash>().enabled)){ //if holding shift down throw instead
			SoundManager.instance.RandomizeSfx(swing);
			GlobalVariableManager.Instance.PLAYER_CAN_MOVE = false;
			GameObject meleeDirectionEnabled = null;
			swingDirection = direction;
			if(direction == 1){
				meleeDirectionEnabled = meleeWeaponRightSwing;
				gameObject.GetComponent<JimAnimationManager>().PlayAnimation("ani_jimSwingR",true);
				//gameObject.GetComponent<tk2dSpriteAnimator>().Play("ani_jimSwingR");
			}else if(direction == 2){
				meleeDirectionEnabled = meleeWeaponLeftSwing;
				gameObject.GetComponent<JimAnimationManager>().PlayAnimation("ani_jimSwingR",true);
				//gameObject.GetComponent<tk2dSpriteAnimator>().Play("ani_jimSwingR");
			}else if(direction == 3){
				//gameObject.GetComponent<tk2dSpriteAnimator>().Play("ani_jimSwingUp");
				gameObject.GetComponent<JimAnimationManager>().PlayAnimation("ani_jimSwingUp",true);
				meleeDirectionEnabled = meleeWeaponTopSwing;

			}else if(direction == 4){
				//gameObject.GetComponent<tk2dSpriteAnimator>().Play("ani_jimSwingDown");
				gameObject.GetComponent<JimAnimationManager>().PlayAnimation("ani_jimSwingDown",true);
				meleeDirectionEnabled = meleeWeaponBotSwing;

			}
			meleeDirectionEnabled.GetComponent<tk2dSpriteAnimator>().Play();

			//meleeDirectionEnabled.SetActive(true);


			//meleeDirectionEnabled.GetComponent<tk2dSpriteAnimator>().Play();

			isSwinging = true;
			gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,0f);

		    if(GlobalVariableManager.Instance.IsPinEquipped(PIN.HEROOFGRIME) && gameObject.GetComponent<PlayerTakeDamage>().currentHp == GlobalVariableManager.Instance.Max_HP){
				gameObject.GetComponent<PinFunctionsManager>().HeroOfGrime(swingDirection,meleeDirectionEnabled.transform.position);
			}

			yield return new WaitForSeconds(.1f);
			meleeDirectionEnabled.SetActive(true);

			meleeDirectionEnabled.transform.GetChild(0).gameObject.SetActive(true);//swoosh

			if(!GlobalVariableManager.Instance.IsPinEquipped(PIN.SCRAPPYSHINOBI)){
				//Scrappy Shinobi
				yield return new WaitForSeconds(.1f);
				meleeDirectionEnabled.transform.GetChild(0).gameObject.SetActive(false);
				yield return new WaitForSeconds(.1f);
				GlobalVariableManager.Instance.PLAYER_CAN_MOVE = true;
				isSwinging = false;
				gameObject.GetComponent<tk2dSpriteAnimator>().Play("ani_jimIdle");
				meleeDirectionEnabled.SetActive(false);
			}else{
				
				GlobalVariableManager.Instance.PLAYER_CAN_MOVE = true;
				gameObject.GetComponent<tk2dSpriteAnimator>().Play("ani_jimIdle");
				isSwinging = false;
				meleeDirectionEnabled.SetActive(false);
			}
		}
	}


	public void SetCanAttack(bool val){
		cantAttack = val;
	}
}

