using UnityEngine;
using System.Collections;

public class OpossumStateController : EnemyStateController
{
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
        }
    }

    protected override void AnimationEventCompleted(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip)
    {
        base.AnimationEventCompleted(animator, clip);

        switch (currentState.GetState()) {
            
            case EnemyState.PREPARE_LEAP:
                currentState = new EnemyLunge();
				Debug.Log("Opossum ani event complete: LUNGE -> RECOVER");

                break;
			case EnemyState.LUNGE:
                currentState = new EnemyRecover();
				Debug.Log("Opossum ani event complete: LUNGE -> RECOVER");

                break;
			case EnemyState.HIT:
				flags |= (int)EnemyFlag.CHASING;
                currentState = new EnemyChase();
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

