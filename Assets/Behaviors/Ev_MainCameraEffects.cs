﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ev_MainCameraEffects : MonoBehaviour {


	//Still Need:
	//-Black Bars
	//Tut popup for armored enemy


	public GameObject player;
	public GameObject tutPopup;
	public GameObject roomManager;//needed for proper camera panning
	public AudioClip zoomSound;
	public AudioClip panWoosh;
	[HideInInspector]
	public GameObject objectToSpawn;
	tk2dCamera thisCam;

	Vector3 targetPanPosition;
	bool camPan;
	bool continuousPanning;
	GameObject objectToFollow;
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
			if(continuousPanning){
				gameObject.transform.position = Vector3.Lerp(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y,-10f),objectToFollow.transform.position, 3*(Time.deltaTime));

			}else{
				gameObject.transform.position = Vector3.Lerp(gameObject.transform.position,targetPanPosition, 3*(Time.deltaTime));
				if(Vector2.Distance(gameObject.transform.position,targetPanPosition) < 1){
					Debug.Log("In target position!!" + triggerEventName);
					if(triggerEventName != null && triggerEventName.Length > 2){ //length check to make sure it's not just "" or something
						CamPanTriggerEvent();
					}
					camPan = false;
				}
			}
		}
	}

	public void ZoomInOut(float zoomAmount, float zs){ 
   		Debug.Log("Camera zoom in out activate ##############");
   		SoundManager.instance.PlaySingle(zoomSound);
   		currentCamZoom = thisCam.ZoomFactor;
   		targetCamZoom = zoomAmount;
   		zoomSpeed = zs;
   		zooming = true;
   		//thisCam.ZoomFactor = 1.5f; //Debug
   }

	public void CameraPan(Vector3 positionToPanTo,string triggerName){
		SoundManager.instance.PlaySingle(panWoosh);
   		if(roomManager != null){
   			roomManager.SetActive(false);
   		}
		triggerEventName = triggerName;
  		targetPanPosition = new Vector3(positionToPanTo.x,positionToPanTo.y,-10);
   		camPan= true;
   }
   public void CameraPan(GameObject objectToFollow, bool continuous){
		SoundManager.instance.PlaySingle(panWoosh);

   		if(roomManager != null){
   			roomManager.SetActive(false);
   		}
		continuousPanning = continuous;
		this.objectToFollow = objectToFollow;
   		camPan= true;
   }


   void CamPanTriggerEvent(){ //TODO: adjust for different events besaides those needed for large trash tut popup
   		ZoomInOut(1.5f,10f);
  		//yield return new WaitForSeconds(.5f);
  		Debug.Log("CAM TIGGER EVENT ACTIVATED");
  		if(triggerEventName == "tutorial"){
			tutPopup.SetActive(true);
			tutPopup.GetComponent<GUI_TutPopup>().SetData("LargeTrash");
		}else if(triggerEventName == "tutorial_pins"){
			tutPopup.SetActive(true);
			tutPopup.GetComponent<GUI_TutPopup>().SetData("Pins");
		}else if(triggerEventName == "tutorial_armored"){
			tutPopup.SetActive(true);
			tutPopup.GetComponent<GUI_TutPopup>().SetData("ArmoredEnemies");
		}else if(triggerEventName == "BossItem"){
			StartCoroutine("SpawnBossReward");

		}
   }

   public void ReturnFromCamEffect(){
  	 	Debug.Log("return from cam effect");
	   	zooming = false;
	   	camPan = false;
	   	continuousPanning = false;
	   	thisCam.ZoomFactor = 1.15f;
	   	//gameObject.transform.position = new Vector3(player.transform.position.x, player.transform.position.y,-10f);

	   	if(roomManager != null){
	   		roomManager.SetActive(true);
	   	}
   }

   IEnumerator SpawnBossReward(){
   		ObjectPool.Instance.GetPooledObject("effect_SmokePuff",targetPanPosition);
   		objectToSpawn.SetActive(true);
   		yield return new WaitForSeconds(.5f);
		ReturnFromCamEffect();
   }

}
