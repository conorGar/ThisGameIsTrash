using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableThrowingObject : ThrowableObject {


	public override void PickUp(){
		gameObject.GetComponent<Renderer>().sortingLayerName = "Layer02";
		gameObject.GetComponent<Animator>().enabled = true;
		base.PickUp();
		physicalCollision.enabled = false;
		player.GetComponent<JimAnimationManager>().PlayAnimation("ani_jimPickUp",true);

		StartCoroutine("PickUpWithDelay");

        if (myShadow != null)
		    myShadow.SetActive(false);
	}

	IEnumerator PickUpWithDelay(){
		yield return new WaitForSeconds(1f);
		player.GetComponent<EightWayMovement>().myLegs.SetActive(false);

		if(livingBody){
			gameObject.GetComponent<tk2dSpriteAnimator>().Play("carry"); 
			GameObject panicSweat = ObjectPool.Instance.GetPooledObject("effect_carrySweat",gameObject.transform.position);
			panicSweat.transform.parent = gameObject.transform;
		}
        if (myShadow != null)
		    myShadow.SetActive(true);
		SoundManager.instance.PlaySingle(SFXBANK.ITEM_CATCH);
		canThrow = true;
	}


	public override void LandingEvent(){
		ObjectPool.Instance.GetPooledObject("effect_largeLand");
		ObjectPool.Instance.ReturnPooledObject(this.gameObject);
	}
}
