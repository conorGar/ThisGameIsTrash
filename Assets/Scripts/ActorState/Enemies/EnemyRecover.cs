using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRecover : IActorState<EnemyState, EnemyTrigger>
{
    public EnemyState GetState()
    {
        return EnemyState.RECOVER;
    }

    public IActorState<EnemyState, EnemyTrigger> OnUpdate(tk2dSpriteAnimator animator, ref int flags)
    {
        return null;
    }

    public IActorState<EnemyState, EnemyTrigger> SendTrigger(EnemyTrigger trigger, GameObject actor, tk2dSpriteAnimator animator, ref int flags)
    {
        switch (trigger) {
            case EnemyTrigger.NOTICE:
                animator.Play(EnemyAnim.GetName(ENEMY_ANIM.CHASE));
                flags |= (int)EnemyFlag.CHASING;
                return new EnemyChase();
			case EnemyTrigger.POWER_HIT:
                animator.Play(EnemyAnim.GetName(ENEMY_ANIM.HIT));
				Debug.Log("-x-x-x-x-x-x-x- Enemy Idle Hit trigger activate -x-x-x-x-x-");

                return new EnemyPowerHit();
            /*case EnemyTrigger.HIT: //took out because grub
                animator.Play(EnemyAnim.GetName(ENEMY_ANIM.HIT));
                return new EnemyHit();*/
			case EnemyTrigger.POPUP:
                animator.Play(EnemyAnim.GetName(ENEMY_ANIM.POP_UP));
                return new EnemyPopout();
			case EnemyTrigger.CHASE:
                animator.Play(EnemyAnim.GetName(ENEMY_ANIM.CHASE));
				flags |= (int)EnemyFlag.CHASING;
                return new EnemyChase();
        }

        return null;
    }
}
