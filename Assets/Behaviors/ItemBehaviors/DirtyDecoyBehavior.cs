using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DirtyDecoyBehavior : MonoBehaviour
{
	int currentHP;
	bool currentlyTakingDamage;
	int damageDealt;

	GameObject returnPlayer;

	void OnEnable(){
		currentHP = 3;
		ObjectPool.Instance.GetPooledObject("effect_smokeBurst",gameObject.transform.position);
		for(int i = 0; i < RoomManager.Instance.currentRoom.enemies.Count;i++){
			GameObject enemy = RoomManager.Instance.currentRoom.enemies[i];
			if(enemy.GetComponent<FollowPlayer>() != null){
				enemy.GetComponent<FollowPlayer>().player = this.gameObject.transform;
			}
			if(enemy.GetComponent<FireTowardPlayer>() != null){
				enemy.GetComponent<FireTowardPlayer>().player = this.gameObject;
			}
		}

	}

	void Start ()
	{
		returnPlayer = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnCollisionEnter2D(Collision2D enemy){
        if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
            if (enemy.gameObject.layer == 9 && !currentlyTakingDamage) { //layer 9 = enemies

                if (enemy.gameObject.tag == "Boss") {
                    damageDealt = enemy.gameObject.GetComponent<Boss>().attkDmg;
                }
                else {
                    damageDealt = enemy.gameObject.GetComponent<Enemy>().attkPower;
                }
                TakeDamage();
            }
        }
	}
	void OnTriggerEnter2D(Collider2D projectile){
        if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
            if (projectile.gameObject.layer == 10 && !currentlyTakingDamage) { //layer 10 = projectiles

                damageDealt = projectile.gameObject.GetComponent<Projectile>().damageToPlayer;

                TakeDamage();
            }
            else if (projectile.gameObject.layer == 16 && !currentlyTakingDamage) {//enemy with non-solid collision(flying enemy)
                if (projectile.gameObject.tag == "Boss") {
                    damageDealt = projectile.gameObject.GetComponent<Boss>().attkDmg;
                }
                else {
                    damageDealt = projectile.gameObject.GetComponent<Enemy>().attkPower;
                }
                TakeDamage();
            }
        }
	}

	void TakeDamage(){
		currentlyTakingDamage = true;
		currentHP -= damageDealt;
		GameObject damageCounter = ObjectPool.Instance.GetPooledObject("HitStars_player",this.gameObject.transform.position);
		damageCounter.GetComponent<Ev_HitStars>().ShowProperDamage(damageDealt);
		damageCounter.SetActive(true);
		GameObject littleStars = ObjectPool.Instance.GetPooledObject("effect_LittleStars",this.gameObject.transform.position);
		littleStars.SetActive(true);
		if(currentHP >0){
			StartCoroutine("HitDelay");
		}else{
			Death();
		}
	}


	void Death(){
		ObjectPool.Instance.GetPooledObject("effect_SmokePuff",gameObject.transform.position);
		for(int i = 0; i < RoomManager.Instance.currentRoom.enemies.Count;i++){
			GameObject enemy = RoomManager.Instance.currentRoom.enemies[i];
			if(enemy.GetComponent<FollowPlayer>() != null){
				enemy.GetComponent<FollowPlayer>().player = returnPlayer.transform;
			}
			if(enemy.GetComponent<FireTowardPlayer>() != null){
				enemy.GetComponent<FireTowardPlayer>().player = returnPlayer;
			}
		}
		ObjectPool.Instance.ReturnPooledObject(this.gameObject);

	}

	IEnumerator HitDelay(){
		yield return new WaitForSeconds(.5f);
		currentlyTakingDamage = false;
	}
}

