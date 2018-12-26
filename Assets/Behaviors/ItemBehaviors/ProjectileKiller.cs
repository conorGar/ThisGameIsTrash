using UnityEngine;
using System.Collections;

public class ProjectileKiller : MonoBehaviour
{

	void OnTriggerEnter2D(Collider2D collider){
		if(collider.gameObject.layer == 10) //projectile layer
			ObjectPool.Instance.ReturnPooledObject(collider.gameObject); 
	}
}

