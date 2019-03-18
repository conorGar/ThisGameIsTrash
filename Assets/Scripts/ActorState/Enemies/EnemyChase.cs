﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : IActorState<EnemyState, EnemyTrigger>
{
    public EnemyState GetState()
    {
        return EnemyState.CHASE;
    }

    public IActorState<EnemyState, EnemyTrigger> OnUpdate(tk2dSpriteAnimator animator, ref int flags)
    {
        return null;
    }

    public IActorState<EnemyState, EnemyTrigger> SendTrigger(EnemyTrigger trigger, GameObject actor, tk2dSpriteAnimator animator, ref int flags)
    {
        switch (trigger) {
            case EnemyTrigger.IDLE: // This can happen if the enemy is chasing and loses range to the player.  It gets confused and goes back to idle.
                animator.Play(EnemyAnim.GetName(ENEMY_ANIM.IDLE));
                flags &= (int)~EnemyFlag.CHASING;
                return new EnemyIdle();
            /*case EnemyTrigger.HIT:
                animator.Play(EnemyAnim.GetName(ENEMY_ANIM.HIT));
                flags &= ~(int)EnemyFlag.CHASING;
                return new EnemyHit();*/
            case EnemyTrigger.PREPARE:
                animator.Play(EnemyAnim.GetName(ENEMY_ANIM.PREPARE));
				flags &= ~(int)EnemyFlag.CHASING;
            
                return new EnemyPrepare();
			case EnemyTrigger.PREPARE_LUNGE:
                animator.Play(EnemyAnim.GetName(ENEMY_ANIM.PREPARE_LUNGE));
				flags &= ~(int)EnemyFlag.CHASING;
            
                return new EnemyPrepare_Lunge();
			case EnemyTrigger.LUNGE:
                animator.Play(EnemyAnim.GetName(ENEMY_ANIM.LEAP));
				flags &= ~(int)EnemyFlag.CHASING;
                return new EnemyLunge();
            case EnemyTrigger.LEAP:
                flags &= ~(int)EnemyFlag.CHASING;
                return new EnemyLeap();
                /*case EnemyTrigger.CHASE_OBJECT:
                    animator.Play(EnemyAnim.GetName(ENEMY_ANIM.CHASE));
                    flags &= ~(int)EnemyFlag.CHASING;
                    flags |= (int)EnemyFlag.CHASING_OBJECT;
                    return new EnemyChaseObject();*/

        }

        return null;
    }
}