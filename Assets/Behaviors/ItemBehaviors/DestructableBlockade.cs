using UnityEngine;
using System.Collections;

public class DestructableBlockade : MonoBehaviour
{

	

	void OnTriggerEnter2D(Collider2D collider){
		Debug.Log("Destructable object collides with trigger" + collider.tag);
		if(collider.tag == "TrashBomb"){
			gameObject.SetActive(false);
		}
	}

	void OnCollisionEnter2D(Collision2D collider){
		Debug.Log("Destructable object collides with collision" +collider.gameObject.tag );

		if(collider.gameObject.tag == "TrashBomb"){
			gameObject.SetActive(false);
		}
	}
}

