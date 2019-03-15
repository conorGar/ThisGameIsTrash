using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GenericEnemyStateController = EnemyStateController<EnemyState, EnemyTrigger>;

[RequireComponent(typeof(GenericEnemyStateController))]
public class FollowPlayer : MonoBehaviour {

	public Transform target;
	public float walkDistance = 10.0f;
	public float chaseSpeed = 10.0f;
	public bool hasSeperateFacingAnimations;
	public bool iBeLerpin;
	public ParticleSystem chasePS;
	public bool returnToPreviousWhenFar;
	private Vector3 smoothVelocity = Vector3.zero;
	private tk2dSpriteAnimator anim;
	public float maxWalkDistance; //how close the enemy will get
    protected GenericEnemyStateController controller;

    Vector3 startScale;

    void Awake()
    {
        controller = GetComponent<GenericEnemyStateController>();
        startScale = transform.localScale;
    }

    void Start () {
		anim = GetComponent<tk2dSpriteAnimator>();
        target = PlayerManager.Instance.player.transform;
	}

	void OnEnable(){
        if (PlayerManager.Instance.player != null)
		    target = PlayerManager.Instance.player.transform; //needs to be in enable because of Dirty Decoy

        controller.RemoveFlag((int)EnemyFlag.CHASING);
	}

	protected void Update () {
        if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
			if (controller.IsFlag((int)EnemyFlag.CHASING) && !controller.IsFlag((int)EnemyFlag.CHASING_OBJECT)) {
                float distance = Vector3.Distance(transform.position, target.position);
                if (distance < walkDistance && distance > maxWalkDistance) { //TODO: 
                    if (iBeLerpin) {
                        transform.position = Vector3.SmoothDamp(transform.position, target.position, ref smoothVelocity, chaseSpeed);
                    } else {
                        transform.position = Vector3.MoveTowards(transform.position, target.position, chaseSpeed * Time.deltaTime);
                    }
                    if (!hasSeperateFacingAnimations) {
                        if (target.transform.position.x < transform.position.x) {
                            transform.localScale = new Vector3(startScale.x*-1, startScale.y, 1);
                        } else {
                            transform.localScale = new Vector3(startScale.x, startScale.y, 1);
                        }
                    }//otherwise for now those specific actors handle it(Questio)
                } else if (returnToPreviousWhenFar) {
                    gameObject.GetComponent<FollowPlayerAfterNotice>().LostSightOfPlayer();
                    if (this.gameObject.GetComponent<WanderWithinBounds>() != null) {
                        this.gameObject.GetComponent<WanderWithinBounds>().ReturnToStart();

                    } else if (this.gameObject.GetComponent<RandomDirectionMovement>() != null) {
                        this.gameObject.GetComponent<RandomDirectionMovement>().StartMoving();

                    }


                    GameObject confused = ObjectPool.Instance.GetPooledObject("effect_confused");
                    confused.transform.parent = this.transform;

                    controller.SendTrigger(EnemyTrigger.IDLE);
                    chasePS.Stop();
                }
            }
        }
	}

	public void StopSound(){

	}
}
