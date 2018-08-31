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


	public override void PickUpEvent(){

		stoneFriend.GetComponent<ActivateDialogWhenClose>().enabled = true; // can talk to stone if carrying hand
		gameObject.GetComponent<BoxCollider2D>().enabled = false;
	}

	public override void DropEvent ()
	{
		//if room = rock room, activate stone dialog
		if(RoomManager.Instance.currentRoom == rocksGatheringRoom){
			stoneFriend.GetComponent<ActivateDialogWhenClose>().ActivateDialog();
		}else{//otherwise disables ability to speak to Stone
			stoneFriend.GetComponent<ActivateDialogWhenClose>().enabled = false;
		}
		gameObject.transform.parent = stoneFriend.transform;//goes back to Stone as parent
	}


}

