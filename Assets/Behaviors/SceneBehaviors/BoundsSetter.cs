using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsSetter : MonoBehaviour {

	/*Ev_MainCamera mycam;
	SwitchScene sceneChanger;

	float minX;
	float maxX;
	float minY;
	float maxY;
	int lRoom;
	int rRoom;
	int tRoom;
	int bRoom;
	string roomName;

	void Start(){
		mycam = GameObject.Find("tk2dCamera").GetComponent<Ev_MainCamera>();
		sceneChanger = gameObject.GetComponent<SwitchScene>();
	}

	public void SetBounds(int roomNum){
		Debug.Log("Room number sent:" + roomNum);
		if(GlobalVariableManager.Instance.WORLD_NUM == 1){
			
			if(roomNum == 12){
				minX = -12.4f; maxX = -11.4f; minY = -10.8f; maxY = -9.8f;//set camera bounds for next room
				lRoom = 11; rRoom = 13; tRoom = 7; bRoom = 17;//set neighboring rooms for next room
				roomName = "c3";
			}else if(roomNum == 1){
				minX = -11.9f; maxX = -5.1f; minY = 48.8f; maxY = 67.9f;
				lRoom = 999; rRoom = 999; tRoom = 99; bRoom = 7;
				roomName = "a3";
			}else if(roomNum == 2){
				minX = 23.6f; maxX = 32.8f; minY = 34.1f; maxY = 55.2f;
				lRoom = 999; rRoom = 3; tRoom = 999; bRoom = 999;
				roomName = "a4";
			}else if(roomNum == 3){
				minX = 62.7f; maxX = 68.3f; minY = 43.6f; maxY = 50.8f;
				lRoom = 2; rRoom = 4; tRoom = 999; bRoom = 8;
				roomName = "a5";
			}else if(roomNum == 4){
				minX = 99.7f; maxX = 127.8f; minY = 43.6f; maxY = 50.8f;
				lRoom = 3; rRoom = 30; tRoom = 27; bRoom = 9;
				roomName = "a6";
			}else if(roomNum == 5){
				minX = -101.6f; maxX = -77.6f; minY = -6.2f; maxY = 22.5f;
				lRoom = 999; rRoom = 6; tRoom = 999; bRoom = 999;
				roomName = "b1";
			}else if(roomNum == 6){
				minX = -44f; maxX = -40f; minY = 32f; maxY = 62f;
				lRoom = 999; rRoom = 7; tRoom = 999; bRoom = 999;
				roomName = "cinemaRoom";
			}else if(roomNum == 7){
				minX = -12.4f; maxX = -7.6f; minY = 7.4f; maxY = 32f;
				lRoom = 6; rRoom = 8; tRoom = 1; bRoom = 12;
				roomName = "b3";
			}else if(roomNum == 8){
				minX = 17.6f; maxX = 74f; minY = 14.8f; maxY = 25.6f;
				lRoom = 9; rRoom = 999; tRoom = 2; bRoom = 13;//fix top room number eventually*****
				roomName = "b4";
			}else if(roomNum == 9){
				minX = 100.9f; maxX = 119.8f; minY = 11.8f; maxY = 27.3f;
				lRoom = 14; rRoom = 999; tRoom = 4; bRoom = 999;
				roomName = "b5";
			}else if(roomNum == 10){
				minX = -86.3f; maxX = -77.6f; minY = -16.8f; maxY = -6.2f;
				lRoom = 999; rRoom = 11; tRoom = 5; bRoom = 999;
				roomName = "c1";
			}else if(roomNum == 11){
				minX = -42.9f; maxX = -41.9f; minY = -10.8f; maxY = -9.8f;
				lRoom = 10; rRoom = 12; tRoom = 999; bRoom = 16;
				roomName = "c2";
			}else if(roomNum == 13){
				minX = 18.4f; maxX = 24.3f; minY = -10.8f; maxY = -4f;
				lRoom = 10; rRoom = 12; tRoom = 999; bRoom = 18;
				roomName = "c4";
			}else if(roomNum == 14){
				minX = 51.7f; maxX = 81.3f; minY = -10.8f; maxY = -4f;
				lRoom = 13; rRoom = 12; tRoom = 999; bRoom = 999;         //CHANGE RIGHT ROOM******
				roomName = "c5";
			}else if(roomNum == 15){
				minX = -88.8f; maxX = -79.7f; minY = -52.7f; maxY = -28.6f;
				lRoom = 999; rRoom = 16; tRoom = 999; bRoom = 20;
				roomName = "e1";
			}else if(roomNum == 16){
				minX = -48.9f; maxX = -41.9f; minY = -28.9f; maxY = -27.6f;
				lRoom = 15; rRoom = 17; tRoom = 11; bRoom = 16;
				roomName = "d2";
			}else if(roomNum == 17){
				minX = -12.4f; maxX = -11.4f; minY = -28.6f; maxY = -27.6f;
				lRoom = 16; rRoom = 18; tRoom = 12; bRoom = 22;
				roomName = "d3";
			}else if(roomNum == 18){
				minX = 20f; maxX = 23f; minY = -27.4f; maxY = -10.8f;
				lRoom = 17; rRoom = 999; tRoom = 13; bRoom = 999;
				roomName = "d4";
			}else if(roomNum == 20){
				minX = -88.8f; maxX = -87.8f; minY = -78.5f; maxY = -69f;
				lRoom = 999; rRoom = 21; tRoom = 15; bRoom = 999;
				roomName = "e1";
			}else if(roomNum == 21){
				minX = -58.1f; maxX = -57.1f; minY = -78.5f; maxY = -69f;
				lRoom = 20; rRoom = 22; tRoom = 999; bRoom = 999;
				roomName = "e2";
			}else if(roomNum == 22){
				minX = -29f; maxX = 2.6f; minY = -86.5f; maxY = -58f;
				lRoom = 21; rRoom = 23; tRoom = 17; bRoom = 999;
				roomName = "e3";
			}else if(roomNum == 23){
				minX = 32.2f; maxX = 41.2f; minY = -59.5f; maxY = -50f;
				lRoom = 22; rRoom = 24; tRoom = 999; bRoom = 999;
				roomName = "e4";
			}else if(roomNum == 27){
				minX = 32.2f; maxX = 41.2f; minY = -59.5f; maxY = -50f;
				lRoom = 22; rRoom = 24; tRoom = 999; bRoom = 999;
				roomName = "topCliff1";
			}


			mycam.SetMinMax(minX,maxX,minY,maxY);
			sceneChanger.SetLimits(minX,(maxX+32f),minY,(maxY+19f));
			sceneChanger.SetRooms(lRoom,rRoom,tRoom,bRoom);
		}
	}//end of setBounds()

	public string GetRoomName(){
		return roomName;
	}*/
}
