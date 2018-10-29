using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialActivator : MonoBehaviour {
	public string startingRoom;//for now only needed for tutorial popup proper function.
	GameObject player;
	bool activatedAlready;
	Room currentRoom;

	public bool largeTrash;
	public bool pins;
	public bool armoredEnemies;
	//TODO: right now only for Large Trash. Should be usable for all objects that can spawn tutorials

	// Use this for initialization
	void OnEnable() {
		player = GameObject.FindGameObjectWithTag("Player");
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
			}else if(armoredEnemies){
				if(!activatedAlready && (GlobalVariableManager.Instance.TUT_POPUPS_SHOWN & GlobalVariableManager.TUTORIALPOPUPS.ARMOREDENEMIES ) != GlobalVariableManager.TUTORIALPOPUPS.ARMOREDENEMIES){
					ActivateTutorial();
					activatedAlready = true;
				}
			}
		}
	}

	void ActivateTutorial(){
        GameStateManager.Instance.PushState(typeof(DialogState));
        Debug.Log("Large Trash tutorial activated xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
		if(largeTrash){
            CamManager.Instance.mainCamEffects.CameraPan(gameObject.transform.position,"tutorial");
		}else if(pins){
            CamManager.Instance.mainCamEffects.CameraPan(gameObject.transform.position,"tutorial_pins");
		}else if(armoredEnemies){
            CamManager.Instance.mainCamEffects.CameraPan(gameObject.transform.position,"tutorial_armored");
		}
	}
}
