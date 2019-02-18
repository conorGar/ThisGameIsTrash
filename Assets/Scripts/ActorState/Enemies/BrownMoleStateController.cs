using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GenericEnemyStateController = EnemyStateController<EnemyState, EnemyTrigger>;

public class BrownMoleStateController : GenericEnemyStateController
{

	protected new void Awake()
    {
        defaultState = new EnemyChase();
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
                GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
                animator.Play(EnemyAnim.GetName(ENEMY_ANIM.CHASE));
                SetFlag((int)EnemyFlag.CHASING);
                currentState = new EnemyChase();
                break;
        }
    }
}

