using UnityEngine;
using System.Collections;

public class DestructableBlockade : MonoBehaviour
{

	public int hp;

	void OnTriggerEnter2D(Collider2D collider){
		Debug.Log("Destructable object collides with trigger" + collider.tag);
		if(collider.tag == "TrashBomb"){
			gameObject.SetActive(false);
		}else if(collider.gameObject.layer == 10){//projectiles
			if(hp > 0){
				hp--;
				ObjectPool.Instance.ReturnPooledObject(collider.gameObject);
			}else{
				gameObject.SetActive(false);
			}
		}
	}

	void OnCollisionEnter2D(Collision2D collider){
		Debug.Log("Destructable object collides with collision" +collider.gameObject.tag );

		if(collider.gameObject.tag == "TrashBomb"){
			gameObject.SetActive(false);
		}else if(collider.gameObject.layer == 10){//projectiles
			if(hp > 0){
				hp--;
				ObjectPool.Instance.ReturnPooledObject(collider.gameObject);
			}else{
				gameObject.SetActive(false);
			}
		}
	}
}

