using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableObject : PickupableObject {

	float landingY;
	bool beingThrown;
	bool canThrow;

	public AudioClip carrySound;

	public List<MonoBehaviour> behaviorsToStop = new List<MonoBehaviour>();
	// Use this for initialization
	void Start(){
		base.Start();
		myBody.drag = 0.5f;
	}
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
				gameObject.layer = 11; //switch to item layer.
				for(int i = 0; i < behaviorsToStop.Count; i++){
					behaviorsToStop[i].enabled = true;
				}

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
		SoundManager.instance.PlaySingle(carrySound);
		gameObject.layer = 15; //switch to thrownTrash layer.
		for(int i = 0; i < behaviorsToStop.Count; i++){
			behaviorsToStop[i].enabled = false;
		}
	}
}
