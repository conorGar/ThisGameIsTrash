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
		Debug.Log("Got here- Item use");

		//maybe try calling a seperate methos here to heal and see if THAT overrides properly?!?

		//Nothing for base item option
	}
}

