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

    public float maxDistanceFromStart; //maybe eventually change this to get the room bounds?

    float minJumpx;
    float maxJumpx;
    float minJumpy;
    float maxJumpy;

	// Use this for initialization
	void Awake(){
		controller = GetComponent<EnemyStateController>();

	}

	void Start ()
	{
	
	}


	void OnEnable(){
		minJumpx = gameObject.transform.position.x - maxDistanceFromStart;
		maxJumpx = gameObject.transform.position.x + maxDistanceFromStart;
		minJumpy = gameObject.transform.position.y - maxDistanceFromStart;
		maxJumpy = gameObject.transform.position.y + maxDistanceFromStart;


	}
	
	// Update is called once per frame
	void Update ()
	{
		if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
			//Debug.Log("Got here - Heron 1");
			//if(Vector2.Distance(PlayerManager.Instance.player.transform.position, gameObject.transform.position) < lungeThreshold){
				//Debug.Log("Got here - Heron 2");

					if(controller.GetCurrentState() == EnemyState.IDLE){
						//Debug.Log("Got here - Heron 3");
						if (leapifier != null)
                            leapifier.Reset();
                        leapDestination = new Vector2(Random.Range(minJumpx,maxJumpx), Random.Range(minJumpy,maxJumpy));
						leapifier = new Leapifier(gameObject, shadow, leapHeight, leapSpeed, leapDestination, leapCurve);

						gameObject.layer = 1; // transparentFX;
						StartCoroutine("LeapBack");
					}
				//}
		switch (controller.GetCurrentState()) {
				
                case EnemyState.LEAP:
				if (leapifier.OnUpdate()) {
                        // Reached the leap destination.
                        gameObject.layer = 9; // Enemy;
                        controller.SendTrigger(EnemyTrigger.RECOVER);
                    }

                    break;
            }
        }
	}
	IEnumerator LeapBack(){
		//leapDestination = (gameObject.transform.position - PlayerManager.Instance.player.transform.position );



		controller.SendTrigger(EnemyTrigger.LEAP);

		//gameObject.GetComponent<Rigidbody2D>().AddForce(slashDir*slashSpeed,ForceMode2D.Impulse);
		while (controller.GetCurrentState() == EnemyState.HIT ||controller.GetCurrentState() == EnemyState.POWER_HIT )
           			yield return null;
		//gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
	}
}

