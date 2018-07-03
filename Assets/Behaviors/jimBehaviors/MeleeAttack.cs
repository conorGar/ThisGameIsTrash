using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour {


	public AudioClip swing;
	public AudioSource audioSource;
	public GameObject meleeWeaponSwing;
	public GameObject meleeWeapon;

	bool cantAttack = false;
	GameObject weapon;
	bool isSwinging = false;
	int swingDirection;
	float turningSpeed;


	void Start () {

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
		if(weapon && isSwinging){
			if(swingDirection ==1){
				weapon.transform.position = new Vector2(gameObject.transform.position.x + 1f,gameObject.transform.position.y+ 1.4f);
			}else if(swingDirection ==2){
				weapon.transform.position = new Vector2(gameObject.transform.position.x,gameObject.transform.position.y + 1.4f);
			}else if(swingDirection == 3){
				weapon.transform.position = new Vector2(gameObject.transform.position.x -.5f,gameObject.transform.position.y + 2f);
			}else if(swingDirection == 4){
				weapon.transform.position = new Vector2(gameObject.transform.position.x + 2f,gameObject.transform.position.y + 2f);
			}
		}
		if(!cantAttack && Time.timeScale != 0f){
			//timescale part = if game isnt paused
			if(GlobalVariableManager.Instance.TRASH_TYPE_SELECTED == 3 && !isSwinging && GlobalVariableManager.Instance.PLAYER_CAN_MOVE && GlobalVariableManager.Instance.MENU_SELECT_STAGE != 11
				&& !gameObject.GetComponent<tk2dSpriteAnimator>().IsPlaying("hurtL") && !gameObject.GetComponent<tk2dSpriteAnimator>().IsPlaying("hurtR")){

				if(Input.GetKeyDown(KeyCode.A)){
					StartCoroutine("Left");
				}else if(Input.GetKeyDown(KeyCode.D)){
					Debug.Log(" D IS PRESSED");
					StartCoroutine("Right");
				}else if(Input.GetKeyDown(KeyCode.S)){
					StartCoroutine("Down");
				}else if(Input.GetKeyDown(KeyCode.W))
					StartCoroutine("Up");
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

	IEnumerator Right(){
		Debug.Log(" RIGHT IS ACTIVATED ");
		GlobalVariableManager.Instance.PLAYER_CAN_MOVE = false;
		if(GlobalVariableManager.Instance.MASTER_SFX_VOL > 0 && audioSource){
			audioSource.clip = swing;
			audioSource.Play();
		}
		gameObject.GetComponent<tk2dSpriteAnimator>().Play("ani_jimSwingR");
		GameObject meleeWeaponSwingInstance;
		meleeWeaponSwingInstance = Instantiate(meleeWeapon, transform.position, Quaternion.identity);
		meleeWeaponSwingInstance.GetComponent<tk2dSpriteAnimator>().Play("plankSwing");
		//meleeWeaponSwingInstance.transform.localScale = new Vector2(-1,0);

		weapon = Instantiate(meleeWeapon, transform.position, Quaternion.identity);
		weapon.GetComponent<tk2dSpriteAnimator>().Play("plankR");
		//weapon.transform.localScale = new Vector2(-1,0);
		weapon.transform.Rotate(0f,90f,0f);

		swingDirection = 1;
		if(GlobalVariableManager.Instance.pinsEquipped[49] != 0 && GlobalVariableManager.Instance.CURRENT_HP == int.Parse(GlobalVariableManager.Instance.characterUpgradeArray[3])){
		//Hero of Grime pin - shoots beam at max hp
			GameObject bullet = Instantiate(GameObject.Find("bullet"), new Vector2(transform.position.x +.3f, transform.position.y), Quaternion.identity);
			bullet.GetComponent<tk2dSpriteAnimator>().Play("beam");
			bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(10f,0f);
		}
		isSwinging = true;
		gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,0f);//player stops moving
		Debug.Log(GlobalVariableManager.Instance.pinsEquipped[32]);
		if(GlobalVariableManager.Instance.pinsEquipped[32] == 0 ){
			//Scrappy Shinobi
			Debug.Log("got here 2");
			turningSpeed = 640;
			weapon.transform.Rotate(Vector2.right, turningSpeed*Time.deltaTime); // set rotation soeed
			Debug.Log("Melee wait now");
			yield return new WaitForSeconds(.3f);
			Debug.Log("Melee wait success");
			Destroy(weapon);
			Destroy(meleeWeaponSwingInstance);
			weapon.transform.Rotate(Vector2.right, 0*Time.deltaTime);
			GlobalVariableManager.Instance.PLAYER_CAN_MOVE = true;
			isSwinging = false;
			gameObject.GetComponent<tk2dSpriteAnimator>().Play("ani_jimIdle");
		}else{
			turningSpeed = 940;
			Debug.Log("Melee wait now");
			yield return new WaitForSeconds(.1f);
			Debug.Log("Melee wait success 2");
			Destroy(weapon);
			Destroy(meleeWeaponSwingInstance);
			GlobalVariableManager.Instance.PLAYER_CAN_MOVE = true;
			gameObject.GetComponent<tk2dSpriteAnimator>().Play("ani_jimIdle");
			isSwinging = false;
		}



	}//end of right()
	IEnumerator Left(){
		GlobalVariableManager.Instance.PLAYER_CAN_MOVE = false;
		if(GlobalVariableManager.Instance.MASTER_SFX_VOL > 0 && audioSource){
			audioSource.clip = swing;
			audioSource.Play();
		}
		gameObject.GetComponent<tk2dSpriteAnimator>().Play("ani_jimSwingL");
		GameObject meleeWeaponSwingInstance;
		meleeWeaponSwingInstance = Instantiate(meleeWeapon, new Vector2(transform.position.x - .5f, transform.position.y), Quaternion.identity);
		meleeWeaponSwingInstance.GetComponent<tk2dSpriteAnimator>().Play("plankSwing");
		weapon = Instantiate(meleeWeapon, transform.position, Quaternion.identity);
		meleeWeaponSwingInstance.transform.localScale = new Vector2(-1f,1f);
		//weapon.transform.localScale = new Vector2(-1,0);
		weapon.transform.Rotate(0f,90f,0f);
		swingDirection = 2;
		if(GlobalVariableManager.Instance.pinsEquipped[49] != 0 && GlobalVariableManager.Instance.CURRENT_HP == int.Parse(GlobalVariableManager.Instance.characterUpgradeArray[3])){
		//Hero of Grime pin - shoots beam at max hp
			GameObject bullet = Instantiate(GameObject.Find("bullet"), new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
			bullet.GetComponent<tk2dSpriteAnimator>().Play("beam");
			bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,-10f);
		}
		isSwinging = true;
		gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,0f);//player stops moving
		if(GlobalVariableManager.Instance.pinsEquipped[32] ==0){
			//Scrappy Shinobi
			turningSpeed = -640;
			weapon.transform.Rotate(Vector2.left, turningSpeed*Time.deltaTime); // set rotation soeed
			yield return new WaitForSeconds(.3f);
			Destroy(weapon);
			Destroy(meleeWeaponSwingInstance);

			weapon.transform.Rotate(Vector2.left, 0*Time.deltaTime);
			GlobalVariableManager.Instance.PLAYER_CAN_MOVE = true;
			isSwinging = false;
			gameObject.GetComponent<tk2dSpriteAnimator>().Play("ani_jimIdleL");
		}else{
			turningSpeed = -940;
			yield return new WaitForSeconds(.1f);
			Destroy(weapon);
			Destroy(meleeWeaponSwingInstance);
			GlobalVariableManager.Instance.PLAYER_CAN_MOVE = true;
			gameObject.GetComponent<tk2dSpriteAnimator>().Play("ani_jimIdleL");
			isSwinging = false;
		}

	}//end of Left()
	IEnumerator Down(){
		GlobalVariableManager.Instance.PLAYER_CAN_MOVE = false;
		if(GlobalVariableManager.Instance.MASTER_SFX_VOL > 0 && audioSource){
			audioSource.clip = swing;
			audioSource.Play();
		}
		gameObject.GetComponent<tk2dSpriteAnimator>().Play("ani_jimSwingDown");
		GameObject meleeWeaponSwingInstance;
		meleeWeaponSwingInstance = Instantiate(meleeWeaponSwing, transform.position, Quaternion.identity);
		meleeWeaponSwingInstance.GetComponent<tk2dSpriteAnimator>().Play("plankL");
		weapon = Instantiate(meleeWeapon, transform.position, Quaternion.identity);
		weapon.GetComponent<tk2dSpriteAnimator>().Play("plankDown");
		weapon.transform.Rotate(0f,-90f,0f);
		swingDirection = 4;
		if(GlobalVariableManager.Instance.pinsEquipped[49] != 0 && GlobalVariableManager.Instance.CURRENT_HP == int.Parse(GlobalVariableManager.Instance.characterUpgradeArray[3])){
		//Hero of Grime pin - shoots beam at max hp
			GameObject bullet = Instantiate(GameObject.Find("bullet"), new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
			bullet.GetComponent<tk2dSpriteAnimator>().Play("beam");
			bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,-10f);
		}
		isSwinging = true;
		gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,0f);//player stops moving
		if(GlobalVariableManager.Instance.pinsEquipped[32] ==0){
			//Scrappy Shinobi
			turningSpeed = 340;
			weapon.transform.Rotate(Vector2.left, turningSpeed*Time.deltaTime); // set rotation soeed
			yield return new WaitForSeconds(.5f);
			Destroy(weapon);
			Destroy(meleeWeaponSwingInstance);

			weapon.transform.Rotate(Vector2.left, 0*Time.deltaTime);
			GlobalVariableManager.Instance.PLAYER_CAN_MOVE = true;
			isSwinging = false;
			gameObject.GetComponent<tk2dSpriteAnimator>().Play("ani_jimIdleL");
		}else{
			turningSpeed = 540;
			yield return new WaitForSeconds(.3f);
			Destroy(weapon);
			Destroy(meleeWeaponSwingInstance);

			GlobalVariableManager.Instance.PLAYER_CAN_MOVE = true;
			gameObject.GetComponent<tk2dSpriteAnimator>().Play("ani_jimIdleL");
			isSwinging = false;
			}
	}//end of Down()

	IEnumerator Up(){
		GlobalVariableManager.Instance.PLAYER_CAN_MOVE = false;
		if(GlobalVariableManager.Instance.MASTER_SFX_VOL > 0 && audioSource){
			audioSource.clip = swing;
			audioSource.Play();
		}
		gameObject.GetComponent<tk2dSpriteAnimator>().Play("ani_jimSwingUp");
		GameObject meleeWeaponSwingInstance;
		meleeWeaponSwingInstance = Instantiate(meleeWeaponSwing, transform.position, Quaternion.identity);
		meleeWeaponSwingInstance.GetComponent<tk2dSpriteAnimator>().Play("stickUp");
		weapon = Instantiate(meleeWeapon, transform.position, Quaternion.identity);
		weapon.GetComponent<tk2dSpriteAnimator>().Play("stickUp");
		weapon.transform.Rotate(0f,-90f,0f);
		swingDirection = 3;
		if(GlobalVariableManager.Instance.pinsEquipped[49] != 0 && GlobalVariableManager.Instance.CURRENT_HP == int.Parse(GlobalVariableManager.Instance.characterUpgradeArray[3])){
		//Hero of Grime pin - shoots beam at max hp
			GameObject bullet = Instantiate(GameObject.Find("bullet"), new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
			bullet.GetComponent<tk2dSpriteAnimator>().Play("beam");
			bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,10f);
		}
		isSwinging = true;
		gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,0f);//player stops moving
		if(GlobalVariableManager.Instance.pinsEquipped[32] ==0){
			//Scrappy Shinobi
			turningSpeed = -440;
			weapon.transform.Rotate(Vector2.left, turningSpeed*Time.deltaTime); // set rotation soeed
			yield return new WaitForSeconds(.5f);
			Destroy(weapon);
			Destroy(meleeWeaponSwingInstance);

			weapon.transform.Rotate(Vector2.left, 0*Time.deltaTime);
			GlobalVariableManager.Instance.PLAYER_CAN_MOVE = true;
			isSwinging = false;
			gameObject.GetComponent<tk2dSpriteAnimator>().Play("ani_jimIdleL");
		}else{
			turningSpeed = -740;
			yield return new WaitForSeconds(.3f);
			Destroy(weapon);
			Destroy(meleeWeaponSwingInstance);

			GlobalVariableManager.Instance.PLAYER_CAN_MOVE = true;
			gameObject.GetComponent<tk2dSpriteAnimator>().Play("ani_jimIdleL");
			isSwinging = false;
		}
	} //end of Up()

	public void SetCanAttack(bool val){
		cantAttack = val;
	}
}

