using UnityEngine;
using System.Collections;

public class ConsumableItemOption : MonoBehaviour
{

	public ConsumableItem myItemDefinition;
	protected int myIndexInInventory; // for removing
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
}

