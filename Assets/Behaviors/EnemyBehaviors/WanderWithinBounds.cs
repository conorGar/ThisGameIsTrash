using UnityEngine;
using System.Collections;

public class WanderWithinBounds : RandomDirectionMovement
{
	public BoxCollider2D myWanderZone; // for when lose sight of player, maybe just have enemies walk back to their location...?

	protected float MIN_X;
	protected float MIN_Y;
	protected float MAX_X;
	protected float MAX_Y;
	Vector2 startPosition;
	public bool boundsSet;
	bool returningToStart;
	// Use this for initialization


	void OnEnable ()
	{
		Debug.Log("Wander in bounds OnEnable() activate -q-q-q-q-q-q-q-q-q");
		//boundsSet = false;

		Debug.Log("wander start position:" + startPosition);
		base.OnEnable();
		Debug.Log("wander start position:" + startPosition);
		Invoke("StartPosDelay",.2f);
		//startPosition = gameObject.transform.position;
		//SetWalkBounds();
	}

	void StartPosDelay(){
		//** was having issues with armored spear mole setting its start position to the prefab's default position
		//** that I could not figure out. This fixed the issue.
		startPosition = gameObject.transform.position;

	}

	// Update is called once per frame
	void Update ()
	{
		//Debug.Log(startPosition);

		if(boundsSet && controller.IsFlag((int)EnemyFlag.MOVING)){
			if(transform.position.x < MIN_X || transform.position.x > MAX_X ||
            	transform.position.y < MIN_Y || transform.position.y > MAX_Y){

				transform.position = new Vector3(
								    Mathf.Clamp(gameObject.transform.position.x, MIN_X, MAX_X),
								    Mathf.Clamp(gameObject.transform.position.y, MIN_Y, MAX_Y),
								    0f);
				TurnToNewDirection();

			}
			base.Update();
		}else if(returningToStart && controller.IsFlag((int)EnemyFlag.MOVING)){ //walk back to original position when lose sight of player
			Debug.Log("returning to start");
			if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
				if (!anim.IsPlaying("hit")) {
					Debug.Log("returning to start - 2" + startPosition);

					gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position,startPosition,4f*Time.deltaTime);


					//face direction of movement
					if(gameObject.transform.position.x < startPosition.x){
						gameObject.transform.localScale = new Vector3(Mathf.Abs(gameObject.transform.localScale.x),startingScale.y,startingScale.z);

					}else{
						gameObject.transform.localScale = new Vector3(startingScale.x*-1,startingScale.y,startingScale.z);

					}

					if(transform.position.x > MIN_X && transform.position.x < MAX_X &&
		            transform.position.y > MIN_Y && transform.position.y < MAX_Y){
		            	//return to normal behavior
		            	Debug.Log("back in start bounds");
		  
		            	returningToStart =false;
		            	boundsSet = true;
						StartMoving();
		            }
	            }
            }



		}


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
		Debug.Log("Set New Bounds activate");
		MIN_X = myWanderZone.bounds.min.x;
        MAX_X = myWanderZone.bounds.max.x;
        MIN_Y = myWanderZone.bounds.min.y;
        MAX_Y = myWanderZone.bounds.max.y;
	}

	public void ReturnToStart(){
		Debug.Log("returning to start - activate" + startPosition);

		boundsSet = false;
        controller.SetFlag((int)EnemyFlag.WALKING);
		returningToStart = true;
		StopAllCoroutines();
	}

	public override void StartMoving(){
		if(!returningToStart){
			base.StartMoving();
		}
	}

	public override void StopMoving(){
		base.StopMoving();
		boundsSet = false;
	}



}

