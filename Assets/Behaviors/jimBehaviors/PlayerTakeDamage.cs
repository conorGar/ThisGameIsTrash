using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTakeDamage : MonoBehaviour {

	public GameObject objectPool;
	public GameObject HPdisplay;
	public Ev_MainCamera currentCam;

	int maxHP;
	int currentHp;
	int damageDealt;

	bool currentlyTakingDamage = false;
	// Use this for initialization
	void Start () {
		maxHP = GlobalVariableManager.Instance.Max_HP;	
		currentHp = maxHP;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D(Collision2D enemy){
		if(enemy.gameObject.layer == 9 && !currentlyTakingDamage){ //layer 9 = enemies
			damageDealt = enemy.gameObject.GetComponent<Enemy>().attkPower;
			currentlyTakingDamage = true;
			GlobalVariableManager.Instance.PLAYER_CAN_MOVE = false;
			currentHp -= damageDealt;
			gameObject.GetComponent<tk2dSpriteAnimator>().Play("hurt");
			Debug.Log("reached this end of hp hud change" + currentHp);
			HPdisplay.GetComponent<GUI_HPdisplay>().UpdateDisplay(currentHp);
			GameObject damageCounter = objectPool.GetComponent<ObjectPool>().GetPooledObject("HitStars",this.gameObject.transform.position);
			damageCounter.GetComponent<Ev_HitStars>().ShowProperDamage(damageDealt);
		
			GameObject littleStars = objectPool.GetComponent<ObjectPool>().GetPooledObject("effect_LittleStars",this.gameObject.transform.position);

			currentCam.StartCoroutine("ScreenShake",.2f);

			if(enemy.transform.position.x < gameObject.transform.position.x){
				gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(17f,0f), ForceMode2D.Impulse);
				damageCounter.GetComponent<Rigidbody2D>().AddForce(new Vector2(4f,10f), ForceMode2D.Impulse);
			}else{
				gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-17f,0f), ForceMode2D.Impulse);
				damageCounter.GetComponent<Rigidbody2D>().AddForce(new Vector2(-4f,10f), ForceMode2D.Impulse);
			}

			if(currentHp <= 0){
				Death();
			}else{
				StartCoroutine("RegainControl");
			}
		}
	}

	IEnumerator RegainControl(){
		yield return new WaitForSeconds(.2f);
		gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,0f);
		GlobalVariableManager.Instance.PLAYER_CAN_MOVE = true;
		yield return new WaitForSeconds(.5f); //brief period of invincibility
		currentlyTakingDamage = false;
		Debug.Log("Regained Control");

	}

	void Death(){

	}
}
