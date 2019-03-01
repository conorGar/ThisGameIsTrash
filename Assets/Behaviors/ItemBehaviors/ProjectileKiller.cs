using UnityEngine;
using System.Collections;

public class ProjectileKiller : MonoBehaviour
{

	void OnTriggerEnter2D(Collider2D collider){
		if(collider.gameObject.layer == 10){ //projectile layer
			Debug.Log("projectile killer collision -x-x-x-x-x-");
			ObjectPool.Instance.GetPooledObject("effect_clank",collider.transform.position);
			ObjectPool.Instance.ReturnPooledObject(collider.gameObject); 
	
		}
	}
}

