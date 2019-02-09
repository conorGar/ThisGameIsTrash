using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    NONE,
    IDLE,
    HIT,
    DEAD
}

public enum EnemyFlag : int
{
    NONE = 0,
    MOVING = 1 << 0
}

public enum EnemyTrigger
{
    IDLE,
    HIT,
    DEATH
}

// Not a whole lot but maybe some common functionality can go here in the future.
public class EnemyStateController<State_Type, State_Trigger> : ActorStateController<State_Type, State_Trigger>
{
}