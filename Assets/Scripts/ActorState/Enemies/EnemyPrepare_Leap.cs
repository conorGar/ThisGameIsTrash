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
		switch (trigger) {
			case EnemyTrigger.HIT:
                animator.Play(EnemyAnim.GetName(ENEMY_ANIM.HIT));
                return new EnemyHit();
			case EnemyTrigger.POWER_HIT:
                animator.Play(EnemyAnim.GetName(ENEMY_ANIM.HIT));
				Debug.Log("-x-x-x-x-x-x-x- Enemy Idle Hit trigger activate -x-x-x-x-x-");

                return new EnemyPowerHit();
        }
        return null;
    }
}

