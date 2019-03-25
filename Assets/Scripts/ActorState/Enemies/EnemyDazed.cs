using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDazed : IActorState<EnemyState, EnemyTrigger>
{
    public EnemyState GetState()
    {
        return EnemyState.DAZED;
    }

    public IActorState<EnemyState, EnemyTrigger> OnUpdate(tk2dSpriteAnimator animator, ref int flags)
    {
        animator.Play(EnemyAnim.GetName(ENEMY_ANIM.DAZED));
        return null;
    }

    public IActorState<EnemyState, EnemyTrigger> SendTrigger(EnemyTrigger trigger, GameObject actor, tk2dSpriteAnimator animator, ref int flags)
    {
        switch (trigger) {
            case EnemyTrigger.CARRIED:
                return new EnemyCarried();
            case EnemyTrigger.IDLE:
                return new EnemyIdle();
        }
        return null;
    }
}
