using UnityEngine;
using System.Collections;

public class Ev_Enemy_Slasher : MonoBehaviour
{
    public GameObject shadow;
	public float slashSpeed;
	public float lungeThreshold;
    public float leapThreshold;

    Leapifier leapifier;
    public float leapHeight;
    public float leapSpeed;
    public AnimationCurve leapCurve;

	protected EnemyStateController controller;
	public GameObject mySlash;
	protected Vector3 startScale;
	bool leapBack;
	Vector2 leapDir;


    void Awake()
    {
        controller = GetComponent<EnemyStateController>();
		startScale = gameObject.transform.localScale;
    }

	// Use this for initialization
	void Start ()
	{
	
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
                    } else {
                        StartCoroutine("PrepareSlash");
                    }
				}
			}switch (controller.GetCurrentState()) {
                case EnemyState.HIT:
				Debug.Log("HIT STATE READ" + leapBack);
				gameObject.GetComponent<Rigidbody2D>().velocity = leapDir*2;
                if(!leapBack){
                	Debug.Log("leap back activate");
                	StartCoroutine("LeapBack");
                	leapBack = true;
                }
                break;
				case EnemyState.POWER_HIT:
					Debug.Log("HIT STATE READ" + leapBack);
					gameObject.GetComponent<Rigidbody2D>().velocity = leapDir*2;
	                if(!leapBack){
	                	Debug.Log("leap back activate");
	                	StartCoroutine("LeapBack");
	                	leapBack = true;
	                }
                break;
                case EnemyState.LEAP:
                    if (leapifier.OnUpdate()) {
                        // Reached the leap destination.
                        gameObject.layer = 9; // Enemy;
                        controller.SendTrigger(EnemyTrigger.IDLE);
                    }

                    break;
            }
		}
	}

	IEnumerator PrepareSlash(){
		if (PlayerManager.Instance.player.transform.position.x < transform.position.x) {
               transform.localScale = new Vector3(startScale.x*-1, startScale.y, 1);
        } else {
               transform.localScale = new Vector3(startScale.x, startScale.y, 1);
        }
		controller.SendTrigger(EnemyTrigger.PREPARE_LEAP);
		while (controller.GetCurrentState() == EnemyState.PREPARE_LEAP)
           			yield return null;
        StartCoroutine("Slash");
	}

	IEnumerator Slash(){
		Vector2 slashDir = ( PlayerManager.Instance.player.transform.position - gameObject.transform.position).normalized*slashSpeed;
		gameObject.GetComponent<Rigidbody2D>().AddForce(slashDir*slashSpeed,ForceMode2D.Impulse);
		mySlash.SetActive(true);
		while (controller.GetCurrentState() == EnemyState.LUNGE)
           			yield return null;
		gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

		controller.SendTrigger(EnemyTrigger.RECOVER);
		mySlash.SetActive(false);
	}

	IEnumerator LeapBack(){
		leapDir = (gameObject.transform.position - PlayerManager.Instance.player.transform.position ).normalized*slashSpeed;
		//gameObject.GetComponent<Rigidbody2D>().AddForce(slashDir*slashSpeed,ForceMode2D.Impulse);
		while (controller.GetCurrentState() == EnemyState.HIT ||controller.GetCurrentState() == EnemyState.POWER_HIT )
           			yield return null;
		gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		leapBack = false;
	}
}

