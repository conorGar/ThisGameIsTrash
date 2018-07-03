using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StringChangeTest : MonoBehaviour {

	/*float fadeSpeed = 1f;
	float alpha = 0f;
	int specialFade = 0;
	public Texture2D fadeOutTexture;*/
	// Use this for initialization

	public Image spawnTest;
	Vector2 refVelocity = new Vector2(0f,0f);
	public tk2dCamera currentCam; 
	Canvas currentCanvas;
	void Start () {
	GameObject tempCanvas = GameObject.Find("HUD");
	currentCanvas = tempCanvas.GetComponent<Canvas>();
	currentCam = tk2dCamera.Instance;
	string test = "abcdefghijklmn";


	test = test.Replace(test[1], 'z');
	//Debug.Log(test);
	StartCoroutine("WaitTest");
		//Debug.Log(test[1].CompareTo('z'));
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	/*void OnGUI (){
		alpha += specialFade * fadeSpeed * Time.deltaTime;
		alpha = Mathf.Clamp(alpha, 0f, .8f); //force the alpha to stay between 0 and 1

		//set color of texture
		GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
		GUI.depth = -100; // a lower number means draw on top
		GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height), fadeOutTexture );
	}
	public void Fade(){
		specialFade = 1;
	}
	*/
	IEnumerator WaitTest(){
		yield return new WaitForSeconds(.4f);
		Debug.Log("Test Wait Success");
		//Fade();
		Spawn();

	}

	void Spawn(){
	//proper camera spawn on GUI, but couldnt get it to move
		Image spawnInstance;
		spawnInstance = Instantiate(spawnTest, currentCam.GetComponent<Camera>().WorldToScreenPoint( new Vector3(-28.5f,-8f,0f)), Quaternion.identity);
		spawnInstance.transform.SetParent(currentCanvas.transform, false);
		spawnInstance.GetComponent<RectTransform>().position = Vector2.MoveTowards(spawnInstance.GetComponent<RectTransform>().position, new Vector2(spawnInstance.GetComponent<RectTransform>().position.x + 30,spawnInstance.GetComponent<RectTransform>().position.y),10f*Time.deltaTime);

	}
}
