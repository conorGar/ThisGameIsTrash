using UnityEngine;
using System.Collections;

public class CamZoomer : MonoBehaviour
{
	public float zoomValFromLeft;
	public float zoomValFromRight;

	void OnTriggerEnter2D(Collider2D collider){
		if(collider.tag == "Player"){
			if(collider.transform.position.x < gameObject.transform.position.x){
					CamManager.Instance.mainCamEffects.ZoomInOut(zoomValFromRight,1f);
			}else{
					CamManager.Instance.mainCamEffects.ZoomInOut(zoomValFromLeft,1f);
			}
		}
	}
}

