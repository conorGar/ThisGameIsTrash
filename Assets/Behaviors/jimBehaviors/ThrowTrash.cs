using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowTrash : MonoBehaviour {

	// Use this for initialization
	//tk2dSpriteAnimator myAnim;
	public AudioClip throwSfx;


	JimAnimationManager myAnim;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKLEFT)){
			StartCoroutine("Throw","left");
		}else if(ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKRIGHT)){
			StartCoroutine("Throw","right");
		}else if(ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKUP)){
			StartCoroutine("Throw","up");
		}else if(ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKDOWN)){
			StartCoroutine("Throw","down");
		}

		myAnim = gameObject.GetComponent<JimAnimationManager>();

	}

	IEnumerator Throw(string direction){

		yield return new WaitForSeconds(.1f);
		GameObject thrownTrash = ObjectPool.Instance.GetPooledObject("thrownTrash",gameObject.transform.position);
		thrownTrash.GetComponent<Ev_ThrownTrash>().direction = direction;
		if(direction == "right"){
			thrownTrash.GetComponent<Rigidbody2D>().velocity = new Vector2(10f,0f);
			myAnim.PlayAnimation("ani_jimThrowR",true);
		}else if(direction == "left"){
			thrownTrash.GetComponent<Rigidbody2D>().velocity = new Vector2(-10f,0f);
			myAnim.PlayAnimation("ani_jimThrowR",true);
		}else if(direction == "down"){
			thrownTrash.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,-10f);
			myAnim.PlayAnimation("ani_jimThrowDown",true);
		}else if(direction == "up"){
			myAnim.PlayAnimation("ani_jimThrowUp",true);
			thrownTrash.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,10f);
		}
	}
}
