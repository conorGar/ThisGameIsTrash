using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class WanderOnPath : MonoBehaviour
{
	public float walkSpeed = 4;
	public float stopTime = 0f;

	int currentMark;
	GameObject destinationMark;
	bool moving;
	bool returningToStart;
	List<GameObject> pathMarks = new List<GameObject>();
	Vector3 startingScale;
	protected tk2dSpriteAnimator anim;

	//TODO: Switch direction facing based on direction of next mark


	// Use this for initialization
	void Start ()
	{
		startingScale = gameObject.transform.localScale;
	}

	void OnEnable(){
		anim = GetComponent<tk2dSpriteAnimator>();
	}

	public void SetPathMarks(List<GameObject> marks){ //Given by Room.cs from enemySpawner's 'PathingMarks.cs'
		for(int i = 0; i < marks.Count;i++){
			pathMarks.Add(marks[i]);
		}
		StartCoroutine("NextMark");

	}


	void Update ()
	{
		if(moving){
			if(Vector2.Distance(gameObject.transform.position,destinationMark.transform.position) > 1){
				gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position,destinationMark.transform.position,walkSpeed*Time.deltaTime);
			}else{
				StartCoroutine("NextMark");
				moving = false;
			}
		}else if(returningToStart){ //walk back to original position when lose sight of player
			Debug.Log("returning to start");
			if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
				if (!anim.IsPlaying("hit")) {
					Debug.Log("returning to start - 2" + pathMarks[0].transform.position);

					gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position,pathMarks[0].transform.position,4f*Time.deltaTime);


					//face direction of movement
					if(gameObject.transform.position.x < pathMarks[0].transform.position.x){
						gameObject.transform.localScale = new Vector3(Mathf.Abs(gameObject.transform.localScale.x),startingScale.y,startingScale.z);

					}else{
						gameObject.transform.localScale = new Vector3(startingScale.x*-1,startingScale.y,startingScale.z);

					}

					if(Vector2.Distance(gameObject.transform.position, pathMarks[0].transform.position) < 1f){
		            	//return to normal behavior
		            	Debug.Log("back in start bounds");
		  
		            	returningToStart =false;
		            	moving = true;
		            }
	            }
            }



		}
	}


	IEnumerator NextMark(){

		yield return new WaitForSeconds(stopTime);
		if(currentMark < (pathMarks.Count-1)){
			currentMark++;
		}else{
			currentMark = 0;
		}
		destinationMark = pathMarks[currentMark];
		moving = true;
	}

	public void StopMoving(){
		Debug.Log("Wander on Path - StopMoving() activated");
		StopAllCoroutines();
		moving = false;
		returningToStart = false;
	}

	public void ReturnToStart(){
		returningToStart = true;

		Debug.Log("Wander on Path - ReturnToStart() activated");
	}
}

