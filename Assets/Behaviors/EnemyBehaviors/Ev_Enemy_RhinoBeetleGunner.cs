using UnityEngine;
using System.Collections;

public class Ev_Enemy_RhinoBeetleGunner : WanderWithinBounds
{

	//AWAKE() temp for debug room
	void Awake(){
		Rect rect = myWanderZone.GetComponent<WanderZone>().GetWanderBounds();
		SetWalkBounds(rect);
		base.Awake();
	}


	protected void Update ()
	{
		if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {

			if(boundsSet && controller.IsFlag((int)EnemyFlag.MOVING)){
			if(transform.position.x < MIN_X || transform.position.x > MAX_X ||
            	transform.position.y < MIN_Y || transform.position.y > MAX_Y){

				transform.position = new Vector3(
								    Mathf.Clamp(gameObject.transform.position.x, MIN_X, MAX_X),
								    Mathf.Clamp(gameObject.transform.position.y, MIN_Y, MAX_Y),
								    0f);
				TurnToNewDirection();

			}
		}
            switch (controller.GetCurrentState()) {
                case EnemyState.IDLE:
                    if (controller.IsFlag((int)EnemyFlag.WALKING)) {
                        transform.position += direction * movementSpeed * Time.deltaTime;
                        if (transform.localScale.x > 0) {
                            if (PlayerManager.Instance.player.transform.position.x < gameObject.transform.position.x) {
                                if (turnOnce == 0) {
                                    Turn();
                                }
                            }
                        } else {
							if (PlayerManager.Instance.player.transform.position.x > gameObject.transform.position.x) {
                                if (turnOnce == 0) {
                                    Turn();
                                }
                            }
                        }
                    }
                    break;
            }
        }
	}
}

