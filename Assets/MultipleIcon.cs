﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleIcon : MonoBehaviour {


	//public ActivateDialogWhenClose myFriend;
	public string myIconAniTrigger;

	// positionOnScreen should be changed by the specific friend event script's method that activates the new dialog.
	public int positionOnScreen; //0 = left, 1 = middle, 2 = right


	// Use this for initialization

	//mnEnable() here that postions icon properly based on 'positionOnScreen' value
	void OnEnable(){
		if(positionOnScreen == 0){
			gameObject.transform.localPosition = new Vector2(-48f,6.5f);
		}else if(positionOnScreen == 1){
			gameObject.transform.localPosition = new Vector2(0f,13f);
		}else if(positionOnScreen == 2){
			gameObject.transform.localPosition = new Vector2(39f,6.5f);
		}
		//gameObject.GetComponent<GUIEffects>().
	}


	// Update is called once per frame
	void Update () {
		
	}
}