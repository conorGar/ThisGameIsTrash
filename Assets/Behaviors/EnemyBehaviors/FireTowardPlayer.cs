using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTowardPlayer : MonoBehaviour {

	public float projectileSpeed;
	public float fireRate;

	GameObject player;
	public GameObject projectile;


	private tk2dSpriteAnimator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<tk2dSpriteAnimator>();
		player  = GameObject.Find("enemy_rat");
		InvokeRepeating("Fire",2.0f,fireRate);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void Fire(){
		Debug.Log("fired");

		anim.Play("throwL");

		if(player.transform.position.x < transform.position.x){
			transform.localScale = new Vector3(1,1,1);
			} else{
			transform.localScale = new Vector3(-1,1,1);
		}
		AnimationControl();



	}

	IEnumerator AnimationControl(){

		yield return new WaitForSeconds(0.7f);
		Vector3 playerPosition = new Vector3(player.transform.position.x,player.transform.position.y,player.transform.position.z);
		GameObject bullet;
		bullet = Instantiate(projectile, transform.position, Quaternion.identity);
		//bullet.transform.rotation = Quaternion.LookRotation(playerPosition);
		//bullet.transform.position += bullet.transform.forward * projectileSpeed * Time.deltaTime;
			//bullet.velocity = (player.transform.position).normalized *projectileSpeed;
		bullet.GetComponent<Rigidbody2D>().velocity = (player.transform.position).normalized *projectileSpeed;

		anim.Play("idleL");
	}
}
