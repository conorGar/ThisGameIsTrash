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
            case EnemyTrigger.HIT:
                return new EnemyHit();
			case EnemyTrigger.POPUP:
                animator.Play(EnemyAnim.GetName(ENEMY_ANIM.POP_UP));
                return new EnemyPopout();
        }

        return null;
    }
}
