using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ev_Enemy_ArmoredMole : MonoBehaviour {

	tk2dSpriteAnimator myAnim;

	int startThrowOnce = 0;
	GameObject player;
	// Use this for initialization
	void Start () {

		player = GameObject.FindGameObjectWithTag("Player");

	}

	void OnEnable(){
		player = GameObject.FindGameObjectWithTag("Player");
		myAnim = gameObject.GetComponent<tk2dSpriteAnimator>();
		//StartCoroutine("TossRock");
	}
	
	// Update is called once per frame
	void Update () {
		if(Vector2.Distance(gameObject.transform.position,player.transform.position) < 20f){
		 if(startThrowOnce == 0){
			startThrowOnce = 1;
			StartCoroutine("TossRock");
			}
		}else{
			//Debug.Log(Vector2.Distance(gameObject.transform.position,player.transform.position));
			if(startThrowOnce == 1){
			CancelInvoke();
			startThrowOnce = 0;
			}

		}
	}

	IEnumerator TossRock(){
		Debug.Log("TossedRock");
		myAnim.Play("throw");
		yield return new WaitForSeconds(.5f);
		ObjectPool.Instance.GetPooledObject("projectile_boulder", gameObject.transform.position,true);
		yield return new WaitForSeconds(.2f);
		myAnim.Play("idle");
		yield return new WaitForSeconds(Random.Range(3f,4.7f));
		StartCoroutine("TossRock");
	}
}
