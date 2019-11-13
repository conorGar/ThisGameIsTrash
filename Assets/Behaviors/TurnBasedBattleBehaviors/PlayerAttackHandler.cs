using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAttackHandler : MonoBehaviour
{
	public bool canAttack = false; //set true by PlayerTurnDelayBar
	public string attackPhase = "CANNOT_ATTACK"; //CHOOSE_ATTACK \ SELECT_ENEMY \ ATTACKING
	public List<WeaponChoice> weaponChoiceList = new List<WeaponChoice>();
	public GameObject myWeaponOptionHolder;


	public enum ThisHero {
		JIM,
		ROBOT,
		ICE_CREAM
	}

	public ThisHero heroType;

	public int arrowPos;
	int weaponDamage;
	// Use this for initialization
	void Start ()
	{
		SpawnWeaponOptions();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(BattleManager.Instance.currentState == BattleManager.CurrentBattleState.NOTHINGATTAKING){
			if(attackPhase == "CHOOSE_ATTACK"){
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
					Debug.Log("Weapon selected");
					weaponDamage = weaponChoiceList[arrowPos].damage;
					arrowPos = 0;
					attackPhase = "SELECT_ENEMY";
				}
			}else if(attackPhase == "SELECT_ENEMY"){
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
	}

	public IEnumerator MoveToAttack(GameObject enemyTarget){ //Handles Player Attack Animations/Movement to selected enemy
		//TODO:Once reach enemy gameObj...
		yield return new WaitForSeconds(.5f);
		BattleManager.Instance.DamageTargetedEnemy(weaponDamage);
		BattleManager.Instance.ReturnFromAttack();
		//TODO: Return to start pos

	}

	void SpawnWeaponOptions(){
		if(heroType == ThisHero.JIM){
			foreach(WeaponDefinition weapon in GlobalVariableManager.Instance.JimEquippedWeapons){
				AddWeaponChoice(weapon);
			}
		}

		myWeaponOptionHolder.SetActive(false);
	}

	public void AddWeaponChoice(WeaponDefinition weapon){
		Debug.Log("Add Weapon Choice activated" + weapon.displayName);
		Vector2 optionSpawnPos = new Vector2(myWeaponOptionHolder.transform.position.x +(3f*weaponChoiceList.Count), myWeaponOptionHolder.transform.position.y);
		GameObject weaponChoice = ObjectPool.Instance.GetPooledObject("weapon_option", optionSpawnPos); //TODO: change 'gameobject.transform.position' to position of WeaponOptionHolder
		weaponChoice.GetComponent<WeaponChoice>().DefineValues(weapon);
		weaponChoice.transform.parent = myWeaponOptionHolder.transform;
		weaponChoice.transform.localScale = new Vector2(3.5f,6); //Set box to proper size
		weaponChoiceList.Add(weaponChoice.GetComponent<WeaponChoice>());
	}


}

