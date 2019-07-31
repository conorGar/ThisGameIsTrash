using UnityEngine;
using System.Collections;


public class Ev_LargeTrash : MonoBehaviour
{
//	public bool isRewardForBoss;



    public LargeGarbage garbage = new LargeGarbage();
    public Sprite collectedDisplaySprite;
    public BoxCollider2D myCollisionBox;


	public GameObject sparkle;
	public GameObject smokePuff;
	public AudioClip returnSound;
	public float distanceUntilPickup =3f;

	//^
	//large trash is aware f the current room it is in. This room is given by roomManger.currentoom at start and when large trash
	//is dropped. If myCurrentRoom = RoomManager.currentRoom, deactivate self. (In update method?) check if the roomManager.room =
	//current room and if so, activate the large trash
	public string trashTitle;
	public Room myCurrentRoom; // used by map Star icons
	[HideInInspector]
	public Rigidbody2D myBody;
	protected bool movePlayerToObject;

	int phase = 0;

	int doOnce = 0;

	void Awake(){
		myBody  = gameObject.GetComponent<Rigidbody2D>();

	}
	// Use this for initialization
	void OnEnable () {
		


		if(GlobalVariableManager.Instance.ROOM_NUM == 101){
			MyCollectionSetUp();
		}else{


			phase = 0;
		
			
		}
	}// end of Start()
	void Update ()
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

//           
            }

            if (movePlayerToObject) {
                PlayerManager.Instance.player.transform.position = Vector2.Lerp(PlayerManager.Instance.player.transform.position, this.gameObject.transform.position, 2 * Time.deltaTime);
            }
        }
	}
	public void PickUp(){
        PlayerManager.Instance.controller.SendTrigger(JimTrigger.PICK_UP_SMALL);
		gameObject.GetComponent<Animator>().enabled = true;
		myCollisionBox.enabled = false; //doesnt collide with player
		StartCoroutine("PickupDelay");
	}

	IEnumerator PickupDelay(){
		yield return new WaitForSeconds(1f);
		PickUpEvent();
		SoundManager.instance.PlaySingle(SFXBANK.ITEM_CATCH);
	}

	public void PickUpEvent(){
//		gameObject.tag = "ActiveLargeTrash";
		Return();
		sparkle.SetActive(false);
	}

	public void Kill(){
		//activated when player dies
		if(phase == 3){
			phase = 0;
			//add mychar value and room number to large trash locations?!?
		}
	}

	void MyCollectionSetUp(){

	}

	IEnumerator Fall(){

		gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,20f);
		yield return new WaitForSeconds(1.5f);
		gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,-20f);
		yield return new WaitForSeconds(1.8f);
		gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,0f);

	}


	public void Return(){
        Debug.Log("Return activated - LARGE TRASH");
        PlayerManager.Instance.controller.SendTrigger(JimTrigger.DELIVER_BIG);
        gameObject.GetComponent<Animator>().enabled = false;
		phase = 0;

		SoundManager.instance.PlaySingle(returnSound);
        // Add this trash item to the large trash list.
        var largeTrashItem = new GlobalVariableManager.LargeTrashItem(garbage.type);
        largeTrashItem.collectedDisplaySprite = collectedDisplaySprite;
        largeTrashItem.collectedTitle = trashTitle;
        GlobalVariableManager.Instance.LARGE_TRASH_LIST.Add(largeTrashItem);
        GlobalVariableManager.Instance.LARGE_GARBAGE_DISCOVERED |= garbage.type;
        GlobalVariableManager.Instance.LARGE_TRASH_COLLECTED++;
		if(GlobalVariableManager.Instance.MASTER_SFX_VOL > 0){
			//play trash pickup on channel 2
		}
		myBody.simulated = true;
		gameObject.transform.parent = null;
		myBody.AddForce(new Vector2(0,10),ForceMode2D.Impulse);
		myBody.gravityScale = 2;
		ObjectPool.Instance.GetPooledObject("effect_pickUpSmoke",gameObject.transform.position);
		StartCoroutine("ReturnSequence");
		
	}// end of Return()

	IEnumerator ReturnSequence(){
		GameStateManager.Instance.PushState(typeof(MovieState));
		CamManager.Instance.mainCamEffects.CameraPan(PlayerManager.Instance.player.transform.position," ");
		CamManager.Instance.mainCamEffects.ZoomInOut(2f,1f);
		yield return new WaitForSeconds(.5f);
		GUIManager.Instance.largeTrashCollectDisplay.indexOfCurrentLargeTrash = garbage.GarbageIndex();
		GUIManager.Instance.largeTrashCollectDisplay.gameObject.SetActive(true);

		CamManager.Instance.mainCamEffects.ReturnFromCamEffect();
		Destroy(gameObject);
	}

}