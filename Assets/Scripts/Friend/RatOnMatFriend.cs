﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RatOnMatFriend : Friend
{
	public ParticleSystem vanishPS;
	public tk2dCamera mainCam;


	void OnEnable(){
		if(GlobalVariableManager.Instance.WORLD_NUM == 1 && nextDialog != "RatMat1"){
			Destroy(gameObject);//onlt happens once
		}else{
			base.OnEnable();
		}
	}

	public override void FinishDialogEvent(){
		Debug.Log("Ec finish dialog event activate");
		gameObject.GetComponent<ActivateDialogWhenClose>().enabled = false; // needed to fix glitch where if player spammed continue button dialog would start again
		StartCoroutine("ReturnCam");
	}

	IEnumerator ReturnCam(){
		yield return new WaitForSeconds(.3f);


		vanishPS.Play();

	
		yield return new WaitForSeconds(.5f);
		gameObject.GetComponent<MeshRenderer>().enabled =true;
		GlobalVariableManager.Instance.PLAYER_CAN_MOVE = true;
		mainCam.GetComponent<Ev_MainCameraEffects>().ReturnFromCamEffect();


		gameObject.SetActive(false);

	}
}

