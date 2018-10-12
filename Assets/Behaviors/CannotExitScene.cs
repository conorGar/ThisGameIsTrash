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
        if (RoomManager.Instance != null)
		    SetLimits(RoomManager.Instance.currentRoom);
	}

	void Update () {

        if (transform.position.x < leftLimit || transform.position.x > rightLimit ||
            transform.position.y < botLimit || transform.position.y > topLimit)
        {
            transform.position = new Vector2(Mathf.Clamp(transform.position.x, leftLimit, rightLimit),
                                         Mathf.Clamp(transform.position.y, botLimit, topLimit));
        }
	}



	public void SetLimits(Room room){
		Rect rect = room.GetRoomBoundaries();
        leftLimit = rect.xMin;
        rightLimit = rect.xMax;
        botLimit = rect.yMin;
        topLimit = rect.yMax;
	}


}