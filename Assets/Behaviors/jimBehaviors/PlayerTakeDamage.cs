using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTakeDamage : MonoBehaviour {

	public GameObject objectPool;
	public GameObject HPdisplay;
	public Ev_MainCamera currentCam;
	public GameObject fadeHelper;//needed for Death()
	public GameObject roomManager; //needed for Death()
	public GameObject trashCollectedDisplay; //needed for Death()
	public GameObject droppedTrashPile;

	public AudioClip hurt;
	int maxHP;
	public int currentHp;
	int damageDealt;

	bool currentlyTakingDamage = false;
	// Use this for initialization
	void Start () {
		maxHP = GlobalVariableManager.Instance.Max_HP;	
		currentHp = maxHP;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D(Collision2D enemy){
		if(enemy.gameObject.layer == 9 && !currentlyTakingDamage){ //layer 9 = enemies
			SoundManager.instance.PlaySingle(hurt);

			if(enemy.gameObject.tag == "Boss"){
				damageDealt = enemy.gameObject.GetComponent<Boss>().attkDmg;
			}else{
				damageDealt = enemy.gameObject.GetComponent<Enemy>().attkPower;

			}
			currentlyTakingDamage = true;
			GlobalVariableManager.Instance.PLAYER_CAN_MOVE = false;
			currentHp -= damageDealt;
			gameObject.GetComponent<JimAnimationManager>().PlayAnimation("hurt",true);
			Debug.Log("reached this end of hp hud change" + currentHp);
			HPdisplay.GetComponent<GUI_HPdisplay>().UpdateDisplay(currentHp);
			GameObject damageCounter = objectPool.GetComponent<ObjectPool>().GetPooledObject("HitStars",this.gameObject.transform.position);
			damageCounter.GetComponent<Ev_HitStars>().ShowProperDamage(damageDealt);
			damageCounter.SetActive(true);
			GameObject littleStars = objectPool.GetComponent<ObjectPool>().GetPooledObject("effect_LittleStars",this.gameObject.transform.position);
			littleStars.SetActive(true);
			currentCam.StartCoroutine("ScreenShake",.2f);

			if(enemy.transform.position.x < gameObject.transform.position.x){
				gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(17f,0f), ForceMode2D.Impulse);
				damageCounter.GetComponent<Rigidbody2D>().AddForce(new Vector2(4f,10f), ForceMode2D.Impulse);
			}else{
				gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-17f,0f), ForceMode2D.Impulse);
				damageCounter.GetComponent<Rigidbody2D>().AddForce(new Vector2(-4f,10f), ForceMode2D.Impulse);
			}

			if(currentHp <= 0){
				
				StartCoroutine("Death");
			
			}else{
				StartCoroutine("RegainControl");
			}
		}
	}

	IEnumerator RegainControl(){
		yield return new WaitForSeconds(.2f);
		gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,0f);
		GlobalVariableManager.Instance.PLAYER_CAN_MOVE = true;
		yield return new WaitForSeconds(.5f); //brief period of invincibility
		currentlyTakingDamage = false;
		Debug.Log("Regained Control");

	}

	public void DropTrash(){
		for(int i = 0; i < GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[0]; i++){
			GameObject droppedTrash = objectPool.GetComponent<ObjectPool>().GetPooledObject("DroppedTrash",gameObject.transform.position);
			droppedTrash.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-3f,3f),Random.Range(11f,17f)), ForceMode2D.Impulse);
		}


	}


	IEnumerator Death(){
		

		gameObject.GetComponent<JimAnimationManager>().PlayAnimation("death",true);
		DropTrash();
		GlobalVariableManager.Instance.DROPPED_TRASH_LOCATION = gameObject.transform.position;
		GlobalVariableManager.Instance.GARBAGE_HAD = GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[0];

		//truck comes to pick player up
		GlobalVariableManager.Instance.CARRYING_SOMETHING = false;
		gameObject.GetComponent<BoxCollider2D>().enabled = false;
	
		gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,0f);
		currentCam.GetComponent<Ev_MainCamera>().enabled = false;
		//GameObject.Find("fadeHelper").GetComponent<Ev_FadeHelper>().WhiteFlash();
		if(GlobalVariableManager.Instance.IsPinEquipped(PIN.DEVILSDEAL)){
			gameObject.GetComponent<PinFunctionsManager>().DevilsDeal();
			yield return new WaitForSeconds(1f);
			fadeHelper.GetComponent<Ev_FadeHelper>().FadeToScene("Hub");
		}else{
		yield return new WaitForSeconds(1f);
		fadeHelper.GetComponent<Ev_FadeHelper>().BlackFade(); //fade to black
		GameObject truck = objectPool.GetComponent<ObjectPool>().GetPooledObject("GarbageTruck",new Vector3(gameObject.transform.position.x - 20, gameObject.transform.position.y,0f));
		truck.GetComponent<Ev_SmallTruck>().ReturnToDumpster();

		yield return new WaitForSeconds(1.5f);
		Debug.Log("Death Pickup - Phase 1");
		//-----------Resetting of needed values----------------//
		roomManager.GetComponent<RoomManager>().currentRoom.DeactivateRoom();
		gameObject.transform.position = new Vector3(0f,-3f,0f); //Start at Beginning of world
		truck.transform.position = new Vector3(-15f,-3f,0f);
		truck.GetComponent<Rigidbody2D>().velocity = new Vector2(50f,0f);
		currentCam.transform.position = new Vector3(0f,0f,-10f);
		roomManager.GetComponent<RoomManager>().Restart();


		yield return new WaitForSeconds(.3f);

		Debug.Log("Death Pickup - Phase 2");
		fadeHelper.GetComponent<Ev_FadeHelper>().FadeIn();
		//gameObject.GetComponent<tk2dSprite>().enabled = true;//disabled by Ev_SmallTruck
		truck.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,0f);

		yield return new WaitForSeconds(.5f);
		Debug.Log("Death Pickup - Phase 3");
		truck.GetComponent<Rigidbody2D>().velocity = new Vector2(50f,0f);
		currentHp = GlobalVariableManager.Instance.Max_HP;
		currentlyTakingDamage = false;
		if(GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[0] > 0){
			droppedTrashPile.SetActive(true);
			droppedTrashPile.transform.position = GlobalVariableManager.Instance.DROPPED_TRASH_LOCATION;
		}
		GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[0] = 0;
		HPdisplay.GetComponent<GUI_HPdisplay>().UpdateDisplay(currentHp);
		trashCollectedDisplay.GetComponent<GUI_TrashCollectedDisplay>().UpdateDisplay(GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[0]);
		GlobalVariableManager.Instance.PLAYER_CAN_MOVE = true;
		gameObject.GetComponent<BoxCollider2D>().enabled = true;


		//----------------------------------------------------//
		yield return new WaitForSeconds(.5f);
		truck.SetActive(false);
		fadeHelper.GetComponent<Ev_FadeHelper>().fadeBack = false;
		}

	}
}
