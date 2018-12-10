using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableThrowingObject : ThrowableObject {


	public override void PickUp(){
		gameObject.GetComponent<Renderer>().sortingLayerName = "Layer02";
		player.GetComponent<JimAnimationManager>().PlayAnimation("ani_pickUpBig",true);
		//gameObject.transform.position = new Vector2(player.transform.position.x,gameObject.transform.position.y);
		GlobalVariableManager.Instance.CARRYING_SOMETHING = true;
		physicalCollision.enabled = false;
		player.GetComponent<EightWayMovement>().enabled = false;
		player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		player.GetComponent<PlayerTakeDamage>().currentlyCarriedObject = this.gameObject;
		Invoke("PickUpWithDelay",.5f);
	}

	void PickUpWithDelay(){
		movePlayerToObject = false;
	
		physicalCollision.enabled = false;

		//move and play the particle system
		beingCarried = true;
		ObjectPool.Instance.GetPooledObject("effect_pickUpSmoke",gameObject.transform.position);
		SoundManager.instance.PlaySingle(pickup);
		//set object to follow player and push up in the sky
		gameObject.transform.parent = player.transform;
		if(!throwableObject){
		myBody.AddForce(new Vector2(0,10),ForceMode2D.Impulse);
			player.GetComponent<EightWayMovement>().carryingAbove = false;

		}else{
			player.GetComponent<EightWayMovement>().carryingAbove = true;
			myBody.AddForce(new Vector2(0,14),ForceMode2D.Impulse);

		}
		myBody.gravityScale = 2;

		pickUpSpin = true;
		spinning = true;
	}


	public override void LandingEvent(){
		ObjectPool.Instance.GetPooledObject("effect_largeLand");
		ObjectPool.Instance.ReturnPooledObject(this.gameObject);
	}
}
