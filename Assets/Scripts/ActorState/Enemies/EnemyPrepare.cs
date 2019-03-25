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
		switch (trigger) {
            case EnemyTrigger.LUNGE: //added because of Rhino Beetle
                animator.Play(EnemyAnim.GetName(ENEMY_ANIM.LUNGE));
				Debug.Log("-x-x-x-x-x-x-x- Enemy Prepare Leap trigger activate -x-x-x-x-x-");
                return new EnemyLunge();
			
        }
        return null;

    }
}
