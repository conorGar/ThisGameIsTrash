using UnityEngine;
using System.Collections;

public class GUI_WorldMap : MonoBehaviour
{
	public float maxZoom = -100f;
	public float minZoom = -7f;
	public Camera miniMapCam;

	float currentZoom = -11f;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(ControllerManager.Instance.GetKey(INPUTACTION.MOVEDOWN)){
			if(currentZoom > maxZoom){
				currentZoom -=.2f;
			}
		}else if(ControllerManager.Instance.GetKey(INPUTACTION.MOVEUP)){
			if(currentZoom < minZoom){
				currentZoom += .2f;
			}
		}

		miniMapCam.transform.localPosition = new Vector3(0,0,currentZoom);
	}
}

