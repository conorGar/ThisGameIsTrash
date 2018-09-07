﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.PostProcessing;

public class Ev_SignPost : MonoBehaviour {

	public GameObject player;
	public float distanceUntilGlow;
	public GameObject signPostHUD;
	public string myText;
	public Sprite myPicture;
	public AudioClip signRise;
	public GameObject mainCamera;
	public PostProcessingProfile blur;
	float lastRealTimeSinceStartup;
	int glowCheck;
	tk2dSpriteAnimator myAnim;


	// Use this for initialization
	void Start () {
		myAnim = gameObject.GetComponent<tk2dSpriteAnimator>();

	}
	
	// Update is called once per frame
	void Update () {
		if(Mathf.Abs(player.transform.position.x - gameObject.transform.position.x) < distanceUntilGlow && Mathf.Abs(player.transform.position.y - gameObject.transform.position.y) < distanceUntilGlow){
			if(glowCheck == 0){
				myAnim.Play("glow");
				glowCheck = 1;
			}
		}else{
			if (glowCheck > 0){
				glowCheck = 0;
			}
		}


		if(ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT))
        {
			if(glowCheck == 1){
				SoundManager.instance.PlaySingle(signRise);
				mainCamera.GetComponent<PostProcessingBehaviour>().profile = blur;
				signPostHUD.SetActive(true);
				signPostHUD.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = myText;
				if(myPicture != null){
					signPostHUD.transform.GetChild(1).GetComponent<Image>().sprite = myPicture;
				}
				glowCheck = 2;
				lastRealTimeSinceStartup = Time.realtimeSinceStartup; 
				Time.timeScale = 0;

			}else if(glowCheck == 2){
				Time.timeScale = 1;
				signPostHUD.SetActive(false);
				mainCamera.GetComponent<PostProcessingBehaviour>().profile = null;
				signPostHUD.transform.localPosition = new Vector3(13f,-8f,10f);
				glowCheck = 0;
			}
		}

		if(signPostHUD.activeInHierarchy){
			signPostHUD.transform.localPosition = Vector3.Lerp(signPostHUD.transform.localPosition,new Vector3(13f,7f,10f),.1f*(Time.realtimeSinceStartup - lastRealTimeSinceStartup));
		}

	}


}
