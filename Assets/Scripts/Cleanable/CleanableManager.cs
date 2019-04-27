using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Little helper class to store cleaning types per world
[Serializable]
public class CleanableWorld
{
    public WORLD world;
    public List<Cleanable> cleanables;
}

// 64 very generic bits.  Editor script will populate all the cleanable items with these bits so it doesn't matter all that much.
public enum CLEANABLE_BIT : long
{
    #region CLEANABLE_BITS_DEFINITION
    NONE = 0,
    C1 = (long)1 << 0,
    C2 = (long)1 << 1,
    C3 = (long)1 << 2,
    C4 = (long)1 << 3,
    C5 = (long)1 << 4,
    C6 = (long)1 << 5,
    C7 = (long)1 << 6,
    C8 = (long)1 << 7,
    C9 = (long)1 << 8,
    C10 = (long)1 << 9,

    C11 = (long)1 << 10,
    C12 = (long)1 << 11,
    C13 = (long)1 << 12,
    C14 = (long)1 << 13,
    C15 = (long)1 << 14,
    C16 = (long)1 << 15,
    C17 = (long)1 << 16,
    C18 = (long)1 << 17,
    C19 = (long)1 << 18,
    C20 = (long)1 << 19,

    C21 = (long)1 << 20,
    C22 = (long)1 << 21,
    C23 = (long)1 << 22,
    C24 = (long)1 << 23,
    C25 = (long)1 << 24,
    C26 = (long)1 << 25,
    C27 = (long)1 << 26,
    C28 = (long)1 << 27,
    C29 = (long)1 << 28,
    C30 = (long)1 << 29,

    C31 = (long)1 << 30,
    C32 = (long)1 << 31,
    C33 = (long)1 << 32,
    C34 = (long)1 << 33,
    C35 = (long)1 << 34,
    C36 = (long)1 << 35,
    C37 = (long)1 << 36,
    C38 = (long)1 << 37,
    C39 = (long)1 << 38,
    C40 = (long)1 << 39,

    C41 = (long)1 << 40,
    C42 = (long)1 << 41,
    C43 = (long)1 << 42,
    C44 = (long)1 << 43,
    C45 = (long)1 << 44,
    C46 = (long)1 << 45,
    C47 = (long)1 << 46,
    C48 = (long)1 << 47,
    C49 = (long)1 << 48,
    C50 = (long)1 << 49,

    C51 = (long)1 << 50,
    C52 = (long)1 << 51,
    C53 = (long)1 << 52,
    C54 = (long)1 << 53,
    C55 = (long)1 << 54,
    C56 = (long)1 << 55,
    C57 = (long)1 << 56,
    C58 = (long)1 << 57,
    C59 = (long)1 << 58,
    C60 = (long)1 << 59,

    C61 = (long)1 << 60,
    C62 = (long)1 << 61,
    C63 = (long)1 << 62,
    C64 = (long)1 << 63
    #endregion
}

// used to save and load cleanable options like bushes and oil spills, by type, by world.
public class CleanableManager : UserDataItem
{
    public static CleanableManager Instance;
    Dictionary<CLEANABLE_TYPE, CLEANABLE_BIT> cleanableLookUp = new Dictionary<CLEANABLE_TYPE, CLEANABLE_BIT>(); // All the state data if something is cleaned already or not.
    public List<CleanableWorld> cleanableWorlds = new List<CleanableWorld>();

    void Awake()
    {
        Instance = this;
    }

    // Finds all the cleanables in the currentScene and marks those that are already clean.
    public void InitCleanables(WORLD world)
    {
        CleanableItem[] cleanableItems = FindObjectsOfType<CleanableItem>();
        List<Cleanable> cleanables = null;
        for (int i = 0; i < cleanableWorlds.Count; i++) {
            if (cleanableWorlds[i].world == world) {
                cleanables = cleanableWorlds[i].cleanables;
                break;
            }
        }

        if (cleanables != null) {
            Debug.Log("Initializing Cleanables for World: " + world);
            for (int i = 0; i < cleanableItems.Length; i++) {
                var item = cleanableItems[i];
                if (item.cleanable != null) {
                    for (int j = 0; j < cleanables.Count; j++) {
                        if (cleanables[j].CleanableType() == item.cleanable.CleanableType()) {
                            item.InitClean(cleanableLookUp[item.cleanable.CleanableType()]);
                            break;
                        }
                    }
                }
            }
        } else {
            Debug.LogError("Setup Cleanable types for World: " + world, gameObject);
        }
    }

    // mark a thing as clean.
    public void Clean(Cleanable cleanable, CLEANABLE_BIT cleanableBit)
    {
        cleanableLookUp[cleanable.CleanableType()] |= cleanableBit;
        UserDataManager.Instance.SetDirty();
    }

    public override string UserDataKey()
    {
        return "Cleanables";
    }

    public override SimpleJSON.JSONObject Save()
    {
        SimpleJSON.JSONNode.longAsString = true; // Needed to store cleanable bitfields properly.
        var json_data = new SimpleJSON.JSONObject();

        // store cleanable bitfields for each type in the game.
        foreach (var item in cleanableLookUp) {
            json_data["TYPE_" + (int)item.Key] = (long)item.Value;
        }

        return json_data;
    }

    public override void Load(SimpleJSON.JSONObject json_data)
    {
        for (int i = 0; i < (int)CLEANABLE_TYPE.SIZE; i++) {
            CLEANABLE_TYPE type = (CLEANABLE_TYPE)i;
            cleanableLookUp[type] = (CLEANABLE_BIT)json_data["TYPE_" + i].AsLong;
        }
    }
}
