using UnityEngine;
using System.Collections;

public class Ev_Enemy_HeronJumper : MonoBehaviour
{
	public float leapSpeed;
	public float lungeThreshold;
	public GameObject shadow;

	Vector2 leapDir;
	Vector2 leapDestination;
	protected EnemyStateController controller;

	Leapifier leapifier;
    public float leapHeight;
    public AnimationCurve leapCurve;
	// Use this for initialization
	void Awake(){
		controller = GetComponent<EnemyStateController>();

	}

	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
			//Debug.Log("Got here - Heron 1");
			if(Vector2.Distance(PlayerManager.Instance.player.transform.position, gameObject.transform.position) < lungeThreshold){
				//Debug.Log("Got here - Heron 2");

					if(controller.GetCurrentState() != EnemyState.LEAP){
						//Debug.Log("Got here - Heron 3");
						if (leapifier != null)
                            leapifier.Reset();

						leapifier = new Leapifier(gameObject, shadow, leapHeight, leapSpeed, leapDestination, leapCurve);

						gameObject.layer = 1; // transparentFX;
						StartCoroutine("LeapBack");
					}
				}
		switch (controller.GetCurrentState()) {
				
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
	IEnumerator LeapBack(){
		Debug.Log("Leap Back Activated");
		leapDestination = (gameObject.transform.position - PlayerManager.Instance.player.transform.position ).normalized*leapSpeed;

		controller.SendTrigger(EnemyTrigger.LEAP);

		//gameObject.GetComponent<Rigidbody2D>().AddForce(slashDir*slashSpeed,ForceMode2D.Impulse);
		while (controller.GetCurrentState() == EnemyState.HIT ||controller.GetCurrentState() == EnemyState.POWER_HIT )
           			yield return null;
		//gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
	}
}

