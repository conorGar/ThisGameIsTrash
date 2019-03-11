using UnityEngine;
using System.Collections;
using GenericEnemyStateController = EnemyStateController<EnemyState, EnemyTrigger>;

public class OpossumStateController : GenericEnemyStateController
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
                animator.Play(EnemyAnim.GetName(ENEMY_ANIM.LEAP));
                currentState = new EnemyLunge();
                break;
			case EnemyState.LUNGE:
                animator.Play(EnemyAnim.GetName(ENEMY_ANIM.WAKE));
                currentState = new EnemyRecover();
                break;
			case EnemyState.RECOVER:
                animator.Play(EnemyAnim.GetName(ENEMY_ANIM.CHASE));
				flags |= (int)EnemyFlag.CHASING;
                currentState = new EnemyChase();
                break;
        }
    }
}

