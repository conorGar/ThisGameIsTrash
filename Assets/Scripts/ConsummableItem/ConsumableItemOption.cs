using UnityEngine;
using System.Collections;

public class ConsumableItemOption : MonoBehaviour
{

	//This serves as the item drop. Note that there is nothing set up where you can drop an item 'template' object and give it data...
	//...every dropped item, the way it is now, must be its own prefab/ tag

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

	void OnTriggerStay2D(Collider2D collider){
		//Pick up

		//TODO: only pick up if press button
		if(collider.gameObject.tag == "Player" && GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)){
			if((GlobalVariableManager.Instance.WeaponInventory.Count + GlobalVariableManager.Instance.CONSUMABLE_INVENTORY.Count) < GlobalVariableManager.Instance.MaxInventorySize){
				GlobalVariableManager.Instance.CONSUMABLE_INVENTORY.Add(this);
				ObjectPool.Instance.ReturnPooledObject(this.gameObject);
			}
		}
	}


	public virtual void Use(Hero hero){ //Activated by menu behavior(s), ie GUI_WeaponEquipScreen


		//Nothing for base item option
	}


	public virtual bool CanUseCheck(Hero hero){
		//Check to make the conditions this item must be used in are met(For example, cant use heal item in targetHero's hp is already max)
		return true;
	}
}

