using UnityEngine;
using System.Collections;

public class ItemSwitch : MonoBehaviour
{
	public GameObject placementPoint;
	public ItemSwitchManager myManager;
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void TriggerSwitch(PickUpItem_SwitchActivator item){ //Activated by PickUpItem_SwitchActivator
		ObjectPool.Instance.GetPooledObject("effect_enemyLand",gameObject.transform.position);
		item.transform.position = placementPoint.transform.position;
		myManager.IncrementActivatedCount();
	}

}

