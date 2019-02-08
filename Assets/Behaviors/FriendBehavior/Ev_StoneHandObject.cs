using UnityEngine;
using System.Collections;

public class Ev_StoneHandObject : PickupableObject
{

	public StoneFriend stoneFriend;
	public Room rocksGatheringRoom;
	public Transform w1FriendManager;
	//can be picked up and carried
	//can talk to rock if carrying and will initialize return hand dialog
	//^ will also activate dialog if drop the hand in the rock room
	//cant be picked up until talk to rocks for first time
	//


	public override void PickUp(){
        player.GetComponent<JimStateController>().SendTrigger(JimTrigger.PICK_UP_DROPPABLE);
        gameObject.GetComponent<Animator>().enabled = true;
		base.PickUp();
		stoneFriend.GetComponent<ActivateDialogWhenClose>().enabled = true; // can talk to stone if carrying hand
		gameObject.GetComponent<BoxCollider2D>().enabled = false;
		StartCoroutine("PickupDelay");
	}

	IEnumerator PickupDelay(){
		yield return new WaitForSeconds(1f);
		PickUpEvent();
		SoundManager.instance.PlaySingle(SFXBANK.ITEM_CATCH);
	}

	void OnEnable(){
        requiresGrabbyGloves = false;
		if(rocksGatheringRoom == null)
		rocksGatheringRoom = GameObject.Find("a4").GetComponent<Room>();
		player = GameObject.FindGameObjectWithTag("Player");

	}


	public override void DropEvent ()
	{
		//if room = rock room, activate stone dialog
		if(RoomManager.Instance.currentRoom == rocksGatheringRoom){
			stoneFriend.GetComponent<ActivateDialogWhenClose>().dialogDefiniton = stoneFriend.myDialogDefiniton;
			if(stoneFriend.handsDelivered.Count <= 0){
				stoneFriend.nextDialog = "Stone1";
				Debug.Log("Set stone's next dialog" + stoneFriend.nextDialog);

			}else{
				stoneFriend.nextDialog = "Stone2";
			}
			stoneFriend.handsDelivered.Add(this.gameObject);
			stoneFriend.GetComponent<ActivateDialogWhenClose>().startNodeName = stoneFriend.nextDialog;
			stoneFriend.GetComponent<ActivateDialogWhenClose>().ActivateDialog();
			gameObject.SetActive(false);

		}
		gameObject.GetComponent<BoxCollider2D>().enabled = true;

		gameObject.transform.parent = w1FriendManager.transform;//goes back to global parent
		gameObject.GetComponent<Animator>().enabled = false;
	}


}

