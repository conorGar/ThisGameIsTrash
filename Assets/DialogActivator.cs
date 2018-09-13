using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogActivator : MonoBehaviour {
	public string myFriend;
	GameObject friendInstance;
	// Use this for initialization
	void OnEnable(){
		for(int i = 0; i < FriendManager.Instance.friends.Count;i++){
			if(FriendManager.Instance.friends[i].friendName == myFriend){
				friendInstance = FriendManager.Instance.friends[i].gameObject;
				break;
			}
		}
		friendInstance.GetComponent<ActivateDialogWhenClose>().GetData(gameObject.GetComponent<ActivateDialogWhenClose>());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
