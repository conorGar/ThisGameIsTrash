using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuartStateController : EnemyStateController
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
                RemoveFlag((int)EnemyFlag.CHASING);
                currentState = new EnemyDead();
                break;
            case EnemyTrigger.INVULNERABLE:
                SetFlag((int)EnemyFlag.INVULNERABLE);
                break;
            case EnemyTrigger.VULNERABLE:
                RemoveFlag((int)EnemyFlag.INVULNERABLE);
                break;
        }
    }

    protected override void AnimationEventCompleted(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip)
    {
        base.AnimationEventCompleted(animator, clip);

        switch (currentState.GetState()) {
            case EnemyState.HIT:
                GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
                SetFlag((int)EnemyFlag.CHASING);
                currentState = new EnemyChase();
                break;
        }
    }

}
