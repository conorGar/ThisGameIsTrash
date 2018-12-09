using UnityEngine;
using System.Collections;

public class WanderWithinBounds : RandomDirectionMovement
{
	public BoxCollider2D myWanderZone; // for when lose sight of player, maybe just have enemies walk back to their location...?

	float MIN_X;
	float MIN_Y;
	float MAX_X;
	float MAX_Y;
	bool boundsSet;
	// Use this for initialization
	void OnEnable ()
	{
		boundsSet = false;
		base.OnEnable();
		//SetWalkBounds();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(boundsSet){
			if(transform.position.x < MIN_X || transform.position.x > MAX_X ||
            transform.position.y < MIN_Y || transform.position.y > MAX_Y){
				transform.position = new Vector3(
								    Mathf.Clamp(gameObject.transform.position.x, MIN_X, MAX_X),
								    Mathf.Clamp(gameObject.transform.position.y, MIN_Y, MAX_Y),
								    0f);
			}
		}
		base.Update();

	}


	public void SetWalkBounds(Rect wanderZone){
		
        MIN_X = wanderZone.xMin;
        MAX_X = wanderZone.xMax;
        MIN_Y = wanderZone.yMin;
        MAX_Y = wanderZone.yMax;
        Debug.Log("MAX X:" + MAX_X + "MAX_Y:" + MAX_Y + "MIN_X:" + MIN_X);
        boundsSet = true;
       
	}

	public void SetNewBounds(){
		MIN_X = myWanderZone.bounds.min.x;
        MAX_X = myWanderZone.bounds.max.x;
        MIN_Y = myWanderZone.bounds.min.y;
        MAX_Y = myWanderZone.bounds.max.y;
	}

}

