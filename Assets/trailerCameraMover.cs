using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trailerCameraMover : MonoBehaviour {

	
	
	void Start(){
		gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(1,0);

	}
}
