using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_Ev_Hash : MonoBehaviour {

	tk2dSpriteAnimator myAnim;
	public GameObject stuart;
	public GameObject stuartShield;
	public AudioClip teleportSound;
	public AudioClip shieldSound;
	public ParticleSystem smokePuff;


	//bool isRunningAway;
	float landY;
	Rigidbody2D myBody;
	bool falling;
	bool onStuart;
	bool returnAfterThrow;
    //Protects Stuart Until Hash is hit
    //^Then Hash runs away


    void Awake()
    {
        myAnim = gameObject.GetComponent<tk2dSpriteAnimator>();
        myBody = gameObject.GetComponent<Rigidbody2D>();
        onStuart = true;
    }

    void Start () {
		gameObject.transform.parent = stuart.transform;
		gameObject.transform.localPosition = new Vector2(0f,3f);//place hash on top of stuart
		gameObject.GetComponent<Renderer>().sortingLayerName = "Layer02";

		Shield();
	}

	void OnEnable(){
		if(returnAfterThrow){
			Revive();
		}
	}

	void Update () {
        if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
            /*float distance = Vector3.Distance(transform.position, player.transform.position);

            if(stuartShield.activeInHierarchy == true){//if hit while shielding Stuart, stops shield and starts runaway
                if(myAnim.CurrentClip.name == "hurt"){
                    stuartShield.SetActive(false);
                    isRunningAway = true;
                }
            }else if(!isRunningAway && distance <10){ //if not shielding stuart, will start running away if the player gets close
                isRunningAway = true;
            }
            if(isRunningAway){
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, -1*8*Time.deltaTime);
                if(myAnim.CurrentClip.name != "walk"){
                    myAnim.Play("walk");
                }
                if(distance > 10){
                    isRunningAway = false;
                    StartCoroutine("Shield");
                }
            }*/
            if (onStuart) {
                gameObject.transform.localPosition = new Vector2(0f, 3f);//place hash on top of stuart
            }

            if (falling) {
                if (gameObject.transform.position.y < landY) {
                    Dazed();
                    myBody.gravityScale = 0f;
                    myBody.velocity = new Vector2(0, 0f);
                    myBody.AddForce(new Vector2(4f * (Mathf.Sign(gameObject.transform.lossyScale.x)), 0f), ForceMode2D.Impulse);//slide
                    falling = false;
                }
            }
        }
	}




	public void Shield(){
	Debug.Log("HASH SHIELD ACTIVATED");
		//myAnim.Play("idle");
		//yield return new WaitForSeconds(Random.Range(3f,6f));
		if(!falling){ //if runningAway wasn't activated in the time between coroutine activate and wait till cast....
			myAnim.Play("cast");

			//yield return new WaitForSeconds(1f);
			SoundManager.instance.PlaySingle(shieldSound);
			stuartShield.SetActive(true);
			stuart.GetComponent<BossStuart>().canDamage = false;
			stuart.GetComponent<InvincibleEnemy>().enabled = true;
			stuart.GetComponent<EnemyTakeDamage>().enabled = false;
			//stuart.GetComponent<FollowPlayer>().enabled = false;
		}
	}

	public void KnockOff(){
		Debug.Log("HASH KNOCKOFF ACTIVATE*****");
		stuartShield.SetActive(false);
		onStuart = false;
		gameObject.transform.parent = null;
		landY = gameObject.transform.position.y - 4;
		myBody.AddForce(new Vector2(7f*(Mathf.Sign(gameObject.transform.lossyScale.x)),4f),ForceMode2D.Impulse);//slide
		myBody.gravityScale = 1;
		gameObject.GetComponent<Renderer>().sortingLayerName = "Layer01";
		falling = true;
		stuart.GetComponent<EnemyTakeDamage>().enabled = true;
	}

	void Dazed(){
		//gameObject.GetComponent<EnemyTakeDamage>().enabled = true;
		gameObject.layer = 11; //switch to ite layer.
		gameObject.GetComponent<ThrowableObject>().enabled = true;
		myAnim.Play("dazed");
		StartCoroutine("ReviveCheck");
	}

	void Revive(){
		if(this.enabled){
			smokePuff.Play();
			SoundManager.instance.PlaySingle(teleportSound);
			gameObject.GetComponent<EnemyTakeDamage>().enabled = false;//cant attack while he is riding Stuart
			gameObject.transform.parent = stuart.transform;
			gameObject.GetComponent<Animator>().enabled = false;
			gameObject.transform.localScale = Vector2.one;
			gameObject.transform.localPosition = new Vector2(0f,3f);//place hash on top of stuart
			onStuart = true;
			gameObject.layer = 1; //switch to tile layer.
			gameObject.GetComponent<ThrowableObject>().enabled = false;
			Shield();
		}else{
			returnAfterThrow = true;
		}
	}

	IEnumerator ReviveCheck(){
		yield return new WaitForSeconds(5f);// delay until can revive
		yield return new WaitUntil(() => gameObject.GetComponent<ThrowableObject>().onGround == true);
		Revive();
	}


}
