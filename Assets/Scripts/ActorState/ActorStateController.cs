using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls the abstract state of an actor in the game.  These actors could be Jim, or an enemy, or a friend.  Something that walks around the game
// and needs to maintain their state.
[RequireComponent(typeof(tk2dSpriteAnimator))]
public class ActorStateController<State_Type, Trigger_Type> : MonoBehaviour
{
    protected tk2dSpriteAnimator animator;

    protected int flags;

    [SerializeField]
    public IActorState<State_Type, Trigger_Type> currentState;

    protected void Awake()
    {
        animator = GetComponent<tk2dSpriteAnimator>();
        //animator.AnimationEventTriggered = AnimationEventCallback;
        animator.AnimationCompleted = AnimationEventCompleted;
    }

    public void Update()
    {
        // monitor flags for animation and state updates.
        currentState.OnUpdate(animator, ref flags);
    }

    public virtual void SendTrigger(Trigger_Type trigger)
    {
        Debug.Log("SendTrigger: " + trigger + " current State: " + currentState.GetState());

        // Resolve triggers that don't care about states and just do stuff.
        AnyStateTrigger(trigger);

        // Resolve triggers based on the current state.
        var newState = currentState.SendTrigger(trigger, gameObject, animator, ref flags);

        if (newState != null)
            currentState = newState;
    }

    public State_Type GetCurrentState()
    {
        return currentState.GetState();
    }

    public bool IsFlag(int flag)
    {
        return (flags & flag) == flag; 
    }

    public void SetFlag(int flag)
    {
        flags |= flag;
    }

    public void RemoveFlag(int flag)
    {
        flags &= ~flag;
    }

    protected virtual void AnyStateTrigger(Trigger_Type trigger)
    {
        // nothing for base class
    }

    protected virtual void AnimationEventCallback(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip, int frameNo)
    {

    }

    protected virtual void AnimationEventCompleted(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip)
    {

    }
}
