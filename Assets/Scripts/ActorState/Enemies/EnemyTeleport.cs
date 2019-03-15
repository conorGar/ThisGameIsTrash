using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTeleport : IActorState<EnemyState, EnemyTrigger>
{
    public EnemyState GetState()
    {
        return EnemyState.TELEPORT;
    }

    public IActorState<EnemyState, EnemyTrigger> OnUpdate(tk2dSpriteAnimator animator, ref int flags)
    {
        animator.Play(EnemyAnim.GetName(ENEMY_ANIM.IDLE)); // TODO: I know they are invis but maybe we add a teleport animation eventually.
        return null;
    }

    public IActorState<EnemyState, EnemyTrigger> SendTrigger(EnemyTrigger trigger, GameObject actor, tk2dSpriteAnimator animator, ref int flags)
    {
        switch (trigger) {
            case EnemyTrigger.IDLE:
                return new EnemyIdle();
        }

        return null;
    }
}
