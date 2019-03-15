using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(B_Ev_Hash))]
public class HashStateController : EnemyStateController
{
    protected new void Awake()
    {
        defaultState = new EnemyIdle();
        base.Awake();
    }

    protected override void AnyStateTrigger(EnemyTrigger trigger)
    {
        switch (trigger) {
            case EnemyTrigger.DEATH: // On death this guy becomes throwable and dazed.
                SetFlag((int)EnemyFlag.THROWABLE);
                GetComponent<B_Ev_Hash>().Dazed();
                currentState = new EnemyDazed();
                break;
            case EnemyTrigger.INVULNERABLE:
                SetFlag((int)EnemyFlag.INVULNERABLE);
                break;
        }
    }

    protected override void AnimationEventCompleted(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip)
    {
        base.AnimationEventCompleted(animator, clip);

        switch (currentState.GetState()) {
            case EnemyState.CAST:
                GetComponent<B_Ev_Hash>().Shield(); // Hash only has one thing he casts so we don't have to check cast type like ex does.
                currentState = new EnemyMerged();
                break;
        }
    }
}
