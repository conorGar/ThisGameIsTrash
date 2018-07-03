using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ev_DisplayTrash : MonoBehaviour {

	GameObject myDisplay;

	// Use this for initialization
	void Start () {
		myDisplay = GameObject.Find("new discovery display");
		//RotateInPlace.enabled = true;
		gameObject.GetComponent<RotateInPlace>().enabled = true;

	}
	
	// Update is called once per frame
	void Update () {
		
	}


}
