using System.Collections;
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
	public AudioClip worldMusic;


    public World world = new World(WORLD.ONE);

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
    	SoundManager.instance.musicSource.clip = worldMusic;
    	SoundManager.instance.musicSource.volume = GlobalVariableManager.Instance.MASTER_MUSIC_VOL;
    	SoundManager.instance.musicSource.Play();

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
    }

    void Update()
    {

    }

    public void SetWorld(WORLD type)
    {
        world.type = type;
    }
}