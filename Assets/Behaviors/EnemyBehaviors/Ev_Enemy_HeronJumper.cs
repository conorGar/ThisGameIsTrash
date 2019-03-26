using UnityEngine;
using System.Collections;

public class Ev_Enemy_HeronJumper : MonoBehaviour
{
	public float leapSpeed;
	public float lungeThreshold;

	Vector2 leapDir;
	Vector2 leapDestination;
	protected EnemyStateController controller;

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
			if(Vector2.Distance(PlayerManager.Instance.player.transform.position, gameObject.transform.position) < lungeThreshold){

					if(controller.GetCurrentState() != EnemyState.LEAP){
						gameObject.layer = 1; // transparentFX;
	                    controller.SendTrigger(EnemyTrigger.LEAP);
						StartCoroutine("LeapBack");
					}
				}
		switch (controller.GetCurrentState()) {
				
                case EnemyState.LEAP:
                    if (Vector2.Distance(transform.position, leapDestination) > 0.1f) {
                        transform.position = Vector2.MoveTowards(gameObject.transform.position, leapDestination, leapSpeed * Time.deltaTime);
                    } else { // landing spot
                        gameObject.layer = 9; // Enemy;
                        controller.SendTrigger(EnemyTrigger.RECOVER);
                    }

                    break;
            }
        }
	}
	IEnumerator LeapBack(){
		Debug.Log("Leap Back Activated");
		leapDestination = (gameObject.transform.position - PlayerManager.Instance.player.transform.position ).normalized*leapSpeed;


		//gameObject.GetComponent<Rigidbody2D>().AddForce(slashDir*slashSpeed,ForceMode2D.Impulse);
		while (controller.GetCurrentState() == EnemyState.HIT ||controller.GetCurrentState() == EnemyState.POWER_HIT )
           			yield return null;
		gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
	}
}

