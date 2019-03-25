using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyPrepare_Leap : IActorState<EnemyState, EnemyTrigger>
{
    public bool hasMultipleAnimations;
    public bool facingLeft;

    public EnemyPrepare_Leap(bool p_hasMultipleAnimAtions = false, bool p_facingLeft = false)
    {
        hasMultipleAnimations = p_hasMultipleAnimAtions;
        facingLeft = p_facingLeft;
    }

    public EnemyState GetState()
    {
        return EnemyState.PREPARE_LEAP;
    }

    public IActorState<EnemyState, EnemyTrigger> OnUpdate(tk2dSpriteAnimator animator, ref int flags)
    {
        if (hasMultipleAnimations) {
            animator.Play(EnemyAnim.GetName(ENEMY_ANIM.PREPARE_LEAP) + (facingLeft ? "L" : "R"));
        } else {
            animator.Play(EnemyAnim.GetName(ENEMY_ANIM.PREPARE_LEAP));
        }
        return null;
    }

    public IActorState<EnemyState, EnemyTrigger> SendTrigger(EnemyTrigger trigger, GameObject actor, tk2dSpriteAnimator animator, ref int flags)
    {
		switch (trigger) {

			case EnemyTrigger.HIT:
                return new EnemyHit();
        }
        return null;
    }
}

