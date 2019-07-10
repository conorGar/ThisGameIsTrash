using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceAroundScreen : MonoBehaviour {

	public float movementSpeed = 0;
	//public float xBoundsMax = 15;
	//public float yBoundsMax = 8;
	public BoxCollider2D bounceBounds;

	private Vector3 direction;
	private Vector3 startScale;
	protected EnemyStateController controller;


	void Awake(){
		startScale = gameObject.transform.localScale;
		controller = GetComponent<EnemyStateController>();

		//set bounds relative to current position
		//xBoundsMax = gameObject.transform.position.x + xBoundsMax;
		//yBoundsMax = gameObject.transform.position.y + yBoundsMax;
	}

	// Use this for initialization
	void Start () {
		direction = (new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0.0f)).normalized;

	}

	// Update is called once per frame
	void Update () {
		if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
            if (controller.GetCurrentState() == EnemyState.IDLE) {
				transform.position += direction*movementSpeed*Time.deltaTime;

				if(transform.position.x < (bounceBounds.bounds.min.x)|| transform.position.x > bounceBounds.bounds.max.x ){
					Debug.Log("changeDirection X");
					direction.x = direction.x*-1;
					if(transform.position.x < (bounceBounds.bounds.min.x))
						transform.localScale = new Vector3(startScale.x*-1,startScale.y,startScale.z);
					else if(transform.position.x > bounceBounds.bounds.max.x )
						transform.localScale = startScale;
				}

				if(transform.position.y < (bounceBounds.bounds.min.y)|| transform.position.y > bounceBounds.bounds.max.y){
					Debug.Log("changeDirection Y");
					direction.y = direction.y*-1;
				}
			}
		}
	}
}
