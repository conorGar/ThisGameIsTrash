using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour {
    public tk2dCamera mainCamera;
    public GameObject player;
    public bool isTransitioning = false;
    public float lerpCamera = 0.0f;
    public float lerpCameraSpeed = 0.1f;

    public Room startRoom;

    public Room currentRoom;
    public Room previousRoom;


	// Use this for initialization
	void Start () {
        currentRoom = startRoom;
        previousRoom = null;
	}
	
	// Update is called once per frame
	void Update () {
        // lerpin'
        if (isTransitioning)
        {
            mainCamera.transform.position = new Vector3(Mathf.Lerp(previousRoom.transform.position.x, currentRoom.transform.position.x, lerpCamera),
                                        Mathf.Lerp(previousRoom.transform.position.y, currentRoom.transform.position.y, lerpCamera),
                                        mainCamera.transform.position.z);

            if (lerpCamera >= 1.0f)
                isTransitioning = false;
            else
                lerpCamera += lerpCameraSpeed;
        }
    }
}
