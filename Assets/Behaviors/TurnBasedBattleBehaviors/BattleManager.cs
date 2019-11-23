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
	public GUI_LevelUpDisplay levelUpDisplay;


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
				if(heroTurnQueue[0].currentAttackPhase == PlayerAttackHandler.ATTACK_PHASES.CANNOT_ATTACK){
					heroTurnQueue[0].ChooseAttackActivate();
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


	public void PlayerBlock(HeroAttacker thisHero, PlayerAttackHandler pah){
		Debug.Log("Player Block activated");
		if(currentState == CurrentBattleState.NOTHINGATTAKING){
				currentlyAttackingPlayer = pah;
				ChangeState(CurrentBattleState.PLAYERATTACK);
				thisHero.myCurrentState = HeroAttacker.HERO_STATE.BLOCKING;
				ReturnFromAttack();
		}
	}

	public void PlayerAttack(PlayerAttackHandler pah, int selectedEnemyNum){
		if(currentState == CurrentBattleState.NOTHINGATTAKING){
			ChangeState(CurrentBattleState.PLAYERATTACK);
			targetedEnemy = enemyList[selectedEnemyNum];
			pah.thisHeroAttacker.myCurrentState = HeroAttacker.HERO_STATE.NORMAL;

			currentlyAttackingPlayer = pah;
			pah.currentAttackPhase = PlayerAttackHandler.ATTACK_PHASES.ATTACKING;
			targetSelectArrow.SetActive(false);
			PauseBars();
			pah.MoveToAttack(targetedEnemy.gameObject);
		}
	}

	public void EnemyAttack(HeroAttacker targetedHero, EnemyAttacker thisEnemy, int dmg){
		Debug.Log("EnemyAttack activate");
		if(currentState == CurrentBattleState.NOTHINGATTAKING){
			ChangeState(CurrentBattleState.ENEMYATTACK);
			Debug.Log(currentState);
			PauseBars();
			targetedHero.TakeDamage(dmg);
			thisEnemy.MoveToAttack(targetedHero.gameObject);
		}
	}

	public void ReturnFromAttack(){
		if(currentState == CurrentBattleState.PLAYERATTACK){
			currentlyAttackingPlayer.gameObject.GetComponent<TurnDelayBar>().StartCount();
			heroTurnQueue.RemoveAt(0); //go to next hero, if any
			Debug.Log("Got here: ReturnFromAttack from" + currentlyAttackingPlayer );
			currentlyAttackingPlayer.currentAttackPhase =   PlayerAttackHandler.ATTACK_PHASES.CANNOT_ATTACK;

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

	public void RemoveEnemy(EnemyAttacker deadEnemy){
		enemyList.Remove(deadEnemy);
		turnDelayBars.Remove(deadEnemy.gameObject.GetComponent<TurnDelayBar>());
		Debug.Log("Enemy removed from battle. Remaining enemies=" + enemyList.Count);
		if(enemyList.Count <= 0){
			EndBattle();
		}else{
			ReturnFromAttack();
		}
	}

	public void RemoveHero(HeroAttacker deadHero){
		heroList.Remove(deadHero);
		turnDelayBars.Remove(deadHero.gameObject.GetComponent<TurnDelayBar>());
		PauseBars();
		Debug.Log("Hero removed from battle. Remaining heroes=" + heroList.Count);
		if(heroList.Count <= 0){
			AllPlayersDead();
		}else{
			ReturnFromAttack();
		}
	}

	public void EndBattle(){ //public for LevelUpDisplay
		Debug.Log("End Battle Activated");
		if (GameStateManager.Instance.GetCurrentState() == typeof(BattleState)) {
			if(CheckXP()){
				levelUpDisplay.gameObject.SetActive(true);
			}else{
				occupiedGridPoints.Clear();
				turnDelayBars.Clear();
				heroList.Clear();
				enemyList.Clear();
				GameStateManager.Instance.PopState();
				battleGUI.gameObject.SetActive(false);
				if(GlobalVariableManager.Instance.partners.Count > 0){
					foreach(HeroAttacker hero in GlobalVariableManager.Instance.partners){
						hero.gameObject.SetActive(false);
					}
				}
				CamManager.Instance.mainCamEffects.ReturnFromCamEffect();
			}
		}
	}

	bool CheckXP(){
		//check to see if

		foreach(HeroAttacker hero in heroList){
			if(hero.LevelUpCheck()){
				levelUpDisplay.SetHero(hero.GetHero());
				levelUpDisplay.SetSpawnPosition(hero.transform.position);
				return true;
			}
		}

		return false;
	}

	void AllPlayersDead(){
		//TODO: Death Sequence

	}
}

