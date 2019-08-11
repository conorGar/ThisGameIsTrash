using UnityEngine;
using System.Collections;

public class Ev_Enemy_BoomerangSlime : FireTowardPlayerEnhanced
{
	GameObject boomerang;
	bool boomerangCatchDelay = false;
	protected override void Update(){
		if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
			if (controller.IsFlag((int)EnemyFlag.CHASING) && controller.GetCurrentState()!= EnemyState.PREPARE && controller.GetCurrentState()!= EnemyState.THROW) {
                if (nextFireTime < Time.time) {
                    controller.SendTrigger(EnemyTrigger.PREPARE);
                    if (buildupSfx != null) {
                        SoundManager.instance.PlaySingle(buildupSfx);
                    }
                }
            }
        }
		if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
          
                if(boomerang && Vector2.Distance(gameObject.transform.position, boomerang.transform.position) < 2f && boomerangCatchDelay){ //'catch' the boomerang
                	RecoverBoomerang();
                }
			 if (controller.GetCurrentState() == EnemyState.DEAD) {
                if(boomerang){
                	ObjectPool.Instance.ReturnPooledObject(boomerang); //remove the boomerang from the screen if dies.
                }
            }
        }
	}
	
    public override void Fire() {
    	Debug.Log("Fire Activated");
        SoundManager.instance.PlaySingle(throwSFX);
        boomerang = ObjectPool.Instance.GetPooledObject(projectile.tag, gameObject.transform.position);
        Vector2 movementDir = (PlayerManager.Instance.player.transform.position - gameObject.transform.position).normalized;
        boomerang.GetComponent<Rigidbody2D>().velocity = new Vector2(movementDir.x, movementDir.y);

		boomerang.GetComponent<Ev_ProjectileBoomerang>().startingPlayerPos = PlayerManager.Instance.player.transform.position;
		boomerang.GetComponent<Ev_ProjectileBoomerang>().myEnemy = this;
		boomerang.GetComponent<Ev_ProjectileBoomerang>().direction = movementDir;
		boomerang.GetComponent<Ev_ProjectileBoomerang>().speedx = 14;
		boomerang.GetComponent<Rigidbody2D>().velocity  = movementDir*14;

        boomerang.GetComponent<Ev_ProjectileBoomerang>().InvokeRepeating("Slowdown",.6f, .1f);
        StartCoroutine("Delay");
    }


    IEnumerator Delay(){ //prevents the slime from immediately catching the boomerang when thrown
	    yield return new WaitForSeconds(.2f);
	    boomerangCatchDelay = true;
	    yield return new WaitForSeconds(8);
	    if(boomerangCatchDelay){ //remove boomerang if still havent caught in 8s (moved past thrower somehow)
	    	RecoverBoomerang();
	    }
    }

    void RecoverBoomerang(){
    	StopCoroutine("Delay");
    	boomerang.GetComponent<Ev_ProjectileBoomerang>().CancelInvoke("Slowdown");
		gameObject.GetComponent<EnemyTakeDamage>().armorRating += 1; //return to armor rating. **IF FUTURE PROBLEMS CREATE SEPEARTE CODE THAT HANDLES ARMOR RATINGS BASED ON STATE
		nextFireTime = fireRate + Time.time + Random.Range(0,randomRateChanger);
		ObjectPool.Instance.ReturnPooledObject(boomerang); //remove the boomerang from the screen
		controller.SendTrigger(EnemyTrigger.CHASE);

		boomerangCatchDelay = false;

    }


}

