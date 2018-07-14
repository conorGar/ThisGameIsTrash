using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPortal : MonoBehaviour {
    public RoomManager roomManager;

    public enum Direction
    {
        HORIZONTAL,
        VERTICAL
    }

    public Direction direction = Direction.HORIZONTAL;
    public Room positiveRoom; // The room that is +x or +y depending on the direction
    public Room negativeRoom; // The room that is -x or -y depending on the direction

	// Use this for initialization
	void Start () {
		
	}

    public void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            if (roomManager.currentRoom == positiveRoom)
            {
                Debug.Log("Entering negative room!");
                roomManager.currentRoom = negativeRoom;
                roomManager.previousRoom = positiveRoom;
                roomManager.isTransitioning = true;
                roomManager.lerpCamera = 0.0f;
            }
            else if (roomManager.currentRoom == negativeRoom)
            {
                Debug.Log("Entering positive room!");
                roomManager.currentRoom = positiveRoom;
                roomManager.previousRoom = negativeRoom;
                roomManager.isTransitioning = true;
                roomManager.lerpCamera = 0.0f;
            }
            else
            {
                Debug.Log("This portal isn't even for the current room.  How did you get here!?");
            }
        }
    }
}
