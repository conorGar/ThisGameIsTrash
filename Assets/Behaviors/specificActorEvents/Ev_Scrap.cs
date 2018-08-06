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

	void Start () {
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
		gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-3f,.3f),20f);
		arc = true;
		InvokeRepeating("Fall",0f,.1f);
		StartCoroutine("WaitForGrab");

		//InvokeRepeating("Test",1f,.4f);
	}
	
	void Update () {
		if(turningSpeed > 0)
			gameObject.transform.Rotate(Vector2.left, turningSpeed*Time.deltaTime);



		/*if(isFollowingPlayer){
			gameObject.transform.localPosition = Vector3.zero;
		}else if(canBeGrabbed){
			if(Mathf.Abs(GameObject.FindGameObjectWithTag("Player").transform.position.x - gameObject.transform.position.x) < 1 && Mathf.Abs(GameObject.FindGameObjectWithTag("Player").transform.position.y - gameObject.transform.position.y) < 1){
				gameObject.transform.parent = GameObject.FindGameObjectWithTag("Player").transform;
				isFollowingPlayer = true;
			}
		}*/

	}
	void OnTriggerEnter2D(Collider2D collision){
		/*if(!isFollowingPlayer){
			//gameObject.transform.parent = GameObject.FindGameObjectWithTag("Player").transform;
			isFollowingPlayer = true;
		}else if(isFollowingPlayer && canBeGrabbed){
            // TODO: Double check which pin this was supposed to be.  It was index 20.
			if(GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[1] < 17 || (GlobalVariableManager.Instance.IsPinEquipped(PIN.HUNGRYFORMORE) && GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[1] < 19)){
				GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[1]++;

			}
			isFollowingPlayer = false;
			if(canBeGrabbed){
				meleeMeter.GetComponent<Ev_CurrentWeapon>().UpdateMelee();
				Destroy(gameObject);
				StartCoroutine("Kill");
			}
		}*/
		if(canBeGrabbed && collision.gameObject.tag == "Player"){
				SoundManager.instance.PlaySingle(pickUp);
				meleeMeter.GetComponent<Ev_CurrentWeapon>().UpdateMelee();
				gameObject.SetActive(false);
				//StartCoroutine("Kill");
			}
	}

	IEnumerator WaitForGrab(){
		yield return new WaitForSeconds(.8f);
		canBeGrabbed = true;
		yield return new WaitForSeconds(.2f);
		arc = false;
		gameObject.transform.GetChild(0).gameObject.SetActive(true);
		gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,0f);


	}

	IEnumerator Kill(){
		yield return new WaitForSeconds(.3f);
		Destroy(gameObject);
	}

	void Fall(){
		if(arc){
			gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,gameObject.GetComponent<Rigidbody2D>().velocity.y - 4f);
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
