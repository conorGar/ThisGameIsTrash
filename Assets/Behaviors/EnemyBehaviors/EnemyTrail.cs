using UnityEngine;
using System.Collections;

public class EnemyTrail : MonoBehaviour
{
	public GameObject trailPiece;
	public float spawnRate;

	void Start ()
	{
	
	}

	void OnEnable(){
		InvokeRepeating("TrailSpawn",spawnRate,spawnRate);
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void TrailSpawn(){
		GameObject trail = ObjectPool.Instance.GetPooledObject(trailPiece.tag,this.gameObject.transform.position);
		trail.GetComponent<Animator>().Play("generalFadeOut",-1,0f);
	}

}

