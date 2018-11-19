using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_Ev_Questio : MonoBehaviour {

	public GameObject mySlashR;
	public GameObject mySlashL;
	public GameObject player;
	public GameObject grabbyGloves;
	public List<MonoBehaviour> dazeDisables = new List<MonoBehaviour>();
	//public GameObject myCamera;
	public GameObject baseShadow;
	public GameObject dazedShadow;
	public GameObject pickupableGlow;

	EnemyTakeDamage myETD;
	tk2dSpriteAnimator myAnim;
	FollowPlayer fp;
	int facingDirection = 0; //0 = left, 1 = right
	int swingOnce;
	int dropItemOnce;
	GameObject dazedStars;

	void Start () {
		myETD = gameObject.GetComponent<EnemyTakeDamage>();
		fp = gameObject.GetComponent<FollowPlayer>();
		myAnim = gameObject.GetComponent<tk2dSpriteAnimator>();

	}
	void OnEnable(){
		if(myETD.currentHp > 12){
			StopAllCoroutines();
			gameObject.GetComponent<FollowPlayer>().enabled = true; //when returning to room without this Q will just stand there
		}
	}
	void Update () {

		if(player.transform.position.x < gameObject.transform.position.x && facingDirection != 0 && swingOnce == 0){
			facingDirection = 0;
		}else if(player.transform.position.x > gameObject.transform.position.x && facingDirection != 1&& swingOnce == 0){
			facingDirection = 1;
		}

		if(fp.enabled == true){
			if(facingDirection == 0 && myAnim.CurrentClip.name != "walkL"){
				myAnim.Play("walkL");
			}else if(facingDirection == 1 && myAnim.CurrentClip.name != "walkR"){
				myAnim.Play("walkR");
			}
			float distance = Vector3.Distance(transform.position, player.transform.position);
			if(distance < 5 && swingOnce == 0){
				Debug.Log("QUESTIO SWING ACTIVATE");
				StartCoroutine("Swing");
				swingOnce = 1;
			}
		}

		if(myETD.currentHp <= 12 && dropItemOnce == 0){
			DropItem();
			dropItemOnce = 1;
			Dazed();
		}

		if(gameObject.layer == 11&& grabbyGloves.activeInHierarchy == false && pickupableGlow.activeInHierarchy == false){
			pickupableGlow.SetActive(true);
		}


	}

	IEnumerator Swing(){
		fp.enabled = false;
		if(facingDirection == 0){
			myAnim.Play("swingL");
		}else{
			myAnim.Play("swingR");
		}
		yield return new WaitForSeconds(.7f);
		gameObject.GetComponent<Rigidbody2D>().velocity = (player.transform.position -gameObject.transform.position).normalized *15;
		yield return new WaitForSeconds(.5f);
		if(myAnim.CurrentClip.name == "swingL"){
			mySlashL.SetActive(true);
		}else{
			mySlashR.SetActive(true);
		}
		gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,0f);
		yield return new WaitForSeconds(.5f);
		//check for directionFacing
		mySlashL.SetActive(false);
		mySlashR.SetActive(false);

		swingOnce = 0;
		myAnim.Play("idleL");
		fp.enabled = true;
	}

	void DropItem(){
		//GlobalVariableManager.Instance.TUT_POPUP_ISSHOWING = true; //stops enemy function
		player.GetComponent<EightWayMovement>().enabled = false;
		player.GetComponent<PlayerTakeDamage>().enabled = false;
		grabbyGloves.SetActive(true);
		grabbyGloves.GetComponent<Ev_SpecialItem>().Toss();
		CamManager.Instance.mainCamEffects.CameraPan(grabbyGloves,true);
		Dazed();
		Invoke("ReturnFromGloveShow",2f);

	}

	void ReturnFromGloveShow(){
		CamManager.Instance.mainCamEffects.ReturnFromCamEffect();
		player.GetComponent<EightWayMovement>().enabled = true;
		player.GetComponent<PlayerTakeDamage>().enabled = true;
		//GlobalVariableManager.Instance.TUT_POPUP_ISSHOWING = false; //stops enemy function

	}

	void Dazed(){
		//gameObject.GetComponent<EnemyTakeDamage>().StopAllCoroutines();//so follow player isn't enabled again
		for(int i = 0; i < dazeDisables.Count; i++){
			dazeDisables[i].enabled = false;
		}
		dazedShadow.SetActive(true);
		baseShadow.SetActive(false);
		gameObject.layer = 11;
		gameObject.GetComponent<ThrowableObject>().enabled = true;
		myAnim.Play("dazed");
		dazedStars = ObjectPool.Instance.GetPooledObject("effect_stars",new Vector3(transform.position.x,transform.position.y+2,0));
		dazedStars.transform.parent = gameObject.transform;
		StopAllCoroutines();
		//this.enabled = false;
	}

}