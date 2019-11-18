using UnityEngine;
using System.Collections;

public class SpecialWeaponChoice : WeaponChoice
{
	public enum SPECIAL_TYPE {
		HEAL,
		BLOCK
	}

	public SPECIAL_TYPE myType;

	public override void DefineValues(WeaponDefinition weapontype){
		
		weaponSprite.SetSprite(weapontype.sprite);
	}
}

