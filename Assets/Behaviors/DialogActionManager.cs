﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class DialogActionManager : MonoBehaviour {

	public Friend friend;
	public GameObject movieScreen;
	public GameObject mainCam;
	public GameObject deadRat;
	public GameObject calendar;
	public DialogManager dialogManager;
	int numberOfActivation;

	FriendEvent newestAddedEvent;


	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void CurrentDialogAction(){
		numberOfActivation++;
		dialogManager.mainCam.GetComponent<PostProcessingBehaviour>().profile = null;
		if(friend.nextDialog == "Jumbo2"){
			if(numberOfActivation == 1){//pan to jumbo hiding in bush...
				mainCam.GetComponent<Ev_MainCameraEffects>().CameraPan(friend.transform.position," ");
				dialogManager.textBox.SetActive(false);
				dialogManager.currentlySpeakingIcon.SetActive(false);
				dialogManager.Invoke("ReturnFromAction",2f);
			}else if(numberOfActivation == 2){//pan to audience...
				dialogManager.textBox.SetActive(false);
				mainCam.GetComponent<Ev_MainCameraEffects>().CameraPan(deadRat.transform.position," ");
				//mainCam.GetComponent<Ev_MainCameraEffects>().ZoomInOut(2f,.1f);
				StartCoroutine("FilmSetPan");
				dialogManager.Invoke("ReturnFromAction",5f);
			}else if(numberOfActivation == 3){//dead rat zoom in...
				dialogManager.ReturnFromAction();
				mainCam.GetComponent<Ev_MainCameraEffects>().CameraPan(deadRat.transform.position," ");
				mainCam.GetComponent<Ev_MainCameraEffects>().ZoomInOut(3f,.1f);
				dialogManager.currentlySpeakingIcon.SetActive(false);
				dialogManager.variableText = friend.GetComponent<JumboFriend>().GetCurrentFilm().Replace('_',' ');
			}else if(numberOfActivation == 4){//return to jumbo after dead rat
				mainCam.GetComponent<Ev_MainCameraEffects>().CameraPan(friend.transform.position," ");
				dialogManager.Invoke("ReturnFromAction",.1f);
				mainCam.GetComponent<Ev_MainCameraEffects>().ZoomInOut(1.15f,4f);
				dialogManager.mainCam.GetComponent<PostProcessingBehaviour>().profile = dialogManager.dialogBlur;

			}
		}
	}

	public void JumboMoviePlay(){
		mainCam.GetComponent<Ev_MainCameraEffects>().CameraPan(new Vector3(-31.4f,70f,-10f),"JumboMovie");

		dialogManager.textBox.SetActive(false);
		dialogManager.mainCam.GetComponent<PostProcessingBehaviour>().profile = null;//TODO: returns to NO effect, not sure if you want this, future Conor
		dialogManager.currentlySpeakingIcon.SetActive(false);



		string filmToPlay = friend.GetComponent<JumboFriend>().GetCurrentFilm();
		movieScreen.GetComponent<tk2dSpriteAnimator>().Play(filmToPlay);
		if(movieScreen.transform.GetChild(1).gameObject.activeInHierarchy){//if film color is enabled
			movieScreen.GetComponent<tk2dSpriteAnimator>().Play(filmToPlay + "_Color");
		}
		friend.GetComponent<JumboFriend>().DeleteCurrentFilm();

		//determine next film
		friend.GetComponent<JumboFriend>().GenerateEventData();
		FriendEvent nextMovie = friend.GenerateEvent();
		Debug.Log("******NEXT MOVIE DAY****** = " + nextMovie.day);
		CalendarManager.Instance.AddFriendEvent(nextMovie);
		newestAddedEvent = nextMovie;
		dialogManager.variableText = friend.GetComponent<JumboFriend>().GetCurrentFilm().Replace('_',' ');
		dialogManager.ChangeIcon("SurprisedAni");
		Debug.Log("***SET VARIABLE TEXT TO: " + friend.GetComponent<JumboFriend>().GetCurrentFilm());


		if(friend.nextDialog == "Start"){
			StartCoroutine("AfterFirstMovie");
		}else{
			dialogManager.Invoke("ReturnFromAction",10f);//10= length of each movie 
		}

	}

	IEnumerator AfterFirstMovie(){
		yield return new WaitForSeconds(10f);
		mainCam.GetComponent<Ev_MainCameraEffects>().CameraPan(friend.transform.position,"JumboMovie");
		yield return new WaitForSeconds(1f);
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		mainCam.GetComponent<Ev_MainCameraEffects>().CameraPan(player.transform.position,"JumboMovie");
		yield return new WaitForSeconds(1f);
		mainCam.GetComponent<Ev_MainCameraEffects>().CameraPan(friend.transform.position,"JumboMovie");
		yield return new WaitForSeconds(.5f);
		dialogManager.Invoke("ReturnFromAction",.1f);//10= length of each movie TODO:check for if this is the first movie or not, if not activate this line of code


	}

	IEnumerator FilmSetPan(){
		yield return new WaitForSeconds(4f);
		mainCam.GetComponent<Ev_MainCameraEffects>().CameraPan(friend.transform.position," ");

	}
	public void RatWithHatIntro(){
		friend.GetComponent<RatWithHatFriend>().hadIntroDialog = true;
		dialogManager.ReturnFromAction();
	}

	public void RatWithHatFinish(){
		dialogManager.FinishDialog();
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		player.GetComponent<EightWayMovement>().enabled = false;
		friend.GetComponent<RatWithHatFriend>().baseStatHUD.SetActive(true);
	}

	public void JumboMovieColor(){
		//set enhancement
		friend.GetComponent<JumboFriend>().movieEnhancement = "color";
		dialogManager.Invoke("ReturnFromAction",.1f);
	}public void JumboMovieSound(){
		//set enhancement
		friend.GetComponent<JumboFriend>().movieEnhancement = "sound";
		dialogManager.Invoke("ReturnFromAction",.1f);
	}public void JumboMovieMarketing(){
		//set enhancement
		friend.GetComponent<JumboFriend>().movieEnhancement = "marketing";
		dialogManager.Invoke("ReturnFromAction",.1f);
	}

	void DayAsStringSet(){
		dialogManager.variableText = newestAddedEvent.day.ToString();
		Debug.Log(">>>>>DAY AS STRING SET TO: " + newestAddedEvent.day.ToString());
		dialogManager.Invoke("ReturnFromAction",.1f);

	}


	public void CalendarMark(){
		dialogManager.textBox.SetActive(false);
		dialogManager.currentlySpeakingIcon.SetActive(false);
		Debug.Log(friend.name);
		Debug.Log(newestAddedEvent.day);
		calendar.SetActive(true);
		calendar.GetComponent<HUD_Calendar>().NewMarkSequence(newestAddedEvent.day,friend.name);
		calendar.GetComponent<HUD_Calendar>().Invoke("LeaveScreen",4f);
		dialogManager.Invoke("ReturnFromAction",5f);

	}





}