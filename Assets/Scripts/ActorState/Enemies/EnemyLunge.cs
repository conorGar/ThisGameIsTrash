﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLunge : IActorState<EnemyState, EnemyTrigger>
{
    public EnemyState GetState()
    {
        return EnemyState.LUNGE;
    }

    public IActorState<EnemyState, EnemyTrigger> OnUpdate(tk2dSpriteAnimator animator, ref int flags)
    {
        return null;
    }

    public IActorState<EnemyState, EnemyTrigger> SendTrigger(EnemyTrigger trigger, GameObject actor, tk2dSpriteAnimator animator, ref int flags)
    {
        switch (trigger) {
            case EnemyTrigger.HIT:
                animator.Play(EnemyAnim.GetName(ENEMY_ANIM.HIT));
                return new EnemyHit();
            case EnemyTrigger.RECOVER:
                animator.Play(EnemyAnim.GetName(ENEMY_ANIM.WAKE)); // TODO: I just picked this one because I thought it would look nice.  Please change!
                return new EnemyRecover();
                break;
        }

        return null;
    }
}