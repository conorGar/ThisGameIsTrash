using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyBattleStarter : MonoBehaviour
{
	Leapifier leapifier;
	public float leapHeight;
	public float leapSpeed;
	public AnimationCurve leapCurve;
	public float leapThreshold;
	public GameObject shadow;
	protected EnemyStateController controller;
	private EnemyPath enemyPath;
	int maxRandomNodeDistance = 7; // how far the enemy can travel when picking random nodes
	protected Vector3 startScale;

	bool leaping;
	// Use this for initialization

	void Awake()
    {
		enemyPath = GetComponent<EnemyPath>();
        controller = GetComponent<EnemyStateController>();
		startScale = gameObject.transform.localScale;

    }

	void Update ()
	{
		if (leaping) {
			if(controller.GetCurrentState() != EnemyState.LEAP){

                    var aligner = GetComponent<AlignedWithObjectOnAxis>();
                    // If the enemy has an aligner and they aren't aligned, leapback to get a better angle before preparing the slash.
                    // Point to leap to should be parallel to the player on the opposite axis at the closest lunge threshold.
                    if (aligner) {
                    	Debug.Log("Got here- enemy battle starter");
                        Vector2 leapDestination = GrabRandomNodePosition();
                       

                        if (leapifier != null)
                            leapifier.Reset();

                        leapifier = new Leapifier(gameObject, shadow, leapHeight, leapSpeed, leapDestination, leapCurve);
                        gameObject.layer = 1; // transparentFX;
                        controller.SendTrigger(EnemyTrigger.LEAP);
                    }else{
                    	Debug.Log(aligner);
                    } 
				
			}
			switch (controller.GetCurrentState()) {
               
                case EnemyState.LEAP:
                    if (leapifier.OnUpdate()) {
                        // Reached the leap destination.
                        gameObject.layer = 9; // Enemy;
						controller.SendTrigger(EnemyTrigger.RECOVER);
                        StopLeap();
                    }

                    break;
               
            }
             
		}
	}

	public void LeapBack(){
		leaping = true;
	}

	void StopLeap(){
		Debug.Log("Stop Leap Activate");
		if (PlayerManager.Instance.player.transform.position.x < transform.position.x) {
               transform.localScale = new Vector3(startScale.x, startScale.y, 1);
        } else {
			transform.localScale = new Vector3(startScale.x*-1, startScale.y, 1);
        }
		leaping = false;
	}


	Vector2 GrabRandomNodePosition(){
		PathGrid pathGrid = enemyPath.pathGrid;
		if (pathGrid != null) {
            Point startPoint = pathGrid.WorldToGrid(transform.position);

            Vector2 referencePlayerPos = PlayerManager.Instance.player.transform.position;

            float whichSide = Random.Range(0,2)*2-1; //jumps random to either left or right side of player
			Vector2 jumpPosition = new Vector2(Random.Range(referencePlayerPos.x-6f,referencePlayerPos.x-4f)*whichSide, Random.Range(referencePlayerPos.y -5f, referencePlayerPos.y +5));
         
			Point destPoint = pathGrid.WorldToClosestGridPoint(jumpPosition);

			Debug.Log("enemy jump destination point:" + pathGrid.GridToWorld(destPoint));

			//Make sure doesnt jump to a node that's already occupied by another enemy
			while(BattleManager.Instance.occupiedGridPoints.Contains(destPoint)){
				destPoint = pathGrid.GetRandomPoint(startPoint, maxRandomNodeDistance);
			}

			BattleManager.Instance.AddOccupiedPointToGrid(destPoint);

            if (startPoint != null) {
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
            return pathGrid.GridToWorld(destPoint);
        }

        Debug.LogError("Could not find grid position for leap for enemy battle start!");
        return Vector2.zero;
	}

}

