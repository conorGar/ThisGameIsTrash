using UnityEngine;
using System.Collections;

public class OpenMap : MonoBehaviour
{

	public GameObject mapDisplay;
	public GameObject openMapPrompt;
	// Update is called once per frame
	void Update ()
	{
		if(GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)){
			if(ControllerManager.Instance.GetKeyDown(INPUTACTION.MAP)){
				ShowMap();
			}else if(ControllerManager.Instance.GetKeyUp(INPUTACTION.MAP)){
				if(openMapPrompt != null && openMapPrompt.activeInHierarchy)
					openMapPrompt.SetActive(false);
				mapDisplay.SetActive(false);
			}
		}
	}

	void ShowMap(){
		
		mapDisplay.SetActive(true);
	}
}

