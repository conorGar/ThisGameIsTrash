using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpearMoleStateController : EnemyStateController
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
                currentState = new EnemyDead();
                break;
        }
    }

    protected override void AnimationEventCompleted(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip)
    {
        base.AnimationEventCompleted(animator, clip);
        Debug.Log("Mole animation completion read with state being: " + currentState);
        switch (currentState.GetState()) {
            case EnemyState.HIT:
                currentState = new EnemyIdle();
                break;
        }
    }
}

