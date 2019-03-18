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
    THROW,
    POPOUT,
    PREPARE_LUNGE,
    VULNERABLE,
    CHASE_OBJECT,
    POWER_HIT,
}

public enum EnemyFlag : int
{
    NONE = 0,
    WALKING = 1 << 0,
    CHASING = 2 << 0,
    CHASING_OBJECT = 3<<0,

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
    PREPARE_LUNGE,
    VULNERABLE,
    POPUP,
    CHASE, // chase for use when need to chase again after already noticed player
    CHASE_OBJECT,
    POWER_HIT
}

// Not a whole lot but maybe some common functionality can go here in the future.
public class EnemyStateController<State_Type, State_Trigger> : ActorStateController<State_Type, State_Trigger>
{
}