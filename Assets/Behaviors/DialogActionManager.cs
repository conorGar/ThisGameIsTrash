using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogActionManager : MonoBehaviour {

	public Friend friend;
	public GameObject movieScreen;
	public GameObject mainCam;
	public GameObject calendar;
	public DialogManager dialogManager;


	FriendEvent newestAddedEvent;


	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void JumboMoviePlay(){
		mainCam.GetComponent<Ev_MainCameraEffects>().CameraPan(new Vector3(-31.4f,70f,-10f),"JumboMovie");
		string filmToPlay = friend.GetComponent<JumboFriend>().GetCurrentFilm();
		movieScreen.GetComponent<tk2dSpriteAnimator>().Play(filmToPlay);
		friend.GetComponent<JumboFriend>().DeleteCurrentFilm();

		//determine next film
		friend.GetComponent<JumboFriend>().GenerateEventData();
		FriendEvent nextMovie = friend.GenerateEvent();
		CalendarManager.Instance.AddFriendEvent(nextMovie);
		newestAddedEvent = nextMovie;
		dialogManager.Invoke("ReturnFromAction",10f);//10= length of each movie
	}

	public void JumboMovieColor(){
		//set enhancement
		friend.GetComponent<JumboFriend>().movieEnhancement = "color";
	}public void JumboMovieSound(){
		//set enhancement
		friend.GetComponent<JumboFriend>().movieEnhancement = "sound";
	}public void JumboMovieMarketing(){
		//set enhancement
		friend.GetComponent<JumboFriend>().movieEnhancement = "marketing";
	}


	public void CalendarMark(){
		calendar.GetComponent<HUD_Calendar>().NewMarkSequence(newestAddedEvent.day,friend.name);
	}


	/*public void NewEventAdded(int dayOfEvent){
		CalendarManager.Instance.AddFriendEvent(new FriendEvent(dayOfEvent,friend));
	}*/


}
