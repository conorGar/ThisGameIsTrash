using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GenericEnemyStateController = EnemyStateController<EnemyState, EnemyTrigger>;

[RequireComponent(typeof(GenericEnemyStateController))]
[RequireComponent(typeof(EnemyPath))]
public class FollowPlayer : MonoBehaviour {

	public BoxCollider2D targetCollider;
	public float walkDistance = 10.0f;
	public float chaseSpeed = 10.0f;
	public bool hasSeperateFacingAnimations;
	public bool iBeLerpin;
	public ParticleSystem chasePS;
	public bool returnToPreviousWhenFar;
	private Vector3 smoothVelocity = Vector3.zero;
	private tk2dSpriteAnimator anim;
    private EnemyPath enemyPath;
    public float pathRefreshTime = 1f;
    private float nextPathRefreshTime = 0f;

    protected GenericEnemyStateController controller;

    void Awake()
    {
        controller = GetComponent<GenericEnemyStateController>();
        enemyPath = GetComponent<EnemyPath>();
    }

    void Start () {
		anim = GetComponent<tk2dSpriteAnimator>();
        targetCollider = PlayerManager.Instance.player.GetComponent<BoxCollider2D>();
	}

	void OnEnable(){
        if (PlayerManager.Instance.player != null)
            targetCollider = PlayerManager.Instance.player.GetComponent<BoxCollider2D>(); //needs to be in enable because of Dirty Decoy

        controller.RemoveFlag((int)EnemyFlag.CHASING);
	}

    // generates a path from the enemies current position to the player.
    private void GenerateChasePath()
    {
        PathGrid pathGrid = RoomManager.Instance.currentRoom.pathGrid;
        if (pathGrid != null) {
            Point startPoint;

            // if there was a path node the enemy was moving toward let that be the start position so the enemy doesn't go backwards with bad estimates.
            if (enemyPath.path != null)
                startPoint = enemyPath.path.Position;
            else
                startPoint = pathGrid.WorldToGrid(transform.position);

            if (startPoint != null) {
                Point destPoint = pathGrid.WorldToGrid(GetTargetsClosestPoint());
                if (destPoint != null)
                    enemyPath.GeneratePath(pathGrid, startPoint, destPoint);
            } else {
                // If no start point was found, the ememy is off the grid.  Try to get them back on the closest point on the grid to where they are.
                startPoint = pathGrid.WorldToClosestGridPoint(transform.position);
                enemyPath.GenerateQuickPath(pathGrid, startPoint);
            }

            if (enemyPath.path != null) {
                enemyPath.FaceNextPathNode();
            }
        }
    }

	protected void Update () {
        if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
            if (controller.IsFlag((int)EnemyFlag.CHASING)) {
                float distance = Vector3.Distance(transform.position, GetTargetsClosestPoint());
                if (distance < walkDistance) { //TODO: 
                    // follow the path until it's consumed.  update the path in intervals to chase the player.
                    if (Time.time > nextPathRefreshTime || !enemyPath.MoveAlongPath(chaseSpeed * Time.deltaTime)) {
                        GenerateChasePath();
                        nextPathRefreshTime = Time.time + pathRefreshTime;
                    }

                // If the player gets far enough away from the enemy, put them into idle and show a confused symbol.
                } else {
                    if (returnToPreviousWhenFar) {
                        gameObject.GetComponent<FollowPlayerAfterNotice>().LostSightOfPlayer();
                        if (this.gameObject.GetComponent<WanderWithinBounds>() != null) {
                            this.gameObject.GetComponent<WanderWithinBounds>().ReturnToStart();
                        }
                    }
                    if (this.gameObject.GetComponent<RandomDirectionMovement>() != null) {
                        this.gameObject.GetComponent<RandomDirectionMovement>().StartMoving();

                    }

                    GameObject confused = ObjectPool.Instance.GetPooledObject("effect_confused");
                    confused.transform.parent = this.transform;

                    enemyPath.ClearPath();
                    controller.SendTrigger(EnemyTrigger.IDLE);
                    chasePS.Stop();
                }
            }
        }
	}

	public void StopSound(){

	}

    // get the closest point to the targets collider from this transform.
    private Vector3 GetTargetsClosestPoint()
    {
        return targetCollider.bounds.ClosestPoint(transform.position);
    }
}
