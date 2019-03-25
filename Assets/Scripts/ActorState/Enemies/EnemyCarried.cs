using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCarried : IActorState<EnemyState, EnemyTrigger>
{
    public EnemyState GetState()
    {
        return EnemyState.CARRIED;
    }

    public IActorState<EnemyState, EnemyTrigger> OnUpdate(tk2dSpriteAnimator animator, ref int flags)
    {
        animator.Play(EnemyAnim.GetName(ENEMY_ANIM.CARRIED));
        return null;
    }

    public IActorState<EnemyState, EnemyTrigger> SendTrigger(EnemyTrigger trigger, GameObject actor, tk2dSpriteAnimator animator, ref int flags)
    {
        switch (trigger) {
            case EnemyTrigger.THROWN:
                return new EnemyThrown();
        }
        return null;
    }
}
