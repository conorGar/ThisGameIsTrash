using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActorState<State_Type, Trigger_Type> {
    State_Type GetState();
    IActorState<State_Type, Trigger_Type> SendTrigger(Trigger_Type T, tk2dSpriteAnimator animator, ref int flags);
    IActorState<State_Type, Trigger_Type> OnUpdate(tk2dSpriteAnimator animator, ref int flags);
}
