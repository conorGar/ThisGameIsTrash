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
                return new EnemyHit();
			case EnemyTrigger.POWER_HIT:
                animator.Play(EnemyAnim.GetName(ENEMY_ANIM.HIT));
                return new EnemyPowerHit();
            case EnemyTrigger.NOTICE:
                flags |= (int)EnemyFlag.CHASING;
                return new EnemyChase();
			case EnemyTrigger.THROW:
                animator.Play(EnemyAnim.GetName(ENEMY_ANIM.THROW));
                return new EnemyThrow();
            case EnemyTrigger.PREPARE:
				animator.Play(EnemyAnim.GetName(ENEMY_ANIM.PREPARE));
                return new EnemyPrepare();
			case EnemyTrigger.PREPARE_LEAP:
                return new EnemyPrepare_Leap();
            case EnemyTrigger.CAST_TELEPORT: // Cast -> Teleport
                return new EnemyCast(CAST_TYPE.TELEPORT);
            case EnemyTrigger.CAST_SPAWN_BLOB:
                return new EnemyCast(CAST_TYPE.SPAWN_ADD, "enemy_slime");
            case EnemyTrigger.CAST_SHIELD:
                return new EnemyCast(CAST_TYPE.SHIELD);
            case EnemyTrigger.VULNERABLE: //added for Grub Enemy
                return new EnemyVulnerable();
			case EnemyTrigger.LUNGE:
                return new EnemyLunge();
			case EnemyTrigger.LEAP:
                return new EnemyLeap();
		
        }

        return null;
    }
}