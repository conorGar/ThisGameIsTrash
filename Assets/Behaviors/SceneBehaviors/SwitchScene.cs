using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour {

	/*public float leftLimit = -15f;
	public float rightLimit = 15f;
	public float upLimit = 7f;
	public float botLimit = -7f;
	//public string leftScene;
	//public string rightScene;
	//public string upScene;
	//public string downScene;
	public float xSpawnLeftScene = 14f;
	public float ySpawnLeftScene;
	public float xSpawnRightScene = -14f;
	public float ySpawnRightScene;
	public float xSpawnTop;
	public float ySpawnTop = -7f;
	public float xSpawnBot;
	public float ySpawnBot = 7f;


	public int numLeftRoom;
	public int numRightRoom;
	public int numTopRoom;
	public int numBotRoom;

	public string roomName;


	public int worldStartingRoomNum;

	bool stopMusicWhenLeaveRoom;
	GameObject player;
	GameObject canvas;
	string sceneToSwitchTo;
	int spawnOnce = 0;
	BoundsSetter myBoundSetter;
	Ev_MainCamera mycam;

	void Start () {
		mycam = GameObject.Find("tk2dCamera").GetComponent<Ev_MainCamera>();
		myBoundSetter = gameObject.GetComponent<BoundsSetter>();
		player = GameObject.FindGameObjectWithTag("Player");
		canvas = GameObject.Find("HUD");

		GlobalVariableManager.Instance.ROOM_NUM = worldStartingRoomNum;

		if(GlobalVariableManager.Instance.WORLD_NUM == 2 && GlobalVariableManager.Instance.ROOM_NUM == 14 ||GlobalVariableManager.Instance.ROOM_NUM == 36)
			stopMusicWhenLeaveRoom = true;

		Debug.Log("Number left room AT START " + numLeftRoom);
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log("spawn once " + spawnOnce);

		if(player != null){
			
			if(player.transform.position.x < leftLimit && spawnOnce == 0){
				
				Debug.Log("Number left room" + numLeftRoom);
				if(numLeftRoom != 999){
				 GlobalVariableManager.Instance.ROOM_NUM = numLeftRoom;
				}
				spawnOnce =1;
				myBoundSetter.SetBounds(GlobalVariableManager.Instance.ROOM_NUM);
				roomName = myBoundSetter.GetRoomName();
				Debug.Log("Room name:" + roomName);
				mycam.Transition("left", roomName);
			}else if(player.transform.position.x > rightLimit && spawnOnce == 0){
				Debug.Log("****PAST RIGHT BOUNDS");
				if(numRightRoom != 999){
				 GlobalVariableManager.Instance.ROOM_NUM = numRightRoom;
				}
				spawnOnce =1;
				myBoundSetter.SetBounds(GlobalVariableManager.Instance.ROOM_NUM);
				roomName = myBoundSetter.GetRoomName();
				mycam.Transition("right", roomName);
			}else if(player.transform.position.y < upLimit && spawnOnce == 0){
				Debug.Log("****PAST Bot BOUNDS");
				if(numBotRoom != 999){
				 GlobalVariableManager.Instance.ROOM_NUM = numBotRoom;
				}
				spawnOnce =1;
				myBoundSetter.SetBounds(GlobalVariableManager.Instance.ROOM_NUM);
				roomName = myBoundSetter.GetRoomName();
				mycam.Transition("bot",roomName);

			}else if(player.transform.position.y > botLimit && spawnOnce == 0){
				Debug.Log("****PAST TOP BOUNDS");
				if(numTopRoom != 999){
				 GlobalVariableManager.Instance.ROOM_NUM = numTopRoom;
				}
				spawnOnce =1;
				myBoundSetter.SetBounds(GlobalVariableManager.Instance.ROOM_NUM);
				roomName = myBoundSetter.GetRoomName();
				mycam.Transition("up", roomName);

			}
		}
	}

	/*IEnumerator FadeOut(){
			
		CanvasGroup canvasGroup = canvas.GetComponent<CanvasGroup>();
		while(canvasGroup.alpha > 0){
			canvasGroup.alpha -= Time.deltaTime / 2;
			yield return null;
		}
		SceneManager.LoadScene(sceneToSwitchTo);
	}*/
	/*
	public void SetLimits(float leftLim, float rightLim, float topLim, float botLim){
		//activted by BoundsSetter
		leftLimit = leftLim;
		rightLimit = rightLim;
		upLimit = topLim;
		botLimit = botLim;

		Debug.Log("Left limit of room:" + leftLimit);
		Debug.Log("Right limit of room:" +  rightLimit);
	

	}
	public void SetRooms(int l, int r, int t, int b){
		//activated by boundSetter
		numLeftRoom = l;
		numRightRoom = r;
		numTopRoom = t;
		numBotRoom = b;
		spawnOnce = 0;
	}


	//if over on left boundary and left room num != null, set roomNUm to left room num
	//than set camera new boundaries left and right and trigger "transition" event- movement var 'left'  - transition event should move by same amount each time, shouldnt need to know bounds
	//^(detatch from parent object), set new boundaries for camera and let 'populate self' populate the 
	//new room.
	//There probably needs to be some sort of 'Boundary Setter' Gameobject that sets the boundary depending
	//on what the room number is(boundary for both this switch scene and for camera...)
	*/
}
