using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : IActorState<EnemyState, EnemyTrigger>
{
    public bool hasMultipleAnimations; // If there's a left and right chase animation.

    public EnemyChase(bool p_hasMultipleAnimations = false)
    {
        hasMultipleAnimations = p_hasMultipleAnimations;
    }

    public EnemyState GetState()
    {
        return EnemyState.CHASE;
    }

    public IActorState<EnemyState, EnemyTrigger> OnUpdate(tk2dSpriteAnimator animator, ref int flags)
    {
        if (hasMultipleAnimations) {
            animator.Play(EnemyAnim.GetName(ENEMY_ANIM.CHASE) + (PlayerManager.Instance.IsLeft(animator.gameObject) ? "L" : "R"));
        } else {
            animator.Play(EnemyAnim.GetName(ENEMY_ANIM.CHASE));
        }
        return null;
    }

    public IActorState<EnemyState, EnemyTrigger> SendTrigger(EnemyTrigger trigger, GameObject actor, tk2dSpriteAnimator animator, ref int flags)
    {
        switch (trigger) {
            case EnemyTrigger.IDLE: // This can happen if the enemy is chasing and loses range to the player.  It gets confused and goes back to idle.
                flags &= (int)~EnemyFlag.CHASING;
                return new EnemyIdle();
            case EnemyTrigger.HIT:
                flags &= ~(int)EnemyFlag.CHASING;
                return new EnemyHit();
            case EnemyTrigger.PREPARE:
                return new EnemyPrepare();
            case EnemyTrigger.PREPARE_LEAP:
                flags &= ~(int)EnemyFlag.CHASING;
                return new EnemyPrepare_Leap();
        }

        return null;
    }
}