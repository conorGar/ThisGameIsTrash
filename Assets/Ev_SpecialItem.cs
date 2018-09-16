using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ev_SpecialItem : MonoBehaviour {

	// Use this for initialization
	public tk2dCamera mainCamera;
	public GameObject player;
	public GameObject upgradeUnlockDisplay;
	public Sprite unlockSprite;
	public bool playerAutoMoveToward;

	Vector3 smoothVelocity = Vector3.zero;
	bool playerIsMovingToward;
	bool beingTossed;
	bool canPickUp;
	float landingY;
	Rigidbody2D myBody;

	void OnEnable() {
		if(playerAutoMoveToward)
			PlayerMoveToward();

		myBody = gameObject.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		if(playerIsMovingToward){
			player.transform.position = Vector3.SmoothDamp(player.transform.position,gameObject.transform.position, ref smoothVelocity, 4f);

				if(player.transform.position.x < transform.position.x){
					transform.localScale = new Vector3(-1,1,1);
				} else{
					//if(!anim.IsPlaying("chaseL"))
						//anim.Play("chaseL");
					transform.localScale = new Vector3(1,1,1);
				}
		}else if(beingTossed && transform.position.y < landingY){
			myBody.gravityScale = 0f;
			myBody.velocity = new Vector2(0,0f);
			canPickUp = true;
		}
	}

	void OnTriggerEnter2D(Collider2D collider){
		if(collider.tag == "Player" && canPickUp){
			gameObject.GetComponent<BoxCollider2D>().enabled = false;
			player.GetComponent<tk2dSpriteAnimator>().Play("ani_jimPickUp");
			gameObject.GetComponent<SpecialEffectsBehavior>().SmoothMovementToPoint(transform.position.x, player.transform.position.y,2f);
			StartCoroutine("PickUp");

		}
	}
	
	IEnumerator PickUp(){
		yield return new WaitForSeconds(2f);
		upgradeUnlockDisplay.SetActive(true);
		//player.GetComponent<ThrowTrash>().enabled = true;
		gameObject.SetActive(false);
	}

	void PlayerMoveToward(){
		player.GetComponent<EightWayMovement>().enabled = false;
		player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		mainCamera.GetComponent<Ev_MainCameraEffects>().CameraPan(gameObject.transform.position,null);
		player.GetComponent<tk2dSpriteAnimator>().Play("ani_jimWalk");
			if(player.transform.position.x < transform.position.x){
					player.transform.localScale = new Vector3(1,1,1);
			} else{
				player.transform.localScale = new Vector3(-1,1,1);
			}
		playerIsMovingToward = true;

	}
	public void Toss()
	{
		transform.parent = null;
		myBody.AddForce(new Vector2(10f*(Mathf.Sign(gameObject.transform.lossyScale.x)),3f),ForceMode2D.Impulse);//slide
		landingY = transform.position.y -3f;
		beingTossed = true;
	}
}
