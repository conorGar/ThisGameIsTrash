using UnityEngine;
using System.Collections;

public class Ev_Enemy_Crab : MonoBehaviour
{
	protected EnemyStateController controller;
	public GameObject mySlash;
	public float distanceThreshold; //how far th crab will run horizontally from the player to align itself before it starts to slash
	public float slashSpeed;
	Vector3 targetDestination = new Vector3();
	public float leapThreshold;
	public float lungeThreshold;

	public GameObject shadow;

	Leapifier leapifier;
    public float leapHeight;
    public float leapSpeed;
    public AnimationCurve leapCurve;

	Vector2 slashDir;

	void Awake()
    {
        controller = GetComponent<EnemyStateController>();
    }

	
	// Update is called once per frame
	void Update ()
	{
		if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
			if(controller.IsFlag((int)EnemyFlag.CHASING)){
				if(Vector2.Distance(PlayerManager.Instance.player.transform.position, gameObject.transform.position) < lungeThreshold){
                    var aligner = GetComponent<AlignedWithObjectOnAxis>();
                    // If the enemy has an aligner and they aren't aligned, leapback to get a better angle before preparing the slash.
                    // Point to leap to should be parallel to the player on the opposite axis at the closest lunge threshold.
                    if (aligner && !aligner.IsAligned()) {
                    	Debug.Log("Crab is not Aligned");
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
                        Debug.Log(leapDestination);
                        leapifier = new Leapifier(gameObject, shadow, leapHeight, leapSpeed, leapDestination, leapCurve);
                        gameObject.layer = 1; // transparentFX;
                        controller.SendTrigger(EnemyTrigger.LEAP);
                    } 
				}
			}switch (controller.GetCurrentState()) {
               
                case EnemyState.LEAP:
                    if (leapifier.OnUpdate()) {
                        // Reached the leap destination.
                        gameObject.layer = 9; // Enemy;
						controller.SendTrigger(EnemyTrigger.RECOVER);
                        StartDash();
                    }

                 

                    break;
                 case EnemyState.LUNGE:
                 	transform.Translate(slashDir*slashSpeed*Time.deltaTime);
                 break;
            }



		}

	}


	public void StartDash(){
		Debug.Log("Start Dash Activated");
		controller.SendTrigger(EnemyTrigger.LUNGE);

		if (transform.position.x < PlayerManager.Instance.player.transform.position.x){
			slashDir = Vector2.right;
		}else{
			slashDir = Vector2.left;
		}

		//mySlash.SetActive(true);
		StartCoroutine("Dash");
	}

	IEnumerator Dash(){
		yield return new WaitForSeconds(2f);
		controller.SendTrigger(EnemyTrigger.RECOVER);
		controller.SendTrigger(EnemyTrigger.IDLE);

		//mySlash.SetActive(false);
	}
}

