using UnityEngine;
using System.Collections;

public class EnemyAttacker : MonoBehaviour
{

	public int currentHP;
	public int damageStr; //TODO: Replace with bank of different weapons?
	public int speed;



	public void TakeDamage(int damage){
		currentHP -= damage;
		//TODO:Check for death
		BattleManager.Instance.ReturnFromAttack();
	}

	public IEnumerator MoveToAttack(){ //Handles Enemy Attack Animations/Movement to selected enemy
		//TODO:Once reach enemy gameObj...
		yield return new WaitForSeconds(.5f);
		BattleManager.Instance.ReturnFromAttack();
		gameObject.GetComponent<EnemyTurnDelayBar>().RestartCount();
		//TODO: Return to start pos

	}

}

