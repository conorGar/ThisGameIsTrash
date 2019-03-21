using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(EnemyStateController))]
public class WanderOnPath : MonoBehaviour
{
	public float walkSpeed = 4;
	public float stopTime = 0f;

	int currentMark;
	GameObject destinationMark;
	bool returningToStart;
	public List<GameObject> pathMarks = new List<GameObject>(); //public for debugging, otherwise should be given by enemy spawner
	Vector3 startingScale;
	protected tk2dSpriteAnimator anim;
	protected EnemyStateController controller;

	//TODO: Switch direction facing based on direction of next mark

	void Awake()
    {
        controller = GetComponent<EnemyStateController>();
    }
	// Use this for initialization
	void Start ()
	{
		startingScale = gameObject.transform.localScale;
	}

	void OnEnable(){
		anim = GetComponent<tk2dSpriteAnimator>();
		//StartCoroutine("NextMark"); //added for debug, remove if causing issues
	}

	public void SetPathMarks(List<GameObject> marks){ //Given by Room.cs from enemySpawner's 'PathingMarks.cs'
		for(int i = 0; i < marks.Count;i++){
			pathMarks.Add(marks[i]);
		}
		StartCoroutine("NextMark");

	}


	void Update ()
	{
		if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
			switch (controller.GetCurrentState()) {
				case EnemyState.IDLE:
                    if (controller.IsFlag((int)EnemyFlag.WALKING)) {
                    	if(returningToStart){

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
				            }

                    }else{
							if(Vector2.Distance(gameObject.transform.position,destinationMark.transform.position) > 1){
								gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position,destinationMark.transform.position,walkSpeed*Time.deltaTime);
							}else{
								StartCoroutine("NextMark");
							}
						}
               }
                    break;
            }
        }
	}


	IEnumerator NextMark(){
		controller.RemoveFlag((int)EnemyFlag.WALKING);

		yield return new WaitForSeconds(stopTime);
		if(currentMark < (pathMarks.Count-1)){
			currentMark++;
		}else{
			currentMark = 0;
		}
		controller.SetFlag((int)EnemyFlag.WALKING);

		destinationMark = pathMarks[currentMark];
	}

	public void StopMoving(){
		Debug.Log("Wander on Path - StopMoving() activated");
		StopAllCoroutines();
		controller.RemoveFlag((int)EnemyFlag.WALKING);

		returningToStart = false;
	}

	public void ReturnToStart(){
		returningToStart = true;
		controller.SetFlag((int)EnemyFlag.WALKING);

		Debug.Log("Wander on Path - ReturnToStart() activated");
	}
}

