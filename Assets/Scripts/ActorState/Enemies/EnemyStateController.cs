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
    THROW,
    POPOUT,
    PREPARE_LEAP,
    VULNERABLE
}

public enum EnemyFlag : int
{
    NONE = 0,
    WALKING = 1 << 0,
    CHASING = 2 << 0,

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
    PREPARE_LEAP,
    VULNERABLE,
    POPUP
}

// Not a whole lot but maybe some common functionality can go here in the future.
public class EnemyStateController<State_Type, State_Trigger> : ActorStateController<State_Type, State_Trigger>
{
}