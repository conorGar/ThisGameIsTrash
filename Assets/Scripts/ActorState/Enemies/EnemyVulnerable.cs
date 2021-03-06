﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyVulnerable : IActorState<EnemyState, EnemyTrigger>
{

	public EnemyState GetState()
    {
        return EnemyState.PREPARE;
    }

    public IActorState<EnemyState, EnemyTrigger> OnUpdate(tk2dSpriteAnimator animator, ref int flags)
    {
		animator.Play(EnemyAnim.GetName(ENEMY_ANIM.VULNERABLE));
        return null;
    }

    public IActorState<EnemyState, EnemyTrigger> SendTrigger(EnemyTrigger trigger, GameObject actor, tk2dSpriteAnimator animator, ref int flags)
    {
		switch (trigger) {
            case EnemyTrigger.RECOVER:
                animator.Play(EnemyAnim.GetName(ENEMY_ANIM.BURROW));
                //flags |= (int)EnemyFlag.CHASING;
                return new EnemyRecover();
        }
        return null;
    }
}

