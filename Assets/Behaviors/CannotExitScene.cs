using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannotExitScene : MonoBehaviour {

	public float leftLimit;
	public float rightLimit;
	public float topLimit;
	public float botLimit;
	public bool globalPos;
	// Update is called once per frame

	void OnEnable(){ 
		SetLimits(RoomManager.Instance.currentRoom);
	}

	void Update () {

		if(transform.position.x < leftLimit){
			transform.localPosition = new Vector2(leftLimit, transform.localPosition.y);
			Debug.Log("Enemy at left limit");
		} else if(transform.position.x > rightLimit){
			transform.localPosition = new Vector2(rightLimit, transform.localPosition.y);
			Debug.Log("Enemy at right limit");

		}
		if(transform.position.y < botLimit){
			transform.localPosition = new Vector2(transform.localPosition.x, botLimit);
			Debug.Log("Enemy at bot limit");

		} else if(transform.position.y > topLimit){
			transform.localPosition = new Vector2(transform.localPosition.x, topLimit);
			Debug.Log("Enemy at top limit");

		}
	}



	public void SetLimits(Room room){
		Rect rect = room.GetRoomCameraBoundaries();
        leftLimit = rect.xMin;
        rightLimit = rect.xMax;
        botLimit = rect.yMin;
        topLimit = rect.yMax;
	}


}