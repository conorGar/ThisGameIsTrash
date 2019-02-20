using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class EnemyPrepare_Leap : IActorState<EnemyState, EnemyTrigger>
{

	public EnemyState GetState()
    {
        return EnemyState.PREPARE_LEAP;
    }

    public IActorState<EnemyState, EnemyTrigger> OnUpdate(tk2dSpriteAnimator animator, ref int flags)
    {
		animator.Play(EnemyAnim.GetName(ENEMY_ANIM.PREPARE_LEAP));
        return null;
    }

    public IActorState<EnemyState, EnemyTrigger> SendTrigger(EnemyTrigger trigger, GameObject actor, tk2dSpriteAnimator animator, ref int flags)
    {
        return null;
    }
}

