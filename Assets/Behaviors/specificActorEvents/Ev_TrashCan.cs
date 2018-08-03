using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ev_TrashCan : MonoBehaviour {

	int hp = 3;
	tk2dSpriteAnimator myAnim;
	tk2dSprite startSprite;

	public GameObject objectPool;
	public AudioClip hitSound;


	void Start () {
		myAnim = gameObject.GetComponent<tk2dSpriteAnimator>();
		//startSprite = gameObject.GetComponent<tk2dSprite>().CurrentSprite;
	}
	
	void Update () {
		
	}

	void OnCollisionEnter2D(Collision2D collision){
		if(collision.gameObject.tag == "Weapon"){
			hp--;
			if(hp>0){
				myAnim.Play("hit");
				SoundManager.instance.PlaySingle(hitSound);
				GameObject damageCounter = objectPool.GetComponent<ObjectPool>().GetPooledObject("HitStars");
				damageCounter.GetComponent<Ev_HitStars>().ShowProperDamage(1);
				damageCounter.SetActive(true);
				GameObject littleStars = objectPool.GetComponent<ObjectPool>().GetPooledObject("effect_LittleStars");
				damageCounter.transform.position = new Vector3((transform.position.x), transform.position.y, transform.position.z);
				littleStars.transform.position = new Vector3((transform.position.x), transform.position.y, transform.position.z);
				littleStars.SetActive(true);
			}else{
				myAnim.Play("fall");
				SoundManager.instance.PlaySingle(hitSound);
			}


		}
	}

	IEnumerator ContinueHit(){
		if(hp > 0){
			yield return new WaitForSeconds(.2f);
			myAnim.Stop();
		}else{
		//spawn pin
		}
	}
}
