using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttatchSelfToObject : MonoBehaviour {

	public GameObject objectToSnapTo;
	public float yAdjustment = 0;
	public float xAdjustment = 0;

	// Use this for initialization
	void Start () {
		gameObject.transform.parent = objectToSnapTo.transform;



	}
	// Update is called once per frame
	void Update () {
			if(yAdjustment == 0){
			gameObject.transform.localPosition = Vector3.zero;
			}else
			gameObject.transform.localPosition = new Vector2(xAdjustment, yAdjustment);
	}
}
