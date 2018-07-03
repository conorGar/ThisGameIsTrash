using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ev_CountdownNumber : MonoBehaviour {
	public Sprite one;
	public Sprite two;
	public Sprite three;
	public Sprite four;
	public Sprite five;
	public Sprite six;
	public Sprite seven;
	public Sprite eight;
	public Sprite nine;
	public Sprite ten;
	public Sprite zero;

	int count = 10;
	Sprite currentImage;
	Color myColor;
	float fadeTime = 0f;
	// Use this for initialization
	void Start () {
		//myColor = gameObject.GetComponent<MeshRenderer>().material.color;


		StartCoroutine("Reset");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public IEnumerator Reset(){
		count--;
		if(count == 2){
			currentImage = two;
		}else if(count == 1){
			currentImage = one;
		}else if(count == 3){
			currentImage = three;
		}else if(count == 4){
			currentImage = four;
		}else if(count == 5){
			currentImage = five;
		}else if(count == 6){
			currentImage = six;
		}else if(count == 7){
			currentImage = seven;
		}else if(count == 8){
			currentImage = eight;
		}else if(count == 9){
			currentImage = nine;
		}else if(count == 10){
			currentImage = ten;
		}else{
			currentImage = zero;
		}

		gameObject.GetComponent<Image>().sprite = currentImage;

		gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(173f,189f);
		//gameObject.GetComponent<RectTransform>().rect.height = 189;
		//FadeIn();

		yield return new WaitForSeconds(1f);

		//StartCoroutine("Reset");
		//FadeOut();
	}

	void FadeIn(){
		/*while(myColor.a < 255f){
			
			fadeTime += Time.deltaTime;
			float blend = Mathf.Clamp01(fadeTime/5);
			myColor.a = Mathf.Lerp(0.4f,1.0f,blend); //(start opacity, end opacity,blend)
			Debug.Log("alpha "+myColor.a);
			Debug.Log("fade time "+ fadeTime);


			//myColor.a += 1f;
			//gameObject.GetComponent<MeshRenderer>().material.color = Color.Lerp(gameObject.GetComponent<MeshRenderer>().material.color,
																					myColor, (5* Time.deltaTime));
			Debug.Log("alpha "+myColor.a);
			//fadeTime ++;
		}*/
	}

	void FadeOut(){
		while(fadeTime < 5){
			fadeTime += Time.deltaTime;
			float blend = Mathf.Clamp01(fadeTime/5);
			myColor.a = Mathf.Lerp(255f,0f,blend); //(start opacity, end opacity,blend)
		}
	}
}
