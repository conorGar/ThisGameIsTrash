using UnityEngine;
using System.Collections;

public class WanderWithinBounds : RandomDirectionMovement
{
	public BoxCollider2D wanderZone;

	float MIN_X;
	float MIN_Y;
	float MAX_X;
	float MAX_Y;
	// Use this for initialization
	void OnEnable ()
	{
		SetWalkBounds();
		base.OnEnable();
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.position = new Vector3(
							    Mathf.Clamp(transform.position.x, MIN_X, MAX_X),
							    Mathf.Clamp(transform.position.y, MIN_Y, MAX_Y),
							    0f);
		base.Update();

	}


	public void SetWalkBounds(){

        MIN_X = wanderZone.bounds.min.x;
        MAX_X = wanderZone.bounds.max.x;
        MIN_Y = wanderZone.bounds.min.y;
        MAX_Y = wanderZone.bounds.max.y;

	}
}

