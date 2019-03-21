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
    DEAD,
    CHARGING,
    DASHING
}

public enum JimFlag : int
{
    NONE = 0,
    MOVING = 1 << 0,
    FACING_LEFT = 1 << 1,
    CARRYING_THROWABLE = 1 << 2,
    CARRYING_DROPABLE = 1 << 3,
    INVULNERABLE = 1 << 4,
}

public enum JimTrigger
{
    IDLE,
    HIT,
    DEATH,
    SWING_RIGHT,
    SWING_LEFT,
    SWING_UP,
    SWING_DOWN,
    CHARGE_RIGHT,
    CHARGE_LEFT,
    CHARGE_UP,
    CHARGE_DOWN,
    THROW,
    SPIN_ATTACK,
    PICK_UP_SMALL,
    PICK_UP_DROPPABLE,
    PICK_UP_THROWABLE,
    DROP_BIG,
    DELIVER_BIG, 
    DASH,
    INVULNERABLE
}

public class JimStateController : ActorStateController<JimState, JimTrigger> {
    protected new void Awake()
    {
        defaultState = new JimIdle();
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
        base.AnimationEventCompleted(animator, clip);

        switch (currentState.GetState()) {
            case JimState.ATTACKING:
                currentState = new JimIdle();
                break;
           
            case JimState.HURT:
				flags |= (int)JimFlag.INVULNERABLE;
                GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
                currentState = new JimIdle();
                break;
            case JimState.PICKING_UP:
                // carrying a droppable
                if ((flags & (int)JimFlag.CARRYING_DROPABLE) == (int)JimFlag.CARRYING_DROPABLE) {
                    animator.Play("ani_jimCarryIdle");
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
            case JimState.THROWING:
                currentState = new JimIdle();
                break;
        }
    }
}