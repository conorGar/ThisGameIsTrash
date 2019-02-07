using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JimHurt : IActorState<JimState, JimTrigger>
{
    public JimState GetState()
    {
        return JimState.HURT;
    }

    public IActorState<JimState, JimTrigger> OnUpdate(tk2dSpriteAnimator animator, ref int flags)
    {
        return null;
    }

    public IActorState<JimState, JimTrigger> SendTrigger(JimTrigger trigger, GameObject actor, tk2dSpriteAnimator animator, ref int flags)
    {
        return null;
    }

}
