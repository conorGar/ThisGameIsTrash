using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTowardPlayer : MonoBehaviour {

	public float projectileSpeed;
	public float fireRate;
	public bool myProjectileFalls = false;

	public GameObject projectile;
	public AudioClip throwSFX;

	[HideInInspector]
	public GameObject player;

	private tk2dSpriteAnimator anim;

	// Use this for initialization
	void OnEnable () {
		anim = GetComponent<tk2dSpriteAnimator>();
		CancelInvoke();
		InvokeRepeating("Fire",2.0f,fireRate);
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void Fire(){
		if(gameObject.activeInHierarchy == false){
			CancelInvoke();
		}
		//Debug.Log("fired");
		if(anim.CurrentClip.name != "hit"){
			anim.Play("throwL");
			if(player.transform.position.x < transform.position.x){
				transform.localScale = new Vector3(1,1,1);
			} else{
				transform.localScale = new Vector3(-1,1,1);
			}
			if(gameObject.activeInHierarchy)
				StartCoroutine("AnimationControl");
		}


	}

	IEnumerator AnimationControl(){

		yield return new WaitForSeconds(0.7f);
		//Vector3 playerPosition = new Vector3(player.transform.position.x,player.transform.position.y,player.transform.position.z);

		Debug.Log("FiredObject");
		GameObject bullet = ObjectPool.Instance.GetPooledObject("projectile_largeRock",gameObject.transform.position);
		bullet.GetComponent<Ev_ProjectileTowrdPlayer>().enabled = true; // starts off disabled only so i didnt have to make another tag for rocks that DONT follow player(like ones that spawn from boulder.) feel free to just do that if tis causes issues
		if(bullet.GetComponent<Ev_ProjectileTowrdPlayer>() != null){
			bullet.GetComponent<Ev_ProjectileTowrdPlayer>().player = this.player;
		}
		bullet.GetComponent<Rigidbody2D>().gravityScale = 0;
		SoundManager.instance.PlaySingle(throwSFX);

		if(!myProjectileFalls)
			bullet.GetComponent<Ev_FallingProjectile>().enabled = false;
		//bullet.GetComponent<Ev_ProjectileTowrdPlayer>().speed = projectileSpeed;
		//bullet.GetComponent<Rigidbody2D>().velocity = (player.transform.position).normalized *projectileSpeed;

		anim.Play("idle");
	}
}
