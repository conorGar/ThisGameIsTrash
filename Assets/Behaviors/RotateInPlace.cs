using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateInPlace : MonoBehaviour {


	public float degree = 45f;

	bool rotateRight = false;
	int doOnce = 0;

	protected float m_frequency = 1f;
	// Use this for initialization
	void Start () {
		//Vector3 m_from = new Vector3(0f,0f,degree);
	 	//Vector3 m_to = new Vector3(0f,0f,(degree*-1));
	}
	
	// Update is called once per frame
	void Update () {
		Quaternion from = Quaternion.Euler(new Vector3(0f,0f,degree));
		Quaternion to = Quaternion.Euler(new Vector3(0f,0f,(degree*-1)));

		float lerp = 0.5f * (1f + Mathf.Sin(Mathf.PI * Time.realtimeSinceStartup * this.m_frequency));
		this.transform.localRotation = Quaternion.Lerp(from,to,lerp);
	}
}
