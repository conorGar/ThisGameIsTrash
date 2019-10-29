using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BattleManager : MonoBehaviour
{
	public static BattleManager Instance;
	public List<TurnDelayBar> turnDelayBars = new List<TurnDelayBar>();
	public List<EnemyAttacker> enemyList = new List<EnemyAttacker>();
	public List<HeroAttacker> heroList = new List<HeroAttacker>();
	public string currentState = "NOTHINGATTACKING"; //PLAYERATTACK or ENEMYATTACK or PLAYERSELECT
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
		if(currentState == "NOTHINGATTACKING"){
			currentState = "PLAYERATTACKING";
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
		if(currentState != "ENEMYATTACKING" && currentState != "PLAYERATTACKING"){
			currentState = "ENEMYATTACKING";
			PauseBars();
			targetedHero.TakeDamage(dmg);
			thisEnemy.StartCoroutine("MoveToAttack");
		}
	}

	public void ReturnFromAttack(){
		if(currentState == "PLAYERATTACKING"){
			currentlyAttackingPlayer.gameObject.GetComponent<TurnDelayBar>().StartCount();
		}else if(currentState == "ENEMYATTACKING"){
			//TODO: restart currently attacking enemy counter
		}
		currentState = "NOTHINGATTACKING";
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
}

