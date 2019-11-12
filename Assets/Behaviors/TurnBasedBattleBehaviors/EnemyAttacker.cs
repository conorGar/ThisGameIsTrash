using UnityEngine;
using System.Collections;

public class EnemyAttacker : MonoBehaviour
{

	public int currentHP;
	public int damageStr; //TODO: Replace with bank of different weapons?
	public int speed;
	protected EnemyStateController controller;

	public string myDeadBodyName;

	void Awake()
    {
        controller = GetComponent<EnemyStateController>();
    }

	public void TakeDamage(int damage){
		if(GameStateManager.Instance.GetCurrentState() == typeof(BattleState)){
		Debug.Log(this.gameObject.name + "Takes " + damage +" damage!");
			currentHP -= damage;
			SpawnDamageStars(damage);
			controller.SendTrigger(EnemyTrigger.HIT);

			//Check for death
			if(currentHP <= 0){
				Death();
			}else{
				BattleManager.Instance.ReturnFromAttack();
			}
		}
	}

	public IEnumerator MoveToAttack(){ //Handles Enemy Attack Animations/Movement to selected enemy
		//TODO:Once reach enemy gameObj...
		controller.SendTrigger(EnemyTrigger.LUNGE);
		Debug.Log("Enemy Move To Attack Activate" + BattleManager.Instance.currentState);
		while (controller.GetCurrentState() == EnemyState.LUNGE) //wait until end of lunge animation
           			yield return null;
		Debug.Log("Enemy Move To Attack Finish" +  BattleManager.Instance.currentState);

		BattleManager.Instance.ReturnFromAttack();
		gameObject.GetComponent<EnemyTurnDelayBar>().StartCount();
		//TODO: Return to start pos

	}

	void SpawnDamageStars(int damageDealt){
		GameObject damageCounter = ObjectPool.Instance.GetPooledObject("HitStars");
		damageCounter.GetComponent<Ev_HitStars>().ShowProperDamage(damageDealt);
		damageCounter.SetActive(true);
		GameObject littleStars = ObjectPool.Instance.GetPooledObject("effect_LittleStars");
		damageCounter.transform.position = new Vector3((transform.position.x), transform.position.y, transform.position.z);
		littleStars.transform.position = new Vector3((transform.position.x), transform.position.y, transform.position.z);
		littleStars.SetActive(true);
		damageCounter.GetComponent<Rigidbody2D>().AddForce(new Vector2(4f,10f), ForceMode2D.Impulse);
	}

	void Death(){
		BattleManager.Instance.RemoveEnemy(this);
		GetComponent<EnemyStateController>().SendTrigger(EnemyTrigger.DEATH);
		GameObject deathSmoke = ObjectPool.Instance.GetPooledObject("effect_SmokePuff"); 
		deathSmoke.transform.position = new Vector3((transform.position.x), transform.position.y, transform.position.z);
		GameObject deathGhost = ObjectPool.Instance.GetPooledObject("effect_DeathGhost",new Vector3((transform.position.x), transform.position.y, transform.position.z));
		deathGhost.GetComponent<Ev_DeathGhost>().OnSpawn();

		GameObject body = ObjectPool.Instance.GetPooledObject("enemyBody",gameObject.transform.position);
		body.GetComponent<tk2dSprite>().SetSprite(myDeadBodyName);

		this.gameObject.SetActive(false);

	}

}

