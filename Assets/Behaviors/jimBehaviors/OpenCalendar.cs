﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCalendar : MonoBehaviour {

	public GameObject calendar;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.C)){
			calendar.SetActive(true);
			Time.timeScale = 0;
			this.enabled = false;

		}
	}
}