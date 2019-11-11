using UnityEngine;
using System.Collections;

public class EnemyAttacker : MonoBehaviour
{

	public int currentHP;
	public int damageStr; //TODO: Replace with bank of different weapons?
	public int speed;
	protected EnemyStateController controller;


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

			//TODO:Check for death
			BattleManager.Instance.ReturnFromAttack();
		}
	}

	public IEnumerator MoveToAttack(){ //Handles Enemy Attack Animations/Movement to selected enemy
		//TODO:Once reach enemy gameObj...
		Debug.Log("Enemy Move To Attack Activate" + BattleManager.Instance.currentState);
		yield return new WaitForSeconds(1.5f);
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

}

