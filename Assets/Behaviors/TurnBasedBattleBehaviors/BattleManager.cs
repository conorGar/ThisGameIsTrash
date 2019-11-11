using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BattleManager : MonoBehaviour
{
	public static BattleManager Instance;
	public BattleGUI battleGUI;
	public List<TurnDelayBar> turnDelayBars = new List<TurnDelayBar>();
	public List<EnemyAttacker> enemyList = new List<EnemyAttacker>();
	public List<HeroAttacker> heroList = new List<HeroAttacker>();
	public enum CurrentBattleState{

		NOTHINGATTAKING,
		ENEMYATTACK,
		PLAYERATTACK
	} 
	public CurrentBattleState currentState;
	public PlayerAttackHandler currentlyAttackingPlayer; //Set by PlayerAttackHandler
	public GameObject targetSelectArrow;
	// Use this for initialization

	public List<PlayerAttackHandler> heroTurnQueue = new List<PlayerAttackHandler>(); //only public for debugging
	int arrowPos;
	EnemyAttacker targetedEnemy;
	public List<Point> occupiedGridPoints = new List<Point>(); // Used to make sure that no two enemies in battle jump to the same node point

    void Awake () {
        Instance = this;
	}

	void Start(){
		//StartBattle();
	}


	void Update(){
		if(currentState == CurrentBattleState.NOTHINGATTAKING){
			if(heroTurnQueue.Count > 0){ //If there's any heroes in the queue
				if(heroTurnQueue[0].attackPhase == "CANNOT_ATTACK"){
					heroTurnQueue[0].attackPhase = "CHOOSE_ATTACK";
				}
			}
		}
	}

	public void AddHero(HeroAttacker hero){
		heroList.Add(hero);
		turnDelayBars.Add(hero.gameObject.GetComponent<TurnDelayBar>());
	}

	public void AddVisibleEnemies(){
		foreach(GameObject enemy in RoomManager.Instance.currentRoom.enemies){
			if(enemy.GetComponent<Renderer>().isVisible){ //all enemies on the screen enter the battle
				enemy.gameObject.GetComponent<EnemyBattleStarter>().LeapBack();
				enemyList.Add(enemy.GetComponent<EnemyAttacker>());
				turnDelayBars.Add(enemy.GetComponent<TurnDelayBar>());
			}
		}
	}

	public void DamageTargetedEnemy(int damage){ //activated by PlayerAttackHandler after reaches Enemy
		targetedEnemy.TakeDamage(damage);
	}

	public void StartBattle(){
		if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {

			GameStateManager.Instance.PushState(typeof(BattleState));
			battleGUI.gameObject.SetActive(true);
			AddVisibleEnemies();
			foreach(TurnDelayBar delayBar in turnDelayBars){
				delayBar.StartCount();
			}
			CamManager.Instance.mainCamEffects.ZoomInOut(1,.3f);
		}
	}


	public void PlayerAttack(PlayerAttackHandler pah, int selectedEnemyNum){
		if(currentState == CurrentBattleState.NOTHINGATTAKING){
			ChangeState(CurrentBattleState.PLAYERATTACK);
			targetedEnemy = enemyList[selectedEnemyNum];
			currentlyAttackingPlayer = pah;
			pah.attackPhase = "ATTACKING";
			targetSelectArrow.SetActive(false);
			PauseBars();
			pah.StartCoroutine("MoveToAttack", targetedEnemy.gameObject);
		}
	}

	public void EnemyAttack(HeroAttacker targetedHero, EnemyAttacker thisEnemy, int dmg){
		Debug.Log("EnemyAttack activate");
		if(currentState == CurrentBattleState.NOTHINGATTAKING){
			ChangeState(CurrentBattleState.ENEMYATTACK);
			Debug.Log(currentState);
			PauseBars();
			targetedHero.TakeDamage(dmg);
			thisEnemy.StartCoroutine("MoveToAttack");
		}
	}

	public void ReturnFromAttack(){
		if(currentState == CurrentBattleState.PLAYERATTACK){
			currentlyAttackingPlayer.gameObject.GetComponent<TurnDelayBar>().StartCount();
			heroTurnQueue.RemoveAt(0); //go to next hero, if any
			Debug.Log("Got here: ReturnFromAttack from" + currentlyAttackingPlayer );
			currentlyAttackingPlayer.attackPhase = "CANNOT_ATTACK";

		}else if(currentState == CurrentBattleState.ENEMYATTACK){
			//TODO: restart currently attacking enemy counter
		}
		ChangeState(CurrentBattleState.NOTHINGATTAKING);
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

	void ChangeState(CurrentBattleState newState){
		Debug.Log("BattleState changed from" + currentState + " to " + newState);
		currentState = newState;
	}

	public void AddHeroToQueue(PlayerAttackHandler hero){
		heroTurnQueue.Add(hero);
	}

	public void AddOccupiedPointToGrid(Point newPoint){
		occupiedGridPoints.Add(newPoint);
	}

	void EndBattle(){
		occupiedGridPoints.Clear();
	}
}

