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

	public void RatWithHatIntro(){
		friend.GetComponent<RatWithHatFriend>().hadIntroDialog = true;
		dialogManager.ReturnFromAction();
	}

	public void RatWithHatFinish(){
		dialogManager.FinishDialog();
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		player.GetComponent<EightWayMovement>().enabled = false;
		friend.GetComponent<RatWithHatFriend>().baseStatHUD.SetActive(true);
		friend.GetComponent<ActivateDialogWhenClose>().enabled = false;
		friend.GetComponent<ActivateDialogWhenClose>().speechBubbleIcon.SetActive(false);

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
