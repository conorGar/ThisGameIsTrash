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
            case EnemyTrigger.HIT:
                animator.Play(EnemyAnim.GetName(ENEMY_ANIM.HIT));
                return new EnemyHit();
			case EnemyTrigger.POPUP:
                animator.Play(EnemyAnim.GetName(ENEMY_ANIM.POP_UP));
                return new EnemyPopout();
        }

        return null;
    }
}
