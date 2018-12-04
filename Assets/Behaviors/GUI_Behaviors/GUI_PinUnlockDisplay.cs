using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GUI_PinUnlockDisplay : MonoBehaviour {

	int phase = 1;
	private float lastRealTimeSinceStartup;
	public GameObject pinTitle;
	public GameObject myPinDescription;
	public GameObject pinImage;
	public AudioClip unlockSFX;
	public AudioClip closeSfx;

	// Use this for initialization
	void Start () {
		
	}

	void OnEnable(){
	phase = 1;
	SoundManager.instance.PlaySingle(unlockSFX);
	}
	
	// Update is called once per frame
	void Update () {

		if(ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT))
        {
			if(phase == 1){
				GameStateManager.Instance.PopState();
				StartCoroutine("Delays");
			}
		}

		//Time.realtimeSinceStartup -= lastRealTimeSinceStartup;

		lastRealTimeSinceStartup = Time.realtimeSinceStartup;
	}

	IEnumerator Delays(){
		Debug.Log("Delays coroutine activated");
		if(phase == 0){
		yield return new WaitForSeconds(.5f);
		phase = 1;
		}else if(phase == 1){
			phase = 2;
			gameObject.GetComponent<SpecialEffectsBehavior>().SmoothMovementToPoint(transform.position.x,5f,.4f);
			SoundManager.instance.PlaySingle(closeSfx);
			yield return new WaitForSeconds(.4f);
			Time.timeScale = 1;
			gameObject.SetActive(false);
		}

	}

	public void SetValues(string thisPinTitle, string pinDescription, Sprite pinSprite){
		pinTitle.GetComponent<TextMeshProUGUI>().text = thisPinTitle;
		myPinDescription.GetComponent<TextMeshProUGUI>().text = pinDescription;
		pinImage.GetComponent<SpriteRenderer>().sprite = pinSprite;
	}
}
