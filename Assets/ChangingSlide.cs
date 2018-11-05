using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangingSlide : MonoBehaviour {

	//public bool changingSlide;
	public float changeDelay;
	public int numOfChanges;
	public List<GameObject> variousDisplays;
	int currentDisplay;
	public IntroManager introManager;


	void OnEnable(){
		StartCoroutine(SlideChange());
	}

	IEnumerator SlideChange(){ //changes what appears on the slide without switching to a new slide
		yield return new WaitForSeconds(changeDelay);
		if(currentDisplay+1 < variousDisplays.Count){
			variousDisplays[currentDisplay].SetActive(false);
			currentDisplay++;
			variousDisplays[currentDisplay].SetActive(true);
		}else{
			currentDisplay++;
		}
		if(currentDisplay < numOfChanges){
			StartCoroutine(SlideChange()); //TODO: is this alright?
			introManager.NextSlide();
		}else{
			Debug.Log("Stopped slide change");
			StopCoroutine(SlideChange());
		}
	}
}
