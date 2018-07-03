using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetParent : MonoBehaviour {

	public string parent;
	// Use this for initialization
	void Start () {
		gameObject.transform.parent = GameObject.Find(parent).transform;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
