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
            if ((flags & (int)JimFlag.CARRYING_DROPABLE) == (int)JimFlag.CARRYING_DROPABLE)
                animator.Play("ani_jimCarryWalk");

            else if ((flags & (int)JimFlag.CARRYING_THROWABLE) == (int)JimFlag.CARRYING_THROWABLE)
                animator.Play("ani_jimCarryAboveWalk");
        } else {
            if ((flags & (int)JimFlag.CARRYING_DROPABLE) == (int)JimFlag.CARRYING_DROPABLE)
                animator.Play("ani_jimCarryIdle");

            else if ((flags & (int)JimFlag.CARRYING_THROWABLE) == (int)JimFlag.CARRYING_THROWABLE)
                animator.Play("ani_jimCarryAboveIdle");
        }

        return null;
    }

    public IActorState<JimState, JimTrigger> SendTrigger(JimTrigger trigger, GameObject actor, tk2dSpriteAnimator animator, ref int flags)
    {
        // Jim is carrying something that can be dropped or delivered, like large trash.
        if ((flags & (int)JimFlag.CARRYING_DROPABLE) == (int)JimFlag.CARRYING_DROPABLE) {
            switch (trigger) {
                case JimTrigger.DELIVER_BIG:
                    animator.Play("excitedJump");
                    flags &= (int)~JimFlag.CARRYING_DROPABLE;
                    return new JimDelivering();

                case JimTrigger.DROP_BIG:
                    animator.Play("ani_jimIdle");
                    flags &= (int)~JimFlag.CARRYING_DROPABLE;
                    return new JimIdle();
            }

            // Jim is carrying something that can be thrown, like enemies!
        } else if ((flags & (int)JimFlag.CARRYING_THROWABLE) == (int)JimFlag.CARRYING_THROWABLE) {
            switch (trigger) {
                case JimTrigger.THROW:
                animator.Play("ani_jimThrowR");
                flags &= (int)~JimFlag.CARRYING_THROWABLE;
                return new JimThrowing();
            }
        }

        return null;
    }

}
