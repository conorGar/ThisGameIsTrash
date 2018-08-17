using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ev_FallingProjectile : MonoBehaviour {
	public GameObject myShadow;
	public float gravityScaleDelay;
	public bool falling;
	// Use this for initialization


	void Start () {
		
		gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
		StartCoroutine("FallDelay");
	}

	void OnEnable(){

		gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
		StartCoroutine("FallDelay");
	}
	
	// Update is called once per frame
	void Update () {
		if(falling)
			myShadow.transform.position = new Vector2(gameObject.transform.position.x,myShadow.transform.position.y);

		if(gameObject.transform.position.y < myShadow.transform.position.y){
			Fell();
		}
	}

	IEnumerator FallDelay(){
		yield return new WaitForSeconds(gravityScaleDelay);
		gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
		myShadow.transform.parent = null;
		falling = true;
	}

	public void Fell(){
		ObjectPool.Instance.GetPooledObject("effect_landingSmoke",gameObject.transform.position);
		myShadow.transform.parent = gameObject.transform;
		myShadow.transform.localPosition = new Vector3(0,-2,0);
		gameObject.SetActive(false);
	}
}
