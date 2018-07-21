using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour {


	public AudioClip swing;
	public AudioSource audioSource;
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
		if(GlobalVariableManager.Instance.pinsEquipped[4] == 12 || GlobalVariableManager.Instance.CARRYING_SOMETHING == true){
			//Cursed pin- can't attack || carrying large trash- can't attack
			Debug.Log("Cant attack because of here");
			cantAttack = true;
		}
		if(GlobalVariableManager.Instance.pinsEquipped[45] == 1){
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
			if(GlobalVariableManager.Instance.TRASH_TYPE_SELECTED == 3 && !isSwinging && GlobalVariableManager.Instance.PLAYER_CAN_MOVE && GlobalVariableManager.Instance.MENU_SELECT_STAGE != 11
				&& !gameObject.GetComponent<tk2dSpriteAnimator>().IsPlaying("hurtL") && !gameObject.GetComponent<tk2dSpriteAnimator>().IsPlaying("hurtR")){

				if(Input.GetKeyDown(KeyCode.A)){
					playerMomentum = 6f;
					this.gameObject.transform.localScale = new Vector3(startingScale.x*-1,startingScale.y, startingScale.z);
					StartCoroutine("Swing",2);
				}else if(Input.GetKeyDown(KeyCode.D)){
					this.gameObject.transform.localScale = startingScale;
					playerMomentum = 6f;
					StartCoroutine("Swing",1);
				}else if(Input.GetKeyDown(KeyCode.S)){
					this.gameObject.transform.localScale = startingScale;
					playerMomentum = 6f;
					StartCoroutine("Swing",4);
				}else if(Input.GetKeyDown(KeyCode.W)){
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

	IEnumerator Swing(int direction){
		GlobalVariableManager.Instance.PLAYER_CAN_MOVE = false;
		GameObject meleeDirectionEnabled = null;
		swingDirection = direction;
		if(direction == 1){
			meleeDirectionEnabled = meleeWeaponRightSwing;
			gameObject.GetComponent<tk2dSpriteAnimator>().Play("ani_jimSwingR");
		}else if(direction == 2){
			meleeDirectionEnabled = meleeWeaponLeftSwing;
			gameObject.GetComponent<tk2dSpriteAnimator>().Play("ani_jimSwingR");
		}else if(direction == 3){
			gameObject.GetComponent<tk2dSpriteAnimator>().Play("ani_jimSwingUp");
			meleeDirectionEnabled = meleeWeaponTopSwing;

		}else if(direction == 4){
			gameObject.GetComponent<tk2dSpriteAnimator>().Play("ani_jimSwingDown");
			meleeDirectionEnabled = meleeWeaponBotSwing;

		}
		meleeDirectionEnabled.GetComponent<tk2dSpriteAnimator>().Play();

		//meleeDirectionEnabled.SetActive(true);


		//meleeDirectionEnabled.GetComponent<tk2dSpriteAnimator>().Play();


		if(GlobalVariableManager.Instance.MASTER_SFX_VOL > 0 && audioSource){
			audioSource.clip = swing;
			audioSource.Play();
		}

		isSwinging = true;
		gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,0f);

		if(GlobalVariableManager.Instance.pinsEquipped[49] != 0 && GlobalVariableManager.Instance.CURRENT_HP == int.Parse(GlobalVariableManager.Instance.characterUpgradeArray[3])){
		//Hero of Grime pin - shoots beam at max hp
			GameObject bullet = Instantiate(GameObject.Find("bullet"), new Vector2(transform.position.x +.3f, transform.position.y), Quaternion.identity);
			bullet.GetComponent<tk2dSpriteAnimator>().Play("beam");
			bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(10f,0f);
		}

		yield return new WaitForSeconds(.1f);
		meleeDirectionEnabled.SetActive(true);

		meleeDirectionEnabled.transform.GetChild(0).gameObject.SetActive(true);//swoosh

		if(GlobalVariableManager.Instance.pinsEquipped[32] == 0 ){
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


	public void SetCanAttack(bool val){
		cantAttack = val;
	}
}

