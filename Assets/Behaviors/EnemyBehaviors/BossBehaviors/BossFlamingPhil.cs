using UnityEngine;
using System.Collections;

public class BossFlamingPhil : Boss
{
	public GeneralGrubFriend friend; // set by "GeneralGrubFriend"
	public GameObject projectile;
	public float fireRate;
	public float randomRateChanger;
	public GameObject bossBlockades;

	public float nextFireTime = 0f;

	void OnEnable(){
		nextFireTime = fireRate + Time.time + + Random.Range(0,randomRateChanger);
		bossBlockades.SetActive(true);

	}
	
	// Update is called once per frame
	void Update ()
	{
		if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
            if (controller.GetCurrentState() == EnemyState.IDLE) {
                if (nextFireTime < Time.time) {
                   FireProjectiles();
        
                }
            }
        }
	}
	public override void ActivateBoss ()
	{
		ActivateHpDisplay();
		//do nothing
	}

	public override void BossDeathEvent(){
		friend.SetFriendState("GENERAL_FIGHT_INTRO");
		CamManager.Instance.mainCamEffects.ReturnFromCamEffect();
		bossBlockades.SetActive(false);

		gameObject.SetActive(false);
	}

	void FireProjectiles(){ //fire 4 bullets diagnolly
		controller.SendTrigger(EnemyTrigger.THROW);

			GameObject bullet = ObjectPool.Instance.GetPooledObject(projectile.tag, gameObject.transform.position);
	        Vector2 movementDir = new Vector2(5,5);
	        bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(movementDir.x, movementDir.y);

			GameObject bullet2 = ObjectPool.Instance.GetPooledObject(projectile.tag, gameObject.transform.position);
	        movementDir = new Vector2(5,-5);
	        bullet2.GetComponent<Rigidbody2D>().velocity = new Vector2(movementDir.x, movementDir.y);

			GameObject bullet3 = ObjectPool.Instance.GetPooledObject(projectile.tag, gameObject.transform.position);
	        movementDir = new Vector2(-5,-5);
	        bullet3.GetComponent<Rigidbody2D>().velocity = new Vector2(movementDir.x, movementDir.y);


			GameObject bullet4 = ObjectPool.Instance.GetPooledObject(projectile.tag, gameObject.transform.position);
	        movementDir = new Vector2(-5,5);
	        bullet4.GetComponent<Rigidbody2D>().velocity = new Vector2(movementDir.x, movementDir.y);


        nextFireTime = fireRate + Time.time + Random.Range(0,randomRateChanger);
	}
}

