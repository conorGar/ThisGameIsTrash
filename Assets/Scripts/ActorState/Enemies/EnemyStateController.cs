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
    DEAD
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
    DEATH
}

// Not a whole lot but maybe some common functionality can go here in the future.
public class EnemyStateController<State_Type, State_Trigger> : ActorStateController<State_Type, State_Trigger>
{
}