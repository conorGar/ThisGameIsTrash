using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableObject : PickupableObject {

	float landingY;
	bool beingThrown;
	bool canThrow;
	// Use this for initialization

	// Update is called once per frame
	void Update () {
		base.Update();
		if(ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT) && canThrow){
			Invoke("Throw",.1f);
		}

		if(beingThrown){
			if(transform.position.y < landingY){
				ObjectPool.Instance.GetPooledObject("effect_enemyLand",gameObject.transform.position);
				Debug.Log("Reached Landing");
				beingThrown = false;
				myBody.gravityScale = 0f;
				myBody.velocity = new Vector2(0,0f);
				myBody.AddForce(new Vector2(4f*(Mathf.Sign(gameObject.transform.lossyScale.x)),0f),ForceMode2D.Impulse);//slide
				beingCarried= false;
				canThrow = false;


			}
		}

	}

	void Throw(){
		//beingCarried = false;
		Debug.Log("Thrown");
		GlobalVariableManager.Instance.CARRYING_SOMETHING = false;
		player.GetComponent<MeleeAttack>().enabled = true;
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
	}
}
