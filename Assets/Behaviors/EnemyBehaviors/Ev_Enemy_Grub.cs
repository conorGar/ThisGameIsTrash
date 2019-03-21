using UnityEngine;
using System.Collections;

public class Ev_Enemy_Grub : FireTowardPlayer
{
	public GameObject weakSpot;
	public GameObject parent;
	public GameObject armor;
	public GameObject burrowingPS;

	Vector2 startingScale = new Vector2();
	private Vector2 direction;
	Vector2 destinationMark;


	int spitOnceCheck;
	float nextActionTime;
	bool landed;
	// Use this for initialization

	void Awake()
    {
        controller = GetComponent<EnemyStateController>();
    }


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
				Debug.Log("Here - Grub");

					if(Time.time > nextActionTime){
						Debug.Log("TIme is > grub action time: " + spitOnceCheck);
						if(spitOnceCheck == 0){
							Debug.Log("Fire rate reached, throw time is now");
							Spit();
							spitOnceCheck = 1;
						}else if(spitOnceCheck == 1){
							Leap();
							spitOnceCheck = 2;
						}
						nextActionTime = Time.time + 3;
					}else{
						Debug.Log(Time.time + " " + nextActionTime);
					}
                    break;
                 case EnemyState.RECOVER:
                 	parent.transform.position = Vector2.MoveTowards(parent.transform.position,destinationMark,3*Time.deltaTime);
                 break;
				
            }
        }
	}

	void Spit(){
		//turnOnce = 1;
		StartCoroutine("Fire");
	}

	void Leap(){
		landed = false;
		Debug.Log("GRUB LEAP ACTIVATED");
		//gameObject.GetComponent<Animator>().Play("leap");
		StartCoroutine("LeapSequence");
	}

	IEnumerator LeapSequence(){


		controller.SendTrigger(EnemyTrigger.PREPARE_LEAP);

		while (controller.GetCurrentState() == EnemyState.PREPARE_LEAP)
           			yield return null;
		weakSpot.SetActive(true);
		gameObject.GetComponent<Animator>().SetTrigger("Leap");
           			
		//while (controller.GetCurrentState() != EnemyState.VULNERABLE)
           			//yield return null;
       // yield return new WaitForSeconds(1f); // 1s = time of leap animation
       yield return new WaitUntil(()  => landed == true); // set true by animator
	   StartCoroutine("GetStuck");
		
	}

	IEnumerator GetStuck(){
		armor.SetActive(false);
		controller.SendTrigger(EnemyTrigger.VULNERABLE); //use recover for when dive in

		yield return new WaitForSeconds(.5f);
		destinationMark = PlayerManager.Instance.player.transform.position;
		controller.SendTrigger(EnemyTrigger.RECOVER); //use recover for when dive in
		//while (controller.GetCurrentState() == EnemyState.RECOVER) //what to do if hit during this time?!?
           			//yield return null;
      	burrowingPS.SetActive(true);
        StartCoroutine("Popout");
       	StopCoroutine("GetStuck");
	}


	IEnumerator Popout(){
		//underground. After a brief period will pop up at player's postion.
		Debug.Log("Grub Popout activated");

		yield return new WaitUntil(() => Vector2.Distance(parent.transform.position, destinationMark) < 2f );
		burrowingPS.SetActive(false);
		armor.SetActive(true);
		weakSpot.SetActive(false);

		controller.SendTrigger(EnemyTrigger.POPUP);
		nextActionTime = Time.time + 3;
		spitOnceCheck = 0;


	}

	public void LeapLand(){ //called from animator (grubLeap)
		landed = true;
	}
}

