﻿using UnityEngine;
using System.Collections;

public class FlamingPhilStateController : EnemyStateController
{

	//moving flag - bounce around screen
	//PREPARE - charge up shot
	//THROW - fire projectiles in four directions

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

        switch (currentState.GetState()) {
            case EnemyState.HIT:
                GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
                currentState = new EnemyIdle();
                break;
			case EnemyState.POWER_HIT:
                GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
                currentState = new EnemyIdle();
                break;
			case EnemyState.THROW:
                GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
                currentState = new EnemyIdle();
                break;
        }
    }
}

