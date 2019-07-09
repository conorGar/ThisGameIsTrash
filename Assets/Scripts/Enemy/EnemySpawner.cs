using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : EditorMonoBehaviour {
    public List<Enemy> enemies;
    public PathGrid pathGrid;
    public bool isMyEnemyDead = false;
    public EnemyInstance myEnemyInstance;
    public string enemyBodyName;
    public bool myEnemyHasNoDeadBody;
    public bool isUpperEnemy;
    [HideInInspector]
    public bool bodyDestroyed;// set true by 'ThrowableBody.cs - Death()'
    string spawnerID;
    bool spawnedBodyAlready;

    // Use this for initialization
    void Start () {
    	spawnerID = gameObject.name;
		/*if(!GlobalVariableManager.Instance.BASIC_ENEMY_LIST.TryGetValue(spawnerID,out myEnemyInstance)){
			
			//myEnemyInstance = GlobalVariableManager.Instance.BASIC_ENEMY_LIST[spawnerID];
			/
			GlobalVariableManager.Instance.BASIC_ENEMY_LIST[spawnerID] = myEnemyInstance;
			Debug.Log(myEnemyInstance.dayOfRevival);
			Debug.Log("DIDNT FIND VALUE" + GlobalVariableManager.Instance.BASIC_ENEMY_LIST[spawnerID]);
		}else{
			myEnemyInstance = GlobalVariableManager.Instance.BASIC_ENEMY_LIST[spawnerID];
			Debug.Log("Found value?");
		}
		if(GlobalVariableManager.Instance.BASIC_ENEMY_LIST[spawnerID].dayOfRevival > GlobalVariableManager.Instance.DAY_NUMBER){
			isMyEnemyDead = true;
		}*/

		if(!GlobalVariableManager.Instance.BASIC_ENEMY_LIST.ContainsKey(spawnerID)){
			GlobalVariableManager.Instance.BASIC_ENEMY_LIST[spawnerID] = myEnemyInstance;
		}else{
			myEnemyInstance = GlobalVariableManager.Instance.BASIC_ENEMY_LIST[spawnerID];
			Debug.Log("Found value?");
		}
		if(GlobalVariableManager.Instance.BASIC_ENEMY_LIST[spawnerID].dayOfRevival > GlobalVariableManager.Instance.DAY_NUMBER){
			isMyEnemyDead = true;
		}


	}
	public bool CheckIfEnemyDead(){
		if(GlobalVariableManager.Instance.BASIC_ENEMY_LIST[spawnerID].dayOfRevival > GlobalVariableManager.Instance.DAY_NUMBER){
			isMyEnemyDead = true;

		}else{
			GlobalVariableManager.Instance.BASIC_ENEMY_LIST[spawnerID].bodyDestroyed = false; //return back to normal if destroyed previous dead body that was spawned
		}
		Debug.Log("My spawner ID:" + spawnerID);
		Debug.Log(GlobalVariableManager.Instance.BASIC_ENEMY_LIST[spawnerID].dayOfRevival);
		return isMyEnemyDead;
	}

	public int GetDeathDate(){
		if(!spawnedBodyAlready && !bodyDestroyed){
		spawnedBodyAlready = true;
		return (GlobalVariableManager.Instance.BASIC_ENEMY_LIST[spawnerID].dayOfRevival - GlobalVariableManager.Instance.DAY_NUMBER);
		}else{
			return 0;
		}
		//0 = respawn
		//1 = skeleton
		//2= rotting body
		//3 = body
	}

	//check if this is an UpperEnemy or just 'Enemy' layer. Called in 'Room.cs'
	public int GetEnemyLayer(){
		if(isUpperEnemy){
			return 23; //UpperEnemy value
		}else{
			return 9;
		}
	}
}
