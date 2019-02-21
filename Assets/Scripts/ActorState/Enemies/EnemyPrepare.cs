using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPrepare : IActorState<EnemyState, EnemyTrigger>
{
    public EnemyState GetState()
    {
        return EnemyState.PREPARE;
    }

    public IActorState<EnemyState, EnemyTrigger> OnUpdate(tk2dSpriteAnimator animator, ref int flags)
    {
		animator.Play(EnemyAnim.GetName(ENEMY_ANIM.PREPARE));
        return null;
    }

    public IActorState<EnemyState, EnemyTrigger> SendTrigger(EnemyTrigger trigger, GameObject actor, tk2dSpriteAnimator animator, ref int flags)
    {
        return null;
    }
}
