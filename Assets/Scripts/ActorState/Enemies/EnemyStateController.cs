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
    LEAP,
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
    MERGED,
    CHASE_OBJECT,
    POWER_HIT,

}

public enum EnemyFlag : int
{
    NONE =                   0,
    WALKING =           1 << 0,
    CHASING =           1 << 1,
    CHASING_OBJECT =    1 << 2,
    THROWABLE =         1 << 3,
    INVULNERABLE =      1 << 4,
    MOVING = WALKING | CHASING | CHASING_OBJECT
}

public enum EnemyTrigger
{
    IDLE,
    HIT,
    NOTICE,
    PREPARE,
    LUNGE,
    LEAP,
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
    MERGE,
    CHASE, // chase for use when need to chase again after already noticed player
    CHASE_OBJECT,
    POWER_HIT
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