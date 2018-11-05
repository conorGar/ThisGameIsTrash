using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroSlide : MonoBehaviour {

	public Color backgroundColor;
	public float slideDuration;
	public List<GameObject> camMarkers;
	public tk2dCamera mainCam;
	public float camPanSpeed = 1;
	public IntroManager introManager;
	public SpriteRenderer background;



	bool camMoving = true;
	GameObject nextMarker;
	int markerIndex;
	// Use this for initialization
	void OnEnable () {
		background.color = backgroundColor;
		if(camMarkers.Count >0){
			mainCam.gameObject.transform.position = camMarkers[0].transform.position;
			nextMarker = camMarkers[0];
		}

		StartCoroutine(SlideEnd());
		

	}
	
	// Update is called once per frame
	void Update () {
		if(camMoving && nextMarker != null){
			if(Vector3.Distance(mainCam.transform.position,nextMarker.transform.position) > 1){
				mainCam.transform.position = Vector3.MoveTowards(mainCam.transform.position,nextMarker.transform.position,camPanSpeed*Time.deltaTime);
			}else{
				if(camMarkers.Count > markerIndex){
					markerIndex++;
					nextMarker = camMarkers[markerIndex];
				}else{
					camMoving = false; //end of slide
				}
			}
		}
	}

	public IEnumerator SlideEnd(){
		yield return new WaitForSeconds(slideDuration);
		introManager.fader.SetTrigger("FadeOut");
			Debug.Log("Fade out");
		yield return new WaitForSeconds(1.5f);
		introManager.NextSlide();
		this.gameObject.SetActive(false);
		StopCoroutine(SlideEnd());
	}

	public IEnumerator ChangingSlideEnd(){
		yield return new WaitForSeconds(slideDuration);
		introManager.NextSlide();
		StopCoroutine(ChangingSlideEnd());
	}


}
