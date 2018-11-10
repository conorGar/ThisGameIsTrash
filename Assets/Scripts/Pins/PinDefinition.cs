using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PIN : ulong
{
    NONE =                                 0,
    BULKYBAG =                  (ulong)1 << 0,
    TIMEENOUGH =                (ulong)1 << 1,
    SPEEDY =                    (ulong)1 << 2,
    CURSED =                    (ulong)1 << 3,
    BLISTERINGBOND =            (ulong)1 << 4,
    PASSIVEPILLAGE =            (ulong)1 << 5,
    FAITHFULWEAPON =            (ulong)1 << 6,
    MYLEGACY =                  (ulong)1 << 7,
    SHARING =                   (ulong)1 << 8,
    MOGARBAGEMOPROBLEMS =       (ulong)1 << 9,
    HAULINHERO =                (ulong)1 << 10,
    APPLEPLUS =                 (ulong)1 << 11,
    PRETTYLUCKY =               (ulong)1 << 12,
    LUCKYDAY =                  (ulong)1 << 13,
    POINTYPIN =                 (ulong)1 << 14,
    THEPINTHATCANKILLTHEPAST =  (ulong)1 << 15,
    WASTEWARRIOR =              (ulong)1 << 16,
    STAYBACK =                  (ulong)1 << 17,
    SCRAPCITY =                 (ulong)1 << 18,
    QUENPINTARANTINO =          (ulong)1 << 19,
    HUNGRYFORMORE =             (ulong)1 << 20,
    NEXTOFPIN =                 (ulong)1 << 21,
    CALLOFTHEWILD =             (ulong)1 << 22,
    TALKYTIME =                 (ulong)1 << 23,
    VITALITYVISION =            (ulong)1 << 24,
    CLAWPIN =                   (ulong)1 << 25,
    WAIFUWANTED =               (ulong)1 << 26,
    PIERCINGPIN =               (ulong)1 << 27,
    SCRAPPYSHINOBI =            (ulong)1 << 28,
    NICEGUY =                   (ulong)1 << 29,
    FORTUNESFRIEND =            (ulong)1 << 30,
    TRASHIERTOMORROW =          (ulong)1 << 31,
    FUELEFFICIENT =             (ulong)1 << 32,
    DEATHSDEAL =                (ulong)1 << 33,
    HARDLYWORKIN =              (ulong)1 << 34,
    MAGNETICPIN =               (ulong)1 << 35,
    TRASHPOWER =                (ulong)1 << 36,
    TRASHRETURN =               (ulong)1 << 37,
    NOTRASHLEFTBEHIND =         (ulong)1 << 38,
    ROTTENAPPLEPLUS =           (ulong)1 << 39,
    SPIRITUALSAFETY =           (ulong)1 << 40,
    DUCKSFX =                   (ulong)1 << 41,
    WORKINHARD =                (ulong)1 << 42,
    WARPPIN =                   (ulong)1 << 43,
    DEVILSDEAL =                (ulong)1 << 44,
    HEROOFGRIME =               (ulong)1 << 45,
    ATTKUP1 =                   (ulong)1 << 46,
    ATTKUP2 =                   (ulong)1 << 47,
    DEFUP1 =                    (ulong)1 << 48,
    DEFUP2 =                    (ulong)1 << 49,
    LINKTOTRASH =                    (ulong)1 << 50,
    DUMPSTERDASH =                    (ulong)1 << 51,
    SMASHYSMASH =                    (ulong)1 << 52,
    Test54 =                    (ulong)1 << 53,
    Test55 =                    (ulong)1 << 54,
    Test56 =                    (ulong)1 << 55,
    Test57 =                    (ulong)1 << 56,
    Test58 =                    (ulong)1 << 57,
    Test59 =                    (ulong)1 << 58,
    Test60 =                    (ulong)1 << 59,
    Test61 =                    (ulong)1 << 60,
    Test62 =                    (ulong)1 << 61,
    Test63 =                    (ulong)1 << 62,
    Test64 =                    (ulong)1 << 63,
    ALL =                       0xFFFFFFFFFFFFFFFF
}

[CreateAssetMenu(fileName = "New Pin", menuName = "TGIT/Pin", order = 2)]
public class PinDefinition : ScriptableObject
{
    //[NonSerialized]
    //private PIN type = PIN.NONE;
    public PIN Type
    {
        set { pinValue = (ulong)value; }
        get { return (PIN)pinValue; }
    }

    [HideInInspector]
    public ulong pinValue;

    public string displayName = "New Pin";
    public string description = "New Pin Description";

    [SerializeField]
    public string sprite = "sprite_name";
    public string titleSprite = "title_sprite_name";
    public int ppValue = 1;
    public int price = 1;
    public int displayPriority = 1;

}
