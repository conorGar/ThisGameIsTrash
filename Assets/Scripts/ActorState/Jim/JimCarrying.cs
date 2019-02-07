using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JimCarrying : IActorState<JimState, JimTrigger>
{
    public JimState GetState()
    {
        return JimState.CARRYING;
    }

    public IActorState<JimState, JimTrigger> OnUpdate(tk2dSpriteAnimator animator, ref int flags)
    {
        if ((flags & (int)JimFlag.MOVING) == (int)JimFlag.MOVING) {
            animator.Play("ani_jimCarryAboveWalk");
        } else {
            animator.Play("ani_jimCarryAboveIdle");
        }

        return null;
    }

    public IActorState<JimState, JimTrigger> SendTrigger(JimTrigger trigger, tk2dSpriteAnimator animator, ref int flags)
    {
        // Jim is carrying something that can be dropped or delivered, like large trash.
        if ((flags & (int)JimFlag.CARRYING_DROPABLE) == (int)JimFlag.CARRYING_DROPABLE) {
            switch (trigger) {
                case JimTrigger.DELIVER_BIG:
                    animator.Play("excitedJump");
                    flags = (int)JimFlag.NONE;
                    return new JimDelivering();

                case JimTrigger.DROP_BIG:
                    animator.Play("ani_jimIdle");
                    flags = (int)JimFlag.NONE;
                    return new JimIdle();
            }
        
        // Jim is carrying something that can be thrown, like enemies!
        } else if ((flags & (int)JimFlag.CARRYING_THROWABLE) == (int)JimFlag.CARRYING_THROWABLE) {
            switch (trigger) {
                case JimTrigger.THROW_RIGHT:
                    animator.Play("ani_jimThrowR");
                    flags = (int)JimFlag.NONE;
                    return new JimThrowing();

                case JimTrigger.THROW_LEFT:
                    animator.Play("ani_jimThrowL");
                    flags = (int)JimFlag.NONE;
                    return new JimThrowing();
            }
        }

        return null;
    }

}
