using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JimDelivering : IActorState<JimState, JimTrigger>
{
    public JimState GetState()
    {
        return JimState.DELIVERING;
    }

    public IActorState<JimState, JimTrigger> OnUpdate(tk2dSpriteAnimator animator, ref int flags)
    {
        return null;
    }

    public IActorState<JimState, JimTrigger> SendTrigger(JimTrigger trigger, tk2dSpriteAnimator animator, ref int flags)
    {
        return null;
    }

}
