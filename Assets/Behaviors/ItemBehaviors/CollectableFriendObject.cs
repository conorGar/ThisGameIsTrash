using UnityEngine;
using System.Collections;

public class CollectableFriendObject : MonoBehaviour
{
	public Friend myFriend;
	public GUI_CollectableFriendHUD myHUD;
	// Use this for initialization
	void Start ()
	{
		for(int i = 0; i < FriendManager.Instance.friends.Count;i++){
			if(FriendManager.Instance.friends[i].tag == myFriend.tag){
				myFriend = FriendManager.Instance.friends[i];
				break;
			}
		}

	}
	
	void OnTriggerEnter2D(Collider2D collider){
		if(collider.tag == "Player"){
			myFriend.PickUpObject(this);
			myHUD.UpdateCollected();
			gameObject.SetActive(false);
		}
	}


}

