using UnityEngine;
using System.Collections;

public class Ev_Enemy_Crab : MonoBehaviour
{
	protected EnemyStateController controller;
	public GameObject mySlash;
	public float distanceThreshold; //how far th crab will run horizontally from the player to align itself before it starts to slash
	public float slashSpeed;
	Vector3 targetDestination = new Vector3();

	void Awake()
    {
        controller = GetComponent<EnemyStateController>();
    }

	
	// Update is called once per frame
	void Update ()
	{
		if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
			if(controller.IsFlag((int)EnemyFlag.CHASING)){
				var aligner = GetComponent<AlignedWithObjectOnAxis>();
				if (aligner && !aligner.IsAligned()) {
					Debug.Log("Hasnt reached alignment");
                        switch (aligner.alignedAxis) {
                     
                            case ALIGNED_AXIS.Y_AXIS:
                                if (transform.position.x < PlayerManager.Instance.player.transform.position.x) { // leap to left of the player
                                    targetDestination = PlayerManager.Instance.player.transform.position + distanceThreshold * Vector3.left;
                                } else { // leap to right of the player
                                    targetDestination = PlayerManager.Instance.player.transform.position + distanceThreshold * Vector3.right;
                                }
                                break;
                        }
                       /* if (leapifier != null)
                            leapifier.Reset();

                        leapifier = new Leapifier(gameObject, shadow, leapHeight, leapSpeed, leapDestination, leapCurve);
                        gameObject.layer = 1; // transparentFX;*/
                        controller.SendTrigger(EnemyTrigger.LEAP);
                    } else {
                        StartDash();
                    }
			}switch (controller.GetCurrentState()) {
               
                case EnemyState.LEAP:
					var aligner = GetComponent<AlignedWithObjectOnAxis>();

                    if (Vector2.Distance(gameObject.transform.position, targetDestination) < .2f) {
                        // Reached the leap destination.
                        gameObject.layer = 9; // Enemy;
                        controller.SendTrigger(EnemyTrigger.IDLE);
                    }else{
					switch (aligner.alignedAxis) {
                     
                            case ALIGNED_AXIS.Y_AXIS:
                                if (transform.position.x < PlayerManager.Instance.player.transform.position.x) { // leap to left of the player
                                    targetDestination = PlayerManager.Instance.player.transform.position + distanceThreshold * Vector3.left;
                                } else { // leap to right of the player
                                    targetDestination = PlayerManager.Instance.player.transform.position + distanceThreshold * Vector3.right;
                                }
                                break;
                        }
                    }

                    break;
            }



		}

	}


	public void StartDash(){
		Vector2 slashDir;
		if (transform.position.x < PlayerManager.Instance.player.transform.position.x){
			slashDir = Vector2.right;
		}else{
			slashDir = Vector2.left;
		}

		gameObject.GetComponent<Rigidbody2D>().velocity = slashDir * slashSpeed;
		mySlash.SetActive(true);
		StartCoroutine("Dash");
	}

	IEnumerator Dash(){
		yield return new WaitForSeconds(2f);
		controller.SendTrigger(EnemyTrigger.RECOVER);
		mySlash.SetActive(false);
	}
}

