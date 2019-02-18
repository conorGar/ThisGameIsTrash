using UnityEngine;
using System.Collections;
using GenericEnemyStateController = EnemyStateController<EnemyState, EnemyTrigger>;

public class Ev_Enemy_Grub : MonoBehaviour
{
	public GameObject weakSpot;
	protected GenericEnemyStateController controller;

	Vector2 startingScale = new Vector2();
	private Vector2 direction;

	int turnOnce;
	// Use this for initialization
	void Start ()
	{
		startingScale = gameObject.transform.lossyScale;
	}
	
	protected void Update () {
		//if idle - will keep turning back and forth, looking for player.
		//if "chasing", will keep popping up from ground

        if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
            switch (controller.GetCurrentState()) {
                case EnemyState.IDLE:
                    if (controller.IsFlag((int)EnemyFlag.WALKING)) {
                        if (direction.x > 0) {
                            if (gameObject.transform.localScale.x < 0) {
                                if (turnOnce == 0) {
                                    Turn();
                                }
                            }
                        } else {
                            if (gameObject.transform.localScale.x > 0) {
                                if (turnOnce == 0) {
                                    Turn();
                                }
                            }
                        }
                    }
                    break;
				if (controller.IsFlag((int)EnemyFlag.CHASING)) {
					Popout();
				}
            }
        }
	}

	void Turn(){
		turnOnce = 1;
		gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x*-1,startingScale.y);
	}

	void Leap(){
		gameObject.GetComponent<Animator>().Play("leap");
		StartCoroutine("LeapSequence");
	}

	IEnumerator LeapSequence(){
		yield return new WaitForSeconds(.2f);//TODO- at certain frame of animation/ end of leap prepare animation...
		weakSpot.SetActive(true);
		yield return new WaitForSeconds(1f); //TODO- switch to end of animation
		StartCoroutine(GetStuck());
	}

	IEnumerator GetStuck(){
		yield return new WaitForSeconds(.3f);
		StartCoroutine("Dive");
	}

	public void Spit(){
		gameObject.GetComponent<FireTowardPlayerEnhanced>().StartCoroutine("Fire");
		controller.SendTrigger(EnemyTrigger.THROW);

	}

	IEnumerator Dive(){
		//switch to dive/vanish animation
		yield return new WaitForSeconds(.4f); //TODO - replace wait with end of animation check
		StartCoroutine("Popout");
	}

	IEnumerator Popout(){
		gameObject.transform.position = PlayerManager.Instance.player.transform.position;
		//switch to ground buldge animation
		yield return new WaitForSeconds(.5f);
		//switch to rise animation

	}
}

