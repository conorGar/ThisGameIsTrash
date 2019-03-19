using UnityEngine;
using System.Collections;
using GenericEnemyStateController = EnemyStateController<EnemyState, EnemyTrigger>;

public class HeronStateController : GenericEnemyStateController
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
           
			case EnemyState.HIT:
				animator.Play(EnemyAnim.GetName(ENEMY_ANIM.IDLE));
				Debug.Log("Opossum ani event complete: RECOVER -> CHASE");
                currentState = new EnemyIdle();
                break;
			case EnemyState.POWER_HIT:
				animator.Play(EnemyAnim.GetName(ENEMY_ANIM.IDLE));
				Debug.Log("Opossum ani event complete: RECOVER -> CHASE");
                currentState = new EnemyIdle();
                break;
			case EnemyState.RECOVER:
                animator.Play(EnemyAnim.GetName(ENEMY_ANIM.IDLE));
                currentState = new EnemyIdle();
                break;
			case EnemyState.PREPARE:
                GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
                currentState = new EnemyThrow();
				Debug.Log("Animation Prepare end state change:" + GetCurrentState());
                break;
			case EnemyState.THROW:
                GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
                currentState = new EnemyIdle();
                break;
        }
    }
}

