﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckStateController : EnemyStateController
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
			case EnemyTrigger.HIT:
                animator.Play(EnemyAnim.GetName(ENEMY_ANIM.HIT));
                RemoveFlag((int)EnemyFlag.CHASING);
                currentState = new EnemyHit();
                break;
			case EnemyTrigger.LUNGE:
                animator.Play(EnemyAnim.GetName(ENEMY_ANIM.LUNGE));
				RemoveFlag((int)EnemyFlag.MOVING);
                currentState = new EnemyLunge();
                break;
			case EnemyTrigger.CHASE:
				flags |= (int)EnemyFlag.CHASING;
                currentState = new EnemyChase();
                break;
        }
    }

    protected override void AnimationEventCompleted(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip)
    {
        base.AnimationEventCompleted(animator, clip);

        switch (currentState.GetState()) {
            case EnemyState.HIT:
                //GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
                //animator.Play(EnemyAnim.GetName(ENEMY_ANIM.CHASE));
                //SetFlag((int)EnemyFlag.CHASING);
                currentState = new EnemyIdle();
                break;
			case EnemyState.LUNGE:
                currentState = new EnemyIdle();

                break;
        }
    }
}
