using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PopulateWorld : MonoBehaviour
{

	public GameObject trash;
	public GameObject recyclables;
	public GameObject compost;
	public PinFunctionsManager playersPFM;

	int armoredEnemySpawn;
	int enemy1;
	int enemy2;
	int enemy3;


	void Awake(){
		
	}

	void Start ()
	{
        FriendList();

       

		
			Debug.Log("World list count check After");
		
			/*if(GlobalVariableManager.Instance.characterUpgradeArray[1][28].CompareTo('o') == 0){
                //2nd gathering perk: amount of trash in world increased by 25%
                WorldManager.Instance.amountTrashHere += Mathf.RoundToInt(WorldManager.Instance.amountTrashHere/ 4);
			}*/
			/*
			if(GlobalVariableManager.Instance.pinsEquipped[4] == 8){
				//cursed pin - less trash
				amntTrashHere -= 5;
			}
			if(GlobalVariableManager.Instance.pinsEquipped[24] == 2){//***i dont think this is really pos24?!?!?
				//Trashier Tmw Pin
				amntTrashHere += 5;
			}
			*/

            //resets Large Trash found in this are this day
            GlobalVariableManager.Instance.LARGE_TRASH_LIST.Clear();

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

			/*if(GlobalVariableManager.Instance.GLOBAL_ENEMY_HP[0].CompareTo("7") == 0){
				//needed for coon to spawn after being defeated by him the previous day
				GlobalVariableManager.Instance.GLOBAL_ENEMY_HP[0] = "8";
			}*/
		//}

		EnemyList();
		if(GlobalVariableManager.Instance.WORLD_NUM == 2) 
			BeachFriendUpdates();
		OtherFriendUpdates();
		PinEffects();
	}

	public void FriendList(){
		if(GlobalVariableManager.Instance.FRIEND_LIST.Count < 2){
			GlobalVariableManager.Instance.FRIEND_LIST.Add("coon1");//coonelius
			GlobalVariableManager.Instance.FRIEND_LIST.Add("rock1");//rock/stone/slab
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
			/*for(int i = 0; i < RoomManager.Instance.rooms.Count; i++){
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
			}//end of for loop*/
		}

	}//end of EnemyList()

	public void BeachFriendUpdates(){
		//Necklace crab room number reset
		/*if(GlobalVariableManager.Instance.FRIEND_LIST[12].Substring(3,4).CompareTo("o") == 0){
			GlobalVariableManager.Instance.GLOBAL_ENEMY_HP[10] = GlobalVariableManager.Instance.GLOBAL_ENEMY_HP[10].Replace(GlobalVariableManager.Instance.GLOBAL_ENEMY_HP[10][0],(char)Random.Range(1,10));
		}*/
	}

	public void OtherFriendUpdates(){
		//-----------Iggy--------------//
	
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
		if(GlobalVariableManager.Instance.IsPinEquipped(PIN.CLAWPIN)){
			//Claw Pin
			GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[1] = 7;
		}
	}
}

