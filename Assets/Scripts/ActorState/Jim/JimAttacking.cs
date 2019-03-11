using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JimAttacking : IActorState<JimState, JimTrigger>
{

    public JimState GetState()
    {
        return JimState.ATTACKING;
    }

    public IActorState<JimState, JimTrigger> OnUpdate(tk2dSpriteAnimator animator, ref int flags)
    {
        return null;
    }

    public IActorState<JimState, JimTrigger> SendTrigger(JimTrigger trigger, GameObject actor, tk2dSpriteAnimator animator, ref int flags)
    {
        switch (trigger) {
            case JimTrigger.HIT:
                animator.Play("hurt");
                return new JimHurt();
			case JimTrigger.CHARGE_RIGHT:
                animator.Play("ani_jimChargeR");
                flags &= ~(int)JimFlag.FACING_LEFT;
                return new JimCharging();

            case JimTrigger.CHARGE_LEFT:
                animator.Play("ani_jimChargeR");
                flags |= (int)JimFlag.FACING_LEFT;
                return new JimCharging();

            case JimTrigger.CHARGE_UP:
                animator.Play("ani_jimChargeUp");
                return new JimCharging();

            case JimTrigger.CHARGE_DOWN:
                animator.Play("ani_jimChargeDown");
                return new JimCharging();

        }

        return null;
    }

}
