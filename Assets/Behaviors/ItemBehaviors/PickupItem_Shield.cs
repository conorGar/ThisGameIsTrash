using UnityEngine;
using System.Collections;

public class PickupItem_Shield : PickupableObject
{
	public BoxCollider2D myCollisionBox;
	Vector3 startScale;


	void OnEnable(){
		requiresGrabbyGloves = false;
		startScale = gameObject.transform.lossyScale;
	}
	protected override void Update ()
	{	
		if(player != null && Vector2.Distance(player.transform.position,gameObject.transform.position) < distanceUntilPickup){
			//Debug.Log("within distance" + GlobalVariableManager.Instance.CARRYING_SOMETHING);
			if(ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT) && !GlobalVariableManager.Instance.CARRYING_SOMETHING && GlobalVariableManager.Instance.PLAYER_CAN_MOVE){//player can move check for fixing glitch where player would pick up dropped object when hit space at 'results'
                // Allow this object to be picked up if it doesn't require the grabby gloves, or they have the grabby gloves.
                if ( !requiresGrabbyGloves || GlobalVariableManager.Instance.IsUpgradeUnlocked(GlobalVariableManager.UPGRADES.GLOVES)) {
                    Debug.Log("PickUpable object...picked up");
					if(pickUpcheck == 0){
	                    movePlayerToObject = true;
	                    PickUp();
	                    pickUpcheck = 1;
                    }
                }
			}else if(ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT) && beingCarried){
					Drop();
			}
		}



	}
	public override void PickUp(){
		player.GetComponent<JimAnimationManager>().PlayAnimation("ani_jimPickUp",true);
		gameObject.GetComponent<Renderer>().sortingLayerName = "Layer02";
		//gameObject.GetComponent<Animator>().enabled = true;
		base.PickUp();
		myCollisionBox.enabled = false; //doesnt collide with player
		beingCarried = true;
		SoundManager.instance.PlaySingle(SFXBANK.ITEM_CATCH);
		//player.GetComponent<EightWayMovement>().enabled = false;
		gameObject.transform.position = new Vector2(player.transform.position.x,player.transform.position.y);
	

	}

	public override void DropEvent(){
		transform.rotation = startRotation;
		gameObject.transform.localScale = startScale;
	}


}

