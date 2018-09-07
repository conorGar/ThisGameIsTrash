using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialActivator : MonoBehaviour {
	public string startingRoom;//for now only needed for tutorial popup proper function.
	GameObject player;
	GameObject mainCamera;
	bool activatedAlready;
	Room currentRoom;

	public bool largeTrash;
	public bool pins;
	public bool armoredEnemies;
	//TODO: right now only for Large Trash. Should be usable for all objects that can spawn tutorials

	// Use this for initialization
	void OnEnable() {
		player = GameObject.FindGameObjectWithTag("Player");
		mainCamera = GameObject.Find("tk2dCamera");
	}
	
	// Update is called once per frame
	void Update () {
		if(player != null && Vector2.Distance(gameObject.transform.position,player.transform.position) < 20f){//only activates tut when on screen (close enough)
			if(largeTrash){
				if(!activatedAlready && RoomManager.Instance.currentRoom.name == startingRoom && (GlobalVariableManager.Instance.TUT_POPUPS_SHOWN & GlobalVariableManager.TUTORIALPOPUPS.LARGETRASH ) != GlobalVariableManager.TUTORIALPOPUPS.LARGETRASH){
						ActivateTutorial();
						activatedAlready = true;
						
				}
			}else if(pins){
				if(!activatedAlready && (GlobalVariableManager.Instance.TUT_POPUPS_SHOWN & GlobalVariableManager.TUTORIALPOPUPS.PINS ) != GlobalVariableManager.TUTORIALPOPUPS.PINS){
					ActivateTutorial();
					activatedAlready = true;
				}
			}
		}
	}

	void ActivateTutorial(){
		Debug.Log("Large Trash tutorial activated xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
		if(largeTrash){
			mainCamera.GetComponent<Ev_MainCameraEffects>().CameraPan(gameObject.transform.position,"tutorial");
		}else if(pins){
			mainCamera.GetComponent<Ev_MainCameraEffects>().CameraPan(gameObject.transform.position,"tutorial_pins");
		}else if(armoredEnemies){
			mainCamera.GetComponent<Ev_MainCameraEffects>().CameraPan(gameObject.transform.position,"tutorial_armored");
		}
		player.GetComponent<EightWayMovement>().enabled = false;
		player.GetComponent<PlayerTakeDamage>().enabled = false;
		GlobalVariableManager.Instance.TUT_POPUP_ISSHOWING = true;

	}
}
