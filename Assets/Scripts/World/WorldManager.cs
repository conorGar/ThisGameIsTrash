using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public static WorldManager Instance;
    public WORLD worldType = WORLD.ONE;
    public GARBAGETYPE garbageType = GARBAGETYPE.STANDARD; 
    public int amountTrashHere = 0;

    public World world = new World(WORLD.ONE);

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        world.type = worldType;

        // TODO: Base this on bag type whether they are going for standard, compost, recyclable, etc.
        GarbageManager.Instance.PopulateGarbage(garbageType, amountTrashHere);
    }

    void Update()
    {

    }

    public void SetWorld(WORLD type)
    {
        world.type = type;
    }
}