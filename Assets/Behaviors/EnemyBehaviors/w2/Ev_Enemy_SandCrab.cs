using UnityEngine;
using System.Collections;

public class Ev_Enemy_SandCrab : MonoBehaviour
{

	public float noticeThreshold;
	protected EnemyStateController controller;
	public float speed;
	Vector2 destinationMark;

	// Use this for initialization
	void Awake (){
		controller = GetComponent<EnemyStateController>();

	}
	
	// Update is called once per frame
	void Update ()
	{
		if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
			if(Vector2.Distance(PlayerManager.Instance.player.transform.position, gameObject.transform.position) < noticeThreshold){
				if(controller.GetCurrentState() == EnemyState.IDLE){
					PrepareLunge();
				}
			}

		

		}
	}


	void PrepareLunge(){
		controller.SendTrigger(EnemyTrigger.PREPARE_LEAP);

	}

	public void SetDestination(){ //Set by Sand Crab state controller
		destinationMark = PlayerManager.Instance.player.transform.position;
		StartCoroutine("StopDelay");
		destinationMark = ( PlayerManager.Instance.player.transform.position - gameObject.transform.position).normalized*speed;
		gameObject.GetComponent<Rigidbody2D>().velocity = destinationMark;

	}

	IEnumerator StopDelay(){
		yield return new WaitForSeconds(1.3f);
		gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

		controller.SendTrigger(EnemyTrigger.RECOVER);
	}
}

