﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdle : IActorState<EnemyState, EnemyTrigger>
{
    public EnemyState GetState()
    {
        return EnemyState.IDLE;
    }

    public IActorState<EnemyState, EnemyTrigger> OnUpdate(tk2dSpriteAnimator animator, ref int flags)
    {
        if ((flags & (int)EnemyFlag.WALKING) == (int)EnemyFlag.WALKING) {
            animator.Play(EnemyAnim.GetName(ENEMY_ANIM.RUN));
        } else {
            animator.Play(EnemyAnim.GetName(ENEMY_ANIM.IDLE));
        }

        return null;
    }

    public IActorState<EnemyState, EnemyTrigger> SendTrigger(EnemyTrigger trigger, GameObject actor, tk2dSpriteAnimator animator, ref int flags)
    {
        switch (trigger) {
            case EnemyTrigger.HIT:
                animator.Play(EnemyAnim.GetName(ENEMY_ANIM.HIT));
                return new EnemyHit();
            case EnemyTrigger.NOTICE:
                animator.Play(EnemyAnim.GetName(ENEMY_ANIM.CHASE));
                flags |= (int)EnemyFlag.CHASING;
                return new EnemyChase();
        }

        return null;
    }
}