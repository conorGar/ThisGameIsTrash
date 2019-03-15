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
			case EnemyTrigger.LUNGE: //added because of grub, if causes isses change Ev_Enmy_grub2
                animator.Play(EnemyAnim.GetName(ENEMY_ANIM.LEAP));
                Debug.Log("-x-x-x-x-x-x-x- Enemy Throw Hit trigger activate -x-x-x-x-x-");
                return new EnemyLunge();
			case EnemyTrigger.IDLE: //added because of Rhino_Beetle
                animator.Play(EnemyAnim.GetName(ENEMY_ANIM.LEAP));
                return new EnemyIdle();
			case EnemyTrigger.CHASE:
                animator.Play(EnemyAnim.GetName(ENEMY_ANIM.CHASE));
				flags |= (int)EnemyFlag.CHASING;
                return new EnemyChase();
			case EnemyTrigger.CHASE_OBJECT:
                animator.Play(EnemyAnim.GetName(ENEMY_ANIM.CHASE));
				flags |= (int)EnemyFlag.CHASING_OBJECT;
                return new EnemyChaseObject();

        }
        return null;
    }
}

