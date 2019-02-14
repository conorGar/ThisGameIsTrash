using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PIN : long
{
    NONE =                                 0,
    BULKYBAG =                  (long)1 << 0,
    TIMEENOUGH =                (long)1 << 1,
    SPEEDY =                    (long)1 << 2,
    CURSED =                    (long)1 << 3,
    BLISTERINGBOND =            (long)1 << 4,
    PASSIVEPILLAGE =            (long)1 << 5,
    FAITHFULWEAPON =            (long)1 << 6,
    MYLEGACY =                  (long)1 << 7,
    SHARING =                   (long)1 << 8,
    MOGARBAGEMOPROBLEMS =       (long)1 << 9,
    HAULINHERO =                (long)1 << 10,
    APPLEPLUS =                 (long)1 << 11,
    PRETTYLUCKY =               (long)1 << 12,
    LUCKYDAY =                  (long)1 << 13,
    POINTYPIN =                 (long)1 << 14,
    THEPINTHATCANKILLTHEPAST =  (long)1 << 15,
    WASTEWARRIOR =              (long)1 << 16,
    STAYBACK =                  (long)1 << 17,
    SCRAPCITY =                 (long)1 << 18,
    QUENPINTARANTINO =          (long)1 << 19,
    HUNGRYFORMORE =             (long)1 << 20,
    NEXTOFPIN =                 (long)1 << 21,
    CALLOFTHEWILD =             (long)1 << 22,
    TALKYTIME =                 (long)1 << 23,
    VITALITYVISION =            (long)1 << 24,
    CLAWPIN =                   (long)1 << 25,
    WAIFUWANTED =               (long)1 << 26,
    PIERCINGPIN =               (long)1 << 27,
    SCRAPPYSHINOBI =            (long)1 << 28,
    NICEGUY =                   (long)1 << 29,
    FORTUNESFRIEND =            (long)1 << 30,
    TRASHIERTOMORROW =          (long)1 << 31,
    FUELEFFICIENT =             (long)1 << 32,
    DEATHSDEAL =                (long)1 << 33,
    HARDLYWORKIN =              (long)1 << 34,
    MAGNETICPIN =               (long)1 << 35,
    TRASHPOWER =                (long)1 << 36,
    TRASHRETURN =               (long)1 << 37,
    NOTRASHLEFTBEHIND =         (long)1 << 38,
    ROTTENAPPLEPLUS =           (long)1 << 39,
    SPIRITUALSAFETY =           (long)1 << 40,
    DUCKSFX =                   (long)1 << 41,
    WORKINHARD =                (long)1 << 42,
    WARPPIN =                   (long)1 << 43,
    DEVILSDEAL =                (long)1 << 44,
    HEROOFGRIME =               (long)1 << 45,
    ATTKUP1 =                   (long)1 << 46,
    ATTKUP2 =                   (long)1 << 47,
    DEFUP1 =                    (long)1 << 48,
    DEFUP2 =                    (long)1 << 49,
    LINKTOTRASH =               (long)1 << 50,
    DUMPSTERDASH =              (long)1 << 51,
    SMASHYSMASH =               (long)1 << 52,
    PROJECTILEPROTECTOR =       (long)1 << 53,
    SNEAKINGSCRAPPER =          (long)1 << 54,
    TREASURETRACKER =           (long)1 << 55,
    COUNTSCRAPULA =             (long)1 << 56,
    DIRTYDECOY =                (long)1 << 57,
    IRRADIATED =                (long)1 << 58,
    A_TRASHBOMB =               (long)1 << 59,
    Test61 =                    (long)1 << 60,
    Test62 =                    (long)1 << 61,
    Test63 =                    (long)1 << 62,
    Test64 =                    (long)1 << 63,
    ALL =                       0xFFFFFFFFFFFFFFF
}

[CreateAssetMenu(fileName = "New Pin", menuName = "TGIT/Pin", order = 2)]
public class PinDefinition : ScriptableObject
{
    //[NonSerialized]
    //private PIN type = PIN.NONE;
    public PIN Type
    {
        set { pinValue = (long)value; }
        get { return (PIN)pinValue; }
    }

    [HideInInspector]
    public long pinValue;

    public string displayName = "New Pin";
    public string description = "New Pin Description";

    [SerializeField]
    public string sprite = "sprite_name";
    public string titleSprite = "title_sprite_name";
    public int ppValue = 1;
    public int price = 1;
    public int displayPriority = 1;
    public bool abilityPin = false;
}
