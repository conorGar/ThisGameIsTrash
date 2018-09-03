using UnityEngine;
using System.Collections;

public class PickupableObject : MonoBehaviour
{	

	public float distanceUntilPickup =3f;
	public GameObject player;
	public AudioClip pickup;
	public AudioClip drop;
	//int bounce = 0;
	//int doOnce = 0;
	float myY;

	bool pickingUp = false;
	Rigidbody2D myBody;

	GameObject myCollision;
	GameObject myShadow;

	bool spinning;
	float t;
	Quaternion startRotation;

	// Use this for initialization
	void Start ()
	{
		myBody  = gameObject.GetComponent<Rigidbody2D>();
		startRotation = transform.rotation;
	}
	
	// Update is called once per frame
	void Update ()
	{	if(Vector2.Distance(player.transform.position,gameObject.transform.position) < distanceUntilPickup){
			
			if(ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT) && !pickingUp && !GlobalVariableManager.Instance.CARRYING_SOMETHING){
				Debug.Log("PickUpable object...picked up");
				PickUp();
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
				gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
				myBody.velocity = Vector2.zero;
				gameObject.transform.localPosition = new Vector2(3.3f, 0f);
				myBody.simulated = false; //prevents item from moving when player runs into a wall or something
				PickUpEvent();
			}
		}
	}

	public void PickUp(){
		GlobalVariableManager.Instance.CARRYING_SOMETHING = true;
		//move and play the particle system
		ObjectPool.Instance.GetPooledObject("effect_pickUpSmoke",gameObject.transform.position);
		SoundManager.instance.PlaySingle(pickup);
		//set object to follow player and push up in the sky
		gameObject.transform.position = new Vector2(player.transform.position.x,gameObject.transform.position.y);
		gameObject.transform.parent = player.transform;
		myBody.AddForce(new Vector2(0,10),ForceMode2D.Impulse);
		myBody.gravityScale = 2;


		spinning = true;

	}

	public void Drop(){
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

