using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour {
    public static RoomManager Instance;
    public tk2dCamera mainCamera;
    public GameObject player;
    public Collider2D playerCollider2D;
    public bool isTransitioning = false;
    public float lerpCamera = 0.0f;
    public float lerpCameraSpeed = 0.1f;

    public Room startRoom;
    public Room currentRoom;
    public Room previousRoom;

    public List<Room> rooms;

    public Vector3 previousCameraPosition;
    public Vector3 targetCameraPosition;

    void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start () {
        playerCollider2D = player.GetComponent<Collider2D>();
        currentRoom = startRoom;
        previousRoom = null;
        currentRoom.ActivateRoom();
	}
	public void Restart(){//called at player death in PlayerTakedamage
		currentRoom = startRoom;
        previousRoom = null;
        currentRoom.ActivateRoom();
	}
	// Update is called once per frame
	void Update () {
        // lerpin'
        if (isTransitioning)
        {
            mainCamera.transform.position = new Vector3(Mathf.Lerp(previousCameraPosition.x, targetCameraPosition.x, lerpCamera),
                                        Mathf.Lerp(previousCameraPosition.y, targetCameraPosition.y, lerpCamera),
                                        mainCamera.transform.position.z);

            if (lerpCamera >= 1.0f)
            {        
                previousRoom.DeactivateRoom();
                currentRoom.ActivateRoom();
                Debug.Log("Enabled ActivateRoom() Here");
                isTransitioning = false;
            }
            else
                lerpCamera += lerpCameraSpeed;
        }
        else
        {
            if (player != null && currentRoom != null)
            {
                Vector3 pos = player.transform.position;

                // if the player is out of the bounds of the room, clamp everything down.
                if (pos.x < currentRoom.roomCollider2D.bounds.min.x || pos.x > currentRoom.roomCollider2D.bounds.max.x ||
                    pos.y < currentRoom.roomCollider2D.bounds.min.y || pos.y > currentRoom.roomCollider2D.bounds.max.y)

                player.transform.position = new Vector3(Mathf.Clamp(pos.x, currentRoom.roomCollider2D.bounds.min.x, currentRoom.roomCollider2D.bounds.max.x),
                                                        Mathf.Clamp(pos.y, currentRoom.roomCollider2D.bounds.min.y, currentRoom.roomCollider2D.bounds.max.y),
                                                        pos.z);
            }
        }
    }

    public void SetCamFollowBounds(Room room)
    {
        //Activated by Room.cs under 'ActivateRoom()'
        Debug.Log("SetCamFollowBounds Activated properly");
        mainCamera.GetComponent<Ev_MainCamera>().enabled = true; //renable following camera after transition
        mainCamera.GetComponent<Ev_MainCamera>().SetMinMax(room);
    }
}
