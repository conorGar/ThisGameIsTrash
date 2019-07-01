using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrubStateController : EnemyStateController
{



	protected new void Awake()
    {
        defaultState = new EnemyPopout();
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
        Debug.Log("ani event completed" + currentState);
        switch (currentState.GetState()) {
			case EnemyState.POPOUT: 
				Debug.Log("Grub changes from popout to idle");
				animator.Play(EnemyAnim.GetName(ENEMY_ANIM.IDLE));
                currentState = new EnemyIdle();
            	break;
            case EnemyState.PREPARE: //prepare for throw
				animator.Play(EnemyAnim.GetName(ENEMY_ANIM.THROW));
                currentState = new EnemyThrow();
            	break;
			case EnemyState.PREPARE_LEAP: //prepare for leap
				animator.Play(EnemyAnim.GetName(ENEMY_ANIM.LUNGE));
                currentState = new EnemyLunge();
            	break;
            case EnemyState.THROW:
				animator.Play(EnemyAnim.GetName(ENEMY_ANIM.RECOVER));
                currentState = new EnemyIdle();
            	break;
            case EnemyState.HIT:
                GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
                currentState = new EnemyVulnerable();
                break;
			case EnemyState.CHASE:
                GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
                currentState = new EnemyIdle();
                break;
        }
    }


	// Callbacks
    protected override void AnimationEventCallback(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip, int frameNo)
    {
       /* var frame = clip.GetFrame(frameNo);
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
        }*/
    }
}
