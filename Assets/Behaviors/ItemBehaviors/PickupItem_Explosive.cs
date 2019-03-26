using UnityEngine;
using System.Collections;

public class PickupItem_Explosive : PickupableObject
{
	public int timeTillExplode;
	//public GameObject explosion;
	//public CircleCollider2D damageZone;
	public GameObject pointerArrow;
	public BoxCollider2D myCollisionBox;
	Vector3 startScale;

	void OnEnable(){
		requiresGrabbyGloves = false;
		startScale = gameObject.transform.lossyScale;
		movePlayerToObject = true;
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
                            Drop();
                    }else if(beingCarried){
	                    if(gameObject.transform.localScale.x > 0)
							gameObject.transform.position = new Vector2(PlayerManager.Instance.player.transform.position.x+.8f,PlayerManager.Instance.player.transform.position.y);
						else
							gameObject.transform.position = new Vector2(PlayerManager.Instance.player.transform.position.x-.8f,PlayerManager.Instance.player.transform.position.y);

						gameObject.transform.localScale = new Vector2(startScale.x*Mathf.Sign(PlayerManager.Instance.player.transform.localScale.x),startScale.y);
	                 
                    }
					break;
            }

           

        }



	}
	public override void PickUp(){
		pointerArrow.SetActive(false);
		PlayerManager.Instance.player.GetComponent<JimAnimationManager>().PlayAnimation("ani_jimPickUp",true);
		gameObject.GetComponent<Renderer>().sortingLayerName = "Layer02";
		//gameObject.GetComponent<Animator>().enabled = true;

		base.PickUp();
		myCollisionBox.enabled = false; //doesnt collide with player
		beingCarried = true;
		//PlayerManager.Instance.controller.SendTrigger(JimTrigger.PICK_UP_DROPPABLE);
		gameObject.transform.position = new Vector2(PlayerManager.Instance.player.transform.position.x+.5f,PlayerManager.Instance.player.transform.position.y);
		gameObject.transform.parent = null; // no parent otherwise collision with this will damage Jim
		SoundManager.instance.PlaySingle(SFXBANK.ITEM_CATCH);
		//player.GetComponent<EightWayMovement>().enabled = false;
	

	}

	public override void DropEvent(){
		StartCoroutine("Explode");
	}

	IEnumerator Explode(){
		yield return new WaitForSeconds(timeTillExplode);
		GameObject explosion = ObjectPool.Instance.GetPooledObject("TrashBomb",gameObject.transform.position);
		yield return new WaitForSeconds(.5f);
		yield return new WaitForSeconds(.5f);
		ObjectPool.Instance.ReturnPooledObject(explosion);
		gameObject.SetActive(false);
	}
}

