using UnityEngine;
using System.Collections;

public class PickupableObject : MonoBehaviour
{	

	public float distanceUntilPickup =3f;
	public AudioClip pickup;
	public AudioClip drop;
	public float carryXAdjustment = 3.3f;
	public GameObject carryMark;
	public bool throwableObject;
	protected bool movePlayerToObject;
    protected bool requiresGrabbyGloves = true; //this should be true, if false i forgot to change after debugging
	public GameObject childPickupObj; // used for objects that need a parent, but whose spriterender cant be on the parent object(like for an intro animation)
	//int bounce = 0;
	//int doOnce = 0;
	float myY;

	[HideInInspector]
	public Rigidbody2D myBody;
    protected bool beingCarried;
	//public bool cannotDrop; // activated by Dumpster to make sure large trash isnt dropped before 'Return()' is activated...

	GameObject myCollision;
	//GameObject myShadow;
	public GameObject dumpster;

	protected bool pickUpSpin;
	public bool spinning;
	float t;
	Quaternion startRotation;

	// Use this for initialization
	public void Start ()
	{
		myBody  = gameObject.GetComponent<Rigidbody2D>();
		startRotation = transform.rotation;
		dumpster = GameObject.Find("Dumpster");
		carryMark = PlayerManager.Instance.player.transform.GetChild(7).gameObject;//TODO: better way to do this...not good
	}
	/*void OnEnable(){
		if(player == null){
			player = GameObject.FindGameObjectWithTag("Player");

		}
	}*/
	// Update is called once per frame
	protected virtual void Update ()
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

	public virtual void PickUp(){
        Debug.Log("Pickup() activated");
        movePlayerToObject = false;
		gameObject.transform.position = new Vector2(PlayerManager.Instance.player.transform.position.x,gameObject.transform.position.y);
		gameObject.transform.parent = PlayerManager.Instance.player.transform;
        PlayerManager.Instance.player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        PlayerManager.Instance.player.GetComponent<PlayerTakeDamage>().currentlyCarriedObject = this.gameObject;
		//move and play the particle system
		ObjectPool.Instance.GetPooledObject("effect_pickUpSmoke",gameObject.transform.position);
		SoundManager.instance.PlaySingle(pickup);
		//set object to follow player and push up in the sky
		gameObject.GetComponent<Animator>().SetTrigger("PickUp");
        beingCarried = true;
	}

	public void Drop(){
        Debug.Log("ITEM DROPPED");
        PlayerManager.Instance.controller.SendTrigger(JimTrigger.DROP_BIG);
        PlayerManager.Instance.player.GetComponent<PlayerTakeDamage>().currentlyCarriedObject = null;
		gameObject.transform.parent = null; //detatch from player transform
		gameObject.transform.position = new Vector2(transform.position.x + 1f * Mathf.Sign(transform.localScale.x), transform.position.y -1f);
		ObjectPool.Instance.GetPooledObject("effect_enemyLand",gameObject.transform.position);
		SoundManager.instance.PlaySingle(drop);
		spinning = false;
		pickUpSpin = false;
		if(myBody !=null)
			myBody.simulated = true;
		t=0f;
		//proper postionining 
		DropEvent();
        beingCarried = false;
	}

	public virtual void PickUpEvent(){
		//nothing for base
	}

	public virtual void DropEvent(){
		
		//nothing for base
	}

	/*IEnumerator PickUpDelay(){
		yield return new Waitfor
	}*/
}

