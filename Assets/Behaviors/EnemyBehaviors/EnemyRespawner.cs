using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRespawner : MonoBehaviour {

	public GameObject myEnemy;
	public float respawnRate;
	public int maxEnemiesAtOnce;
	public Room myRoom;
	[HideInInspector]
	public List<GameObject> currentEnemies;

	int doOnce = 0;

	// Use this for initialization
	void Start () {
		InvokeRepeating("Spawn",.4f,respawnRate);
	}
	
	// Update is called once per frame
	void Update () {
		if(RoomManager.Instance.currentRoom != myRoom){//TODO: probably not the best way to keep this from going when player isnt in room but whatever
			CancelInvoke();
			for(int i = 0; i < currentEnemies.Count;i++){
				currentEnemies[i].SetActive(false);
			}
			currentEnemies.Clear();
			doOnce = 0;
		}else if(doOnce == 0){
			Debug.Log("in my room");
			doOnce = 1;
			Start();
		}
	}
	void Spawn(){
		StartCoroutine("SpawnDelay");
	}


	IEnumerator SpawnDelay(){

		Debug.Log("GOT HERE SPAWN 2");
		yield return new WaitForSeconds(Random.Range(1.1f,3.1f));
		Debug.Log("GOT HERE SPAWN");
		if(currentEnemies.Count < maxEnemiesAtOnce){
			GameObject spawnedEnemy = ObjectPool.Instance.GetPooledObject(myEnemy.tag,gameObject.transform.position);
			spawnedEnemy.GetComponent<EnemyTakeDamage>().myRespawner = this;
			currentEnemies.Add(spawnedEnemy);
			Debug.Log("ENEMY SHOULDVE BEEN SPAWNED");
		}
	}
}
