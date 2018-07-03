using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillSelfAfterTime : MonoBehaviour {
	public float timeUntilDeath;
	// Use this for initialization
	void Start () {
		Invoke("Kill",timeUntilDeath);
	}
	
	void Kill(){
		Destroy(gameObject);
	}
}
