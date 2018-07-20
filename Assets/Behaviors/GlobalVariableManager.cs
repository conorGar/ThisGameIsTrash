using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariableManager : MonoBehaviour {


	//Note that many of the lists are given items in 'PopulateWorld' behavior.

	public static GlobalVariableManager Instance {get; private set;}

	public int value;
	public string[] characterUpgradeArray = new string[8]{
											"abcdefghijolmnpqrstuvwxy1234567890,./;'*[]-<>?:(){}!^",
											"abcdefghijklmnpqrstuvwxy1234567890,./;'",
											"0",
											"5",
											"0",
											"0",
											"0",
											"0"};
	public List<int> pinsEquipped = new List<int>{3}; 
	public int ARROW_POSITION = 1;
	public int CURRENT_HP = 1;
	public int DAY_NUMBER = 1;
	public int IS_HIDDEN = 0;
	public int MASTER_MUSIC_VOL = 30;
	public int MASTER_SFX_VOL = 30;
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
	public int[] shopPins = new int[]{0,0,0};
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

	public List<string> GLOBAL_ENEMY_HP = new List<string>();
	public List<int> STATUE_LIST = new List<int>(); //for w2 statue spawning
                                                    //public int RACOON_LOCATION = 16;
                                                    //public int RACOON_TRASH_AQUIRED = 0;
                                                    //-----------Garbage-related variables------------//

    public struct LargeTrashItem
    {
        public LARGEGARBAGE type;
        public int spriteIndex;
        public bool isViewed;
        public Sprite collectedDisplaySprite;

        public LargeTrashItem(LARGEGARBAGE p_type)
        {
            type = p_type;
            spriteIndex = 0;
            isViewed = false;
            collectedDisplaySprite = null;
        }
    };

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
	public List<Vector2> LARGE_TRASH_LOCATIONS = new List<Vector2>();
	public int MY_NUM_IN_ROOM = 0;
	public int ROOM_PLAYER_DIED_IN = 0;
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

    public WORLDS WORLDS_UNLOCKED = WORLDS.NONE;

    public enum WORLDS : int
    {
        NONE =               0,
        ONE =           1 << 0,
        TWO =           1 << 1,
        THREE =         1 << 2,
        FOUR =          1 << 3
    }

    // Returns the index of the World.
    // Probably best to use only with the defined enum values above.
    public int WorldIndex(WORLDS world)
    {
        for (int i=0; i < sizeof(WORLDS); ++i)
        {
            if ((int)world == 1 << i)
                return i;
        }
        return 0; 
    }

    public WORLDS WorldByIndex(int index)
    {
        return (WORLDS)(1 << index);
    }

    //---------------------------------------------------------------------//

    private void Awake(){
		if(Instance == null){
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}else{
			Destroy(gameObject);
		}
		if(pinsEquipped.Count < 2){
			for(int i = 0; i < 53; i++){
				pinsEquipped.Add(0);
			}
		}

#if DEBUG
        TestWorldIndex();
#endif
    }

    private void TestWorldIndex()
    {
        Debug.Log("World Index 1 == " + WorldIndex(WORLDS.ONE) + " enum from index: " + WorldByIndex(0));
        Debug.Log("World Index 2 == " + WorldIndex(WORLDS.TWO) + " enum from index: " + WorldByIndex(1));
        Debug.Log("World Index 3 == " + WorldIndex(WORLDS.THREE) + " enum from index: " + WorldByIndex(2));
        Debug.Log("World Index 4 == " + WorldIndex(WORLDS.FOUR) + " enum from index: " + WorldByIndex(3));

        WORLDS world = WORLDS.NONE;
        world |= WORLDS.ONE;
    }
}
