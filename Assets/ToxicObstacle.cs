using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicObstacle : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if(GlobalVariableManager.Instance.IsPinEquipped(PIN.IRRADIATED)){
			gameObject.layer = 8; //turns into tile layer if have radiation pin
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
