using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceAroundScreen : MonoBehaviour {

	public float movementSpeed = 0;
	public float xBoundsMax = 15;
	public float yBoundsMax = 8;


	private Vector3 direction;

	// Use this for initialization
	void Start () {
		direction = (new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0.0f)).normalized;

	}
	
	// Update is called once per frame
	void Update () {
		transform.position += direction*movementSpeed*Time.deltaTime;

		if(transform.position.x < (xBoundsMax*-1)|| transform.position.x > xBoundsMax ){
			//Debug.Log("changeDirection X");
			direction.x = direction.x*-1;
			if(transform.position.x < (xBoundsMax*-1))
				transform.localScale = new Vector3(-1,1,1);
			else if(transform.position.x > xBoundsMax )
				transform.localScale = new Vector3(1,1,1);
		}

		if(transform.position.y < (yBoundsMax*-1)|| transform.position.y > yBoundsMax){
			//Debug.Log("changeDirection Y");
			direction.y = direction.y*-1;
		}
	}
}
