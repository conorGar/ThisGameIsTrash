using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CleanableItem : MonoBehaviour
{

	public int hp;
	public int spawnChance = 0; // out of 10
	public List<GameObject> possibleSpawnableItems = new List<GameObject>();
	bool isClean;


	void OnTriggerEnter2D(Collider2D collider){
		if(hp > 0){
			hp--;
		}else{
			if(!isClean)
				SpawnItem();
		}
	}


	void SpawnItem(){
		int spawnsItem = Random.Range(spawnChance,11);

		if(spawnsItem == 10){
			ObjectPool.Instance.GetPooledObject(possibleSpawnableItems[Random.Range(0,possibleSpawnableItems.Count)].tag,gameObject.transform.position);
		}

		isClean = true;
	}

}

