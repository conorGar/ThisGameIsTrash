using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	public int damageToPlayer;
	public bool destroyOnTileCollision;


	void OnTriggerEnter2D(Collider2D collider){
		if(destroyOnTileCollision && collider.gameObject.layer == 8) //tile layer
			ObjectPool.Instance.ReturnPooledObject(this.gameObject); 
	}


}
