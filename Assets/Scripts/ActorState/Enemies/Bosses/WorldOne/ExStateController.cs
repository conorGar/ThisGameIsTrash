using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(B_Ev_Ex))]
public class ExStateController : EnemyStateController
{
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
                GetComponent<B_Ev_Ex>().Dazed();
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
                animator.Play(EnemyAnim.GetName(ENEMY_ANIM.IDLE));
                currentState = new EnemyTeleport();
                GetComponent<B_Ev_Ex>().Teleport();
                break;
            case EnemyState.CAST:
                var state = (EnemyCast)currentState;

                switch (state.GetCastType()) {
                    case CAST_TYPE.TELEPORT:
                        currentState = new EnemyTeleport();
                        GetComponent<B_Ev_Ex>().Teleport();
                        break;
                    case CAST_TYPE.SPAWN_ADD:
                        GetComponent<B_Ev_Ex>().SpawnBlob();
                        break;
                }
                break;
        }
    }

}