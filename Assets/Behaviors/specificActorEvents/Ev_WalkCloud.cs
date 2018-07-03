using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ev_WalkCloud : MonoBehaviour {

	float size = 0.2f;
	private bool fade = false;
	private float faderValue = 0f;
	// Use this for initialization
	void Start () {
		
		transform.localScale = new Vector3(.4f,.4f,.3f);
		InvokeRepeating("Grow",.1f,.1f);
		StartCoroutine("MainBehavior");

	}
	
	// Update is called once per frame
	void Update () {

		//fade out self
		if(fade && gameObject.GetComponent<tk2dSprite>().color.a > 0f){
			//Debug.Log(gameObject.GetComponent<tk2dSprite>().color.a);
			gameObject.GetComponent<tk2dSprite>().color = new Color(1f,1f,1f, 1f - faderValue);
			faderValue += .1f;
			}
	}

	void Grow () {
		if(transform.localScale.x < .7){
			transform.localScale = new Vector3(size,size,size);
			size = size +.1f;
			}
	}
	IEnumerator MainBehavior(){
		//Debug.Log("MAIN BEHAVIOR ACTIVATED");
		yield return new WaitForSeconds(.5f);
		fade = true;
		yield return new WaitForSeconds(.4f);
		Destroy(gameObject);
	}

	public void MoveLeft(){
		//Debug.Log("Move left");
		this.GetComponent<Rigidbody2D>().velocity = new Vector2(-3f,1f);
	}
	public void MoveRight(){
		//Debug.Log("Move right");
		this.GetComponent<Rigidbody2D>().velocity = new Vector2(3f,1f);
	}
	/*public void MoveDown(){
		this.GetComponent<Rigidbody2D>().velocity = new Vector2(-2,-5);
	}
	public void MoveUp(){
		this.GetComponent<Rigidbody2D>().velocity = new Vector2(2,2);
	}
	*/
}
