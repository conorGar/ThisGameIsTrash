using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyStateController))]
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
    public bool usePathGrid;
    private EnemyPath enemyPath;
    public float pathRefreshTime = 1f;
    private float nextPathRefreshTime = 0f;

    protected EnemyStateController controller;

    void Awake()
    {
        controller = GetComponent<EnemyStateController>();
        enemyPath = GetComponent<EnemyPath>();
    }

    void Start () {
		anim = GetComponent<tk2dSpriteAnimator>();
        targetCollider = PlayerManager.Instance.player.GetComponent<BoxCollider2D>();
	}

	void OnEnable(){
        if (PlayerManager.Instance.player != null)
            targetCollider = PlayerManager.Instance.player.GetComponent<BoxCollider2D>(); //needs to be in enable because of Dirty Decoy
	}

    // generates a path from the enemies current position to the player.
    private void GenerateChasePath()
    {
        if (enemyPath != null) {
            PathGrid pathGrid = enemyPath.pathGrid;
            if (pathGrid != null) {
                Point startPoint;

                // if there was a path node the enemy was moving toward let that be the start position so the enemy doesn't go backwards with bad estimates.
                if (enemyPath.path != null)
                    startPoint = enemyPath.path.Position;
                else
                    startPoint = pathGrid.WorldToGrid(GetMyClosestPoint());

                if (startPoint != null) {
                    Point destPoint = pathGrid.WorldToGrid(GetTargetsClosestPoint());
                    if (destPoint != null)
                        enemyPath.GeneratePath(pathGrid, startPoint, destPoint);
                } else {
                    // If no start point was found, the enemy is off the grid.  Try to get them back on the closest point on the grid to where they are.
                    startPoint = pathGrid.WorldToClosestGridPoint(transform.position);
                    enemyPath.GenerateQuickPath(pathGrid, startPoint);
                }

                if (enemyPath.path != null) {
                    enemyPath.FaceNextPathNode();
                }
            }
        }
    }

	protected void Update () {
        if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
            if (controller.IsFlag((int)EnemyFlag.CHASING)) {
                float distance = Vector3.Distance(transform.position, GetTargetsClosestPoint());
                if (distance < walkDistance) { //TODO: 
                    if (usePathGrid) {
                        if (enemyPath != null) {
                            // follow the path until it's consumed.  update the path in intervals to chase the player.
                            if (Time.time > nextPathRefreshTime || !enemyPath.MoveAlongPath(chaseSpeed * Time.deltaTime)) {
                                GenerateChasePath();
                                ResetPathRefreshTime();
                            }
                        }
                    } else {
                        transform.position = Vector3.MoveTowards(transform.position, GetTargetsClosestPoint(), chaseSpeed * Time.deltaTime);
                        if (!hasSeperateFacingAnimations)
                            transform.localScale = new Vector3(Mathf.Sign(targetCollider.transform.position.x - transform.position.x),
                                                      transform.localScale.y,
                                                      transform.localScale.z);
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

                    if (enemyPath != null)
                        enemyPath.ClearPath();

                    controller.SendTrigger(EnemyTrigger.IDLE);
                    chasePS.Stop();
                }
            }
        }
	}

    public void ResetPathRefreshTime()
    {
        nextPathRefreshTime = Time.time + pathRefreshTime;
    }

	public void StopSound(){

	}

    // get the closest point to on my collider to the target's transform
    private Vector3 GetMyClosestPoint()
    {
        return enemyPath.bodyCollider.bounds.ClosestPoint(targetCollider.transform.position);
    }

    // get the closest point on the target's collider from this transform.
    private Vector3 GetTargetsClosestPoint()
    {
        return targetCollider.bounds.ClosestPoint(transform.position);
    }

    // if two chasing enemies collide they can get stuck trying to get through narrow passages.
    // Have the closest enemy to it's target go while the other waits until another path is made.
    private void OnCollisionStay2D (Collision2D collision)
    {
        if (usePathGrid) {
            if (collision.gameObject.layer == 9) {
                var otherFollowPlayer = collision.gameObject.GetComponent<FollowPlayer>();
                if (otherFollowPlayer != null && otherFollowPlayer.usePathGrid) { // enemy... that uses a pathgrid too
                    float myDist = float.MaxValue, otherDist = float.MaxValue;
                    if (controller.IsFlag((int)EnemyFlag.CHASING)) {
                        myDist = Vector3.SqrMagnitude(GetTargetsClosestPoint() - transform.position);
                    }

                    if (otherFollowPlayer.controller.IsFlag((int)EnemyFlag.CHASING)) {
                        otherDist = Vector3.SqrMagnitude(GetTargetsClosestPoint() - otherFollowPlayer.transform.position);
                    }

                    // The larger distance must wait.
                    if (myDist > otherDist) {
                        enemyPath.ClearPath();
                        ResetPathRefreshTime();
                    } else {
                        otherFollowPlayer.enemyPath.ClearPath();
                        otherFollowPlayer.ResetPathRefreshTime();
                    }
                }
            }
        }
    }
}
