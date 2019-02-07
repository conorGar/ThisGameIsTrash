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
        }

        return null;
    }

}
