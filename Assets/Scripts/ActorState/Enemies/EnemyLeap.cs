using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLeap : IActorState<EnemyState, EnemyTrigger>
{
    public EnemyState GetState()
    {
        return EnemyState.LEAP;
    }

    public IActorState<EnemyState, EnemyTrigger> OnUpdate(tk2dSpriteAnimator animator, ref int flags)
    {
        animator.Play(EnemyAnim.GetName(ENEMY_ANIM.LEAP));
        return null;
    }

    public IActorState<EnemyState, EnemyTrigger> SendTrigger(EnemyTrigger trigger, GameObject actor, tk2dSpriteAnimator animator, ref int flags)
    {
        switch (trigger) {
            case EnemyTrigger.IDLE:
                return new EnemyIdle();
			case EnemyTrigger.RECOVER:
				animator.Play(EnemyAnim.GetName(ENEMY_ANIM.WAKE));
                return new EnemyRecover();
        }

        return null;
    }
}