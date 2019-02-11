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
	public GameObject target;

	private tk2dSpriteAnimator anim;

	// Use this for initialization
	void OnEnable () {
		Debug.Log("Fire toward player on enable activated");
		anim = GetComponent<tk2dSpriteAnimator>();
		CancelInvoke();
        //InvokeRepeating("Fire",2.0f,fireRate);

        if (PlayerManager.Instance.player)
            target = PlayerManager.Instance.player;
        StartCoroutine("Fire");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator Fire(){
		yield return new WaitForSeconds(fireRate);
		if(!GlobalVariableManager.Instance.IS_HIDDEN){ //wont fire at player if player is hidden
			if(gameObject.activeInHierarchy == false){
				StopCoroutine("Fire");
			}
			Debug.Log("fired");
			if(anim.CurrentClip.name != "hit"){
				anim.Play("throwL");
				if(target.transform.position.x < transform.position.x){
					transform.localScale = new Vector3(1,1,1);
				} else{
					transform.localScale = new Vector3(-1,1,1);
				}
				if(gameObject.activeInHierarchy)
					StartCoroutine("AnimationControl");
					StopCoroutine("Fire");
			}
		}

	}

	IEnumerator AnimationControl(){

		yield return new WaitForSeconds(0.7f);
		//Vector3 playerPosition = new Vector3(player.transform.position.x,player.transform.position.y,player.transform.position.z);

		Debug.Log("FiredObject");
		GameObject bullet = ObjectPool.Instance.GetPooledObject(projectile.tag,gameObject.transform.position);
		bullet.GetComponent<Ev_ProjectileTowrdPlayer>().enabled = true; // starts off disabled only so i didnt have to make another tag for rocks that DONT follow player(like ones that spawn from boulder.) feel free to just do that if tis causes issues
		if(bullet.GetComponent<Ev_ProjectileTowrdPlayer>() != null){
			bullet.GetComponent<Ev_ProjectileTowrdPlayer>().target = this.target;
		}
		bullet.GetComponent<Rigidbody2D>().gravityScale = 0;
		SoundManager.instance.PlaySingle(throwSFX);

		if(!myProjectileFalls && bullet.GetComponent<Ev_FallingProjectile>() !=null)
			bullet.GetComponent<Ev_FallingProjectile>().enabled = false;
		//bullet.GetComponent<Ev_ProjectileTowrdPlayer>().speed = projectileSpeed;
		//bullet.GetComponent<Rigidbody2D>().velocity = (player.transform.position).normalized *projectileSpeed;

		anim.Play("idle");
		StartCoroutine("Fire");
		StopCoroutine("AnimationControl");

	}
}
