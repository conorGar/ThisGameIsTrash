using UnityEngine;
using System.Collections;

public class BoomerangSlimeStateController : EnemyStateController
{

	EnemyState stateAtHit = EnemyState.NONE;
	protected new void Awake()
    {
        defaultState = new EnemyIdle();
        base.Awake();
    }

    protected override void AnyStateTrigger(EnemyTrigger trigger)
    {
        switch (trigger) {
            case EnemyTrigger.DEATH:
                animator.Play(EnemyAnim.GetName(ENEMY_ANIM.DEAD));
                RemoveFlag((int)EnemyFlag.CHASING);
                currentState = new EnemyDead();
                break;
		case EnemyTrigger.HIT:
				stateAtHit = currentState.GetState();
				Debug.Log("Boomerang Slime state at hit: " + stateAtHit);
                break;
		case EnemyTrigger.PREPARE:
                animator.Play(EnemyAnim.GetName(ENEMY_ANIM.PREPARE));
                RemoveFlag((int)EnemyFlag.CHASING);
                currentState = new EnemyPrepare();
				gameObject.GetComponent<EnemyTakeDamage>().armorRating -= 1; //decrease armor rating. **IF FUTURE PROBLEMS CREATE SEPEARTE CODE THAT HANDLES ARMOR RATINGS BASED ON STATE

                break;
        }
    }

    protected override void AnimationEventCompleted(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip)
    {
        base.AnimationEventCompleted(animator, clip);

        switch (currentState.GetState()) {
            
            case EnemyState.PREPARE:
                currentState = new EnemyThrow();
                gameObject.GetComponent<Ev_Enemy_BoomerangSlime>().Fire();
                break;

			case EnemyState.HIT:
				if(stateAtHit == EnemyState.THROW){
                	currentState = new EnemyThrow();
                }else{
					flags |= (int)EnemyFlag.CHASING;
					currentState = new EnemyChase();
                }
                break;
			case EnemyState.POWER_HIT:
				flags |= (int)EnemyFlag.CHASING;
                currentState = new EnemyChase();
                break;
			case EnemyState.RECOVER:
				flags |= (int)EnemyFlag.CHASING;
                currentState = new EnemyChase();
                break;
        }
    }
}

