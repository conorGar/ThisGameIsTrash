using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRecover : IActorState<EnemyState, EnemyTrigger>
{
    public bool hasMultipleAnimations;
    public bool facingLeft;

    public EnemyRecover(bool p_hasMultipleAnimAtions = false, bool p_facingLeft = false)
    {
        hasMultipleAnimations = p_hasMultipleAnimAtions;
        facingLeft = p_facingLeft;
    }

    public EnemyState GetState()
    {
        return EnemyState.RECOVER;
    }

    public IActorState<EnemyState, EnemyTrigger> OnUpdate(tk2dSpriteAnimator animator, ref int flags)
    {
        if (hasMultipleAnimations) {
            animator.Play(EnemyAnim.GetName(ENEMY_ANIM.RECOVER) + (facingLeft ? "L" : "R"));
        } else {
            animator.Play(EnemyAnim.GetName(ENEMY_ANIM.RECOVER));
        }
        return null;
    }

    public IActorState<EnemyState, EnemyTrigger> SendTrigger(EnemyTrigger trigger, GameObject actor, tk2dSpriteAnimator animator, ref int flags)
    {
        switch (trigger) {
            case EnemyTrigger.NOTICE:
                flags |= (int)EnemyFlag.CHASING;
                return new EnemyChase();
			case EnemyTrigger.POWER_HIT:
                animator.Play(EnemyAnim.GetName(ENEMY_ANIM.HIT));
                return new EnemyPowerHit();
			case EnemyTrigger.POPUP:
                return new EnemyPopout();
			case EnemyTrigger.VULNERABLE: //added for Grub Enemy
                return new EnemyVulnerable();
			case EnemyTrigger.CHASE:
				flags |= (int)EnemyFlag.CHASING;
                return new EnemyChase();
			case EnemyTrigger.PREPARE://added for Grub Enemy
				animator.Play(EnemyAnim.GetName(ENEMY_ANIM.PREPARE));
                return new EnemyPrepare();
			case EnemyTrigger.LUNGE://added for Crab Enemy
				animator.Play(EnemyAnim.GetName(ENEMY_ANIM.LUNGE));
                return new EnemyLunge();
			case EnemyTrigger.IDLE://added for Crab Enemy
				animator.Play(EnemyAnim.GetName(ENEMY_ANIM.IDLE));
                return new EnemyIdle();
        }

        return null;
    }
}
