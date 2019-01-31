using UnityEngine;
using System.Collections;

public class OpenMap : MonoBehaviour
{

	public GameObject mapDisplay;
	public GameObject openMapPrompt;
	public GameObject miniMapCam;

	// Update is called once per frame
	void Update ()
	{
		if(GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)){
			if(ControllerManager.Instance.GetKeyDown(INPUTACTION.MAP)){
				ShowMap();
			}
		}else if(GameStateManager.Instance.GetCurrentState() == typeof(PopupState)){
			if(ControllerManager.Instance.GetKeyDown(INPUTACTION.MAP)){
				if(openMapPrompt != null && openMapPrompt.activeInHierarchy)
					openMapPrompt.SetActive(false);
				mapDisplay.SetActive(false);
				miniMapCam.SetActive(false);
				GameStateManager.Instance.PopState();
			}
		}
	}

	void ShowMap(){
		miniMapCam.SetActive(true);
		mapDisplay.SetActive(true);
		GameStateManager.Instance.PushState(typeof(PopupState));
	}
}

