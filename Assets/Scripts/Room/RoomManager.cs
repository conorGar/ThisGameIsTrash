using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviour {
    public static RoomManager Instance;
    public bool isTransitioning = false;
    public float lerpCamera = 0.0f;
    public float lerpCameraSpeed = 0.1f;

    public Room startRoom;
    public Room currentRoom;
    public Room previousRoom;

    public List<Room> rooms;
    public List<GameObject> ObjectsClearedOnLeavingRoom;

    public Vector3 previousCameraPosition;
    public Vector3 targetCameraPosition;

    void Awake()
    {
        Instance = this;
        currentRoom = startRoom;
        previousRoom = null;
    }

    void Start()
    {
        // check for the checkpoint debug config if a spawncheckpoint was configured.
        var configs = Resources.FindObjectsOfTypeAll<CheckpointDebugConfig>();
        if (configs.Length > 0) {
            CheckpointDebugConfig config = configs[0];
            if (config.isOn) {
                Checkpoint checkpoint = GameObject.Find(config.checkpointLookup.dictionary[SceneManager.GetActiveScene().path]).GetComponent<Checkpoint>();
                if (checkpoint != null) {
                    CheckpointManager.Instance.lastCheckpoint = checkpoint;
                    currentRoom = checkpoint.myRoom;

                    PlayerManager.Instance.player.transform.position = CheckpointManager.Instance.lastCheckpoint.transform.position; //Start at debug checkpoint
                    CamManager.Instance.mainCam.transform.position = new Vector3(CheckpointManager.Instance.lastCheckpoint.transform.position.x, CheckpointManager.Instance.lastCheckpoint.transform.position.y, -10f);
                }
            }
        }
    }

    public void Restart(Room respawnRoom){//called at player death in PlayerTakedamage
		currentRoom = respawnRoom;
        previousRoom = null;
        currentRoom.ActivateRoom();
	}
	// Update is called once per frame
	void Update () {
        // lerpin'
        if (isTransitioning)
        {
            CamManager.Instance.mainCam.transform.position = new Vector3(Mathf.Lerp(previousCameraPosition.x, targetCameraPosition.x, lerpCamera),
                                        Mathf.Lerp(previousCameraPosition.y, targetCameraPosition.y, lerpCamera),
                                        CamManager.Instance.mainCam.transform.position.z);

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
            if (currentRoom != null)
            {
                Vector3 pos = PlayerManager.Instance.player.transform.position;

                // if the player is out of the bounds of the room, clamp everything down.
                if (pos.x < currentRoom.roomCollider2D.bounds.min.x || pos.x > currentRoom.roomCollider2D.bounds.max.x ||
                    pos.y < currentRoom.roomCollider2D.bounds.min.y || pos.y > currentRoom.roomCollider2D.bounds.max.y)

                PlayerManager.Instance.player.transform.position = new Vector3(Mathf.Clamp(pos.x, currentRoom.roomCollider2D.bounds.min.x, currentRoom.roomCollider2D.bounds.max.x),
                                                        Mathf.Clamp(pos.y, currentRoom.roomCollider2D.bounds.min.y, currentRoom.roomCollider2D.bounds.max.y),
                                                        pos.z);
            }
        }
    }

    public void ActivateCurrentRoom()
    {
        currentRoom.ActivateRoom();
    }

    public void SetCamFollowBounds(Room room)
    {
        //Activated by Room.cs under 'ActivateRoom()'
        Debug.Log("SetCamFollowBounds Activated properly");
        CamManager.Instance.mainCam.enabled = true; //renable following camera after transition
        CamManager.Instance.mainCam.SetMinMax(room);
    }
}
