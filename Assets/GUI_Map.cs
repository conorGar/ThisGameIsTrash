using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUI_Map : MonoBehaviour {
	public GameObject playerIcon;
    public GameObject trashIconPrefab;
    public List<GameObject> trashIcons;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnEnable(){
        // Set current player position in world.
        playerIcon.transform.position = RoomManager.Instance.currentRoom.miniMapPosition.position;

        // Loop through the rooms and display a trash icon for any room that has trash.  TODO: maybe even add the number of trash to pick up?
        if(GlobalVariableManager.Instance.IsPinEquipped(PIN.TREASURETRACKER)){
	        for (int i = 0; i < RoomManager.Instance.rooms.Count; i++) {
	            Room room = RoomManager.Instance.rooms[i];

	            if (room.HasTrash()) {
	                if (room.miniMapPosition != null) { // TODO: some rooms don't have miniMapPositions defined yet but they need too for this to work.
	                    GameObject go = ObjectPool.Instance.GetPooledObject(trashIconPrefab.tag, room.miniMapPosition.position);
	                    go.transform.Translate(0f, -.5f, 0f);
	                    go.SetActive(true);
	                    trashIcons.Add(go);
	                }
	            }
	        }
        }
	}

    void OnDisable()
    {
        // Return trash icons to the pool!
        for (int i = 0; i < trashIcons.Count; i++) {
            ObjectPool.Instance.ReturnPooledObject(trashIcons[i]);
        }

        trashIcons.Clear();
    }
}
