using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableObject : PickupableObject {

	float landingY;
	bool beingThrown;
	protected bool canThrow;
	public AudioClip carrySound;
	public GameObject myShadow;
	Room currentRoom;//for staying in bounds
//	public BoxCollider2D physicalCollision;

	public List<MonoBehaviour> behaviorsToStop = new List<MonoBehaviour>();
	// Use this for initialization
	void Start(){
		base.Start();
		myBody.drag = 0.5f;
	}


	// Update is called once per frame
	void Update () {
		base.Update();
		if(ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT) && canThrow && !GlobalVariableManager.Instance.TUT_POPUP_ISSHOWING){
			Invoke("Throw",.1f);
		}

		if(beingThrown){
			if(transform.position.y < landingY){
				ObjectPool.Instance.GetPooledObject("effect_enemyLand",gameObject.transform.position);
				Debug.Log("Reached Landing");
				beingThrown = false;
				myBody.gravityScale = 0f;
				myBody.velocity = new Vector2(0,0f);
				myBody.AddForce(new Vector2(-4f*(Mathf.Sign(gameObject.transform.lossyScale.x)),0f),ForceMode2D.Impulse);//slide
				beingCarried= false;
				canThrow = false;
				myShadow.SetActive(true);
			//	physicalCollision.enabled = false;
				gameObject.layer = 11; //switch to item layer.
				for(int i = 0; i < behaviorsToStop.Count; i++){
					behaviorsToStop[i].enabled = true;
				}
				gameObject.GetComponent<CannotExitScene>().enabled = false;


			}
		}

	}

	void Throw(){
		//beingCarried = false;
		player.GetComponent<PlayerTakeDamage>().currentlyCarriedObject = null;
		if(currentRoom != RoomManager.Instance.currentRoom){
			gameObject.GetComponent<CannotExitScene>().SetLimits(RoomManager.Instance.currentRoom);
		}
	//	physicalCollision.enabled = false;
		//gameObject.GetComponent<CannotExitScene>().enabled = true;
		Debug.Log("Thrown");
		GlobalVariableManager.Instance.CARRYING_SOMETHING = false;
		player.GetComponent<MeleeAttack>().enabled = true;
		if(gameObject.GetComponent<BoxCollider2D>()!=null){
			gameObject.GetComponent<BoxCollider2D>().enabled = true;
		}
		player.GetComponent<JimAnimationManager>().PlayAnimation("ani_jimThrowR",true);
		spinning = true;
		landingY = transform.position.y -3f;
		beingThrown = true;
		gameObject.transform.parent = null;
		myBody.simulated = true;
		myBody.velocity = new Vector2(22f*(Mathf.Sign(player.transform.lossyScale.x)),2f);
		myBody.gravityScale = 1f;
	
	}

	public override void PickUpEvent(){
		canThrow = true;
		myShadow.SetActive(false);
		SoundManager.instance.PlaySingle(carrySound);
		gameObject.layer = 15; //switch to thrownTrash layer.
		for(int i = 0; i < behaviorsToStop.Count; i++){
			behaviorsToStop[i].enabled = false;
		}
	}




}
