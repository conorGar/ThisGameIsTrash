using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PickUpItem_SwitchActivator : PickupableObject
{
	public List<ItemSwitch> possibleSwitches = new List<ItemSwitch>();
	public float distanceNeeded = 5f;
	public GameObject pointerArrow;

	void Awake(){
		requiresGrabbyGloves = false;
	}

	public override void PickUp(){
		base.PickUp();
		pointerArrow.SetActive(false);
	}


	public override void DropEvent(){
		for(int i = 0; i < possibleSwitches.Count; i++){
			if(Vector2.Distance(gameObject.transform.position, possibleSwitches[i].transform.position) < distanceNeeded){
				possibleSwitches[i].TriggerSwitch(this);
				this.enabled = false;
				break;
			}
		}
	}
}

