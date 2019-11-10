using UnityEngine;
using System.Collections;

public class EnemyAttacker : MonoBehaviour
{

	public int currentHP;
	public int damageStr; //TODO: Replace with bank of different weapons?
	public int speed;



	public void TakeDamage(int damage){
		if(GameStateManager.Instance.GetCurrentState() == typeof(BattleState)){
			currentHP -= damage;
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

}

