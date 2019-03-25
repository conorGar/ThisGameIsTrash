using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enemy is merged with another enemy.  Like Hash being part of Stuart in the boss battle.
public class EnemyMerged : IActorState<EnemyState, EnemyTrigger>
{
    public EnemyState GetState()
    {
        return EnemyState.MERGED;
    }

    public IActorState<EnemyState, EnemyTrigger> OnUpdate(tk2dSpriteAnimator animator, ref int flags)
    {
        animator.Play(EnemyAnim.GetName(ENEMY_ANIM.MERGED));
        return null;
    }

    public IActorState<EnemyState, EnemyTrigger> SendTrigger(EnemyTrigger trigger, GameObject actor, tk2dSpriteAnimator animator, ref int flags)
    {
        switch (trigger) {
            case EnemyTrigger.HIT:
                return new EnemyHit();
        }

        return null;
    }
}
