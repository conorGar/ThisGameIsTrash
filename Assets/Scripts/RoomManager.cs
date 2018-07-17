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

    public Vector3 previousCameraPosition;
    public Vector3 targetCameraPosition;

	// Use this for initialization
	void Start () {
        currentRoom = startRoom;
        previousRoom = null;
        SetCamFollowBounds(currentRoom);
	}
	
	// Update is called once per frame
	void Update () {
        // lerpin'
        if (isTransitioning)
        {
            mainCamera.transform.position = new Vector3(Mathf.Lerp(previousCameraPosition.x, targetCameraPosition.x, lerpCamera),
                                        Mathf.Lerp(previousCameraPosition.y, targetCameraPosition.y, lerpCamera),
                                        mainCamera.transform.position.z);

//<<<<<<< HEAD
            if (lerpCamera >= 1.0f){
          		currentRoom.ActivateRoom();
                isTransitioning = false;
            }else
//=======
            if (lerpCamera >= 1.0f)
            {
                isTransitioning = false;
                currentRoom.ActivateRoom();
            }
            else
//>>>>>>> refs/remotes/origin/digital_smash
                lerpCamera += lerpCameraSpeed;
        }
    }

//<<<<<<< HEAD
    public void SetCamFollowBounds(float leftLimit, float rightLimit, float topLimit, float botLimit){
    	//Activated by Room.cs under 'ActivateRoom()'
    	Debug.Log("SetCamFollowBounds Activated properly");
    	mainCamera.GetComponent<Ev_MainCamera>().enabled = true; //renable following camera after transition
    	mainCamera.GetComponent<Ev_MainCamera>().SetMinMax(leftLimit,rightLimit,topLimit,botLimit);
//=======
	}
    public void SetCamFollowBounds(Room room)
    {
        //Activated by Room.cs under 'ActivateRoom()'
        Debug.Log("SetCamFollowBounds Activated properly");
        mainCamera.GetComponent<Ev_MainCamera>().enabled = true; //renable following camera after transition
        mainCamera.GetComponent<Ev_MainCamera>().SetMinMax(room);
//>>>>>>> refs/remotes/origin/digital_smash
    }
}
