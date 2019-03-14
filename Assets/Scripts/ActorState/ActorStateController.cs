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
    protected IActorState<State_Type, Trigger_Type> defaultState;

    protected void Awake()
    {
        animator = GetComponent<tk2dSpriteAnimator>();
        currentState = defaultState;
        flags = 0;
        animator.AnimationCompleted = AnimationEventCompleted;
		animator.AnimationEventTriggered = AnimationEventCallback;
    }

    protected void OnEnable()
    {
        currentState = defaultState;
        flags = 0;

#if DEBUG_ACTORS
        Debug.Log("Actor Enabled: " + name);
#endif
    }

    protected void OnDisable()
    {
#if DEBUG_ACTORS
        Debug.Log("Actor Disabled: " + name);
#endif
    }

    public void Update()
    {
        // monitor flags for animation and state updates.
        currentState.OnUpdate(animator, ref flags);
    }

    public virtual void SendTrigger(Trigger_Type trigger)
    {
        Debug.Log("SendTrigger: " + trigger + " current State: " + currentState.GetState() + " for object [" + name + "]");

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

    // returns true if any of the flags passed in match.
    public bool IsFlag(int flag)
    {
        return (flags & flag) > 0; 
    }

    public void SetFlag(int flag)
    {
        flags |= flag;
    }

    public void RemoveFlag(int flag)
    {
        flags &= ~flag;
    }

    public void ClearFlags()
    {
        flags = 0;
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
#if DEBUG_ANIMATION
        Debug.Log("Animation Completed: Clip Name: " + clip.name + " for object [" + name + "]");
#endif
    }
}
