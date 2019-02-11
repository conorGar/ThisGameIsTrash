using UnityEngine;
using System.Collections;

public class TrailerMasterControl : MonoBehaviour
{
	public tk2dCamera trailerCam;
	public GameObject jumboFocus;
	public GameObject chipFocus;
	public GameObject lemonFocus;
	public GameObject dialogCanvas;
	// Use this for initialization
	void Start ()
	{
		DialogManager.Instance.dialogTitle = "JumboTrailerStart";
		DialogManager.Instance.canContinueDialog = true;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(Input.GetKeyDown(KeyCode.J)){
			gameObject.GetComponent<TrailerCam>().enabled = false;
			gameObject.GetComponent<Ev_MainCameraEffects>().CameraPan(jumboFocus,true);
		}
		if(Input.GetKeyDown(KeyCode.C)){
			gameObject.GetComponent<TrailerCam>().enabled = false;

			gameObject.GetComponent<Ev_MainCameraEffects>().CameraPan(chipFocus,true);
		}
		if(Input.GetKeyDown(KeyCode.L)){
			gameObject.GetComponent<TrailerCam>().enabled = false;

			gameObject.GetComponent<Ev_MainCameraEffects>().CameraPan(lemonFocus,true);
		}
		if(Input.GetKeyDown(KeyCode.R)){
			//return to player
			gameObject.GetComponent<Ev_MainCameraEffects>().CameraPan(PlayerManager.Instance.player, true);
		}
		if(Input.GetKeyDown(KeyCode.F)){
			//follow player again
			gameObject.GetComponent<TrailerCam>().enabled = true;
		}
		if(Input.GetKeyDown(KeyCode.Space)){
			if(!dialogCanvas.activeInHierarchy)
				
				dialogCanvas.SetActive(true);

		}
		if(Input.GetKeyDown(KeyCode.Return)){

			dialogCanvas.SetActive(false);
		}
		if(Input.GetKeyDown(KeyCode.Alpha1)){
			//Slow Zoom in to 2.8
			gameObject.GetComponent<Ev_MainCameraEffects>().ZoomInOut(2.8f,.1f);
		}if(Input.GetKeyDown(KeyCode.Alpha2)){
			//Slow Zoom back to 1.15
			gameObject.GetComponent<Ev_MainCameraEffects>().ZoomInOut(1.15f,1f);
		}if(Input.GetKeyDown(KeyCode.Alpha3)){
			//Slow Zoom out to .65
			gameObject.GetComponent<Ev_MainCameraEffects>().ZoomInOut(.65f,1f);
		}
	}
}

