using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GenericEnemyStateController = EnemyStateController<EnemyState, EnemyTrigger>;

[RequireComponent(typeof(GenericEnemyStateController))]
public class RandomDirectionMovement : MonoBehaviour {

	public float movementSpeed = 0;
	public float stopTime = 2;
    public float afterHitStopTime = 0f;
    public float nextMoveTime = 0f;
    public float moveMaxTime = 0f;
    //public GameObject walkCloud;
    public ParticleSystem walkPS;
	//public float walkCloudYadjust = 0.8f;


	private Vector3 direction;
	protected tk2dSpriteAnimator anim;
	protected Vector3 startingScale;
	int turnOnce = 0;

    protected GenericEnemyStateController controller;
    public BoxCollider2D bodyCollider;
    private BreadCrumb path;
    public GameObject linePrefab;
    private LineRenderer line;
    public bool DebugClickMode;

    // Use this for initialization
    void Awake()
    {
        controller = GetComponent<GenericEnemyStateController>();
        var go = GameObject.Instantiate(linePrefab);
        line = go.GetComponent<LineRenderer>();
        line.positionCount = 0;
        line.startWidth = 0.25f;
        line.endWidth = .25f;
        line.startColor = Color.red;
        line.endColor = Color.red;
        line.transform.parent = transform;
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
                        // follow the path if there is one.  consume the nodes as the enemy reaches them.
                        if (path != null) {
                            // set a new destination (the point on the grid, offset by where the bodyCollider actually is)
                            Vector2 destination = RoomManager.Instance.currentRoom.pathGrid.GridToWorld(path.Position) - bodyCollider.offset * Mathf.Sign(transform.localScale.x);
                            transform.position = Vector3.MoveTowards(transform.position, destination, movementSpeed * Time.deltaTime);

                            if (Vector2.Distance(transform.position, destination) < 0.001f) {
                                transform.position = destination;
                                path = path.Next;

                                // turn toward the next node.
                                if (path != null)
                                    transform.localScale = new Vector3(Mathf.Sign(RoomManager.Instance.currentRoom.pathGrid.GridToWorld(path.Position).x - transform.position.x),
                                                                                  transform.localScale.y,
                                                                                  transform.localScale.z);
                            }
                        } else {
                            StopMoving();
                        }
                    } else {
                        // Click debug mode for checking the node grid is good and doesn't generate bad paths.
                        if (DebugClickMode) {
                            ProcessDebugClickMode();
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

    // Pick a node on the path grid and move towards it.
	public virtual void StartMoving(){
        PathGrid pathGrid = RoomManager.Instance.currentRoom.pathGrid;
        if (pathGrid != null) {
            Point startPoint = pathGrid.WorldToGrid(transform.position);
            if (startPoint != null) {
                Point destPoint = pathGrid.GetRandomPoint(startPoint, 10);
                GeneratePath(pathGrid, startPoint, destPoint);
            } else {
                // If no start point was found, the ememy is off the grid.  Try to get them back on the closest point on the grid to where they are.
                startPoint = pathGrid.WorldToClosestGridPoint(transform.position);
                GenerateQuickPath(pathGrid, startPoint);
            }

            if (path != null) {
                transform.localScale = new Vector3(Mathf.Sign(RoomManager.Instance.currentRoom.pathGrid.GridToWorld(path.Position).x - transform.position.x),
                                                  transform.localScale.y,
                                                  transform.localScale.z);
            }

            controller.SetFlag((int)EnemyFlag.WALKING);
            if (walkPS != null && !walkPS.isPlaying)
                walkPS.Play();

            var curr = path;
            int nodeCount = 0;

            // estimate how long it should take the enemy to finish their path.  That way if they get stuck on something for too long they can stop moving if they need to.
            while (curr != null) {
                nodeCount++;
                curr = curr.Next;
            }

            moveMaxTime = nodeCount * pathGrid.width / movementSpeed + 1f;
        }
	}

	public virtual void StopMoving(){
        controller.RemoveFlag((int)EnemyFlag.WALKING);
        
        if (walkPS.isPlaying)
            walkPS.Stop();

        path = null;

		StopAllCoroutines();
		gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        nextMoveTime = Time.time + stopTime;

        ClearDebugPath();
    }

    //helpers
    private void GeneratePath(PathGrid pathGrid, Point startPoint, Point destPoint)
    {
        if (pathGrid != null && startPoint != null && destPoint != null) {
            // Get the best path to the random point, if it exists....
            path = Pathfinder.FindPath(pathGrid, startPoint, destPoint);

            DrawDebugPath(pathGrid);
        }
    }

    // Generates a straight line to a point on the grid, ignoring any pathing logic.
    private void GenerateQuickPath(PathGrid pathGrid, Point destPoint)
    {
        if (pathGrid != null && destPoint != null) {
            path = Pathfinder.FindQuickPath(destPoint);

            DrawDebugPath(pathGrid);
        }
    }

    private void DrawDebugPath(PathGrid pathGrid)
    {
#if DEBUG_PATHFINDING
        BreadCrumb curr = path;
        int index = 0;

        // Render the path for debugging
        while (curr != null) {
            line.positionCount = index + 1;
            line.SetPosition(index, pathGrid.GridToWorld(curr.Position));
            curr = curr.Next;
            index++;
        }
#endif
    }

    private void ClearDebugPath()
    {
#if DEBUG_PATHFINDING
        line.positionCount = 0;
#endif
    }

    private void ProcessDebugClickMode()
    {
        // left click to path to a point on the grid.
        if (Input.GetMouseButton(0)) {
            //Convert mouse click point to grid coordinates
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            PathGrid pathGrid = RoomManager.Instance.currentRoom.pathGrid;
            if (pathGrid != null) {
                Point destPos = pathGrid.WorldToGrid(worldPos);
                if (destPos != null) {
                    Point startPos = pathGrid.WorldToGrid((Vector2)transform.position + bodyCollider.offset);
                    GeneratePath(pathGrid, startPos, destPos);

                    controller.SetFlag((int)EnemyFlag.WALKING);

                    if (walkPS != null && !walkPS.isPlaying)
                        walkPS.Play();
                }
            }
        }
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
}
