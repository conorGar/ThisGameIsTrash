﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannotExitScene : MonoBehaviour {

	public float leftLimit;
	public float rightLimit;
	public float topLimit;
	public float botLimit;
	// Update is called once per frame
	void Update () {

		//use 'local position' because it's based on roomname gameobject, nbd if needs to be changed, but will have to go back
		if(transform.localPosition.x < leftLimit){
			transform.localPosition = new Vector2(leftLimit, transform.localPosition.y);
		} else if(transform.localPosition.x > rightLimit){
			transform.localPosition = new Vector2(rightLimit, transform.localPosition.y);
		}
		if(transform.localPosition.y < botLimit){
			transform.localPosition = new Vector2(transform.localPosition.x, botLimit);
		} else if(transform.localPosition.y > topLimit){
			transform.localPosition = new Vector2(transform.localPosition.x, topLimit);
		}
	}

	/*public void SetLimits(float lLimit, float rLimit, float tLimit, float bLimit){
		leftLimit = lLimit;
		rightLimit = rLimit;
		topLimit = tLimit;
		botLimit = bLimit;
	}*/ // just set by hand in scene editor
}