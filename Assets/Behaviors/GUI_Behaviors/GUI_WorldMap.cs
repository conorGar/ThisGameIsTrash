using UnityEngine;
using System.Collections;

public class GUI_WorldMap : MonoBehaviour
{
	public float maxZoom = -30f;
	public float minZoom = -3f;
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

		miniMapCam.gameObject.transform.position = new Vector3(PlayerManager.Instance.player.transform.position.x,
                                                               PlayerManager.Instance.player.transform.position.y,
                                                               currentZoom);
	}
}

