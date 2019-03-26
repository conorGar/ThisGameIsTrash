using UnityEngine;
using System.Collections;

public class Ev_Enemy_SpikeBlock : MonoBehaviour
{

	public float distanceUntilMove;
	public float movementSpeed = 5;
	public Transform startPoint;
	public Transform endPoint;

	GameObject player;
	bool returning;
	bool moving;
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(moving){
			if(Vector2.Distance(gameObject.transform.position, endPoint.position) > 1f){
				Vector2.MoveTowards(gameObject.transform.position,endPoint.position, movementSpeed*Time.deltaTime);
			}else{
				moving = false;
				returning = true;
			}
		}else if(returning){
			if(Vector2.Distance(gameObject.transform.position, startPoint.position) > 1f){
				Vector2.MoveTowards(gameObject.transform.position,endPoint.position, .3f*Time.deltaTime);
			}else{
				returning = false;
			}
		}else if(Vector2.Distance(player.transform.position,gameObject.transform.position) < distanceUntilMove){
			TriggerMovement();
		}
	}

	void TriggerMovement(){
		Debug.Log("Spike block movement triggered");
		moving = true;
	}
}

