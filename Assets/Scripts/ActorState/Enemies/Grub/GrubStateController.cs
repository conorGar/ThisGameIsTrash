﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GenericEnemyStateController = EnemyStateController<EnemyState, EnemyTrigger>;

public class GrubStateController : GenericEnemyStateController {

	public Ev_Enemy_Grub grubBehavior;

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
                GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
                animator.Play(EnemyAnim.GetName(ENEMY_ANIM.CHASE));
                SetFlag((int)EnemyFlag.CHASING);
                currentState = new EnemyChase();
                break;
            case EnemyState.PREPARE: //prepare for leap
				animator.Play(EnemyAnim.GetName(ENEMY_ANIM.LEAP));
                currentState = new EnemyLunge();
            	break;
			case EnemyState.LUNGE: //hit the ground after Jump
				animator.Play(EnemyAnim.GetName(ENEMY_ANIM.LEAP));
                currentState = new EnemyIdle(); //EnemyIdle()? for when stuck in ground??
            	break;
            case EnemyState.THROW:
            	break;
        }
    }


	// Callbacks
    protected override void AnimationEventCallback(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip, int frameNo)
    {
        var frame = clip.GetFrame(frameNo);
#if DEBUG_ANIMATION
        Debug.Log("Animation Trigger Check: " + frame.eventInfo + " Clip Name: " + clip.name + " Frame No: " + frameNo);
#endif

        switch (frame.eventInfo) {
            case "SPIT":
				currentState = new EnemyThrow();
				grubBehavior.Spit();
                break;
            default:
#if DEBUG_ANIMATION
                Debug.Log("Animation Trigger Not Found: " + frame.eventInfo);
#endif
                break;
        }
    }
}
