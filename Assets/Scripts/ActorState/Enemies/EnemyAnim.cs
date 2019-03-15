using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ENEMY_ANIM
{
    IDLE,
    HIT,
    DEAD,
    DAZED,
    WALK,
    RUN,
    CHASE,
    SWING,
    SHAKE,
    SWOOP,
    PREPARE,
    SLEEP,
    WAKE,
    POP_UP,
    THROW,
    CARRIED,
    LUNGE,
    RECOVER,
    PREPARE_LEAP,
    VULNERABLE,
    BURROW,
    CAST,
    MERGED
}

public class EnemyAnim
{
    public static string GetName(ENEMY_ANIM anim)
    {
        switch (anim) {
            case ENEMY_ANIM.IDLE: return "idle";
            case ENEMY_ANIM.HIT: return "hit";
            case ENEMY_ANIM.DEAD: return "dead";
            case ENEMY_ANIM.DAZED: return "dazed";
            case ENEMY_ANIM.WALK: return "walk";
            case ENEMY_ANIM.RUN: return "run";
            case ENEMY_ANIM.CHASE: return "chase";
            case ENEMY_ANIM.SWING: return "swing";
            case ENEMY_ANIM.SHAKE: return "shake";
            case ENEMY_ANIM.SWOOP: return "swoop";
            case ENEMY_ANIM.PREPARE: return "prepare";
            case ENEMY_ANIM.SLEEP: return "sleep";
            case ENEMY_ANIM.WAKE: return "wake";
            case ENEMY_ANIM.POP_UP: return "popUp";
            case ENEMY_ANIM.THROW: return "throw";
            case ENEMY_ANIM.CARRIED: return "carried";
            case ENEMY_ANIM.LUNGE: return "lunge";
            case ENEMY_ANIM.RECOVER: return "recover";
            case ENEMY_ANIM.PREPARE_LEAP: return "prepare_leap";
			case ENEMY_ANIM.VULNERABLE: return "vulnerable";
			case ENEMY_ANIM.BURROW: return "burrow";
            case ENEMY_ANIM.CAST: return "cast";
            case ENEMY_ANIM.MERGED: return "merged";
        }

        Debug.LogError("Animation Name for " + anim + " is undefined.  Define it here!");
        return "null_anim_name";
    }
}
