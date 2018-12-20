using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	public int damageToPlayer;
	public bool destroyOnTileCollision;
	public GameObject myShadow;

	void OnTriggerEnter2D(Collider2D collider){
		if(destroyOnTileCollision && collider.gameObject.layer == 8) //tile layer
			ObjectPool.Instance.ReturnPooledObject(this.gameObject); 
		else if(GlobalVariableManager.Instance.IsPinEquipped(PIN.PROJECTILEPROTECTOR) && collider.gameObject.tag == "Weapon"){
			ObjectPool.Instance.GetPooledObject("effect_thrownImpact",gameObject.transform.position);
			ObjectPool.Instance.ReturnPooledObject(this.gameObject); 
		}
	}


}
