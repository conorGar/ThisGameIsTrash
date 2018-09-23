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
	//int bounce = 0;
	//int doOnce = 0;
	float myY;


	[HideInInspector]
	public Rigidbody2D myBody;
	protected bool beingCarried;
	//public bool cannotDrop; // activated by Dumpster to make sure large trash isnt dropped before 'Return()' is activated...

	GameObject myCollision;
	GameObject myShadow;
	public GameObject dumpster;

	bool pickUpSpin;
	public bool spinning;
	float t;
	Quaternion startRotation;

	// Use this for initialization
	public void Start ()
	{
		myBody  = gameObject.GetComponent<Rigidbody2D>();
		startRotation = transform.rotation;
		dumpster = GameObject.Find("Dumpster");
	}
	
	// Update is called once per frame
	protected void Update ()
	{	if(Vector2.Distance(player.transform.position,gameObject.transform.position) < distanceUntilPickup){
			//Debug.Log("within distance" + GlobalVariableManager.Instance.CARRYING_SOMETHING);
			if(ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT) && !GlobalVariableManager.Instance.CARRYING_SOMETHING){
				Debug.Log("PickUpable object...picked up");
				PickUp();
			}else if(ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT) && beingCarried && !throwableObject){
				if(Vector2.Distance(player.transform.position,dumpster.transform.position) > 15f) //TODO: temp solution for making sure trash isnt dropped before 'Return' is activated
					Drop();
			}
		}else{
//			Debug.Log(Vector2.Distance(player.transform.position,gameObject.transform.position));
		}

		if(spinning){
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
					player.GetComponent<MeleeAttack>().enabled = false;
					if(gameObject.GetComponent<SpriteRenderer>() != null)
						gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Layer02"; // makes sure in front of player
					else
						gameObject.GetComponent<Renderer>().sortingLayerName= "Layer02"; // makes sure in front of player

					myBody.simulated = false; //prevents item from moving when player runs into a wall or something
					PickUpEvent();
					pickUpSpin = false;
				}
			}
		}
	}

	public void PickUp(){
		if(gameObject.GetComponent<BoxCollider2D>()!=null){
			gameObject.GetComponent<BoxCollider2D>().enabled = false;
		}
		GlobalVariableManager.Instance.CARRYING_SOMETHING = true;
		//move and play the particle system
		beingCarried = true;
		ObjectPool.Instance.GetPooledObject("effect_pickUpSmoke",gameObject.transform.position);
		SoundManager.instance.PlaySingle(pickup);
		//set object to follow player and push up in the sky
		gameObject.transform.position = new Vector2(player.transform.position.x,gameObject.transform.position.y);
		gameObject.transform.parent = player.transform;
		if(!throwableObject){
		myBody.AddForce(new Vector2(0,10),ForceMode2D.Impulse);
		}else{
			myBody.AddForce(new Vector2(0,14),ForceMode2D.Impulse);

		}
		myBody.gravityScale = 2;

		pickUpSpin = true;
		spinning = true;

	}

	public void Drop(){
		beingCarried = false;
		gameObject.transform.position = new Vector2(transform.position.x + 1f, transform.position.y -1f);
		ObjectPool.Instance.GetPooledObject("effect_enemyLand",gameObject.transform.position);
		GlobalVariableManager.Instance.CARRYING_SOMETHING = false;
		SoundManager.instance.PlaySingle(drop);
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

