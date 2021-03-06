﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public static WorldManager Instance;
    public WORLD worldType = WORLD.ONE;
    public GARBAGETYPE garbageType = GARBAGETYPE.STANDARD; 
    public int amountTrashHere = 0;
	public PinFunctionsManager playersPFM;
	public int worldNumber; //TODO: used temporarily for large trash manager because couldnt figure out WORLD variable system quickly


    public World world = new World(WORLD.ONE);

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {

    	GlobalVariableManager.Instance.ROOM_NUM = worldNumber; // TODO: added this only for way to determine if the player is at HUB or in world for friends that switch between the two
        world.type = worldType;
		if(GlobalVariableManager.Instance.IsPinEquipped(PIN.HUNGRYFORMORE)){
				//Hungry For More pin
				playersPFM.HungryForMore();
		}
        // TODO: Base this on bag type whether they are going for standard, compost, recyclable, etc.
        GarbageManager.Instance.PopulateGarbage(garbageType, amountTrashHere);

        LargeTrashManager.Instance.EnableProperTrash(worldNumber);

        // Initializing stuff from other managers.  Maybe we should put most of that kind of thing here to preserve ordering?

        CalendarManager.Instance.StartDay();

        // Activate any world stuff pertaining to friends.
        FriendManager.Instance.OnWorldStart(world);

        // Activate the first room!
        RoomManager.Instance.ActivateCurrentRoom();

        // Clean cleanables
        CleanableManager.Instance.InitCleanables(world.type);
    }

    void Update()
    {

    }

    public void SetWorld(WORLD type)
    {
        world.type = type;
    }
}