using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PopulateWorld : MonoBehaviour
{

	public GameObject trash;
	public GameObject recyclables;
	public GameObject compost;


	int numberOfRoomsInWorld;
	int amntTrashHere;
	int armoredEnemySpawn;
	int enemy1;
	int enemy2;
	int enemy3;


	void Start ()
	{
		if(GlobalVariableManager.Instance.WORLD_NUM == 1){
			numberOfRoomsInWorld = 42;
			amntTrashHere = 49;
		}else if(GlobalVariableManager.Instance.WORLD_NUM == 2){
			numberOfRoomsInWorld = 60;
			amntTrashHere = 65;
		}else if(GlobalVariableManager.Instance.WORLD_NUM == 3){
			numberOfRoomsInWorld = 49;
			amntTrashHere = 70;
		}else if(GlobalVariableManager.Instance.WORLD_NUM == 4){
			numberOfRoomsInWorld = 22;
			amntTrashHere = 70;
		}
		Debug.Log("World list count check");
		Debug.Log(GlobalVariableManager.Instance.WORLD_LIST.Count);

		if(GlobalVariableManager.Instance.GLOBAL_ENEMY_HP.Count < 6){
			GlobalVariableManager.Instance.GLOBAL_ENEMY_HP.Add("4");
			GlobalVariableManager.Instance.GLOBAL_ENEMY_HP.Add("22");//boss 1
			GlobalVariableManager.Instance.GLOBAL_ENEMY_HP.Add("6");
			GlobalVariableManager.Instance.GLOBAL_ENEMY_HP.Add("7");
			GlobalVariableManager.Instance.GLOBAL_ENEMY_HP.Add("abcdefghijklmnpqrstuvwxyz1234567");
			GlobalVariableManager.Instance.GLOBAL_ENEMY_HP.Add("25");//Boppy
			GlobalVariableManager.Instance.GLOBAL_ENEMY_HP.Add("12");//Questio
			GlobalVariableManager.Instance.GLOBAL_ENEMY_HP.Add("abcdefghijklmnpqrstuvwxyz");
			GlobalVariableManager.Instance.GLOBAL_ENEMY_HP.Add("15");//ex
			GlobalVariableManager.Instance.GLOBAL_ENEMY_HP.Add("10");//hash
			GlobalVariableManager.Instance.GLOBAL_ENEMY_HP.Add("0aefghijkl");//necklace crab
			GlobalVariableManager.Instance.GLOBAL_ENEMY_HP.Add("26");//Dudley(11) 26 for intro, switches to 17 after
			GlobalVariableManager.Instance.GLOBAL_ENEMY_HP.Add("17");
			GlobalVariableManager.Instance.GLOBAL_ENEMY_HP.Add("17");
			GlobalVariableManager.Instance.GLOBAL_ENEMY_HP.Add("13");//mask oni
			GlobalVariableManager.Instance.GLOBAL_ENEMY_HP.Add("13");//mask oni
			GlobalVariableManager.Instance.GLOBAL_ENEMY_HP.Add("13");//mask oni
			GlobalVariableManager.Instance.GLOBAL_ENEMY_HP.Add("4");//oniSama - starts 4, switched to 3 after end of intro
			GlobalVariableManager.Instance.GLOBAL_ENEMY_HP.Add("abcdefg");//onsen towel oni alive/dead
			GlobalVariableManager.Instance.GLOBAL_ENEMY_HP.Add("abcdefg");//w2 pelican guard string
			GlobalVariableManager.Instance.GLOBAL_ENEMY_HP.Add("29");//noodle ghost
			GlobalVariableManager.Instance.GLOBAL_ENEMY_HP.Add("30");//noodle ghost
			GlobalVariableManager.Instance.GLOBAL_ENEMY_HP.Add("30");//noodle ghost
			GlobalVariableManager.Instance.GLOBAL_ENEMY_HP.Add("34");//west noodle ghost(set up different because of previous room enemy spawns)
			GlobalVariableManager.Instance.GLOBAL_ENEMY_HP.Add("36");//OGSS
			GlobalVariableManager.Instance.GLOBAL_ENEMY_HP.Add("70");//death / final boss
			GlobalVariableManager.Instance.GLOBAL_ENEMY_HP.Add("17");//noodle ghost start
		}

		if(GlobalVariableManager.Instance.WORLD_LIST.Count < 2){
			Debug.Log("World list count check After");
			if(GlobalVariableManager.Instance.WORLD_ROOM_DISCOVER.Count < 5){
			//assigns string so com knows how many new rooms were discovered at result screen
				GlobalVariableManager.Instance.WORLD_ROOM_DISCOVER.Add(GlobalVariableManager.Instance.WORLD_ROOM_DISCOVER[GlobalVariableManager.Instance.WORLD_NUM - 1]);
			}
			Debug.Log(GlobalVariableManager.Instance.characterUpgradeArray[1].Length);
			if(GlobalVariableManager.Instance.characterUpgradeArray[1][28].CompareTo('o') == 0){
				//2nd gathering perk: amount of trash in world increased by 25%
				amntTrashHere = amntTrashHere + Mathf.RoundToInt(amntTrashHere/4);
			}
			/*if(GlobalVariableManager.Instance.pinsEquipped[24] == 1){
				//Hungry For More pin
				amntTrashHere += 5;
			}
			if(GlobalVariableManager.Instance.pinsEquipped[4] == 8){
				//cursed pin - less trash
				amntTrashHere -= 5;
			}
			if(GlobalVariableManager.Instance.pinsEquipped[24] == 2){//***i dont think this is really pos24?!?!?
				//Trashier Tmw Pin
				amntTrashHere += 5;
			}
			*/
			Debug.Log("Styart of first for loop");
			for(int i = 0; i < numberOfRoomsInWorld; i++){
				GlobalVariableManager.Instance.WORLD_LIST.Add("0");
			}
			if(GlobalVariableManager.Instance.GARBAGE_DISCOVERY_LIST.Count > 4){
				//resets Large Trash found in this are this day
				GlobalVariableManager.Instance.GARBAGE_DISCOVERY_LIST.RemoveAt(4);
			}
			int amountOfGarbageInRoom;
			int populateWhichRoomNext;

			/*for(int i = 0; i < numberOfRoomsInWorld; i++){
				amountOfGarbageInRoom = Random.Range(1,2);
				populateWhichRoomNext = Random.Range(0, numberOfRoomsInWorld);
				if(GlobalVariableManager.Instance.WORLD_LIST[populateWhichRoomNext].CompareTo("0") == 0){
					//determines the type of trash that 'populate self' spawns. 
					string typeOfTrash = (GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED.Count).ToString();
					if(amountOfGarbageInRoom == 0)
						GlobalVariableManager.Instance.WORLD_LIST[populateWhichRoomNext] = "ooo" + typeOfTrash + typeOfTrash + typeOfTrash;
					else if(amountOfGarbageInRoom == 1)
						GlobalVariableManager.Instance.WORLD_LIST[populateWhichRoomNext] = "aoo" + typeOfTrash + typeOfTrash + typeOfTrash;
					else if(amountOfGarbageInRoom == 2)
						GlobalVariableManager.Instance.WORLD_LIST[populateWhichRoomNext] = "abo" + typeOfTrash+ typeOfTrash + typeOfTrash;

					amntTrashHere = amntTrashHere -amountOfGarbageInRoom;
				}
			} //end of for loop
			Debug.Log("End of For loop");
			*/

			TrashSetUp();

			if(GlobalVariableManager.Instance.GLOBAL_ENEMY_HP[0].CompareTo("7") == 0){
				//needed for coon to spawn after being defeated by him the previous day
				GlobalVariableManager.Instance.GLOBAL_ENEMY_HP[0] = "8";
			}
		}
		Debug.Log("World list count:  " + GlobalVariableManager.Instance.WORLD_LIST.Count);


		if(GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED.Count == 5){
			//Hp Bonus with Cassie(set back at 'return events'
			GlobalVariableManager.Instance.characterUpgradeArray[3] = (GlobalVariableManager.Instance.characterUpgradeArray[3] + 1);
		}
		FriendList();
		EnemyList();
		if(GlobalVariableManager.Instance.WORLD_NUM == 2) 
			BeachFriendUpdates();
		OtherFriendUpdates();
		PinEffects();
	}

	public void FriendList(){
		if(GlobalVariableManager.Instance.FRIEND_LIST.Count < 2){
			GlobalVariableManager.Instance.FRIEND_LIST.Add("coon1");//coonelius
			GlobalVariableManager.Instance.FRIEND_LIST.Add("0a5efghijklmnpqrstuvwxy?!,/>(){}<");
			GlobalVariableManager.Instance.FRIEND_LIST.Add("05-bq1,(chijkl");//Jumbo
			GlobalVariableManager.Instance.FRIEND_LIST.Add("abcdefgh");
			GlobalVariableManager.Instance.FRIEND_LIST.Add("a9");
			GlobalVariableManager.Instance.FRIEND_LIST.Add("5");//Pablo
			GlobalVariableManager.Instance.FRIEND_LIST.Add("5");//Eel
			GlobalVariableManager.Instance.FRIEND_LIST.Add("0abcdefghijklmnq/>;?");//Chip(7)
			GlobalVariableManager.Instance.FRIEND_LIST.Add("05abcuv>ghi+-,/:");//DJKK
			GlobalVariableManager.Instance.FRIEND_LIST.Add("05!bcflrv+");//Lil' Krill
			GlobalVariableManager.Instance.FRIEND_LIST.Add("66abcdefghijklmnpqrstuvw");//Accountant
			GlobalVariableManager.Instance.FRIEND_LIST.Add("8");//Iggy next show countdown
			GlobalVariableManager.Instance.FRIEND_LIST.Add("abcdef05");//Fishy Dee
			GlobalVariableManager.Instance.FRIEND_LIST.Add("99");//Countdown until next murder(13)
			GlobalVariableManager.Instance.FRIEND_LIST.Add("0aqrs");//Sue
			GlobalVariableManager.Instance.FRIEND_LIST.Add("aaaa");//Will dafriend? maybe?
			GlobalVariableManager.Instance.FRIEND_LIST.Add("a0cdefgq");//Rock(16)
			GlobalVariableManager.Instance.FRIEND_LIST.Add("a");//Various one rs(17)
			GlobalVariableManager.Instance.FRIEND_LIST.Add("05adefgh");//Dojo FIghters
			GlobalVariableManager.Instance.FRIEND_LIST.Add("00abg");//Fruit Bowl(19)
			GlobalVariableManager.Instance.FRIEND_LIST.Add("00hzgl");//Death Cats(20)
			GlobalVariableManager.Instance.FRIEND_LIST.Add("0");//Death Cats dropoff
			GlobalVariableManager.Instance.FRIEND_LIST.Add("00a");// Da Monkey King
			GlobalVariableManager.Instance.FRIEND_LIST.Add("9aei");// Grey
			GlobalVariableManager.Instance.FRIEND_LIST.Add("0abcdef");//Murderer Trais(24)
			GlobalVariableManager.Instance.FRIEND_LIST.Add("xxx");//Murderer Traits discovered(25)
			GlobalVariableManager.Instance.FRIEND_LIST.Add("xxx");//Murderer String(26) - set blank at murder scene start
			GlobalVariableManager.Instance.FRIEND_LIST.Add("0ac5egjk");//Rat Builders
			GlobalVariableManager.Instance.FRIEND_LIST.Add("abcdef"); // Suicide Friends(28)
		}
		/*if(int.Parse(GlobalVariableManager.Instance.FRIEND_LIST[4][1]) > 0){
			//Count down till Jumbo's next film...
			GlobalVariableManager.Instance.FRIEND_LIST[4] = GlobalVariableManager.Instance.FRIEND_LIST[4].Replace(GlobalVariableManager.Instance.FRIEND_LIST[4][1],(char)(int.Parse(GlobalVariableManager.Instance.FRIEND_LIST[4].Substring(1,2)) - 1));
		}
		*/
	}

	public void EnemyList(){
		if(GlobalVariableManager.Instance.WORLD_ENEMY_LIST.Count < 2){
			for(int i = 0; i < numberOfRoomsInWorld; i++){
				if(GlobalVariableManager.Instance.DAY_NUMBER < 2){
					armoredEnemySpawn = Random.Range(10,14);
				}else{
					armoredEnemySpawn = 99;
				}

				enemy1 = Random.Range(0,3);
				enemy2 = Random.Range(5,8);
				enemy3 = Random.Range(9,11);

				string en1Str = enemy1.ToString();
				string en2Str = enemy2.ToString();
				string en3Str = enemy3.ToString();
				if(enemy3 == 10){
				//had to check for these 2 outcomes(10 and 11) because they are double digits
					if(armoredEnemySpawn != 10){
						GlobalVariableManager.Instance.WORLD_ENEMY_LIST.Add(en1Str + en2Str+ "q");
					}else{
						GlobalVariableManager.Instance.WORLD_ENEMY_LIST.Add("a" + en2Str + "q");
					}
	
				}else if(enemy3 == 11){
					if (armoredEnemySpawn != 10){
						GlobalVariableManager.Instance.WORLD_ENEMY_LIST.Add(en1Str + en2Str + "r");
					}else{
						GlobalVariableManager.Instance.WORLD_ENEMY_LIST.Add("a" + en2Str + "r");
					}
				}else{
					if (armoredEnemySpawn == 10){
						GlobalVariableManager.Instance.WORLD_ENEMY_LIST.Add("a" + en2Str + en3Str);
					}else{
						GlobalVariableManager.Instance.WORLD_ENEMY_LIST.Add(en1Str + en2Str + en3Str);
					}
				}
			}//end of for loop
		}

	}//end of EnemyList()

	public void BeachFriendUpdates(){
		//Necklace crab room number reset
		if(GlobalVariableManager.Instance.FRIEND_LIST[12].Substring(3,4).CompareTo("o") == 0){
			GlobalVariableManager.Instance.GLOBAL_ENEMY_HP[10] = GlobalVariableManager.Instance.GLOBAL_ENEMY_HP[10].Replace(GlobalVariableManager.Instance.GLOBAL_ENEMY_HP[10][0],(char)Random.Range(1,10));
		}
	}

	public void OtherFriendUpdates(){
		//-----------Iggy--------------//
		if(GlobalVariableManager.Instance.FRIEND_LIST[1].Substring(1,2).CompareTo("a") != 0 && GlobalVariableManager.Instance.FRIEND_LIST[1].Substring(1,2).CompareTo(":") != 0){
		//resets Iggy if you talked to him today or won/lost a trivia game today
			GlobalVariableManager.Instance.FRIEND_LIST[1] = GlobalVariableManager.Instance.FRIEND_LIST[1].Replace(GlobalVariableManager.Instance.FRIEND_LIST[1][1], 'a');
		}
		//----------------------------//


		//--------Coonelius---------//

		//commented out this part just because it was giving me errors while trying to figure out camera and didnt look into it...

		/*if(	int.Parse(GlobalVariableManager.Instance.FRIEND_LIST[0].Substring(1,2)) > 1){
			//coon countdown till next phase
			if(GlobalVariableManager.Instance.pinsEquipped[27] <= 2){
				// ^ deja vu pin, no countdown
				GlobalVariableManager.Instance.FRIEND_LIST[0] = GlobalVariableManager.Instance.FRIEND_LIST[0].Replace(GlobalVariableManager.Instance.FRIEND_LIST[0][1],(char)(int.Parse(GlobalVariableManager.Instance.FRIEND_LIST[0].Substring(1,2)) - 1));
			}
		}*/
		//-------------------------//

		//--------Rock-----------//
		if(	GlobalVariableManager.Instance.FRIEND_LIST[16].Substring(0,1).CompareTo("z") == 0){
			GlobalVariableManager.Instance.FRIEND_LIST[16] = GlobalVariableManager.Instance.FRIEND_LIST[16].Replace(GlobalVariableManager.Instance.FRIEND_LIST[16][0], 'x');
		}
		//----------------------//


		//------------****** WORLD 3 *****------------------//

		//---Dojo Figters-----//
		/*if(	int.Parse(GlobalVariableManager.Instance.FRIEND_LIST[18].Substring(1,2)) > 5 && int.Parse(GlobalVariableManager.Instance.FRIEND_LIST[18].Substring(1,2)) != 9){
		//Countdown until dojo challenger appointment
			if(GlobalVariableManager.Instance.pinsEquipped[27] <= 2){
			//Deja Vu Pin - No countdown
				GlobalVariableManager.Instance.FRIEND_LIST[18] = GlobalVariableManager.Instance.FRIEND_LIST[18].Replace(GlobalVariableManager.Instance.FRIEND_LIST[18],(int.Parse(GlobalVariableManager.Instance.FRIEND_LIST[18].Substring(1,2)) - 1).ToString());

			}
		}*/

		//^ 		commented out this part just because it was giving me errors while trying to figure out camera and didnt look into it...


		//-------------------------------------------------//

		//------------Sue-------------//

		if(GlobalVariableManager.Instance.FRIEND_LIST[14].Substring(1,2).CompareTo("a") != 0){
			GlobalVariableManager.Instance.FRIEND_LIST[14] = GlobalVariableManager.Instance.FRIEND_LIST[14].Replace((GlobalVariableManager.Instance.FRIEND_LIST[14][0]).ToString(),(int.Parse(GlobalVariableManager.Instance.FRIEND_LIST[14][0].ToString()) - 1).ToString());

		}

		//---------------------------//
	}//end of OtherFriendUpdates()

	public void PinEffects(){
		if(GlobalVariableManager.Instance.pinsEquipped[29] != 0){
			//Claw Pin
			GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[1] = 7;
		}
	}

	public void TrashSetUp(){

	//takes the position of each 'trashSpawner' gameobject in the scene and stores it in a list. Then, for each
	// item in the list, it will randomly select an item in the list and spawn a piece of trash there and replace
	//that list's item maybe with '999' or some value to show that trash has already spawned there, then will go
	//throught the list again, pick a random spot, see if the value != 999, and if not then place a trash there,
	//etc until the world's trash limit is reached!

		GameObject[] trashSpawns = GameObject.FindGameObjectsWithTag("Trash");
		List<Vector3> trashSpawnPositions = new List<Vector3>();

		//getting all the spawner positions...
		for(int i = 0; i < trashSpawns.Length; i++){
			trashSpawnPositions.Add(trashSpawns[i].transform.position);
		}

		int whichSpawner;

		//populating world with trash....
		for(int j = 0; j < amntTrashHere; j++){
			whichSpawner = Random.Range(0,trashSpawnPositions.Count);
            GameObject go = null;
            switch (GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED.Count)
            {
                case 3:
                    ObjectPool.Instance.GetPooledObject("generic_garbage", trashSpawnPositions[whichSpawner]);
                    break;
                case 4:
                    ObjectPool.Instance.GetPooledObject("generic_garbage", trashSpawnPositions[whichSpawner]);
                    break;
                case 5:
                    ObjectPool.Instance.GetPooledObject("generic_garbage", trashSpawnPositions[whichSpawner]);
                    break;
            }

			trashSpawnPositions.RemoveAt(whichSpawner);
		}

	}
}

