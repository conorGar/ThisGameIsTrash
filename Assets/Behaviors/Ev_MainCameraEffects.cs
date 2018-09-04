using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ev_MainCameraEffects : MonoBehaviour {


	//Still Need:
	//-Black Bars
	//Tut popup for armored enemy


	public GameObject player;
	public GameObject tutPopup;
	public GameObject roomManager;//needed for proper camera panning

	[HideInInspector]
	public GameObject objectToSpawn;
	tk2dCamera thisCam;

	Vector3 targetPanPosition;
	bool camPan;
	string triggerEventName;
	bool zooming;
	float currentCamZoom;
	float targetCamZoom;
	float zoomSpeed;

	void Start () {
		thisCam = gameObject.GetComponent<tk2dCamera>();

	}
	
	// Update is called once per frame
	void Update () {
		if(zooming){
				
			thisCam.ZoomFactor = Mathf.Lerp(thisCam.ZoomFactor,targetCamZoom,zoomSpeed*(Time.deltaTime));
				//thisCam.ZoomFactor = 1;
				//Debug.Log("zooming" + currentCamZoom + "   " + targetCamZoom);
				if(thisCam.ZoomFactor == targetCamZoom){
					zooming = false;
					Debug.Log("Zoom stopped");
				}
		}
		if(camPan){
			gameObject.transform.position = Vector3.Lerp(gameObject.transform.position,targetPanPosition, 3*(Time.deltaTime));
			Debug.Log("camPan");
			if(Vector2.Distance(gameObject.transform.position,targetPanPosition) < 1){
				Debug.Log("In target position!!" + triggerEventName);
				if(triggerEventName != null && triggerEventName.Length > 2){ //length check to make sure it's not just "" or something
					CamPanTriggerEvent();
				}
				camPan = false;
			}
		}
	}

	public void ZoomInOut(float zoomAmount, float zs){ 
   		Debug.Log("Camera zoom in out activate ##############");
   		currentCamZoom = thisCam.ZoomFactor;
   		targetCamZoom = zoomAmount;
   		zoomSpeed = zs;
   		zooming = true;
   		//thisCam.ZoomFactor = 1.5f; //Debug
   }

	public void CameraPan(Vector3 positionToPanTo,string triggerName){
   		gameObject.GetComponent<Ev_MainCamera>().enabled = false;
   		if(roomManager != null){
   			roomManager.SetActive(false);
   		}
		triggerEventName = triggerName;
  		targetPanPosition = new Vector3(positionToPanTo.x,positionToPanTo.y,-10);
   		camPan= true;
   }

   void CamPanTriggerEvent(){ //TODO: adjust for different events besaides those needed for large trash tut popup
   		ZoomInOut(1.5f,10f);
  		//yield return new WaitForSeconds(.5f);
  		Debug.Log("CAM TIGGER EVENT ACTIVATED");
  		if(triggerEventName == "tutorial"){
			tutPopup.SetActive(true);
			tutPopup.GetComponent<GUI_TutPopup>().SetData("LargeTrash");
		}else if(triggerEventName == "BossItem"){
			StartCoroutine("SpawnBossReward");

		}
   }

   public void ReturnFromCamEffect(){
	   	zooming = false;
	   	camPan = false;
	   	thisCam.ZoomFactor = 1.15f;
	   	gameObject.transform.position = new Vector2(player.transform.position.x, player.transform.position.y);
	   	gameObject.GetComponent<Ev_MainCamera>().enabled = true;
	   	if(roomManager != null){
	   		roomManager.SetActive(true);
	   	}
   }

   IEnumerator SpawnBossReward(){
   		ObjectPool.Instance.GetPooledObject("effect_SmokePuff",targetPanPosition);
   		objectToSpawn.SetActive(true);
   		yield return new WaitForSeconds(.5f);
		ReturnFromCamEffect();
		player.GetComponent<EightWayMovement>().enabled = true;
		player.GetComponent<PlayerTakeDamage>().enabled = true;
   }

}
