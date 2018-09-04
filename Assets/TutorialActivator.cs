using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialActivator : MonoBehaviour {
	public string startingRoom;//for now only needed for tutorial popup proper function.
	GameObject player;
	GameObject mainCamera;
	bool activatedAlready;
	Room currentRoom;
	//TODO: right now only for Large Trash. Should be usable for all objects that can spawn tutorials

	// Use this for initialization
	void OnEnable() {
		player = GameObject.FindGameObjectWithTag("Player");
		mainCamera = GameObject.Find("tk2dCamera");
	}
	
	// Update is called once per frame
	void Update () {
		if(player != null && Vector2.Distance(gameObject.transform.position,player.transform.position) < 20f){//only activates tut when on screen (close enough)
			if(!activatedAlready && RoomManager.Instance.currentRoom.name == startingRoom && (GlobalVariableManager.Instance.TUT_POPUPS_SHOWN & GlobalVariableManager.TUTORIALPOPUPS.LARGETRASH ) != GlobalVariableManager.TUTORIALPOPUPS.LARGETRASH){
					ActivateTutorial();//TODO:Test properly
					activatedAlready = true;
					
			}
		}
	}

	void ActivateTutorial(){
		Debug.Log("Large Trash tutorial activated xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
		mainCamera.GetComponent<Ev_MainCameraEffects>().CameraPan(gameObject.transform.position,"tutorial");
		player.GetComponent<EightWayMovement>().enabled = false;
		player.GetComponent<PlayerTakeDamage>().enabled = false;
		GlobalVariableManager.Instance.TUT_POPUP_ISSHOWING = true;

	}
}
