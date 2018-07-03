using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waitToRunBehavior : MonoBehaviour {

	public float waitTime = 0;
	//public string scriptName = "";

	// Use this for initialization
	void Start () {
		this.GetComponent<FollowPlayer>().enabled = false;
		Invoke("Activate",waitTime);
	}

	void Activate(){
		this.GetComponent<FollowPlayer>().enabled = true;
	}


}
