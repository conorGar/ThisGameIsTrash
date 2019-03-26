using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PLAYERSTATTYPE
{
    NONE,
    HP,
    PP,
    BAG_SIZE,
    STAR_POINTS,
    STAR_BITS
}

[Serializable]
public class PlayerStat {
    PLAYERSTATTYPE type;    // stat type
    [SerializeField]
    int MAX_VALUE;          // How much the of the stat the player has earned over the course of the game.

    [SerializeField]
    int CURRENT_VALUE;      // How much of the stat the player has currently (Maybe they took damage or spent it, etc.)

    public PlayerStat(PLAYERSTATTYPE _type, int _maxValue)
    {
        type = _type;
        MAX_VALUE = _maxValue;
        CURRENT_VALUE = 0;
    }

    public void UpdateMax(int _amount)
    {
        MAX_VALUE += _amount;
    }

    public void UpdateCurrent(int _amount)
    {
        CURRENT_VALUE += _amount;
    }

    public void SetCurrent(int _amount)
    {
        CURRENT_VALUE = _amount;
    }

    // Returns the raw max value without any modifiers from pins equipped or effects.
    public int GetMaxRaw()
    {
        return MAX_VALUE;
    }

    // Returns the max value with modifiers from pins equipped or effects.
    public int GetMax()
    {
        switch (type) {
            case PLAYERSTATTYPE.HP:
                // Apple Plus Pin: Add 1 to hp.
                return MAX_VALUE + (GlobalVariableManager.Instance.IsPinEquipped(PIN.APPLEPLUS) ? 1 : 0);

            case PLAYERSTATTYPE.BAG_SIZE:
                // Bulky Bag Pin: Add 2 to bag size.
                return MAX_VALUE + (GlobalVariableManager.Instance.IsPinEquipped(PIN.BULKYBAG) ? 2 : 0);

            case PLAYERSTATTYPE.PP:
                return MAX_VALUE;

            case PLAYERSTATTYPE.STAR_POINTS:
                return MAX_VALUE;
            case PLAYERSTATTYPE.STAR_BITS:
            	return MAX_VALUE;
            default:
                return MAX_VALUE;
        }
    }

    // Returns how much of the value they have that isn't lost (in the case of hp) or spent (in the case of star points)
    public int GetCurrent()
    {
        return CURRENT_VALUE;
    }

    // Resets the current value to the max value (with modifiers.
    public void ResetCurrent()
    {
        CURRENT_VALUE = GetMax();
    }
}
