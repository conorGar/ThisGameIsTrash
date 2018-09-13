using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : EditorMonoBehaviour {
    public List<Enemy> enemies;
    public bool isMyEnemyDead = false;
    public EnemyInstance myEnemyInstance;
    string spawnerID;

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
		}
		Debug.Log("My spawner ID:" + spawnerID);
		Debug.Log(GlobalVariableManager.Instance.BASIC_ENEMY_LIST[spawnerID].dayOfRevival);
		return isMyEnemyDead;
	}
}
