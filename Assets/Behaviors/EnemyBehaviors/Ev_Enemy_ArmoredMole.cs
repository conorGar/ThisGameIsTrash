using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ev_Enemy_ArmoredMole : MonoBehaviour {

	tk2dSpriteAnimator myAnim;

	int startThrowOnce = 0;
	int tossRockOnce;
	GameObject player;
	GameObject myBoulder;
	// Use this for initialization
	void Start () {

		player = GameObject.FindGameObjectWithTag("Player");

	}

	void OnEnable(){
		player = GameObject.FindGameObjectWithTag("Player");
		myAnim = gameObject.GetComponent<tk2dSpriteAnimator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
            if (Vector2.Distance(gameObject.transform.position, player.transform.position) < 20f) {
                if (startThrowOnce == 0) {
                    StartCoroutine("TossRock");
                    startThrowOnce = 1;
                }
            } else {
                //Debug.Log(Vector2.Distance(gameObject.transform.position,player.transform.position));
                if (startThrowOnce == 1) {
                    StopAllCoroutines();
                    tossRockOnce = 0;
                    startThrowOnce = 0;
                }

            }
        }
	}

	IEnumerator TossRock(){
		Debug.Log("TossedRock");
		if(tossRockOnce == 0){
	        tossRockOnce = 1;
	        myAnim.Play("throw");
	        yield return new WaitForSeconds(.5f);
	        myBoulder = ObjectPool.Instance.GetPooledObject("projectile_boulder", gameObject.transform.position,true);
	        //myBoulder.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f,8f),ForceMode2D.Impulse);
	        yield return new WaitForSeconds(.2f);
	        myAnim.Play("idle");
	        yield return new WaitForSeconds(Random.Range(3f,4.7f));
	        tossRockOnce = 0;
	        StartCoroutine("TossRock");
		}
	}
}
