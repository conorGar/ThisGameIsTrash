using UnityEngine;
using System.Collections;

public class ThrowableBody : ThrowableObject
{
	public int bodyHP = 3;
	public GameObject fliesPS;

	[HideInInspector]
	public bool poisioned;

	string mySpawnerID;


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

	public void Poison(){
		poisioned = true;
		gameObject.GetComponent<tk2dSprite>().color = new Color(.17f,1f,.25f);//green color to toxic 
		fliesPS.SetActive(true);
	}

	public void SetSpawnerID(string id){
		mySpawnerID = id;
	}

	public IEnumerator Impact(GameObject pointOfImpact){
		gameObject.GetComponent<BoxCollider2D>().enabled = false;
		gameObject.transform.position = pointOfImpact.transform.position;
		float currentDirection = gameObject.GetComponent<Rigidbody2D>().velocity.x;
		gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		ObjectPool.Instance.GetPooledObject("effect_thrownImpact",transform.position);
		if(currentDirection < 0)
			gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(3f,5f),ForceMode2D.Impulse);
		else
			gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-3f,5f),ForceMode2D.Impulse);
		yield return new WaitForSeconds(.2f);

	}

	public void TakeDamage(){
		bodyHP--;
		if(bodyHP <= 0){
			Death();
		}
	}

	public void Death(){
		GlobalVariableManager.Instance.BASIC_ENEMY_LIST[this.mySpawnerID].bodyDestroyed = true;
		myBody.gravityScale = 0f;
		myBody.velocity = new Vector2(0,0f);
		beingCarried= false;
		canThrow = false;
		myShadow.SetActive(true);
		gameObject.layer = 11; //switch to item layer.
		this.gameObject.SetActive(false);
	}
}

