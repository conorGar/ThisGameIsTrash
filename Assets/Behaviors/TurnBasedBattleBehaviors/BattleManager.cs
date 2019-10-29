using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BattleManager : MonoBehaviour
{
	public static BattleManager Instance;
	public List<TurnDelayBar> turnDelayBars = new List<TurnDelayBar>();
	public List<EnemyAttacker> enemyList = new List<EnemyAttacker>();
	public List<HeroAttacker> heroList = new List<HeroAttacker>();
	public enum BattleState{

		NOTHINGATTAKING,
		ENEMYATTACK,
		PLAYERATTACK
	} 
	public BattleState currentState;
	public PlayerAttackHandler currentlyAttackingPlayer; //Set by PlayerAttackHandler
	public GameObject targetSelectArrow;
	// Use this for initialization
	int arrowPos;
	EnemyAttacker targetedEnemy;

    void Awake () {
        Instance = this;
	}

	void Start(){
		StartBattle();
	}


	void Update(){

	}

	public void DamageTargetedEnemy(int damage){ //activated by PlayerAttackHandler after reaches Enemy
		targetedEnemy.TakeDamage(damage);
	}

	public void StartBattle(){
		foreach(TurnDelayBar delayBar in turnDelayBars){
			delayBar.StartCount();
		}
	}


	public void PlayerAttack(PlayerAttackHandler pah, int selectedEnemyNum){
		if(currentState == BattleState.NOTHINGATTAKING){
			ChangeState(BattleState.PLAYERATTACK);
			targetedEnemy = enemyList[selectedEnemyNum];
			currentlyAttackingPlayer = pah;
			pah.attackPhase = "ATTACKING";
			targetSelectArrow.SetActive(false);
			PauseBars();
			pah.MoveToAttack(targetedEnemy.gameObject);
		}
	}

	public void EnemyAttack(HeroAttacker targetedHero, EnemyAttacker thisEnemy, int dmg){
		Debug.Log("EnemyAttack activate");
		if(currentState == BattleState.NOTHINGATTAKING){
			ChangeState(BattleState.ENEMYATTACK);
			Debug.Log(currentState);
			PauseBars();
			targetedHero.TakeDamage(dmg);
			thisEnemy.StartCoroutine("MoveToAttack");
		}
	}

	public void ReturnFromAttack(){
		if(currentState == BattleState.PLAYERATTACK){
			currentlyAttackingPlayer.gameObject.GetComponent<TurnDelayBar>().StartCount();
		}else if(currentState == BattleState.ENEMYATTACK){
			//TODO: restart currently attacking enemy counter
		}
		ChangeState(BattleState.NOTHINGATTAKING);
		ResumeBars();
	}


	void ResumeBars(){
		foreach (TurnDelayBar delayBar in turnDelayBars){
			delayBar.ResumeCount();
		}
	}



	void PauseBars(){
		foreach (TurnDelayBar delayBar in turnDelayBars){
			delayBar.Pause();
		}
	}

	void ChangeState(BattleState newState){
		Debug.Log("BattleState changed from" + currentState + " to " + newState);
		currentState = newState;
	}
}

