using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LargeTrashManager : MonoBehaviour
{

	//all large trash is placed in an empty game object "holder" that itself has 4 seperate "world holders". This holder is saved across
	//scenes. When large trash is returned, it is removed from this holder.

	//everything should be enabled/activated by WorldManager.cs and Ev_Results.cs(deactivates)


	public List<GameObject> worlds = new List<GameObject>();//holds the 4 sub parents for the four worlds
	public static LargeTrashManager Instance;

	void Awake(){

		Instance = this;
	}
	public void EnableProperTrash(int currentWorld){
        // activated by WorldManager

		worlds[currentWorld -1].SetActive(true);

        // TODO: this function can only return active objects.  It skips over anything script that's disabled.
        var largeTrashList = worlds[currentWorld - 1].gameObject.GetComponentsInChildren<Ev_LargeTrash>();
        for (int i = 0; i < largeTrashList.Length; i++) {
            // Do not activate large trash that has already been discovered.
            if (GlobalVariableManager.Instance.IsLargeTrashDiscovered(largeTrashList[i].garbage.type)) {
                largeTrashList[i].gameObject.SetActive(false);
            } else {
                largeTrashList[i].gameObject.SetActive(true);
            }
        }

    }

	public void DisableProperTrash(int currentWorld){ // activated by Ev_results
		
			worlds[currentWorld -1].SetActive(false);
	
	}

	public void EnableSpecificTrash(string trashName, int world){
		Debug.Log("Enable Specific Trash CALL");

		foreach (Transform child in worlds[world -1].transform){
			Debug.Log("Enable Specific Trash check" + child.name +" " + trashName);
			if(child.name == trashName){
				
				child.gameObject.SetActive(true);
				break;
			}
		}
	}
}

