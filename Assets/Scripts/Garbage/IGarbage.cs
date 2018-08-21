﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GARBAGETYPE
{
    STANDARD,
    COMPOST,
    RECYCLABLE,
    LARGE
};

public enum STANDARDGARBAGE : uint
{
    NONE =                  0,
    PAPER =                 1 << 0,
    IPOD =                  1 << 1,
    BOOK =                  1 << 2,
    ART_SUPPLIES =          1 << 3,
    BAG_SPICY =             1 << 4,
    CHIPS =                 1 << 5,
    BAG_BBQ =               1 << 6,
    CASETTE =               1 << 7,
    CHINESE =               1 << 8,
    JUICE =                 1 << 9,
    LIGHTBULB =             1 << 10,
    MUG =                   1 << 11,
    PARTY =                 1 << 12,
    SOCK =                  1 << 13,
    TISSUE =                1 << 14,
    TOILET =                1 << 15,
    HAIR =                  1 << 16,
    FISH =                  1 << 17,
    NEEDLE =                1 << 18,
    BABY =                  1 << 19,
    ARM =                   1 << 20,
    CHILDHOOD =             1 << 21,
    MOM_PRES =              1 << 22,
    SIZE =                  1 << 22
}

public enum COMPOSTGARBAGE : int
{
    NONE =                  0,
    COMPOST_TEST_1 =        1 << 0,
    COMPOST_TEST_2 =        1 << 1,
    COMPOST_TEST_3 =        1 << 2,
    SIZE = 1 << 2
}

public enum RECYCLABLEGARBAGE : int
{
    NONE =                  0,
    RECYCLABLE_TEST_1 =     1 << 0,
    RECYCLABLE_TEST_2 =     1 << 1,
    RECYCLABLE_TEST_3 =     1 << 2,
    SIZE = 1 << 2
}

public enum LARGEGARBAGE : int
{
    NONE =                  0,
    CHAIR =                 1 << 0,
    TESTTRASH2 =            1 << 1,
    TESTTRASH3 =            1 << 2,
    TESTTRASH4 =            1 << 3,
    SIZE =                  1 << 3
}




public interface IGarbage
{
    int GarbageIndex();
    string GarbageSprite();
}



public class StandardGarbage : IGarbage
{
    public STANDARDGARBAGE type;
    private static Dictionary<STANDARDGARBAGE, string> SPRITELOOKUP = new Dictionary<STANDARDGARBAGE, string>()
    {
        { STANDARDGARBAGE.NONE,                     "" },
        { STANDARDGARBAGE.PAPER,                    "trash_paper2" },
        { STANDARDGARBAGE.IPOD,                     "trash_ipod2" },
        { STANDARDGARBAGE.BOOK,                     "trash_book2" },
        { STANDARDGARBAGE.ART_SUPPLIES,             "trash_artSupplies" },
        { STANDARDGARBAGE.BAG_SPICY,                "trash_bagSpicy2" },
        { STANDARDGARBAGE.CHIPS,                    "trash_chips" },
        { STANDARDGARBAGE.BAG_BBQ,                  "trash_bbqBag2" },
        { STANDARDGARBAGE.CASETTE,                  "trash_casette2" },
        { STANDARDGARBAGE.CHINESE,                  "trash_chinese2" },
        { STANDARDGARBAGE.JUICE,                    "trash_juice2" },
        { STANDARDGARBAGE.LIGHTBULB,                "trash_lightbulb2" },
        { STANDARDGARBAGE.MUG,                      "trash_mug2" },
        { STANDARDGARBAGE.PARTY,                    "trash_party" },
        { STANDARDGARBAGE.SOCK,                     "trash_sock2" },
        { STANDARDGARBAGE.TISSUE,                   "trash_tissue2" },
        { STANDARDGARBAGE.TOILET,                   "trash_toiletPaper2" },
        { STANDARDGARBAGE.HAIR,                     "trash_hairs2" },
        { STANDARDGARBAGE.FISH,                     "trash_fish2" },
        { STANDARDGARBAGE.NEEDLE,                   "trash_needle2" },
        { STANDARDGARBAGE.BABY,                     "trash_baby2" },
        { STANDARDGARBAGE.ARM,                      "trash_arm2" },
        { STANDARDGARBAGE.CHILDHOOD,                "trash_childhood2" },
        { STANDARDGARBAGE.MOM_PRES,                 "trash_momPres2" }
    };

    public int GarbageIndex()
    {
        for (int i = 0; i < sizeof(STANDARDGARBAGE) * 8; ++i)
        {
            if ((int)type == 1 << i)
                return i;
        }
        return 0;
    }

    public string GarbageSprite()
    {
        return StandardGarbage.SPRITELOOKUP[type];
    }

    public static STANDARDGARBAGE GarbageByIndex(int index)
    {
        return (STANDARDGARBAGE)(1 << index);
    }

    public static int Count()
    {
        for (int i = 0; i < sizeof(STANDARDGARBAGE) * 8; ++i)
        {
            if ((int)STANDARDGARBAGE.SIZE == 1 << i)
                return i;
        }
        return 0;
    }
}




public class CompostGarbage : IGarbage
{
    public COMPOSTGARBAGE type;
    private static Dictionary<COMPOSTGARBAGE, string> SPRITELOOKUP = new Dictionary<COMPOSTGARBAGE, string>()
    {
        { COMPOSTGARBAGE.NONE,                              "" },
        { COMPOSTGARBAGE.COMPOST_TEST_1,                    "" },
        { COMPOSTGARBAGE.COMPOST_TEST_2,                    "" },
        { COMPOSTGARBAGE.COMPOST_TEST_3,                    "" }
    };

    public int GarbageIndex()
    {
        for (int i = 0; i < sizeof(COMPOSTGARBAGE) * 8; ++i)
        {
            if ((int)type == 1 << i)
                return i;
        }
        return 0;
    }

    public string GarbageSprite()
    {
        return CompostGarbage.SPRITELOOKUP[type];
    }

    public static COMPOSTGARBAGE ByIndex(int index)
    {
        return (COMPOSTGARBAGE)(1 << index);
    }

    public static int Count()
    {
        for (int i = 0; i < sizeof(COMPOSTGARBAGE) * 8; ++i)
        {
            if ((int)COMPOSTGARBAGE.SIZE == 1 << i)
                return i;
        }
        return 0;
    }
}




public class RecyclableGarbage : IGarbage
{
    public RECYCLABLEGARBAGE type;
    private static Dictionary<RECYCLABLEGARBAGE, string> SPRITELOOKUP = new Dictionary<RECYCLABLEGARBAGE, string>()
    {
        { RECYCLABLEGARBAGE.NONE,                                 "" },
        { RECYCLABLEGARBAGE.RECYCLABLE_TEST_1,                    "" },
        { RECYCLABLEGARBAGE.RECYCLABLE_TEST_2,                    "" },
        { RECYCLABLEGARBAGE.RECYCLABLE_TEST_3,                    "" }
    };

    public int GarbageIndex()
    {
        for (int i = 0; i < sizeof(RECYCLABLEGARBAGE) * 8; ++i)
        {
            if ((int)type == 1 << i)
                return i;
        }
        return 0;
    }

    public string GarbageSprite()
    {
        return RecyclableGarbage.SPRITELOOKUP[type];
    }

    public static RECYCLABLEGARBAGE ByIndex(int index)
    {
        return (RECYCLABLEGARBAGE)(1 << index);
    }

    public static int Count()
    {
        for (int i = 0; i < sizeof(RECYCLABLEGARBAGE) * 8; ++i)
        {
            if ((int)RECYCLABLEGARBAGE.SIZE == 1 << i)
                return i;
        }
        return 0;
    }
}




public class LargeGarbage : IGarbage
{
    public LARGEGARBAGE type;
    private static Dictionary<LARGEGARBAGE, string> SPRITELOOKUP = new Dictionary<LARGEGARBAGE, string>()
    {
        { LARGEGARBAGE.NONE,                          "" },
        { LARGEGARBAGE.CHAIR,                         "" },
        { LARGEGARBAGE.TESTTRASH2,                    "" },
        { LARGEGARBAGE.TESTTRASH3,                    "" },
        { LARGEGARBAGE.TESTTRASH4,                    "" }
    };


    public int GarbageIndex()
    {
        for (int i = 0; i < sizeof(LARGEGARBAGE) * 8; ++i)
        {
            if ((int)type == 1 << i)
                return i;
        }
        return 0;
    }

    public string GarbageSprite()
    {
        return LargeGarbage.SPRITELOOKUP[type];
    }

    public static LARGEGARBAGE ByIndex(int index)
    {
        return (LARGEGARBAGE)(1 << index);
    }

    public static int Count()
    {
        for (int i = 0; i < sizeof(LARGEGARBAGE) * 8; ++i)
        {
            if ((int)LARGEGARBAGE.SIZE == 1 << i)
                return i;
        }
        return 0;
    }
}