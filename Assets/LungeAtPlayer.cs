using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LungeAtPlayer : MonoBehaviour {

	public GameObject player;
	public float speed = 15;
	public float distanceTillLunge = 8;

	Vector2 movementDir;
	int lungeOnce = 0;
	tk2dSpriteAnimator myAnim;

	void Start () {
		if(player == null){
			player = GameObject.FindGameObjectWithTag("Player");
		}
		myAnim = this.gameObject.GetComponent<tk2dSpriteAnimator>();

	}
	
	// Update is called once per frame
	void Update () {
		if(Vector2.Distance(player.transform.position,gameObject.transform.position) < distanceTillLunge && lungeOnce == 0){
			StartCoroutine("Lunge");
			lungeOnce = 1;
		}
	}

	IEnumerator Lunge(){
		gameObject.GetComponent<FollowPlayer>().enabled = false;
		gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		myAnim.Play("prepare");
		yield return new WaitForSeconds(.5f);
		gameObject.GetComponent<EnemyTakeDamage>().dontStopWhenHit = true;
		ObjectPool.Instance.GetPooledObject("effect_enemyLand",gameObject.transform.position); //dash clouds
		myAnim.Play("chase");
		movementDir = (player.transform.position - gameObject.transform.position).normalized * speed;
		gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(movementDir.x,movementDir.y);
		yield return new WaitForSeconds(1f);
		gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		gameObject.GetComponent<EnemyTakeDamage>().dontStopWhenHit = false;
		yield return new WaitForSeconds(.5f);
		gameObject.GetComponent<FollowPlayer>().enabled = true;
		lungeOnce = 0;


	}
}
