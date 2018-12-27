using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariableManager : UserDataItem {



	public static GlobalVariableManager Instance {get; private set;}

	public int value;

    private long pinsDiscoveredValue = (long)(PIN.BULKYBAG); //| PIN.TREASURETRACKER | PIN.COUNTSCRAPULA | PIN.PROJECTILEPROTECTOR);
    public PIN PINS_DISCOVERED
    {
        set { pinsDiscoveredValue = (long)value; }
        get { return (PIN)pinsDiscoveredValue; }
    }

    private long pinsEquippedValue = (long)(PIN.NONE);
    public PIN PINS_EQUIPPED
    {
        set { pinsEquippedValue = (long)value; }
        get { return (PIN)pinsEquippedValue; }
    }
	private long pinsViewedValue = (long)(PIN.NONE);
    public PIN PINS_VIEWED
    {
        set { pinsViewedValue = (long)value; }
        get { return (PIN)pinsViewedValue; }
    }

    public int DEJAVUCOUNT = 0;
    public int CURSEVALUE = 0;
    public int MOMONEYVALUE = 0;

	/*public string[] characterUpgradeArray = new string[7]{
											"abcdefghijolmnpqrstuvwxy1234567890,./;'*[]-<>?:(){}!^",
											"abcdefghijklmnpqrstuvwxy1234567890,./;'",
											"0",
											//"5",
											"0",
											"0",
											"0",
											"0"};*/
	public int ARROW_POSITION = 1;

	//base stats
	public PlayerStat HP_STAT = new PlayerStat(PLAYERSTATTYPE.HP, 5);
    public PlayerStat BAG_SIZE_STAT = new PlayerStat(PLAYERSTATTYPE.BAG_SIZE, 10);
    public PlayerStat PP_STAT = new PlayerStat(PLAYERSTATTYPE.PP, 5);
    public PlayerStat STAR_POINTS_STAT = new PlayerStat(PLAYERSTATTYPE.STAR_POINTS, 0);


	public int DAY_NUMBER = 1;
	public bool IS_HIDDEN = false;
	public float MASTER_MUSIC_VOL = .3f;
	public float MASTER_SFX_VOL = .5f;
	public int MENU_SELECT_STAGE = 1;
	public bool PLAYER_CAN_MOVE = true;
	public bool TUT_POPUP_ON = true;
	public bool CARRYING_SOMETHING= false;
	public int PROGRESS_LV = 0;
	public int TOTAL_TRASH = 0;
	public float X_SELF = 0;
	public float Y_SELF = 0;
	public float SPAWN_POS_X;
	public float SPAWN_POS_Y;

    public List<PinDefinition> shopPins = new List<PinDefinition> { null, null, null };

    public enum UPGRADES
    {
        NONE = 0,
        GLOVES = 1<<0
    }

    public UPGRADES UPGRADES_UNLOCKED = UPGRADES.NONE;

    public enum TUTORIALPOPUPS{

	NONE = 		0,
	LARGETRASH =		1<<0,
	ARMOREDENEMIES = 	1<<1,
	DAYNIGHT = 			1<<2,
	PINS = 				1<<3,

	}

	public TUTORIALPOPUPS TUT_POPUPS_SHOWN = TUTORIALPOPUPS.NONE;

    //-------------Enemy Global Variables------------//
    public enum BOSSES : int
    {
        NONE =           0,
        ONE =       1 << 0,
        TWO =       1 << 1,
        THREE =     1 << 2,
        FOUR =      1 << 3,
    }

    public BOSSES BOSSES_KILLED = BOSSES.NONE;

	public List<int> BOSS_HP_LIST = new List<int>{
													99, //Stuart
													99,
													99,
													99,
													99,
													99,
													99,
													99,
													99,
													99

												 };
	public List<int> STATUE_LIST = new List<int>(); //for w2 statue spawning
                                                    
                                                   
             //-----------Garbage-related variables------------//

    public struct LargeTrashItem
    {
        public LARGEGARBAGE type;
        public bool isViewed;
        public Sprite collectedDisplaySprite;
        public string collectedTitle;


        public LargeTrashItem(LARGEGARBAGE p_type)
        {
            type = p_type;
            isViewed = false;
            collectedDisplaySprite = null;
            collectedTitle = "";
        }
    };

	public Dictionary<string,EnemyInstance> BASIC_ENEMY_LIST = new Dictionary<string,EnemyInstance>();
    public List<LargeTrashItem> LARGE_TRASH_LIST = new List<LargeTrashItem>();

	public STANDARDGARBAGE STANDARD_GARBAGE_DISCOVERED = STANDARDGARBAGE.NONE;
    public STANDARDGARBAGE STANDARD_GARBAGE_VIEWED = STANDARDGARBAGE.NONE;

    public COMPOSTGARBAGE COMPOST_GARBAGE_DISCOVERED = COMPOSTGARBAGE.NONE;
    public COMPOSTGARBAGE COMPOST_GARBAGE_VIEWED = COMPOSTGARBAGE.NONE;

    public RECYCLABLEGARBAGE RECYCLABLE_GARBAGE_DISCOVERED = RECYCLABLEGARBAGE.NONE;
    public RECYCLABLEGARBAGE RECYCLABLE_GARBAGE_VIEWED = RECYCLABLEGARBAGE.NONE;

    public LARGEGARBAGE LARGE_GARBAGE_DISCOVERED = LARGEGARBAGE.NONE;
    public LARGEGARBAGE LARGE_GARBAGE_VIEWED = LARGEGARBAGE.NONE;

    // Not super excited about this but I can't make a generic enum and I'm not clever enough to code this better. -Steve
    // bag is locked if they haven't gathered the first garbage item.
    public bool IsBagLocked(int bagIndex)
    {
        switch (bagIndex)
        {
            case 0:
                return (STANDARD_GARBAGE_DISCOVERED & STANDARDGARBAGE.PAPER) != STANDARDGARBAGE.PAPER;
            case 1:
                return (COMPOST_GARBAGE_DISCOVERED & COMPOSTGARBAGE.COMPOST_TEST_1) != COMPOSTGARBAGE.COMPOST_TEST_1;
            case 2:
                return (RECYCLABLE_GARBAGE_DISCOVERED & RECYCLABLEGARBAGE.RECYCLABLE_TEST_1) != RECYCLABLEGARBAGE.RECYCLABLE_TEST_1;
            default:
                return true;
        }
    }

    public int GARBAGE_HAD = 0;
	public int LARGE_TRASH_COLLECTED = 0;
	//public List<Vector2> LARGE_TRASH_LOCATIONS = new List<Vector2>();
	public int MY_NUM_IN_ROOM = 0;
	public Vector3 DROPPED_TRASH_LOCATION = Vector3.zero;
	public List<int> TODAYS_TRASH_AQUIRED = new List<int>{0,0,0};
	public int TRASH_TYPE_SELECTED = 3;
	public int TRASH_AQUIRED = 0;
	public int ENEMIES_DEFEATED = 0;
	public bool SCENE_IS_TRANSITIONING = false;

	//------World-Related Global Vars ---------------//

	public int AMOUNT_TRASH_IN_WORLD = 0;
	public List<string> CALENDAR = new List<string>(); //put as string for now, not sure if it can be int
	public List<string> FRIEND_LIST = new List<string>();
	public List<string> WORLD_ENEMY_LIST = new List<string>();
	public int ROOM_NUM = 0;
	public int TIME_IN_DAY = -90;
	public int WORLD_NUM = 1;

	public List<string> WORLD_ROOM_DISCOVER = new List<string>{
												"obcdefghijklmnpqrstuvwxyz123456789;',.",
												"abcdefghijklmnpqrstuvwxyz123",
												"abcdefghijklmnpqrstuvwxyz123",
												"abcdefghijklmnpqrstuvwxyz123",
												};
	public List<string> WORLD_SIGNS_READ = new List<string>{
												"abcdefghijklmnpqrstuvwxyz12{}",
												"abcdefghijklmn/:rstuv}xy1234567",
												"abcdefghilm",
												"",
												"abcdefghijklmn",
												};		

    public WORLD WORLDS_UNLOCKED = WORLD.ONE;

    //---------------------------------------------------------------------//

    private void Awake(){
		if(Instance == null){
			Instance = this;
            Instance.PP_STAT = new PlayerStat(PLAYERSTATTYPE.PP, 5);
			DontDestroyOnLoad(gameObject);
		}else{
			Destroy(gameObject);
		}
    }

    // User Data implementation
    public override string UserDataKey()
    {
        return "GlobalVariable";
    }

    public override SimpleJSON.JSONObject Save()
    {
        SimpleJSON.JSONNode.longAsString = true; // Needed to store pins properly.
        var json_data = new SimpleJSON.JSONObject();

        // Game Settings.
        json_data["MASTER_MUSIC_VOL"] = MASTER_MUSIC_VOL;
        json_data["MASTER_SFX_VOL"] = MASTER_SFX_VOL;

        // Gameplay and collectables.
        json_data["DAY_NUMBER"] = DAY_NUMBER;

        json_data["pinsDiscoveredValue"] = pinsDiscoveredValue;
        json_data["pinsEquippedValue"] = pinsEquippedValue;
        json_data["pinsViewedValue"] = pinsViewedValue;

        json_data["STANDARD_GARBAGE_DISCOVERED"] = (uint)STANDARD_GARBAGE_DISCOVERED;
        json_data["STANDARD_GARBAGE_VIEWED"] = (uint)STANDARD_GARBAGE_VIEWED;
        json_data["COMPOST_GARBAGE_DISCOVERED"] = (uint)COMPOST_GARBAGE_DISCOVERED;
        json_data["COMPOST_GARBAGE_VIEWED"] = (uint)COMPOST_GARBAGE_VIEWED;
        json_data["RECYCLABLE_GARBAGE_DISCOVERED"] = (uint)RECYCLABLE_GARBAGE_DISCOVERED;
        json_data["RECYCLABLE_GARBAGE_VIEWED"] = (uint)RECYCLABLE_GARBAGE_VIEWED;
        json_data["LARGE_GARBAGE_DISCOVERED"] = (uint)LARGE_GARBAGE_DISCOVERED;
        json_data["LARGE_GARBAGE_VIEWED"] = (uint)LARGE_GARBAGE_VIEWED;

        // Stats and things
        json_data["STAR_POINTS"] = STAR_POINTS_STAT.GetMaxRaw();
        json_data["CURRENT_STAR_POINTS"] = STAR_POINTS_STAT.GetCurrent();

        json_data["Max_HP"] = HP_STAT.GetMaxRaw();
        json_data["BAG_SIZE"] = BAG_SIZE_STAT.GetMaxRaw();
        json_data["PPVALUE"] = PP_STAT.GetMaxRaw();
        json_data["DEJAVUCOUNT"] = DEJAVUCOUNT;
        json_data["CURSEVALUE"] = CURSEVALUE;
        json_data["MOMONEYVALUE"] = MOMONEYVALUE;
        json_data["PROGRESS_LV"] = PROGRESS_LV;
        json_data["UPGRADES"] = (uint)UPGRADES_UNLOCKED;

        json_data["TUT_POPUPS_SHOWN"] = (uint)TUT_POPUPS_SHOWN;

        return json_data;
    }

    public void SetDefaultStats()
    {
        STAR_POINTS_STAT.ResetCurrent();
        HP_STAT.ResetCurrent();
        PP_STAT.ResetCurrent();
        BAG_SIZE_STAT.ResetCurrent();
    }

    public override void Load(SimpleJSON.JSONObject json_data)
    {
        MASTER_MUSIC_VOL = json_data["MASTER_MUSIC_VOL"].AsFloat;
        MASTER_SFX_VOL = json_data["MASTER_SFX_VOL"].AsFloat;

        DAY_NUMBER = json_data["DAY_NUMBER"].AsInt;

        pinsDiscoveredValue = json_data["pinsDiscoveredValue"].AsLong;
        pinsEquippedValue = json_data["pinsEquippedValue"].AsLong;
        pinsViewedValue = json_data["pinsViewedValue"].AsLong;


        STANDARD_GARBAGE_DISCOVERED = (STANDARDGARBAGE)json_data["STANDARD_GARBAGE_DISCOVERED"].AsInt;
        STANDARD_GARBAGE_VIEWED = (STANDARDGARBAGE)json_data["STANDARD_GARBAGE_VIEWED"].AsInt;
        COMPOST_GARBAGE_DISCOVERED = (COMPOSTGARBAGE)json_data["COMPOST_GARBAGE_DISCOVERED"].AsInt;
        COMPOST_GARBAGE_VIEWED = (COMPOSTGARBAGE)json_data["COMPOST_GARBAGE_VIEWED"].AsInt;
        RECYCLABLE_GARBAGE_DISCOVERED = (RECYCLABLEGARBAGE)json_data["RECYCLABLE_GARBAGE_DISCOVERED"].AsInt;
        RECYCLABLE_GARBAGE_VIEWED = (RECYCLABLEGARBAGE)json_data["RECYCLABLE_GARBAGE_VIEWED"].AsInt;
        LARGE_GARBAGE_DISCOVERED = (LARGEGARBAGE)json_data["LARGE_GARBAGE_DISCOVERED"].AsInt;
        LARGE_GARBAGE_VIEWED = (LARGEGARBAGE)json_data["LARGE_GARBAGE_VIEWED"].AsInt;

        STAR_POINTS_STAT = new PlayerStat(PLAYERSTATTYPE.STAR_POINTS, json_data["STAR_POINTS"].AsInt);
        STAR_POINTS_STAT.SetCurrent(json_data["CURRENT_STAR_POINTS"].AsInt);

        HP_STAT = new PlayerStat(PLAYERSTATTYPE.HP, json_data["Max_HP"].AsInt);
        HP_STAT.ResetCurrent();

        BAG_SIZE_STAT = new PlayerStat(PLAYERSTATTYPE.BAG_SIZE, json_data["BAG_SIZE"].AsInt);
        HP_STAT.ResetCurrent();

        PP_STAT = new PlayerStat(PLAYERSTATTYPE.PP, json_data["PPVALUE"].AsInt);

        DEJAVUCOUNT = json_data["DEJAVUCOUNT"].AsInt;
        CURSEVALUE = json_data["CURSEVALUE"].AsInt;
        MOMONEYVALUE = json_data["MOMONEYVALUE"].AsInt;
        PROGRESS_LV = json_data["PROGRESS_LV"].AsInt;
        UPGRADES_UNLOCKED = (UPGRADES)json_data["UPGRADES"].AsInt;

        TUT_POPUPS_SHOWN = (TUTORIALPOPUPS)json_data["TUT_POPUPS_SHOWN"].AsInt;
    }

    // helpers
    public bool IsPinDiscovered(PIN p_type)
    {
        return (PINS_DISCOVERED & p_type) == p_type;
    }

    public bool IsPinEquipped(PIN p_type)
    {
        return (PINS_EQUIPPED & p_type) == p_type;
    }

    public bool IsPinViewed(PIN p_type){
    	return (PINS_VIEWED & p_type) == p_type;
    }

    public bool IsWorldUnlocked(WORLD world_type)
    {
		return (WORLDS_UNLOCKED & world_type) == world_type;
    }

    public bool IsUpgradeUnlocked(UPGRADES upgrade_type)
    {
        return (UPGRADES_UNLOCKED & upgrade_type) == upgrade_type;
    }

    public bool IsLargeTrashDiscovered(LARGEGARBAGE large_garbage_type)
    {
        return (LARGE_GARBAGE_DISCOVERED & large_garbage_type) == large_garbage_type;
    }
}