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
                    if (ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT) && beingCarried && !throwableObject) {
                        if (Vector2.Distance(PlayerManager.Instance.player.transform.position, dumpster.transform.position) > 15f) //TODO: temp solution for making sure trash isnt dropped before 'Return' is activated
                            Drop();
                    }
                    break;
            }

            if (movePlayerToObject) {
                PlayerManager.Instance.player.transform.position = Vector2.Lerp(PlayerManager.Instance.player.transform.position, this.gameObject.transform.position, 2 * Time.deltaTime);
            }
        }



	}
	public override void PickUp(){
		PlayerManager.Instance.player.GetComponent<JimAnimationManager>().PlayAnimation("ani_jimPickUp",true);
		gameObject.GetComponent<Renderer>().sortingLayerName = "Layer02";
		//gameObject.GetComponent<Animator>().enabled = true;
		base.PickUp();
		myCollisionBox.enabled = false; //doesnt collide with player
		beingCarried = true;
		SoundManager.instance.PlaySingle(SFXBANK.ITEM_CATCH);
		//player.GetComponent<EightWayMovement>().enabled = false;
		gameObject.transform.position = new Vector2(PlayerManager.Instance.player.transform.position.x,PlayerManager.Instance.player.transform.position.y);
	

	}

	public override void DropEvent(){
		//transform.rotation = startRotation;
		gameObject.transform.localScale = startScale;
	}


}

