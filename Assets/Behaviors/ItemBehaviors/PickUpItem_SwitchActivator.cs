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
	protected override void Update(){

		if (PlayerManager.Instance.player != null) {
            switch (PlayerManager.Instance.player.GetComponent<JimStateController>().GetCurrentState()) {
                case JimState.IDLE:
                    if (PlayerManager.Instance.player != null && Vector2.Distance(PlayerManager.Instance.player.transform.position, gameObject.transform.position) < distanceUntilPickup) {
                        if (ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT)) {//player can move check for fixing glitch where player would pick up dropped object when hit space at 'results'                                                                                                                                                                       // Allow this object to be picked up if it doesn't require the grabby gloves, or they have the grabby gloves.
                            if (!requiresGrabbyGloves || GlobalVariableManager.Instance.IsUpgradeUnlocked(GlobalVariableManager.UPGRADES.GLOVES)) {
                                Debug.Log("PickUpable object...picked up");
                                movePlayerToObject = true;
                                PickUp();
                            }
                        }
                    }
                    break;

                case JimState.CARRYING:
                    if (ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT) && beingCarried) {
                            Drop();
                    }
                    break;
            }


		}
	}
	public override void DropEvent(){
		for(int i = 0; i < possibleSwitches.Count; i++){
			if(Vector2.Distance(gameObject.transform.position, possibleSwitches[i].transform.position) < distanceNeeded){
				ObjectPool.Instance.GetPooledObject("effect_cleanSparkles",gameObject.transform.position);
				possibleSwitches[i].TriggerSwitch(this);
				this.enabled = false;
				break;
			}
		}
	}
}

