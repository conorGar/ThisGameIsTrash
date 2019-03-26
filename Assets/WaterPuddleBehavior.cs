using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPuddleBehavior : MonoBehaviour {


	
	void OnTriggerEnter2D(Collider2D collider){
		if(collider.tag == "Player"){
			collider.gameObject.GetComponent<EightWayMovement>().SlowdownSpeed();
		}
	}

	void OnTriggerExit2D(Collider2D collider){
		if(collider.tag == "Player"){
			collider.gameObject.GetComponent<EightWayMovement>().SpeedReturn();

		}
	}
}
