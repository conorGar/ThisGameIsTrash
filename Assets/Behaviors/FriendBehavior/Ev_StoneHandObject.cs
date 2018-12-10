using UnityEngine;
using System.Collections;

public class Ev_StoneHandObject : PickupableObject
{

	public StoneFriend stoneFriend;
	public Room rocksGatheringRoom;
	//can be picked up and carried
	//can talk to rock if carrying and will initialize return hand dialog
	//^ will also activate dialog if drop the hand in the rock room
	//cant be picked up until talk to rocks for first time
	//
	void Awake(){
		player = GameObject.FindGameObjectWithTag("Player");
	}

	public override void PickUp(){
		base.PickUp();
		stoneFriend.GetComponent<ActivateDialogWhenClose>().enabled = true; // can talk to stone if carrying hand
		gameObject.GetComponent<BoxCollider2D>().enabled = false;
	}

	void OnEnable(){
        requiresGrabbyGloves = false;
		if(rocksGatheringRoom == null)
		rocksGatheringRoom = GameObject.Find("a4").GetComponent<Room>();
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
		//gameObject.transform.parent = stoneFriend.transform;//goes back to Stone as parent
	}


}

