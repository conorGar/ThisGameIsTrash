using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogActivator : MonoBehaviour {
	public string myFriend;
	GameObject friendInstance;
	public GameObject dialogCanvas;
	public GameObject dialogManager;
	public DialogActionManager  dialogActionManager;
	public GameObject dialogIcon;

	//So this sets the local data(canvasHud and such) to the loaded friends at the start of the scene, by assigning it those values from its own 'ActivateDialogWhenClose)

	void OnEnable(){
		for(int i = 0; i < FriendManager.Instance.friends.Count;i++){
			if(FriendManager.Instance.friends[i].friendName == myFriend){
				friendInstance = FriendManager.Instance.friends[i].gameObject;
				break;
			}
		}
		friendInstance.GetComponent<ActivateDialogWhenClose>().GetData(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
