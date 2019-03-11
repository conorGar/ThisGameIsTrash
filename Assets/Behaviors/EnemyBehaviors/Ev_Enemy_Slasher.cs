using UnityEngine;
using System.Collections;
using GenericEnemyStateController = EnemyStateController<EnemyState, EnemyTrigger>;

public class Ev_Enemy_Slasher : MonoBehaviour
{
	public float slashSpeed;
	public float lungeThreshold;
	protected GenericEnemyStateController controller;
	public GameObject mySlash;


    void Awake()
    {
        controller = GetComponent<GenericEnemyStateController>();
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
			}
		}
	}

	IEnumerator PrepareSlash(){
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
}

