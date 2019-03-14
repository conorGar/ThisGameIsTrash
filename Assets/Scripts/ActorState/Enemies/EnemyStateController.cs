using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    NONE,
    IDLE,
    HIT,
    NOTICE,
    CHASE,
    PREPARE,
    LUNGE,
    RECOVER,
    DEAD,
    DAZED,
    THROW,
    CARRIED,
    THROWN,
    POPOUT,
    PREPARE_LEAP,
    CAST,
    TELEPORT,
    SPAWN_ADD,
    VULNERABLE,
    MERGED
}

public enum EnemyFlag : int
{
    NONE = 0,
    WALKING = 1 << 0,
    CHASING = 1 << 1,
    THROWABLE = 1 << 2,
    INVULNERABLE = 1 << 3,
    MOVING = WALKING | CHASING
}

public enum EnemyTrigger
{
    IDLE,
    HIT,
    NOTICE,
    PREPARE,
    LUNGE,
    RECOVER,
    DEATH,
    THROW,
    CARRIED,
    THROWN,
    PREPARE_LEAP,
    VULNERABLE,
    INVULNERABLE,
    CAST_TELEPORT,
    CAST_SPAWN_BLOB,
    CAST_SHIELD,
    POPUP,
    MERGE
}

// Not a whole lot but maybe some common functionality can go here in the future.
public class EnemyStateController : ActorStateController<EnemyState, EnemyTrigger>
{
    // Whether the enemy is in a state that allows hitting or not.
    public virtual bool IsHittable()
    {
        var state = currentState.GetState();
        return !IsFlag((int)EnemyFlag.INVULNERABLE) &&
               state != EnemyState.HIT && 
               state != EnemyState.DEAD && 
               state != EnemyState.TELEPORT &&
               state != EnemyState.DAZED &&
               state != EnemyState.CARRIED &&
               state != EnemyState.THROWN;
    }

    // Whether the enemy is in a state that hits the player (does damage).
    public virtual bool IsHitting()
    {
        var state = currentState.GetState();
        return state != EnemyState.PREPARE_LEAP &&
               state != EnemyState.HIT; 
    }
}