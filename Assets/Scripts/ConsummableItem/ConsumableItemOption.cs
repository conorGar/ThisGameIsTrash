using UnityEngine;
using System.Collections;

public class ConsumableItemOption : MonoBehaviour
{

	public CONSUMABLE thisItem;

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

	protected virtual void Use(Hero hero){
		//Nothing for base item option
	}
}

