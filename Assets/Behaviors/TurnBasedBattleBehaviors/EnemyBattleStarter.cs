using UnityEngine;
using System.Collections;

public class EnemyBattleStarter : MonoBehaviour
{
	Leapifier leapifier;
	public float leapHeight;
	public float leapSpeed;
	public AnimationCurve leapCurve;
	public float leapThreshold;
	public GameObject shadow;
	protected EnemyStateController controller;

	bool leaping;
	// Use this for initialization

	void Awake()
    {
        controller = GetComponent<EnemyStateController>();
    }

	void Update ()
	{
		if (leaping) {
			if(controller.IsFlag((int)EnemyFlag.CHASING)){



                    var aligner = GetComponent<AlignedWithObjectOnAxis>();
                    // If the enemy has an aligner and they aren't aligned, leapback to get a better angle before preparing the slash.
                    // Point to leap to should be parallel to the player on the opposite axis at the closest lunge threshold.
                    if (aligner && !aligner.IsAligned()) {
                        Vector3 leapDestination = new Vector3();
                        switch (aligner.alignedAxis) {
                            case ALIGNED_AXIS.X_AXIS:
                                if (transform.position.y < PlayerManager.Instance.player.transform.position.y) { // leap to below the player
                                    leapDestination = PlayerManager.Instance.player.transform.position + leapThreshold * Vector3.down;
                                } else { // leap to above the player
                                    leapDestination = PlayerManager.Instance.player.transform.position + leapThreshold * Vector3.up;
                                }
                                break;
                            case ALIGNED_AXIS.Y_AXIS:
                                if (transform.position.x < PlayerManager.Instance.player.transform.position.x) { // leap to left of the player
                                    leapDestination = PlayerManager.Instance.player.transform.position + leapThreshold * Vector3.left;
                                } else { // leap to right of the player
                                    leapDestination = PlayerManager.Instance.player.transform.position + leapThreshold * Vector3.right;
                                }
                                break;
                        }

                        if (leapifier != null)
                            leapifier.Reset();

                        leapifier = new Leapifier(gameObject, shadow, leapHeight, leapSpeed, leapDestination, leapCurve);
                        gameObject.layer = 1; // transparentFX;
                        controller.SendTrigger(EnemyTrigger.LEAP);
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
		leaping = false;
	}

}

