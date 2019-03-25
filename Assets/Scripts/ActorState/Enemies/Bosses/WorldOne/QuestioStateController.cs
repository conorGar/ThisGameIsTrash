using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(B_Ev_Questio))]
public class QuestioStateController : EnemyStateController
{
    bool isFacingLeftOnLunge; // yeah...

    protected new void Awake()
    {
        defaultState = new EnemyIdle();
        base.Awake();
    }

    protected override void AnyStateTrigger(EnemyTrigger trigger)
    {
        switch (trigger) {
            case EnemyTrigger.DEATH: // On death this guy becomes throwable and dazed.
                ClearFlags();
                SetFlag((int)EnemyFlag.THROWABLE);
                GetComponent<B_Ev_Questio>().Dazed();
                currentState = new EnemyDazed();
                break;
        }
    }

    protected override void AnimationEventCompleted(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip)
    {
        base.AnimationEventCompleted(animator, clip);

        switch (currentState.GetState()) {
            case EnemyState.HIT:
                GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
                currentState = new EnemyIdle();
                break;
            case EnemyState.PREPARE_LEAP:
                isFacingLeftOnLunge = PlayerManager.Instance.IsLeft(gameObject);
                Debug.Log("IS FACING LEFT ON LUNGE: " + isFacingLeftOnLunge);
                currentState = new EnemyLunge(true, isFacingLeftOnLunge);
                GetComponent<B_Ev_Questio>().Leap();
                break;
            case EnemyState.RECOVER:
                GetComponent<B_Ev_Questio>().Recovered();
                currentState = new EnemyIdle();
                break;
        }
    }

    // Used to tweak specific state variables after a trigger occurs (like multiple chase and swing animations).  Almost certain I hate this but it's pretty clean otherwise. -Steve
    public override void SendTrigger(EnemyTrigger trigger)
    {
        base.SendTrigger(trigger);

        switch (currentState.GetState()) {
            case EnemyState.CHASE:
                ((EnemyChase)currentState).hasMultipleAnimations = true;
                break;
            case EnemyState.PREPARE_LEAP:
                ((EnemyPrepare_Leap)currentState).hasMultipleAnimations = true;
                ((EnemyPrepare_Leap)currentState).facingLeft = PlayerManager.Instance.IsLeft(gameObject);
                break;
            case EnemyState.RECOVER:
                ((EnemyRecover)currentState).hasMultipleAnimations = true;
                ((EnemyRecover)currentState).facingLeft = isFacingLeftOnLunge;
                break;
        }
    }

    public override bool IsHittable()
    {
        // No hitting on the lunge (leap)
        if (currentState.GetState() == EnemyState.LUNGE)
            return false;

        return base.IsHittable();
    }
}
