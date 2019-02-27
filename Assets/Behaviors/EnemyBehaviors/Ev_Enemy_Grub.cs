using UnityEngine;
using System.Collections;
using GenericEnemyStateController = EnemyStateController<EnemyState, EnemyTrigger>;

public class Ev_Enemy_Grub : FireTowardPlayer
{
	public GameObject weakSpot;
	protected GenericEnemyStateController controller;

	Vector2 startingScale = new Vector2();
	private Vector2 direction;

	int spitOnceCheck;
	float nextActionTime;
	// Use this for initialization
	void Start ()
	{
		startingScale = gameObject.transform.lossyScale;
	}
	
	protected override void Update () {
		//if idle - will keep turning back and forth, looking for player.
		//if "chasing", will keep popping up from ground

        if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
            switch (controller.GetCurrentState()) {
                case EnemyState.IDLE:
					if(Time.time > nextActionTime){
						if(spitOnceCheck == 0){
						Debug.Log("Fire rate reached, throw time is now");
						Spit();
						spitOnceCheck = 1;
					}else if(spitOnceCheck == 1){
						Leap();
						spitOnceCheck = 2;
					}
					nextActionTime = Time.time + 3;
					}
                    break;
				
            }
        }
	}

	void Spit(){
		//turnOnce = 1;
		StartCoroutine("Fire");
	}

	void Leap(){
		//gameObject.GetComponent<Animator>().Play("leap");
		StartCoroutine("LeapSequence");
	}

	IEnumerator LeapSequence(){


		controller.SendTrigger(EnemyTrigger.PREPARE_LEAP);

		while (controller.GetCurrentState() == EnemyState.PREPARE_LEAP)
           			yield return null;
		weakSpot.SetActive(true);
           			
		while (controller.GetCurrentState() != EnemyState.VULNERABLE)
           			yield return null;

        StartCoroutine("GetStuck");
		
	}

	IEnumerator GetStuck(){
		yield return new WaitForSeconds(.3f);
		controller.SendTrigger(EnemyTrigger.RECOVER); //use recover for when dive in
		while (controller.GetCurrentState() == EnemyState.RECOVER) //what to do if hit during this time?!?
           			yield return null;
        weakSpot.SetActive(false);
        StartCoroutine("Popout");
        StopCoroutine("GetStuck");
	}


	IEnumerator Popout(){
		//underground. After a brief period will pop up at player's postion.
		yield return new WaitForSeconds(3f);
		gameObject.transform.position = PlayerManager.Instance.player.transform.position;
		controller.SendTrigger(EnemyTrigger.POPUP);
		nextActionTime = Time.time + 3;
		spitOnceCheck = 0;


	}
}

