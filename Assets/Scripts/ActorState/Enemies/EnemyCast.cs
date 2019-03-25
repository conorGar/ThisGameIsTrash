using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CAST_TYPE
{
    NONE,
    TELEPORT,
    SPAWN_ADD,
    SHIELD
}

public class EnemyCast : IActorState<EnemyState, EnemyTrigger>
{
    private CAST_TYPE cast_type;
    private string prefabTag;

    // extra constructor to pass CAST_TYPE and other parameters
    public EnemyCast(CAST_TYPE p_cast_type, string p_prefabTag = "")
    {
        cast_type = p_cast_type;
        prefabTag = p_prefabTag;
    }

    public EnemyState GetState()
    {
        return EnemyState.CAST;
    }

    public CAST_TYPE GetCastType()
    {
        return cast_type;
    }

    public string GetPrefabTag()
    {
        return prefabTag;
    }

    public IActorState<EnemyState, EnemyTrigger> OnUpdate(tk2dSpriteAnimator animator, ref int flags)
    {
        animator.Play(EnemyAnim.GetName(ENEMY_ANIM.CAST));
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
