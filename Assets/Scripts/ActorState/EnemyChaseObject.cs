using UnityEngine;
using System.Collections;

public class EnemyChaseObject : IActorState<EnemyState, EnemyTrigger>
{
    public EnemyState GetState()
    {
        return EnemyState.CHASE_OBJECT;
    }

    public IActorState<EnemyState, EnemyTrigger> OnUpdate(tk2dSpriteAnimator animator, ref int flags)
    {
        return null;
    }

    public IActorState<EnemyState, EnemyTrigger> SendTrigger(EnemyTrigger trigger, GameObject actor, tk2dSpriteAnimator animator, ref int flags)
    {
        switch (trigger) {
      
			case EnemyTrigger.PREPARE_LEAP:
                animator.Play(EnemyAnim.GetName(ENEMY_ANIM.PREPARE_LEAP));
				flags &= ~(int)EnemyFlag.CHASING_OBJECT;
            
                return new EnemyPrepare_Leap();
			case EnemyTrigger.LUNGE:
                animator.Play(EnemyAnim.GetName(ENEMY_ANIM.LEAP));
				flags &= ~(int)EnemyFlag.CHASING_OBJECT;
                return new EnemyLunge();
			case EnemyTrigger.CHASE:
                animator.Play(EnemyAnim.GetName(ENEMY_ANIM.CHASE));
				flags &= ~(int)EnemyFlag.CHASING_OBJECT;
				flags |= (int)EnemyFlag.CHASING;
                return new EnemyChase();

        }

        return null;
    }
}