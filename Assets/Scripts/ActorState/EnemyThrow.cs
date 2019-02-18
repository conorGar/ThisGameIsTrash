using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyThrow : IActorState<EnemyState, EnemyTrigger>
{
	public EnemyState GetState()
    {
        return EnemyState.HIT;
    }

    public IActorState<EnemyState, EnemyTrigger> OnUpdate(tk2dSpriteAnimator animator, ref int flags)
    {
        return null;
    }

    public IActorState<EnemyState, EnemyTrigger> SendTrigger(EnemyTrigger trigger, GameObject actor, tk2dSpriteAnimator animator, ref int flags)
    {
        return null;
    }
}

