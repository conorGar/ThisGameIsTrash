using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ev_ThrownTrash : MonoBehaviour {

	public string direction; //given by 'ThrowTrash.cs'
	Vector3 startingScale;


	void Start () {
		startingScale = gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnEnable(){
		gameObject.GetComponent<BoxCollider2D>().enabled = true;//cant collide again

	}

	void OnTriggerEnter2D(Collider2D collider){
		Debug.Log("throw trash collided-------");
		gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		gameObject.GetComponent<Ev_FallingProjectile>().falling = true;
			if(direction == "right"){
				this.gameObject.transform.localScale = startingScale;
				gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-3f,5f), ForceMode2D.Impulse);
			}else if(direction == "left"){
				this.gameObject.transform.localScale = new Vector3(startingScale.x*-1,startingScale.y, startingScale.z);
				gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(3f,5f), ForceMode2D.Impulse);
			}else if(direction == "up"){
				this.gameObject.transform.localScale = startingScale;
				gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(3f,-1f), ForceMode2D.Impulse);
			}else if(direction == "down"){
				this.gameObject.transform.localScale = startingScale;
				gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(3f,1f), ForceMode2D.Impulse);
			}
		gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
		gameObject.GetComponent<BoxCollider2D>().enabled = false;//cant collide again
	}
}
