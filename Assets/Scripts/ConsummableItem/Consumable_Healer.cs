using UnityEngine;
using System.Collections;

public class Consumable_Healer : ConsumableItemOption
{

	public int healAmnt;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	protected override void Use(Hero hero){
		if(hero.currentHP < hero.maxHP){
			hero.currentHP += healAmnt;
			Remove();
		}
	}

	void Remove(){
		GlobalVariableManager.Instance.CONSUMABLE_INVENTORY.RemoveAt(myIndexInInventory);
	}
}

