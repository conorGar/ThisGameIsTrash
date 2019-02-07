using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JimIdle : IActorState<JimState, JimTrigger>
{
    public JimState GetState()
    {
        return JimState.IDLE;
    }

    public IActorState<JimState, JimTrigger> OnUpdate(tk2dSpriteAnimator animator, ref int flags)
    {
        if ((flags & (int)JimFlag.MOVING) == (int)JimFlag.MOVING) {
            animator.Play("ani_jimWalk");
        } else {
            animator.Play("ani_jimIdle");
        }

        return null;
    }

    public IActorState<JimState, JimTrigger> SendTrigger(JimTrigger trigger, GameObject actor, tk2dSpriteAnimator animator, ref int flags )
    {
        switch (trigger) {
            case JimTrigger.HIT:
                animator.Play("hurt");
                return new JimHurt();

            case JimTrigger.SWING_RIGHT:
                animator.Play("ani_jimSwingR");
                flags &= ~(int)JimFlag.FACING_LEFT;
                return new JimAttacking();

            case JimTrigger.SWING_LEFT:
                animator.Play("ani_jimSwingR");
                flags |= (int)JimFlag.FACING_LEFT;
                return new JimAttacking();

            case JimTrigger.SWING_UP:
                animator.Play("ani_jimSwingUp");
                return new JimAttacking();

            case JimTrigger.SWING_DOWN:
                animator.Play("ani_jimSwingDown");
                return new JimAttacking();

            case JimTrigger.PICK_UP_SMALL:
                animator.Play("ani_jimPickUp");
                return new JimPickingUp();

            case JimTrigger.PICK_UP_THROWABLE:
                animator.Play("ani_jimPickUp");
                flags = (int)JimFlag.CARRYING_THROWABLE;
                return new JimPickingUp();

            case JimTrigger.PICK_UP_LARGE_TRASH:
                animator.Play("ani_jimPickUp");
                flags = (int)JimFlag.CARRYING_DROPABLE;
                return new JimPickingUp();
        }

        return null;
    }
}
