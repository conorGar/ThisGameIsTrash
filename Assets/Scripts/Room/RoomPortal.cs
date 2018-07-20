using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPortal : EditorMonoBehaviour {
    public RoomManager roomManager;

    public enum Direction
    {
        HORIZONTAL,
        VERTICAL
    }

    public Direction direction = Direction.HORIZONTAL;
    public Room positiveRoom; // The room that is +x or +y depending on the direction
    public Room negativeRoom; // The room that is -x or -y depending on the direction
    public GameObject player;
    private Collider2D collider2d;
    private Collider2D playerCollider2d;

	// Use this for initialization
	void Start () {
        player = null;
        collider2d = GetComponent<Collider2D>();
        playerCollider2d = null;
	}

    public void Update()
    {
        // lerp a player through the portal and then null the player when done.
        if (player != null)
        {
            // moving positively
            if (roomManager.currentRoom == positiveRoom)
            {
                // moving through the portal on the +x-axis
                if (direction == Direction.HORIZONTAL)
                {
                    player.transform.position = new Vector3(Mathf.Lerp(collider2d.bounds.min.x - playerCollider2d.bounds.extents.x, collider2d.bounds.max.x + playerCollider2d.bounds.extents.x + 0.5f, roomManager.lerpCamera),
                                                            player.transform.position.y,
                                                            player.transform.position.z);
                }
                // moving through the portal on the +y-axis
                else
                {
                    player.transform.position = new Vector3(player.transform.position.x,
                                                            Mathf.Lerp(collider2d.bounds.min.y - playerCollider2d.bounds.extents.y, collider2d.bounds.max.y + playerCollider2d.bounds.extents.y + 0.5f, roomManager.lerpCamera),
                                                            player.transform.position.z);
                }
            }
            // moving negatively
            else if (roomManager.currentRoom == negativeRoom)
            {
                // moving through the portal on the -x-axis
                if (direction == Direction.HORIZONTAL)
                {
                    player.transform.position = new Vector3(Mathf.Lerp(collider2d.bounds.max.x + playerCollider2d.bounds.extents.x, collider2d.bounds.min.x - playerCollider2d.bounds.extents.x - 0.5f, roomManager.lerpCamera),
                                                            player.transform.position.y,
                                                            player.transform.position.z);
                }
                // moving through the portal on the -y-axis
                else
                {
                    player.transform.position = new Vector3(player.transform.position.x,
                                                            Mathf.Lerp(collider2d.bounds.max.y + playerCollider2d.bounds.extents.y, collider2d.bounds.min.y - playerCollider2d.bounds.extents.y - 0.5f, roomManager.lerpCamera),
                                                            player.transform.position.z);
                }
            }

            if (roomManager.lerpCamera >= 1.0f)
            {
                player = null;
                playerCollider2d = null;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {	
    	Debug.Log("Collided with roomPortal");
        if (player == null)
        {
            // TODO: Clean this up!
            if (collider.gameObject.CompareTag("Player"))
            {
                if (roomManager.currentRoom == positiveRoom)
                {
                    roomManager.currentRoom = negativeRoom;
                    roomManager.previousRoom = positiveRoom;
                    player = collider.gameObject;
                    playerCollider2d = player.GetComponent<Collider2D>();
                    roomManager.mainCamera.GetComponent<Ev_MainCamera>().enabled = false; //disable camera following for transition. Enabled uner RoomManager: SetCamBOunds
                    roomManager.previousCameraPosition = roomManager.mainCamera.GetComponent<Ev_MainCamera>().transform.position;
                    roomManager.isTransitioning = true;
                    roomManager.lerpCamera = 0.0f;

                    Rect roomBoundaries = roomManager.currentRoom.GetRoomCameraBoundaries();
                    if (direction == Direction.VERTICAL)
                    {
                        // moving y-
                        roomManager.targetCameraPosition = new Vector3(Mathf.Clamp(player.transform.position.x, roomBoundaries.xMin, roomBoundaries.xMax), roomBoundaries.yMax, roomManager.previousCameraPosition.z);
                    }
                    else
                    {
                        // moving x-
                        roomManager.targetCameraPosition = new Vector3(roomBoundaries.xMax, Mathf.Clamp(player.transform.position.y, roomBoundaries.yMin, roomBoundaries.yMax), roomManager.previousCameraPosition.z);
                    }
                }
                else if (roomManager.currentRoom == negativeRoom)
                {
                    roomManager.currentRoom = positiveRoom;
                    roomManager.previousRoom = negativeRoom;
                    player = collider.gameObject;
                    playerCollider2d = player.GetComponent<Collider2D>();
                    roomManager.mainCamera.GetComponent<Ev_MainCamera>().enabled = false; //disable camera following for transition. Enabled uner RoomManager: SetCamBOunds
                    roomManager.previousCameraPosition = roomManager.mainCamera.GetComponent<Ev_MainCamera>().transform.position;
                    roomManager.isTransitioning = true;
                    roomManager.lerpCamera = 0.0f;

                    Rect roomBoundaries = roomManager.currentRoom.GetRoomCameraBoundaries();
                    if (direction == Direction.VERTICAL)
                    {
                        // moving y+
                        roomManager.targetCameraPosition = new Vector3(Mathf.Clamp(player.transform.position.x, roomBoundaries.xMin, roomBoundaries.xMax), roomBoundaries.yMin, roomManager.previousCameraPosition.z);
                    }
                    else
                    {
                        // moving x+
                        roomManager.targetCameraPosition = new Vector3(roomBoundaries.xMin, Mathf.Clamp(player.transform.position.y, roomBoundaries.yMin, roomBoundaries.yMax), roomManager.previousCameraPosition.z);
                    }
                }
                else
                {
                    Debug.Log("This portal isn't even for the current room.  How did you get here!?");
                }
            }
        }
    }
}
