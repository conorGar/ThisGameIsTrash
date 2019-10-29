using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAttackHandler : MonoBehaviour
{
	public bool canAttack = false; //set true by PlayerTurnDelayBar
	public string attackPhase = "CANNOT_ATTACK"; //CHOOSE_ATTACK \ SELECT_ENEMY \ ATTACKING
	public List<WeaponChoice> weaponChoiceList = new List<WeaponChoice>();
	public GameObject myWeaponOptionHolder;

	public int arrowPos;
	int weaponDamage;
	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
		if(attackPhase == "CHOOSE_ATTACK"){
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
				weaponDamage = weaponChoiceList[arrowPos].Damage;
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
				BattleManager.Instance.targetSelectArrow.transform.position = BattleManager.Instance.enemyList[arrowPos].transform.position;
			}else if(ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVERIGHT) && arrowPos < BattleManager.Instance.enemyList.Count-1){
				arrowPos++;
				BattleManager.Instance.targetSelectArrow.transform.position = BattleManager.Instance.enemyList[arrowPos].transform.position;
			}else if(ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT)){
				BattleManager.Instance.PlayerAttack(this, arrowPos); //< This sets this behavior's phase to 'Attacking' to avoid this setting itself but not actually being allowed to attack in battleManager(edge case)
			}
		}
	}

	public void MoveToAttack(GameObject enemyTarget){ //Handles Player Attack Animations/Movement to selected enemy
		//TODO:Once reach enemy gameObj...
		BattleManager.Instance.DamageTargetedEnemy(weaponDamage);
		BattleManager.Instance.ReturnFromAttack();
		//TODO: Return to start pos

	}


}

