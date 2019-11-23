using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAttackHandler : MonoBehaviour
{
	public bool canAttack = false; //set true by PlayerTurnDelayBar

	public enum ATTACK_PHASES{
		CANNOT_ATTACK,
		CHOOSE_ATTACK,
		SELECT_ENEMY,
		SELECT_HERO, //for healing and stuff
		ATTACKING
	}
	private EnemyStateController controller;

	public ATTACK_PHASES currentAttackPhase = ATTACK_PHASES.CANNOT_ATTACK;

	public List<WeaponChoice> weaponChoiceList = new List<WeaponChoice>();
	public GameObject myWeaponOptionHolder;




	public Hero heroType;
	int heroTypeLocation;

	[HideInInspector]
	public HeroAttacker thisHeroAttacker;

	public int arrowPos;
	int prevArrowPos;
	int weaponDamage;
	Vector2 attackTargetPosition;
	Vector2 attackStartingPosition;
	float attackDisFromEnemy; //distance between hero and enemy needed until hero switches to 'hitting' ani varies based on enemy size
	bool returnToStartPositon;

	// Use this for initialization
	void Awake(){

		if(!myWeaponOptionHolder.activeInHierarchy){ //Weapon Holder needs to start ACTIVE so that weapon options are drawn from pool properly
			myWeaponOptionHolder.SetActive(true);
		}

		//find the location of this hero in the global hero list(for spawning weapon options)
		for(int i = 0; i < GlobalVariableManager.Instance.HeroData.Count; i++){
			Debug.Log(heroType.heroName + " " + GlobalVariableManager.Instance.HeroData[i].heroName );

			if(heroType.heroName == GlobalVariableManager.Instance.HeroData[i].heroName){
				Debug.Log("****Found hero match: " + heroType.heroName);
				heroTypeLocation = i;
				break;
			}
		}

		thisHeroAttacker = gameObject.GetComponent<HeroAttacker>();
	}

	void Start ()
	{
		SpawnWeaponOptions();
	}
	
	// Update is called once per frame
	void Update ()
	{

		if(GameStateManager.Instance.GetCurrentState() == typeof(BattleState)){

			//Handle attack position movements
			if(currentAttackPhase == ATTACK_PHASES.ATTACKING && (attackTargetPosition != Vector2.zero)){ 
				if(Vector2.Distance(gameObject.transform.position,attackTargetPosition) > attackDisFromEnemy){
					gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, attackTargetPosition, 10*Time.deltaTime);
					Debug.Log(Vector2.Distance(gameObject.transform.position,attackTargetPosition));
				}else{
				StartCoroutine("Attack");
				}
			}else if(returnToStartPositon){
			 	if(Vector2.Distance(gameObject.transform.position,attackStartingPosition) > 1f){ //Return to starting position after attack
					gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, attackStartingPosition, 10*Time.deltaTime);
				}else{
				returnToStartPositon = false;
				}
			}

			//Handle Attack option navigation
			if(BattleManager.Instance.currentState == BattleManager.CurrentBattleState.NOTHINGATTAKING){
				if(currentAttackPhase == ATTACK_PHASES.CHOOSE_ATTACK){
					CamManager.Instance.mainCamEffects.CameraPan(this.gameObject.transform.position,"");
					//select through the various options
					if(!myWeaponOptionHolder.activeInHierarchy){
						myWeaponOptionHolder.SetActive(true);
					}
					if(ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVELEFT) && arrowPos > 0){
						weaponChoiceList[arrowPos].Unhighlight();
						arrowPos--;
						weaponChoiceList[arrowPos].Highlight();
					}else if(ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVERIGHT) && arrowPos < weaponChoiceList.Count -1){
						weaponChoiceList[arrowPos].Unhighlight();
						arrowPos++;
						weaponChoiceList[arrowPos].Highlight();
					}else if(ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT)){
						Debug.Log("Weapon selected" + weaponChoiceList[arrowPos].name);

						prevArrowPos = arrowPos;

						if(weaponChoiceList[arrowPos].myWeaponType == WeaponChoice.WEAPON_TYPE.BASIC){
							weaponDamage = weaponChoiceList[arrowPos].damage;
							arrowPos = 0;
							currentAttackPhase =  ATTACK_PHASES.SELECT_ENEMY;
							Vector2 attackArrowPos = new Vector2(BattleManager.Instance.enemyList[arrowPos].transform.position.x, BattleManager.Instance.enemyList[arrowPos].transform.position.y + BattleManager.Instance.enemyList[arrowPos].GetComponent<tk2dSprite>().scale.y + 2f); //Place arrow above enemy 2f is for size of arrow itself
							BattleManager.Instance.targetSelectArrow.transform.position = attackArrowPos ;

						}else if(weaponChoiceList[arrowPos].myWeaponType == WeaponChoice.WEAPON_TYPE.BLOCK){
							Debug.Log("Read block weapon properly");
							BattleManager.Instance.PlayerBlock(gameObject.GetComponent<HeroAttacker>(),this);

						}
					}
				}else if(currentAttackPhase == ATTACK_PHASES.SELECT_ENEMY){
					if(myWeaponOptionHolder.activeInHierarchy){
						myWeaponOptionHolder.SetActive(false);
					}
					if(!BattleManager.Instance.targetSelectArrow.activeInHierarchy){
						BattleManager.Instance.targetSelectArrow.SetActive(true);
					}
					if(ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVELEFT) && arrowPos > 0){

						arrowPos--;
						Vector2 attackArrowPos = new Vector2(BattleManager.Instance.enemyList[arrowPos].transform.position.x, BattleManager.Instance.enemyList[arrowPos].transform.position.y + BattleManager.Instance.enemyList[arrowPos].GetComponent<tk2dSprite>().scale.y + 2f); //Place arrow above enemy 2f is for size of arrow itself
						BattleManager.Instance.targetSelectArrow.transform.position = attackArrowPos ;
					}else if(ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVERIGHT) && arrowPos < BattleManager.Instance.enemyList.Count-1){
						arrowPos++;
						Vector2 attackArrowPos = new Vector2(BattleManager.Instance.enemyList[arrowPos].transform.position.x, BattleManager.Instance.enemyList[arrowPos].transform.position.y + BattleManager.Instance.enemyList[arrowPos].GetComponent<tk2dSprite>().scale.y + 2f); //Place arrow above enemy 2f is for size of arrow itself
						BattleManager.Instance.targetSelectArrow.transform.position = attackArrowPos;
					}else if(ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT)){
						BattleManager.Instance.PlayerAttack(this, arrowPos); //< This sets this behavior's phase to 'Attacking' to avoid this setting itself but not actually being allowed to attack in battleManager(edge case)
					}
				}
			}else{
				if(myWeaponOptionHolder.activeInHierarchy){
						myWeaponOptionHolder.SetActive(false);
				}
				if(BattleManager.Instance.targetSelectArrow.activeInHierarchy){
					BattleManager.Instance.targetSelectArrow.SetActive(false);
				}
			}

			//Make the weapon options not be backward when player is facing left
			if(gameObject.transform.localScale.x < 0){
				myWeaponOptionHolder.transform.localScale = new Vector2(-1.79f,1);
			}else{
				myWeaponOptionHolder.transform.localScale = new Vector2(1.79f,1);
			}


		}
	}

	public void MoveToAttack(GameObject enemyTarget){ //Handles Player Attack Animations/Movement to selected enemy

		//calculate distance between player and enemy needed until player switches to attack anis, based on size of enemy
		//if(enemyTarget.transform.position.x < gameObject.transform.position.x){
			attackDisFromEnemy = enemyTarget.GetComponent<tk2dSprite>().GetBounds().max.x +.5f; //+.5f to account for player size
		//}
		//else{
			//attackDisFromEnemy = enemyTarget.GetComponent<tk2dSprite>().GetBounds().min.x + .5f;//+.5f to account for player size

		//}

		if(controller){
			controller.SendTrigger(EnemyTrigger.CHASE);

		}else{
			GetComponent<JimStateController>().SetFlag((int)JimFlag.MOVING);

		}

		Debug.Log("*****Attack Distance From Enemy:" + attackDisFromEnemy);

		attackTargetPosition = enemyTarget.transform.position;
		attackStartingPosition = gameObject.transform.position;

	


	}

	public IEnumerator Attack(){
		bool enemyOnLeft;
		if(attackTargetPosition.x < gameObject.transform.position.x){
			enemyOnLeft = true;
		}else{
			enemyOnLeft = false;
		}

		attackTargetPosition = Vector2.zero; //set back to zero because wanted to make sure attack wasnt triggered instantly due to position not being set in a short enough time after changing to attack state

		if(controller){ //use the prescence of a HeroStateController to determine if this is Jim or not
			controller.SendTrigger(EnemyTrigger.LUNGE);

			while (controller.GetCurrentState() == EnemyState.PREPARE_LEAP)
           			yield return null;
		}else{
			GetComponent<JimStateController>().RemoveFlag((int)JimFlag.MOVING);
			gameObject.GetComponent<tk2dSpriteAnimator>().Play("ani_jimSwingR2");


			if(enemyOnLeft){
				PlayerManager.Instance.controller.SendTrigger(JimTrigger.SWING_LEFT);

			}else{
				PlayerManager.Instance.controller.SendTrigger(JimTrigger.SWING_RIGHT);
			}


			while (PlayerManager.Instance.controller.GetCurrentState() != JimState.ATTACKING)
           		yield return null;

           	yield return new WaitForSeconds(.2f); // hardcoded for duration on jim animation
		}

		returnToStartPositon = true;
		BattleManager.Instance.DamageTargetedEnemy(weaponDamage);
		BattleManager.Instance.ReturnFromAttack();

	}

	void SpawnWeaponOptions(){
		Debug.Log("Spawn weapon options activated for " + gameObject.name);
		foreach(WeaponDefinition weapon in GlobalVariableManager.Instance.HeroData[heroTypeLocation].myEquippedWeapons){
			Debug.Log(weapon.displayName);
			AddWeaponChoice(weapon);
		}


		myWeaponOptionHolder.SetActive(false);
	}


	public void ChooseAttackActivate(){ //Activated by BattleManager based on Hero Queue
		arrowPos = prevArrowPos;
		currentAttackPhase = ATTACK_PHASES.CHOOSE_ATTACK;

	}

	public void AddWeaponChoice(WeaponDefinition weapon){
		Debug.Log("Add Weapon Choice activated" + weapon.displayName);
		Vector2 optionSpawnPos = new Vector2(myWeaponOptionHolder.transform.position.x +(3f*weaponChoiceList.Count), myWeaponOptionHolder.transform.position.y);
		GameObject weaponChoice = ObjectPool.Instance.GetPooledObject("weapon_option", optionSpawnPos); 
		weaponChoice.GetComponent<WeaponChoice>().DefineValues(weapon);
		weaponChoice.GetComponent<WeaponChoice>().Unhighlight();
		weaponChoice.transform.parent = myWeaponOptionHolder.transform;
		weaponChoice.transform.localScale = new Vector2(3.5f,6); //Set box to proper size
		weaponChoiceList.Add(weaponChoice.GetComponent<WeaponChoice>());
	}


}

