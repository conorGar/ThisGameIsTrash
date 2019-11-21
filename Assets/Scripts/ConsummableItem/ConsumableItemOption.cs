using UnityEngine;
using System.Collections;

public class ConsumableItemOption : MonoBehaviour
{

	public ConsumableItem myItemDefinition;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		//TODO: check when selected/highlight and such....

	}



	public virtual void Use(Hero hero){ //Activated by menu behavior(s), ie GUI_WeaponEquipScreen


		//Nothing for base item option
	}


	public virtual bool CanUseCheck(Hero hero){
		//Check to make the conditions this item must be used in are met(For example, cant use heal item in targetHero's hp is already max)
		return true;
	}
}

