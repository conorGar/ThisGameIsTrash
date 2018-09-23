using UnityEngine;
using System.Collections;

public class OpenMap : MonoBehaviour
{

	public GameObject mapDisplay;
	
	// Update is called once per frame
	void Update ()
	{
		if(Input.GetKeyDown(KeyCode.M)){
			ShowMap();
		}else if(Input.GetKeyUp(KeyCode.M)){
			mapDisplay.SetActive(false);
		}
	}

	void ShowMap(){
		mapDisplay.SetActive(true);
	}
}

