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
            	Debug.Log("Popup hit trigger...triggered.");
                return new EnemyHit();
        }
        return null;
    }
}

