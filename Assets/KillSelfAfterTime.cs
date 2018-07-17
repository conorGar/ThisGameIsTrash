using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillSelfAfterTime : MonoBehaviour {
	public float timeUntilDeath;
	public bool deactivateInsteadOfKill;
	// Use this for initialization
	void Start () {
		Invoke("Kill",timeUntilDeath);
	}
	
	void Kill(){
		if(deactivateInsteadOfKill){
			this.gameObject.SetActive(false);
		}else{
			Destroy(gameObject);
		}
	}
}
