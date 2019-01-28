using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class DialogActionManager : MonoBehaviour {

	public Friend friend;
	public GameObject movieScreen;
	public GameObject deadRat;
	public GameObject calendar;
	public DialogManager dialogManager;
	int numberOfActivation;
	public AudioClip projectorPlay;

	bool movieIsPlaying;
	FriendEvent newestAddedEvent;


	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void CalendarMark(){
		dialogManager.textBox.SetActive(false);
		dialogManager.currentlySpeakingIcon.gameObject.SetActive(false);
		Debug.Log(friend.name);
		Debug.Log(newestAddedEvent.day);
		calendar.SetActive(true);
		calendar.GetComponent<HUD_Calendar>().NewMarkSequence(newestAddedEvent.day,newestAddedEvent);
		calendar.GetComponent<HUD_Calendar>().Invoke("LeaveScreen",4f);
		dialogManager.Invoke("ReturnFromAction",5f);

	}





}
