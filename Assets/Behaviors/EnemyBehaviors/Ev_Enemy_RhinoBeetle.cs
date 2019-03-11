using UnityEngine;
using System.Collections;
using GenericEnemyStateController = EnemyStateController<EnemyState, EnemyTrigger>;

public class Ev_Enemy_RhinoBeetle : FollowPlayerAfterNotice
{
	public float projectileSpeed;
	public GameObject projectile;
	public BoxCollider2D jumpBounds;
	GameObject target;
	float MIN_X;
	float MIN_Y;
	float MAX_X;
	float MAX_Y;
	float nextActionTime;
	int throwOnceCheck;
	// Use this for initialization
	void OnEnable(){
		base.OnEnable();
		SetWalkBounds(jumpBounds);
	}
	
	// Update is called once per frame
	void Update ()
	{
		base.Update();
		if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
			if(controller.IsFlag((int)EnemyFlag.CHASING)){
				if(Vector2.Distance(gameObject.transform.position,PlayerManager.Instance.player.transform.position) < 5){
					//Jumps away if player gets close
					StartCoroutine("LeapAway");
				}else{
				if(Time.time > nextActionTime){
						if(throwOnceCheck == 0){
							Debug.Log("Fire rate reached, throw time is now");
							//StartCoroutine("MakeBall");
							throwOnceCheck = 1;
						}

					}
				}
			}
        }
	}

	IEnumerator LeapAway(){
		Debug.Log("Leapaway started");
		//checks if near one of the bounds, if so jump in a direction away from player but not past bounds
		controller.SendTrigger(EnemyTrigger.LUNGE); //should stop chasing flag as well...
		ObjectPool.Instance.GetPooledObject("effect_enemyLand",new Vector2(gameObject.transform.position.x,gameObject.transform.position.y-1));

		if(PlayerManager.Instance.player.transform.position.x < gameObject.transform.position.x){
			if(Mathf.Abs(transform.position.x - MAX_X) < 5f){
				if(Mathf.Abs(transform.position.y - MAX_Y) > Mathf.Abs(transform.position.y - MIN_Y)){
					//Jump up if there is more distance between current position and highest possible
					if(Mathf.Abs(PlayerManager.Instance.player.transform.position.x - transform.position.x) <3f){
						//jump left if player is following from nearly directl above/below
						gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-45f,0f),ForceMode2D.Impulse);

					}else{
						gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f,40f),ForceMode2D.Impulse);
					}
				}else{
					//Jump Down
					if(Mathf.Abs(PlayerManager.Instance.player.transform.position.x - transform.position.x) <3f){
						//jump left if player is following from nearly directl above/below
						gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-45f,0f),ForceMode2D.Impulse);

					}else{
						gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f,-40f),ForceMode2D.Impulse);
					}
				}
			}else{
				//Jump Right
				gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(40f,0f),ForceMode2D.Impulse);

			}

		}else{
			if(Mathf.Abs(transform.position.x - MIN_X) < 5f){
				if(Mathf.Abs(transform.position.y - MAX_Y) > Mathf.Abs(transform.position.y - MIN_Y)){
					if(Mathf.Abs(PlayerManager.Instance.player.transform.position.x - transform.position.x) <3f){
						//jump right if player is following from nearly directl above/below
						gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(45f,0f),ForceMode2D.Impulse);

					}else{
						gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f,40f),ForceMode2D.Impulse);
					}
				}else{
					//Jump Down
					if(Mathf.Abs(PlayerManager.Instance.player.transform.position.x - transform.position.x) <3f){
						//jump right if player is following from nearly directl above/below
						gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(45f,0f),ForceMode2D.Impulse);

					}else{
						gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f,-40f),ForceMode2D.Impulse);
					}

				}
			}else{
				//Jump Left
				gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-40f,0f),ForceMode2D.Impulse);

			}
		}

		//while (controller.GetCurrentState() == EnemyState.LUNGE)
           			//yield return null;
        yield return new WaitForSeconds(.2f);
		gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		controller.SendTrigger(EnemyTrigger.RECOVER);
		while (controller.GetCurrentState() == EnemyState.RECOVER)
           			yield return null;
		



	}



	IEnumerator MakeBall(){
		controller.SendTrigger(EnemyTrigger.PREPARE); //should stop chasing flag as well...
		while (controller.GetCurrentState() == EnemyState.PREPARE)
           			yield return null;
        ThrowBall();

	}

	void ThrowBall(){
		controller.SendTrigger(EnemyTrigger.THROW);
		GameObject bullet = ObjectPool.Instance.GetPooledObject(projectile.tag,gameObject.transform.position);
					bullet.GetComponent<Ev_ProjectileTowrdPlayer>().enabled = true; // starts off disabled only so i didnt have to make another tag for rocks that DONT follow player(like ones that spawn from boulder.) feel free to just do that if tis causes issues

			if(bullet.GetComponent<Ev_ProjectileTowrdPlayer>() != null){
				bullet.GetComponent<Ev_ProjectileTowrdPlayer>().speed = projectileSpeed;
				bullet.GetComponent<Ev_ProjectileTowrdPlayer>().target = this.target;
			}
		bullet.GetComponent<Rigidbody2D>().gravityScale = 0;
		//SoundManager.instance.PlaySingle(throwSFX);
		nextActionTime = Time.time + 3;
		throwOnceCheck = 0;
		controller.SendTrigger(EnemyTrigger.CHASE); //should stop chasing flag as well...


	}
	public void SetWalkBounds(BoxCollider2D limits){
		
        MIN_X =	limits.bounds.min.x;
        MAX_X = limits.bounds.max.x;
        MIN_Y = limits.bounds.min.y;
        MAX_Y = limits.bounds.max.y;

       
	}
}

