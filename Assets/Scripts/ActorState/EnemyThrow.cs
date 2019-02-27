using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyThrow : IActorState<EnemyState, EnemyTrigger>
{
	public EnemyState GetState()
    {
        return EnemyState.THROW;
    }

    public IActorState<EnemyState, EnemyTrigger> OnUpdate(tk2dSpriteAnimator animator, ref int flags)
	{ 
		animator.Play(EnemyAnim.GetName(ENEMY_ANIM.THROW));
        return null;
    }

    public IActorState<EnemyState, EnemyTrigger> SendTrigger(EnemyTrigger trigger, GameObject actor, tk2dSpriteAnimator animator, ref int flags)
    {
		switch (trigger) {
            case EnemyTrigger.HIT:
                animator.Play(EnemyAnim.GetName(ENEMY_ANIM.HIT));
                Debug.Log("-x-x-x-x-x-x-x- Enemy Throw Hit trigger activate -x-x-x-x-x-");
                return new EnemyHit();

        }
        return null;
    }
}

