using UnityEngine;
using System.Collections;

public class Test_SmoothMove : MonoBehaviour
{
	public float targetX;
	int isMoving;
	float timeBeforeReturn;
	GameObject currentCam;
	Camera cam;
	// Use this for initialization
	void Start ()
	{
		currentCam = GameObject.Find("tk2dCamera");
		cam = currentCam.GetComponent<Camera>();

		//test
		Activate(-4f,1f);
	}
	
	// Update is called once per frame
	void Update ()
	{	
		if(isMoving == 1){
			if(gameObject.transform.position.x < targetX){
				gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(targetX - gameObject.transform.position.x, 0f);
			}else{
				gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			}
		}else if(isMoving ==2){
			if(currentCam.transform.position.x - 16.73f < gameObject.transform.position.x){
				gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2((currentCam.transform.position.x -16.73f) - gameObject.transform.position.x, 0f);
			}else{
				Destroy(gameObject);
			}
		}

	}

	public void Activate(float t, float returnTime){
		//startingX = transform.position.x;
		targetX = t;
		timeBeforeReturn = returnTime;
		isMoving = 1;
		if(returnTime != null && returnTime != 0){
			StartCoroutine("Return");
		}
	}

	IEnumerator Return(){
		yield return new WaitForSeconds(timeBeforeReturn);
		isMoving = 2;
	}
}

