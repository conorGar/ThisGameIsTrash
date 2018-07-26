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
		if(!GlobalVariableManager.Instance.BASIC_ENEMY_LIST.ContainsValue(myEnemyInstance)){
			GlobalVariableManager.Instance.BASIC_ENEMY_LIST[spawnerID] = myEnemyInstance;
		}
		if(GlobalVariableManager.Instance.BASIC_ENEMY_LIST[spawnerID].dayOfRevival > GlobalVariableManager.Instance.DAY_NUMBER){
			isMyEnemyDead = true;
		}




	}
	public bool CheckIfEnemyDead(){
		if(GlobalVariableManager.Instance.BASIC_ENEMY_LIST[spawnerID].dayOfRevival > GlobalVariableManager.Instance.DAY_NUMBER){
			isMyEnemyDead = true;
		}
		return isMyEnemyDead;
	}
}
