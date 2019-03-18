using System.Collections;
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
				Debug.Log("-x-x-x-x-x-x-x- Enemy Idle Hit trigger activate -x-x-x-x-x-");

                return new EnemyHit();
			case EnemyTrigger.POWER_HIT:
                animator.Play(EnemyAnim.GetName(ENEMY_ANIM.HIT));
				Debug.Log("-x-x-x-x-x-x-x- Enemy Idle Hit trigger activate -x-x-x-x-x-");

                return new EnemyPowerHit();
            case EnemyTrigger.NOTICE:
                animator.Play(EnemyAnim.GetName(ENEMY_ANIM.CHASE));
                flags |= (int)EnemyFlag.CHASING;
                return new EnemyChase();
			case EnemyTrigger.THROW:
                animator.Play(EnemyAnim.GetName(ENEMY_ANIM.THROW));
               // flags |= (int)EnemyFlag.CHASING;
                return new EnemyThrow();
            case EnemyTrigger.PREPARE:
				animator.Play(EnemyAnim.GetName(ENEMY_ANIM.PREPARE));
               // flags |= (int)EnemyFlag.CHASING;
                return new EnemyPrepare();
			case EnemyTrigger.PREPARE_LEAP:
				animator.Play(EnemyAnim.GetName(ENEMY_ANIM.PREPARE_LEAP));
                return new EnemyPrepare_Leap();
			case EnemyTrigger.LUNGE:
				animator.Play(EnemyAnim.GetName(ENEMY_ANIM.LEAP));
                return new EnemyLunge();
			case EnemyTrigger.VULNERABLE: //added for Grub Enemy
				animator.Play(EnemyAnim.GetName(ENEMY_ANIM.VULNERABLE));
                return new EnemyVulnerable();
        }

        return null;
    }
}