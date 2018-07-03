using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttatchCamera : MonoBehaviour {


			//As of right now, this script is unneccessary and doesn't do anything	
	GameObject mainCamera;

	void Start () {
		mainCamera = GameObject.Find("tk2dCamera");
	}
	
	void Update () {
		//mainCamera.transform.position = new Vector3(transform.position.x - 11.75f,transform.position.y-9f,-10f);
		/*mainCamera.transform.position = new Vector3(
							Mathf.Clamp(transform.position.x, -13.48f, -11.48f),
							Mathf.Clamp(transform.position.y, -10.08f, -9.08f),
							-10f);*/
	}
}
