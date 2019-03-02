using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CleanableItem : MonoBehaviour
{

	public int hp;
	public int spawnChance = 0; // out of 10
	public List<GameObject> possibleSpawnableItems = new List<GameObject>();
	public GameObject dirtyLookingObject;
	bool isClean;
	public ParticleSystem dirtyPS;



	void OnTriggerEnter2D(Collider2D collider){
		if(collider.tag == "Weapon"){
			if(hp > 0){
				hp--;
				ObjectPool.Instance.GetPooledObject("effect_dirtyHit",gameObject.transform.position);
				gameObject.GetComponent<Animator>().Play("dirtyHitBounce",0,-1f);
			
			}else{
				if(!isClean)
					SpawnItem();
			}
		}
	}


	void SpawnItem(){
		int spawnsItem = Random.Range(spawnChance,11);
		ObjectPool.Instance.GetPooledObject("effect_dirtyHit",gameObject.transform.position);

		if(spawnsItem == 10){
			Debug.Log("Spawns Item");
			GameObject myObject = ObjectPool.Instance.GetPooledObject(possibleSpawnableItems[Random.Range(0,possibleSpawnableItems.Count)].tag,gameObject.transform.position);
			myObject.transform.parent = this.transform;
			//TODO: System for determining how the item comes out
			if(myObject.GetComponent<Animator>() != null)
				myObject.GetComponent<Animator>().SetTrigger("Right");

		
		}

		ObjectPool.Instance.GetPooledObject("effect_cleanSparkles",gameObject.transform.position);
		gameObject.GetComponent<SpriteRenderer>().color = Color.white;
		dirtyPS.Stop();
		dirtyLookingObject.SetActive(false);
		isClean = true;
	}

}

