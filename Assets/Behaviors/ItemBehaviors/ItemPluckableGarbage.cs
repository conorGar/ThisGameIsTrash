using UnityEngine;
using System.Collections;

public class ItemPluckableGarbage : MonoBehaviour
{

	public float distanceUntilPickup =3f;
	public AudioClip pickup;
	public GameObject childPickupObj; // used for objects that need a parent, but whose spriterender cant be on the parent object(like for an intro animation)
	public int trashAmnt;
	public int hp;
	public ItemSecretButton revealButton;
	float myY;


	public BoxCollider2D myCollision;

	bool movePlayerToObject;
	protected bool pickUpSpin;
	public bool spinning;
	float t;
	Quaternion startRotation;

	public void OnEnable ()
	{
		startRotation = transform.rotation;
		myCollision = gameObject.GetComponent<BoxCollider2D>();
		myCollision.enabled = true;
	}


	protected virtual void Update ()
	{
        if (PlayerManager.Instance.player != null) {
            switch (PlayerManager.Instance.player.GetComponent<JimStateController>().GetCurrentState()) {
                case JimState.IDLE:
                    if (PlayerManager.Instance.player != null && Vector2.Distance(PlayerManager.Instance.player.transform.position, gameObject.transform.position) < distanceUntilPickup) {
                        if (ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT)) {//player can move check for fixing glitch where player would pick up dropped object when hit space at 'results'                                                                                                                                                                       // Allow this object to be picked up if it doesn't require the grabby gloves, or they have the grabby gloves.
                            
                                movePlayerToObject = true;
                                PickUp();
                            
                        }
                    }
                    break;

           
            }

            if (movePlayerToObject) {
            	myCollision.enabled = false;
                PlayerManager.Instance.player.transform.position = Vector2.Lerp(PlayerManager.Instance.player.transform.position, this.gameObject.transform.position, 2 * Time.deltaTime);
            }else{
            	myCollision.enabled = true;
            }
        }
	}

	public virtual void PickUp(){
		hp--;
		if(hp > 0){
			gameObject.GetComponent<tk2dSpriteAnimator>().Play("pull");
			gameObject.GetComponent<Renderer>().sortingLayerName = "Layer02"; // in front of player
			gameObject.transform.position = new Vector2(PlayerManager.Instance.player.transform.position.x,gameObject.transform.position.y);
			PlayerManager.Instance.player.GetComponent<JimStateController>().SendTrigger(JimTrigger.PICK_UP_SMALL);
	        PlayerManager.Instance.player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		}else{
			gameObject.GetComponent<tk2dSpriteAnimator>().Play("pluck");
			if(revealButton != null){
				revealButton.gameObject.SetActive(true);
			}
			myCollision.enabled = false;
	        Debug.Log("Pickup() activated");
	        movePlayerToObject = false;
	       
			gameObject.transform.position = new Vector2(PlayerManager.Instance.player.transform.position.x,gameObject.transform.position.y);
	        PlayerManager.Instance.player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			PlayerManager.Instance.player.GetComponent<JimStateController>().SendTrigger(JimTrigger.PICK_UP_SMALL);
					
			
			//move and play the particle system
			StartCoroutine("Kill");

			ObjectPool.Instance.GetPooledObject("effect_pickUpSmoke",gameObject.transform.position);
			SoundManager.instance.PlaySingle(pickup);
			//set object to follow player and push up in the sky
			if(gameObject.GetComponent<Animator>()!=null)
				gameObject.GetComponent<Animator>().SetTrigger("PickUp");
		}


	}

	IEnumerator Kill(){

		yield return new WaitForSeconds(1f);
		for(int i = 0; i < trashAmnt; i++){
			if(GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[0] <= GlobalVariableManager.Instance.BAG_SIZE_STAT.GetMax()){
				GameObject trash = ObjectPool.Instance.GetPooledObject("DroppedTrashHoming",gameObject.transform.position);
				trash.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-1f,1f),Random.Range(2f,4f)),ForceMode2D.Impulse);
			}
		}
		ObjectPool.Instance.GetPooledObject("effect_landingSmoke",gameObject.transform.position);
        ObjectPool.Instance.ReturnPooledObject(gameObject);

    }
}

