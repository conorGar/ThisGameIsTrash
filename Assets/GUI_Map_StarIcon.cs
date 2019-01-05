using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUI_Map_StarIcon : MonoBehaviour {

	//public string myLargeTrashName;
	//public LargeGarbage myGarbage = new LargeGarbage();
	public LARGEGARBAGE thisGarbage;
	public GameObject starParent;
	//public int worldChildIndex; // which world categorizer under LargeTrashManager, 0 = w1, 1 = w2, etc...
	//public GUI_Map map;

	// Use this for initialization
	void OnEnable () {

		Debug.Log("Map Star OnEnable() activate");
		for(int i = 0; i < starParent.transform.childCount; i++){
			if ((GlobalVariableManager.Instance.LARGE_GARBAGE_DISCOVERED & thisGarbage) == thisGarbage|| (GlobalVariableManager.Instance.LARGE_GARBAGE_VIEWED & thisGarbage) == thisGarbage) {
            			Destroy(this.gameObject);
      
			}
		}

		//TODO: update star position based on location of the large trash

			/*if (myGarbage.myCurrentRoom.miniMapPosition != null) {
                gameObject.transform.position = myLargeTrash.myCurrentRoom.miniMapPosition.position;
            }
            else {
                gameObject.transform.position = Vector3.zero;
            }*/
		
		
	}

	// Update is called once per frame
	void Update () {
		
	}

	public void SetNewStarPosition(){
		//TODO: update star position based on location of the large trash

	}
}
