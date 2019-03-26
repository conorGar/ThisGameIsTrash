using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLunge : IActorState<EnemyState, EnemyTrigger>
{
    public bool hasMultipleAnimations;
    public bool facingLeft;

    public EnemyLunge(bool p_hasMultipleAnimAtions = false, bool p_facingLeft = false)
    {
        hasMultipleAnimations = p_hasMultipleAnimAtions;
        facingLeft = p_facingLeft;
    }

    public EnemyState GetState()
    {
        return EnemyState.LUNGE;
    }

    public IActorState<EnemyState, EnemyTrigger> OnUpdate(tk2dSpriteAnimator animator, ref int flags)
    {
        if (hasMultipleAnimations) {
            animator.Play(EnemyAnim.GetName(ENEMY_ANIM.LUNGE) + (facingLeft ? "L" : "R"));
        } else {
            animator.Play(EnemyAnim.GetName(ENEMY_ANIM.LUNGE));
        }
        return null;
    }

    public IActorState<EnemyState, EnemyTrigger> SendTrigger(EnemyTrigger trigger, GameObject actor, tk2dSpriteAnimator animator, ref int flags)
    {
        switch (trigger) {
			case EnemyTrigger.POWER_HIT:
                animator.Play(EnemyAnim.GetName(ENEMY_ANIM.HIT));
                return new EnemyPowerHit();
            case EnemyTrigger.RECOVER:
                return new EnemyRecover();
			case EnemyTrigger.VULNERABLE:
                return new EnemyVulnerable();
			case EnemyTrigger.POPUP:
                return new EnemyPopout();
			case EnemyTrigger.THROW: //added because of Rhino Beetle's ability to throw while leaping
                return new EnemyThrow();
        }

        return null;
    }
}
