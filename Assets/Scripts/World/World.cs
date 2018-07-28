using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WORLD : int
{
    NONE =      0,
    ONE =       1 << 0,
    TWO =       1 << 1,
    THREE =     1 << 2,
    FOUR =      1 << 3
}

public class World {
    public WORLD type;

    public World(WORLD p_type)
    {
        type = p_type;
    }

    // Returns the index of the World.
    // Probably best to use only with the defined enum values above.
    public int WorldIndex()
    {
        for (int i = 0; i < sizeof(WORLD) * 8; ++i)
        {
            if ((int)type == 1 << i)
                return i;
        }
        return 0;
    }

    public static WORLD WorldByIndex(int index)
    {
        return (WORLD)(1 << index);
    }
}
