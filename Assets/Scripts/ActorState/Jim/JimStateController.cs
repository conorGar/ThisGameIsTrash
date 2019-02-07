using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum JimState
{
    NONE,
    IDLE,
    HURT,
    ATTACKING,
    PICKING_UP,
    CARRYING,
    THROWING,
    DELIVERING,
    DEAD
}

[Serializable]
public enum JimFlag : int
{
    NONE = 0,
    MOVING = 1 << 0,
    FACING_LEFT = 1 << 1,
    CARRYING_THROWABLE = 1 << 2,
    CARRYING_DROPABLE = 1 << 3,
}

[Serializable]
public enum JimTrigger
{
    HIT,
    DEATH,
    SWING_RIGHT,
    SWING_LEFT,
    SWING_UP,
    SWING_DOWN,
    THROW_RIGHT,
    THROW_LEFT,
    SPIN_ATTACK,
    PICK_UP_SMALL,
    PICK_UP_LARGE_TRASH,
    PICK_UP_THROWABLE,
    DROP_BIG,
    DELIVER_BIG
}

public class JimStateController : ActorStateController<JimState, JimTrigger> {
    protected new void Awake()
    {
        currentState = new JimIdle();
        base.Awake();
    }

    protected override void AnyStateTrigger(JimTrigger trigger)
    {
        switch (trigger) {
            case JimTrigger.DEATH:
                animator.Play("death");
                currentState = new JimDead();
                break;
        }
    }

    protected override void AnimationEventCompleted(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip)
    {
#if DEBUG_ANIMATION
        Debug.Log("Animation Completed: Clip Name: " + clip.name);
#endif
        switch (currentState.GetState()) {
            case JimState.ATTACKING:
                currentState = new JimIdle();
                break;
            case JimState.HURT:
                GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
                currentState = new JimIdle();
                break;
            case JimState.PICKING_UP:
                // carrying a droppable
                if ((flags & (int)JimFlag.CARRYING_DROPABLE) == (int)JimFlag.CARRYING_DROPABLE) {
                    animator.Play("jimCarryAboveIdle");
                    currentState = new JimCarrying();

                    // carrying a throwable
                } else if ((flags & (int)JimFlag.CARRYING_THROWABLE) == (int)JimFlag.CARRYING_THROWABLE) {
                    animator.Play("ani_jimCarryAboveIdle");
                    currentState = new JimCarrying();

                    // Carried small trash.  Go back to idle.
                } else {
                    currentState = new JimIdle();
                }
                break;
        }
    }
}