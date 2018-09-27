using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInstance : MonoBehaviour {

	//public string spawnerId;
	public int dayOfRevival;//set at death of enemy by EnemyTakeDamage
	[HideInInspector]
	public bool bodyDestroyed;// set true by 'ThrowableBody.cs - Death()'

	//attatched to an 


	// Use this for initialization
	void Start () {
		

		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/*public bool HasAliveEnemy(){//checked by EnemySpawner
		if(dayOfRevival > GlobalVariableManager.Instance.DAY_NUMBER){
			return false; 
		}else{
			 return true;
		}
	}*/
}
