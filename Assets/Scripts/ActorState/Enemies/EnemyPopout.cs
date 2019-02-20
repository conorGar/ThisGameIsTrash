using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyPopout : IActorState<EnemyState, EnemyTrigger>
{

	public EnemyState GetState()
    {
        return EnemyState.HIT;
    }

    public IActorState<EnemyState, EnemyTrigger> OnUpdate(tk2dSpriteAnimator animator, ref int flags)
	{ 
		animator.Play(EnemyAnim.GetName(ENEMY_ANIM.POP_UP));
        return null;
    }

    public IActorState<EnemyState, EnemyTrigger> SendTrigger(EnemyTrigger trigger, GameObject actor, tk2dSpriteAnimator animator, ref int flags)
    {
		switch (trigger) {
            case EnemyTrigger.HIT:
                animator.Play(EnemyAnim.GetName(ENEMY_ANIM.HIT));
				Debug.Log("-x-x-x-x-x-x-x- Enemy Popout Hit trigger activate -x-x-x-x-x-");

                return new EnemyHit();
        }
        return null;
    }
}

