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
	public bool areaTitleActivator;
	public Sprite areaTitleSprite;
	public bool camZoomer;
	public float newCamZoomVal;
	public AreaGarbageManager areaManager;
	public bool interiorRoomPortal;
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
					
					if(camZoomer){
                		CamManager.Instance.mainCamEffects.ZoomInOut(1.15f,1f);
					}if(areaTitleActivator && areaManager!=null){
						areaManager.myHUD.gameObject.SetActive(false);
                	}
                	if(interiorRoomPortal){
                		//turn off the interior layer culling mask on the camera when leave the room
                		CamManager.Instance.tk2dMainCam.ScreenCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("InteriorTiles"));
						CamManager.Instance.tk2dMainCam.ScreenCamera.cullingMask |= 1 << LayerMask.NameToLayer("tiles");

						//change back player layer to collide with exterior tiles
						PlayerManager.Instance.player.layer = 12;
                	}

                	if(roomManager.startRoom == roomManager.currentRoom){
                		GUIManager.Instance.LeaveTownHUDSpawns();
                	}

                    roomManager.currentRoom = negativeRoom;
                    roomManager.previousRoom = positiveRoom;
                    player = collider.gameObject;
                    playerCollider2d = player.GetComponent<Collider2D>();
                    CamManager.Instance.mainCam.enabled = false; //disable camera following for transition. Enabled uner RoomManager: SetCamBOunds
                    roomManager.previousCameraPosition = CamManager.Instance.mainCam.transform.position;
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
					if(areaTitleActivator && areaManager !=null){
						areaManager.myHUD.gameObject.SetActive(true);
                		GUIManager.Instance.areaTitle.SetActive(true);
                		GUIManager.Instance.areaTitle.GetComponent<SpriteRenderer>().sprite = areaTitleSprite;
                		GUIManager.Instance.areaTitle.GetComponent<Animator>().Play("areaTitleAni",0,-1f);
                	}
                	if(camZoomer){
                		CamManager.Instance.mainCamEffects.ZoomInOut(newCamZoomVal,1f);
                	}
					if(interiorRoomPortal){
                		//turn on the interior layer culling mask on the camera
                		CamManager.Instance.tk2dMainCam.ScreenCamera.cullingMask |= 1 << LayerMask.NameToLayer("InteriorTiles");
                		//turn off regular tiles from camera view
						CamManager.Instance.tk2dMainCam.ScreenCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("tiles"));

						//change player's layer so doesn't collide with exterior, invisible tiles
						PlayerManager.Instance.player.layer = 26;

                	}

					if(roomManager.startRoom == roomManager.currentRoom){
                		GUIManager.Instance.LeaveTownHUDSpawns();
                	}
					
                    roomManager.currentRoom = positiveRoom;
                    roomManager.previousRoom = negativeRoom;
                    player = collider.gameObject;
                    playerCollider2d = player.GetComponent<Collider2D>();
                    CamManager.Instance.mainCam.enabled = false; //disable camera following for transition. Enabled uner RoomManager: SetCamBOunds
                    roomManager.previousCameraPosition = CamManager.Instance.mainCam.transform.position;
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

            roomManager.previousRoom.DeactivateRoom();
        }
    }
}
