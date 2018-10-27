using UnityEngine;
using System.Collections;

public class InvincibleEnemy : MonoBehaviour
{
	Rigidbody2D player;

	void Start(){
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();

	}

	void OnTriggerEnter2D(Collider2D melee){
		if(melee.tag == "Weapon"){
			Deflect();
			//Debug.Log("Collision with weapon: ");

		}
	}

	void Deflect(){
		GameObject littleStars = ObjectPool.Instance.GetPooledObject("effect_LittleStars");
		littleStars.transform.position = new Vector3((transform.position.x), transform.position.y, transform.position.z);
		littleStars.SetActive(true);

		if(gameObject.transform.position.x < player.transform.position.x){
			player.AddForce(new Vector2(2,0),ForceMode2D.Impulse);
		}else{
			player.AddForce(new Vector2(-2,0),ForceMode2D.Impulse);	
		}
	}
}

