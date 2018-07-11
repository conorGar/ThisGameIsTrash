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
	public string BOSSES_KILLED = "abcd";
	public List<string> GLOBAL_ENEMY_HP = new List<string>();
	public List<int> STATUE_LIST = new List<int>(); //for w2 statue spawning
	//public int RACOON_LOCATION = 16;
	//public int RACOON_TRASH_AQUIRED = 0;
	//-----------Garbage-related variables------------//
	public List<string> GARBAGE_DISCOVERY_LIST = new List<string>{
																"obcdefghijklmnpqrstuvwxy1?",
																"abcdefghijklmnpqrstuvwxy1?",
																"abcdefghijklmnpqrstuvwxy1?",
																"abcdefghijklmnpqrstuvwxy1}3456789"
																};
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
	public List<string> WORLD_LIST = new List<string>();
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

    public WORLDS WORLDS_UNLOCKED = WORLDS.WORLD_NONE;

    public enum WORLDS : int
    {
        WORLD_NONE =      0,
        WORLD_1 =    1 << 0,
        WORLD_2 =    1 << 1,
        WORLD_3 =    1 << 2,
        WORLD_4 =    1 << 3
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
        Debug.Log("World Index 1 == " + WorldIndex(WORLDS.WORLD_1));
        Debug.Log("World Index 2 == " + WorldIndex(WORLDS.WORLD_2));
        Debug.Log("World Index 3 == " + WorldIndex(WORLDS.WORLD_3));
        Debug.Log("World Index 4 == " + WorldIndex(WORLDS.WORLD_4));
    }
}
