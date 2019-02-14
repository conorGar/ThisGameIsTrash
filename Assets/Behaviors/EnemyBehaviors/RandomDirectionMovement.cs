using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GenericEnemyStateController = EnemyStateController<EnemyState, EnemyTrigger>;

[RequireComponent(typeof(GenericEnemyStateController))]
public class RandomDirectionMovement : MonoBehaviour {

	public float movementSpeed = 0;
	public float minMoveTime = 0;
	public float maxMoveTime = 0;
	public float stopTime = 2;
	//public GameObject walkCloud;
	public ParticleSystem walkPS;
	//public float walkCloudYadjust = 0.8f;


	private Vector3 direction;
	protected tk2dSpriteAnimator anim;
	int bounceOffObject;
	protected Vector3 startingScale;
	int turnOnce = 0;

    protected GenericEnemyStateController controller;

    // Use this for initialization
    void Awake()
    {
        controller = GetComponent<GenericEnemyStateController>();
    }

    void Start(){
		startingScale = gameObject.transform.localScale;

	}

	protected void OnEnable () {
        anim = GetComponent<tk2dSpriteAnimator>();
        StartMoving();
	}
	
	// Update is called once per frame
	protected void Update () {
        if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
            switch (controller.GetCurrentState()) {
                case EnemyState.IDLE:
                    if (controller.IsFlag((int)EnemyFlag.WALKING)) {
                        transform.position += direction * movementSpeed * Time.deltaTime;
                        if (direction.x > 0) {
                            if (gameObject.transform.localScale.x < 0) {
                                if (turnOnce == 0) {
                                    Turn();
                                }
                            }
                        } else {
                            if (gameObject.transform.localScale.x > 0) {
                                if (turnOnce == 0) {
                                    Turn();
                                }
                            }
                        }
                    }
                    break;
            }
        }
	}
	void Turn(){
		turnOnce = 1;
		gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x*-1,startingScale.y,startingScale.z);
	}

	public void TurnToNewDirection(){
		//activated by things like "Wander within bounds" when enemy reaches end of the bounds so it doesnt just keep walking into it.
		Debug.Log("Turn to new direction activated:" + gameObject.name);
		turnOnce = 1;
		gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x*-1,startingScale.y,startingScale.z);
		direction = (new Vector3(direction.x*-1, direction.y*-1, 0.0f)).normalized;

	}

    // Enemy will move in a direction for a random amount of time, idle, and go again.
	IEnumerator Moving(){
		turnOnce = 0;

		yield return new WaitForSeconds(Random.Range(minMoveTime,maxMoveTime));

        if (controller.IsFlag((int)EnemyFlag.WALKING)) {
		    bounceOffObject = 0;

		    if(walkPS !=null && walkPS.isPlaying)
			    walkPS.Stop();

            controller.RemoveFlag((int)EnemyFlag.WALKING);

		    yield return new WaitForSeconds(stopTime);
            StartMoving();
		}
	}

	void OnCollisionEnter2D(Collision2D collision){
		//go a different direction when bump into something
		if(bounceOffObject == 0 && controller.IsFlag((int)EnemyFlag.MOVING)){
            StartMoving();
			bounceOffObject = 1;
		}
	}

    // Enemy picks a random direction and starts moving.
	public virtual void StartMoving(){
        controller.SetFlag((int)EnemyFlag.WALKING);

        if (walkPS != null && !walkPS.isPlaying)
			walkPS.Play();

		direction = (new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0.0f)).normalized;
		StartCoroutine(Moving());
	}

	public virtual void StopMoving(){
        controller.RemoveFlag((int)EnemyFlag.WALKING);
        
        if (walkPS.isPlaying)
            walkPS.Stop();

		StopAllCoroutines();
		gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
	}
}
