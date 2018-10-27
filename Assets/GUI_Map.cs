using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUI_Map : MonoBehaviour {


	public GameObject myRooms;
	public GameObject playerIcon;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnEnable(){

		for(int i = 0; i < myRooms.transform.childCount; i++){
			if(RoomManager.Instance.currentRoom.name == myRooms.transform.GetChild(i).name){
				playerIcon.transform.position = myRooms.transform.GetChild(i).transform.position;//set current player position in world
				break;
			}
		}
	}

	public Vector3 GetRoomPosition(string room){//used by star icons
		for(int i = 0; i < myRooms.transform.childCount; i++){
			if(room == myRooms.transform.GetChild(i).name){
				return myRooms.transform.GetChild(i).position;//set current player position in world
				break;
			}
		}
		Debug.Log("Star Icon's position for room:"+ room + " was not found...");
		return Vector3.zero;
	}
}
