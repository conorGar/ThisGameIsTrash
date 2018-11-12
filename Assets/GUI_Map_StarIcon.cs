using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUI_Map_StarIcon : MonoBehaviour {

	public string myLargeTrashName;
	public int worldChildIndex; // which world categorizer under LargeTrashManager, 0 = w1, 1 = w2, etc...
	public GUI_Map map;
	Ev_LargeTrash myLargeTrash;
	// Use this for initialization
	void Start () {
		GameObject currentWorldLargeTrash =LargeTrashManager.Instance.transform.GetChild(worldChildIndex).gameObject;
		for(int i = 0; i <currentWorldLargeTrash.transform.childCount; i++){
			Debug.Log(currentWorldLargeTrash.transform.GetChild(i).name + "  " + myLargeTrashName);
			if(currentWorldLargeTrash.transform.GetChild(i).name == myLargeTrashName){
				myLargeTrash = currentWorldLargeTrash.transform.GetChild(i).GetComponent<Ev_LargeTrash>();
				break;
			}
		}
		if(myLargeTrash == null){//if cant find large trash in the hiearchy, then destroy because player got trash already
			Debug.Log("could not find star icon large trash and was destroyed");
			Destroy(gameObject);
		}else{
		OnEnable();
		}
		
	}

	void OnEnable(){
		if(myLargeTrash != null){
            // Set the star to the minimap position if it has one (some rooms don't right now).
            if (myLargeTrash.myCurrentRoom.miniMapPosition != null) {
                gameObject.transform.position = myLargeTrash.myCurrentRoom.miniMapPosition.position;
            }
            else {
                gameObject.transform.position = Vector3.zero;
            }
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
