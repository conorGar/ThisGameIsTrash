using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ev_Scrap : MonoBehaviour {

	//tk2dSpriteAnimator anim;
	bool arc = false;
	bool canBeGrabbed = false;
	int turningSpeed = 0;
	bool isFollowingPlayer = false;
	GameObject meleeMeter;
	public AudioClip pickUp;
	public Sprite spr1;
	public Sprite spr2;
	public Sprite spr3;
	public Sprite spr4;
	public Sprite spr5;

	[HideInInspector]
	public float landingY; //given by EnemyTakeDamage.cs

	void OnStart(){

	}

	void OnEnable () {
		if(meleeMeter == null)
			meleeMeter = GameObject.Find("meleeMeter") as GameObject;
		int whichAni = Random.Range(1,6);
		if(whichAni == 1)
			gameObject.GetComponent<SpriteRenderer>().sprite = spr1;
		else if(whichAni == 2)
			gameObject.GetComponent<SpriteRenderer>().sprite = spr2;
		else if(whichAni == 3)
			gameObject.GetComponent<SpriteRenderer>().sprite = spr3;
		else if(whichAni == 4)
			gameObject.GetComponent<SpriteRenderer>().sprite = spr4;
		else if(whichAni == 5)
			gameObject.GetComponent<SpriteRenderer>().sprite = spr5;

	
		turningSpeed = 360;
		gameObject.GetComponent<Rigidbody2D>().gravityScale = 2;
		 
		gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-2f,2f),Random.Range(18f,8f)),ForceMode2D.Impulse);
		arc = true;

		//InvokeRepeating("Fall",0f,.1f);
		StartCoroutine("WaitForGrab");

		//InvokeRepeating("Test",1f,.4f);
	}
	
	void Update () {
		if(turningSpeed > 0)
			gameObject.transform.Rotate(Vector2.left, turningSpeed*Time.deltaTime);




	}
	void OnTriggerEnter2D(Collider2D collision){
		
		if(canBeGrabbed && collision.gameObject.tag == "Player"){
				SoundManager.instance.PlaySingle(pickUp);
				GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[1]++;
				meleeMeter.GetComponent<Ev_CurrentWeapon>().UpdateMelee();
				if(GlobalVariableManager.Instance.IsPinEquipped(PIN.COUNTSCRAPULA)){
					int healChance = Random.Range(0,16);
					Debug.Log("count scrapula heal attempt: " + healChance);
					if(healChance == 5){
						collision.gameObject.GetComponent<PlayerTakeDamage>().Heal(1);
					}
				}
				gameObject.SetActive(false);

			}
	}

	IEnumerator WaitForGrab(){
		yield return new WaitForSeconds(.4f);// delay so doesnt instantly stop(starts below landing)
		Debug.Log("scrap is waiting to reach: " + landingY);
		yield return new WaitUntil(() => landingY >= transform.position.y);
		Debug.Log("scrap has reached landing" + landingY + transform.position.y);
		canBeGrabbed = true;
		arc = false;
		turningSpeed = 0;

		gameObject.transform.Rotate(Vector2.left, 0f);
		gameObject.transform.GetChild(0).gameObject.SetActive(true);
		gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
		gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,0f);


	}

	IEnumerator Kill(){
		yield return new WaitForSeconds(.3f);
		Destroy(gameObject);
	}

	void Fall(){
		if(arc){
			gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,gameObject.GetComponent<Rigidbody2D>().velocity.y - 2f);
		}else{
			CancelInvoke();
			turningSpeed = 0;
			gameObject.transform.Rotate(Vector2.left, 0f);

		}
	}
	public void Test(){
		//GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[1]++;
		meleeMeter.GetComponent<Ev_CurrentWeapon>().UpdateMelee();
	}
}
