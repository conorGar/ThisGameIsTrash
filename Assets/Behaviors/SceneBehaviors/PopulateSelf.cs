using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulateSelf : MonoBehaviour {
/*
	public int myRoomNum;
	public int pos1x;
	public int pos1y;
	public int pos2x;
	public int pos2y;
	public int pos3x;
	public int pos3y;
	public float enemyPos1x;
	public float enemyPos2x;
	public float enemyPos3x;
	public float enemyPos1y;
	public float enemyPos2y;
	public float enemyPos3y;
	public int enemiesInRoom = 3;

	public GameObject hud;
	public bool thisRoomHasNoEnemies = false;

	public GameObject enemy1;
	public GameObject enemy2;
	public GameObject enemy3;
	public GameObject armoredEnemy = null;
	public GameObject player;

	void Start(){
		GlobalVariableManager.Instance.ROOM_NUM = myRoomNum;
		GlobalVariableManager.Instance.PLAYER_CAN_MOVE = true;
		GameObject hudBack;
		SpawnPlayer();
		hudBack = Instantiate(hud,transform.position,Quaternion.identity);
		/*for(int i = 0; i < 3; i++){ // spawn trash loop
			GameObject spawnedTrash;
			Debug.Log(GlobalVariableManager.Instance.WORLD_LIST.Count);
			//Debug.Log(GlobalVariableManager.Instance.WORLD_LIST[myRoomNum]);
			if(GlobalVariableManager.Instance.WORLD_LIST[myRoomNum].Substring(0,1).CompareTo("o") != 0 && GlobalVariableManager.Instance.WORLD_LIST[myRoomNum].Substring(0,1).CompareTo("0") !=  0){
				//Debug.Log()
				if(i == 0){
					GlobalVariableManager.Instance.MY_NUM_IN_ROOM = 0;
					if(GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED.Count == 3)
						spawnedTrash = Instantiate(GameObject.Find("generic_garbage"),new Vector2(pos1x, pos1y), Quaternion.identity);
					else if(GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED.Count == 4)
						spawnedTrash = Instantiate(GameObject.Find("recyclables"), new Vector2(pos1x, pos1y), Quaternion.identity);
					else if(GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED.Count == 5)
						spawnedTrash = Instantiate(GameObject.Find("compost"), new Vector2(pos1x, pos1y), Quaternion.identity);
				} else if(i == 1){
					GlobalVariableManager.Instance.MY_NUM_IN_ROOM = 1;
					if(GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED.Count == 3)
						spawnedTrash = Instantiate(GameObject.Find("generic_garbage"), new Vector2(pos2x, pos2y), Quaternion.identity);
					else if(GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED.Count == 4)
						spawnedTrash = Instantiate(GameObject.Find("recyclables"), new Vector2(pos2x, pos2y), Quaternion.identity);
					else if(GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED.Count == 5)
						spawnedTrash = Instantiate(GameObject.Find("compost"), new Vector2(pos2x, pos2y), Quaternion.identity);
				} else if(i == 2){
					GlobalVariableManager.Instance.MY_NUM_IN_ROOM = 2;
					if(GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED.Count == 3)
						spawnedTrash = Instantiate(GameObject.Find("generic_garbage"),new Vector2( pos3x, pos3y), Quaternion.identity);
					else if(GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED.Count == 4)
						spawnedTrash = Instantiate(GameObject.Find("recyclables"), new Vector2(pos3x, pos3y), Quaternion.identity);
					else if(GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED.Count == 5)
						spawnedTrash = Instantiate(GameObject.Find("compost"), new Vector2(pos3x, pos3y), Quaternion.identity);
				} 
			}
		} // end of for loop that spawns trash */

		//V changes room discover string for this world
		/*
		if(GlobalVariableManager.Instance.WORLD_ROOM_DISCOVER[(GlobalVariableManager.Instance.WORLD_NUM -1)].Substring(myRoomNum, myRoomNum+1).CompareTo("o") != 0){
			if(GlobalVariableManager.Instance.pinsEquipped[25] != 0){
				//Call of The Wild pin
				GlobalVariableManager.Instance.TOTAL_TRASH++;
				GameObject callWildPopup;
				callWildPopup = Instantiate(GameObject.Find("UpgradeActor_bonusDisplay"), new Vector3(3,3,0), Quaternion.identity);
				callWildPopup.GetComponent<tk2dSpriteAnimator>().Play("CallWild");
			}
			GlobalVariableManager.Instance.WORLD_ROOM_DISCOVER[(GlobalVariableManager.Instance.WORLD_NUM -1)] = GlobalVariableManager.Instance.WORLD_ROOM_DISCOVER[(GlobalVariableManager.Instance.WORLD_NUM -1)].Replace(GlobalVariableManager.Instance.WORLD_ROOM_DISCOVER[(GlobalVariableManager.Instance.WORLD_NUM -1)][myRoomNum], 'o');
		}
		if(!thisRoomHasNoEnemies)
			StartCoroutine("enemySpawn");

	}

	IEnumerator enemySpawn(){ // obsulete!!!!****
		//Debug.Log("Enemy Spawn Activated");
		yield return new WaitForSeconds(.3f);
			if(GlobalVariableManager.Instance.pinsEquipped[30] != 0){
			//waifu wanted pin
				int waifuChance;
				if(GlobalVariableManager.Instance.pinsEquipped[34] != 0){
					//Fortune's Friend pin
					waifuChance = Random.Range(3,10);
				}else{
					waifuChance = Random.Range(1,10);
				}
				if(waifuChance == 10){
					enemy1 = GameObject.Find("upgradeActor_japaneseWoman");
					enemy2 = GameObject.Find("upgradeActor_japaneseWoman");
					enemy3 = GameObject.Find("upgradeActor_japaneseWoman");
				}
			}
		string myEnemyListVal = GlobalVariableManager.Instance.WORLD_ENEMY_LIST[myRoomNum];
		//Debug.Log("Enemy list value:"  + myEnemyListVal);
		if(myEnemyListVal.Substring(0,1).CompareTo("9") == 0 ||myEnemyListVal.Substring(0,1).CompareTo("5") == 0||myEnemyListVal.Substring(0,1).CompareTo("1") == 0){
			Debug.Log("**---Enemy 1 clear to spawn---**");
			if(enemy1 != null){
				GameObject spawnSmoke = Instantiate(GameObject.Find("death_smoke"), transform.position, Quaternion.identity);
				//spawnSmoke.GetComponent<Ev_deathSmoke>.Spawn();
				GameObject spawnedEnemy;

				spawnedEnemy = Instantiate(enemy1, new Vector3(enemyPos1x,enemyPos1y,0),Quaternion.identity);
				spawnedEnemy.GetComponent<EnemyTakeDamage>().myPosInBasicEnemyStr = 0;
				Debug.Log("**---Enemy 1 spawned---**");
			}
		}else if((myEnemyListVal.Substring(1,2).CompareTo("q") == 0||myEnemyListVal.Substring(1,2).CompareTo("6") == 0||myEnemyListVal.Substring(1,2).CompareTo("2") == 0 )&& enemiesInRoom > 1){
			Debug.Log("**---Enemy 2 clear to spawn---**");
			if(enemy2 != null){
				GameObject spawnSmoke = Instantiate(GameObject.Find("death_smoke"), transform.position, Quaternion.identity);
				//spawnSmoke.GetComponent<Ev_deathSmoke>.Spawn();
				GameObject spawnedEnemy;
				spawnedEnemy = Instantiate(enemy2, new Vector3(enemyPos2x,enemyPos2y,0),Quaternion.identity);
				spawnedEnemy.GetComponent<EnemyTakeDamage>().myPosInBasicEnemyStr = 1;
				Debug.Log("**---Enemy 2 spawned---**");
			}
		}else if((myEnemyListVal[2].CompareTo('q') == 0||myEnemyListVal[2].CompareTo('9') == 0||myEnemyListVal[2].CompareTo("3") == 0)&& enemiesInRoom > 2){
			Debug.Log("**---Enemy 3 clear to spawn---**");
			if(enemy3 != null){
				//GameObject spawnSmoke = Instantiate(GameObject.Find("death_smoke"), transform.position, Quaternion.identity);
				//spawnSmoke.GetComponent<Ev_deathSmoke>.Spawn();
				GameObject spawnedEnemy;
				spawnedEnemy = Instantiate(enemy3, new Vector3(enemyPos3x,enemyPos3y,0),Quaternion.identity);
				Debug.Log("**---Enemy 3 spawned---**");
				spawnedEnemy.GetComponent<EnemyTakeDamage>().myPosInBasicEnemyStr = 2;
				Debug.Log("**---Enemy 3 spawned 2---**");
			}else{
				Debug.Log("**---Enemy 3 is NULL");
			}
		}
		if(armoredEnemy != null && GlobalVariableManager.Instance.DAY_NUMBER > 1){
			float enemySpawnx = 0;
			float enemySpawny = 0;
			float previousEnemyY = 0;
			for(int i = 0; i < 3; i++){
			//Armored Enemy spawn
				if(myEnemyListVal.Substring(i,i+1).CompareTo("a") == 0){
					if(i == 0){
						enemySpawnx = enemyPos1x;
						enemySpawny = enemyPos1y;
					}else if(i == 1){
						enemySpawnx = enemyPos2x;
						enemySpawny = enemyPos2y;
						previousEnemyY = enemyPos1y;
					}else if(i == 2){
						enemySpawnx = enemyPos3x;
						enemySpawny = enemyPos3y;
						previousEnemyY = enemyPos2y;
					}
				GameObject spawnSmoke = Instantiate(GameObject.Find("death_smoke"), transform.position, Quaternion.identity);
				//spawnSmoke.GetComponent<Ev_deathSmoke>.Spawn();
				GameObject spawnedEnemy;
				spawnedEnemy = Instantiate(armoredEnemy, new Vector3(enemySpawnx,enemySpawny,0),Quaternion.identity);
				spawnedEnemy.GetComponent<EnemyTakeDamage>().myPosInBasicEnemyStr = i;
				}
			}//end of for loop
		}

	}//end of enemySpawn()

	void SpawnPlayer(){
		Debug.Log("SpawnPlayer Activated");
		GameObject playerInstance = Instantiate(player, new Vector2(GlobalVariableManager.Instance.SPAWN_POS_X, GlobalVariableManager.Instance.SPAWN_POS_Y), Quaternion.identity);
	}
	*/

}

