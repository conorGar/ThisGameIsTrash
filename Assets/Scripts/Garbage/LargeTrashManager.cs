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


	}

	public void DisableProperTrash(int currentWorld){ // activated by Ev_results
		
			worlds[currentWorld -1].SetActive(false);
	
	}

}

