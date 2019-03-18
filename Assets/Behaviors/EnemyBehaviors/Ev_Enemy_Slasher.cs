using UnityEngine;
using System.Collections;
using GenericEnemyStateController = EnemyStateController<EnemyState, EnemyTrigger>;

public class Ev_Enemy_Slasher : MonoBehaviour
{
	public float slashSpeed;
	public float lungeThreshold;
	protected GenericEnemyStateController controller;
	public GameObject mySlash;
	protected Vector3 startScale;
	bool leapBack;
	Vector2 leapDir;

    void Awake()
    {
        controller = GetComponent<GenericEnemyStateController>();
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
					StartCoroutine("PrepareSlash");
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

