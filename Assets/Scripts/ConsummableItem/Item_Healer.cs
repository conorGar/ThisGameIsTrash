using UnityEngine;
using System.Collections;

public class Item_Healer : ConsumableItemOption
{

	public int healAmnt;

	public override void Use(Hero hero){ //Activated by menu behavior(s), ie GUI_WeaponEquipScreen
		Debug.Log("Got here- HEAL use");

		hero.currentHP+= healAmnt;
		if(hero.currentHP < hero.maxHP){
			hero.currentHP = hero.maxHP;
		}
	}

	public override bool CanUseCheck(Hero hero){
		if(hero.currentHP < hero.maxHP){
			return true;
		}else{
			return false;
		}

	}
}

