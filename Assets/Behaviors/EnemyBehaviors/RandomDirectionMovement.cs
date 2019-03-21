using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyStateController))]
[RequireComponent(typeof(EnemyPath))]
public class RandomDirectionMovement : MonoBehaviour {

	public float movementSpeed = 0;
    public int maxRandomNodeDistance = 10; // how far the enemy can travel when picking random nodes
	public float stopTime = 2; // how long the enemy will stop after they move
    public float afterHitStopTime = 0f; // how long the enemy will stop after they are hit
    private float nextMoveTime = 0f;
    private float moveMaxTime = 0f; // how long the enemy will wait to pick a new destination if they get stuck on something.
    //public GameObject walkCloud;
    public ParticleSystem walkPS;

	protected Vector3 direction;
	protected tk2dSpriteAnimator anim;
	protected Vector3 startingScale;
	protected int turnOnce = 0;
	[HideInInspector]
	public bool mopSlow;


    protected EnemyStateController controller;
    private EnemyPath enemyPath;

    // Use this for initialization
    protected void Awake()
    {
        controller = GetComponent<EnemyStateController>();
        enemyPath = GetComponent<EnemyPath>();
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
                        // follow the path until it's consumed.
                        if (!enemyPath.MoveAlongPath(movementSpeed * Time.deltaTime)) {
                            StopMoving();
                        }
                    } else {
                        // Click debug mode for checking the node grid is good and doesn't generate bad paths.
                        if (enemyPath.ProcessDebugClickMode()) {
                            controller.SetFlag((int)EnemyFlag.WALKING);

                            if (walkPS != null && !walkPS.isPlaying)
                                walkPS.Play();
                        } else if (Time.time > nextMoveTime) {
                            StartMoving();
                        }
                    }
                    break;
                case EnemyState.HIT:
                    // Keep updating nextMoveTime until the hit animation ends.
                    nextMoveTime = Time.time + afterHitStopTime;
                    break;
            }
        }
	}
	protected void Turn(){
		turnOnce = 1;
		Debug.Log("Turn() activate for:" + gameObject.name);
		gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x*-1,startingScale.y,startingScale.z);
	}

	public void TurnToNewDirection(){
		//activated by things like "Wander within bounds" when enemy reaches end of the bounds so it doesnt just keep walking into it.
		Debug.Log("Turn to new direction activated:" + gameObject.name);
		turnOnce = 1;
		gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x*-1,startingScale.y,startingScale.z);
		direction = (new Vector3(direction.x*-1, direction.y*-1, 0.0f)).normalized;

	}

	void OnTriggerEnter2D(Collider2D collider){
		if(collider.tag == "mopTrail"){
			collider.GetComponent<MopPuddle>().SlowDownEnemy(this);
		}
	}

    // Enemy picks a random direction and starts moving.
	public virtual void StartMoving(){
        PathGrid pathGrid = enemyPath.pathGrid;
        if (pathGrid != null) {
            Point startPoint = pathGrid.WorldToGrid(transform.position);
            if (startPoint != null) {
                Point destPoint = pathGrid.GetRandomPoint(startPoint, maxRandomNodeDistance);
                if (destPoint != null)
                    enemyPath.GeneratePath(pathGrid, startPoint, destPoint);
            } else {
                // If no start point was found, the ememy is off the grid.  Try to get them back on the closest point on the grid to where they are.
                startPoint = pathGrid.WorldToClosestGridPoint(transform.position);
                enemyPath.GenerateQuickPath(pathGrid, startPoint);
            }

            if (enemyPath.path != null) {
                enemyPath.FaceNextPathNode();
                controller.SetFlag((int)EnemyFlag.WALKING);
                if (walkPS != null && !walkPS.isPlaying)
                    walkPS.Play();

                moveMaxTime = enemyPath.CalculateMoveEstimate(pathGrid, movementSpeed);
            }
        }
	}

	public virtual void StopMoving(){
        controller.RemoveFlag((int)EnemyFlag.WALKING);
        
        if (walkPS.isPlaying)
            walkPS.Stop();

		StopAllCoroutines();
		gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        nextMoveTime = Time.time + stopTime;

        enemyPath.ClearPath();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8) { // tiles
            if (controller.IsFlag((int)EnemyFlag.WALKING))
                StopMoving();
        }
    }

    // Stop moving if a collision with another enemy happens
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 9) { // enemy
            if (controller.IsFlag((int)EnemyFlag.WALKING))
                StopMoving();
        }
	}

	public void SlowDown(float slowdownSpeedDecrement){
		movementSpeed -= slowdownSpeedDecrement;
	}

	public void SpeedUp(float speedUpIncrement){
		movementSpeed += speedUpIncrement;
	}

}
