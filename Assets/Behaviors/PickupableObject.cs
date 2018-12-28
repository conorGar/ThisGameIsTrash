using UnityEngine;
using System.Collections;

public class PickupableObject : MonoBehaviour
{	

	public float distanceUntilPickup =3f;
	public GameObject player;
	public AudioClip pickup;
	public AudioClip drop;
	public float carryXAdjustment = 3.3f;
	public GameObject carryMark;
	public bool throwableObject;
	protected bool movePlayerToObject;
    protected bool requiresGrabbyGloves = false;
	public GameObject childPickupObj; // used for objects that need a parent, but whose spriterender cant be on the parent object(like for an intro animation)
	//int bounce = 0;
	//int doOnce = 0;
	float myY;
	[HideInInspector]
	protected int pickUpcheck = 0;

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
		if(player == null){
			player = GameObject.FindGameObjectWithTag("Player");
			carryMark = player.transform.GetChild(7).gameObject;//TODO: better way to do this...not good
		}

	}
	/*void OnEnable(){
		if(player == null){
			player = GameObject.FindGameObjectWithTag("Player");

		}
	}*/
	// Update is called once per frame
	protected virtual void Update ()
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
			}else if(ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT) && beingCarried && !throwableObject){
				if(Vector2.Distance(player.transform.position,dumpster.transform.position) > 15f) //TODO: temp solution for making sure trash isnt dropped before 'Return' is activated
					Drop();
			}
		}else{
//			Debug.Log(Vector2.Distance(player.transform.position,gameObject.transform.position));
		}

		if(movePlayerToObject){
			player.transform.position = Vector2.Lerp(player.transform.position,this.gameObject.transform.position,2*Time.deltaTime);
		}

		/*if(spinning){
			if (t<1f){
				transform.rotation = startRotation * Quaternion.AngleAxis(t/1f * 360f, Vector3.forward);
				t += Time.deltaTime;
			}else{
				spinning = false;
				t = 0f;
				if(pickUpSpin){
					beingCarried = true;
					gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
					myBody.velocity = Vector2.zero;
					if(!throwableObject){
						gameObject.transform.localPosition = new Vector2(carryXAdjustment, 0f);
					}else{
						gameObject.transform.localPosition = carryMark.transform.localPosition;
						gameObject.GetComponent<ThrowableObject>().enabled = true;
					}
					//player.GetComponent<MeleeAttack>().enabled = false;
					player.GetComponent<EightWayMovement>().enabled = true;
					player.GetComponent<JimAnimationManager>().PlayAnimation("ani_jimCarryAbove",true);

					if(childPickupObj == null){
						if(gameObject.GetComponent<SpriteRenderer>() != null)
							gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Layer02"; // makes sure in front of player
						else
							gameObject.GetComponent<Renderer>().sortingLayerName= "Layer02"; // makes sure in front of player
					}else{
						if(childPickupObj.GetComponent<SpriteRenderer>() != null)
							childPickupObj.GetComponent<SpriteRenderer>().sortingLayerName = "Layer02"; // makes sure in front of player
						else
							childPickupObj.GetComponent<Renderer>().sortingLayerName= "Layer02"; // makes sure in front of player
					}
					myBody.simulated = false; //prevents item from moving when player runs into a wall or something
					PickUpEvent();
					pickUpSpin = false;
				}
			}
		}*/
	}

	public virtual void PickUp(){
		movePlayerToObject = false;
		/*if(gameObject.GetComponent<BoxCollider2D>()!=null){
			gameObject.GetComponent<BoxCollider2D>().enabled = false;
		}*/
		gameObject.transform.position = new Vector2(player.transform.position.x,gameObject.transform.position.y);
		gameObject.transform.parent = player.transform;
		Debug.Log("Pickup() activated");
		GlobalVariableManager.Instance.CARRYING_SOMETHING = true;
		player.GetComponent<EightWayMovement>().enabled = false;
		player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		player.GetComponent<PlayerTakeDamage>().currentlyCarriedObject = this.gameObject;
		//move and play the particle system
		ObjectPool.Instance.GetPooledObject("effect_pickUpSmoke",gameObject.transform.position);
		SoundManager.instance.PlaySingle(pickup);
		//set object to follow player and push up in the sky
		gameObject.GetComponent<Animator>().SetTrigger("PickUp");
		if(!throwableObject){
			
			//myBody.AddForce(new Vector2(0,10),ForceMode2D.Impulse);
			player.GetComponent<EightWayMovement>().carryingAbove = false;

		}else{
			player.GetComponent<EightWayMovement>().carryingAbove = true;
			//myBody.AddForce(new Vector2(0,14),ForceMode2D.Impulse);
			//myBody.gravityScale = 2;
			//beingCarried = true;

		}


	}

	public void Drop(){
		beingCarried = false;
		player.GetComponent<PlayerTakeDamage>().currentlyCarriedObject = null;
		Debug.Log("ITEM DROPPED");
		gameObject.transform.parent = null; //detatch from player transform
		gameObject.transform.position = new Vector2(transform.position.x + 1f, transform.position.y -1f);
		ObjectPool.Instance.GetPooledObject("effect_enemyLand",gameObject.transform.position);
		player.GetComponent<EightWayMovement>().clipOverride = false;
		GlobalVariableManager.Instance.CARRYING_SOMETHING = false;
		SoundManager.instance.PlaySingle(drop);
		spinning = false;
		pickUpSpin = false;
		myBody.simulated = true;
		t=0f;
		//proper postionining 
		DropEvent();
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

