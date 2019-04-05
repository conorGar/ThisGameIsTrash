using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CleanableItem : MonoBehaviour
{

	public int hp;
	public int spawnChance = 0; // out of 10
	public List<GameObject> possibleSpawnableItems = new List<GameObject>();
	public GameObject dirtyLookingObject;
	public bool isClean;
	public ParticleSystem dirtyPS;


	public AreaGarbageManager myAreaManager;


	void OnTriggerEnter2D(Collider2D collider){
		if(collider.tag == "Weapon"){
			if(hp > 0){
				hp--;
				ObjectPool.Instance.GetPooledObject("effect_dirtyHit",gameObject.transform.position);
				gameObject.GetComponent<Animator>().Play("dirtyHitBounce",0,-1f);
			
			}else{
				if(!isClean){
					SpawnItem();
					GameObject numberDisplay = ObjectPool.Instance.GetPooledObject("display_pollutedCleanNum",gameObject.transform.position);
					numberDisplay.transform.parent = this.transform;
					numberDisplay.GetComponent<tk2dTextMesh>().text = myAreaManager; // number of clean bushes in manager
					numberDisplay.GetComponent<Animator>().Play("pollutedCleanNumDisplayAni",0,-1f);
				}
			}
		}
	}


	void SpawnItem(){
		myAreaManager.CleanedFilty();
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

