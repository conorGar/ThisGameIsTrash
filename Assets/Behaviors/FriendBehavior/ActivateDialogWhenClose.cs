﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Friend))]
public class ActivateDialogWhenClose : MonoBehaviour {

	//** --------------Really just the general dialog activator ---------------**//



    public Friend friend;

	

	public float xDistanceThreshold;
	public float yDistanceThreshold;
	//public GameObject myDialogIcon;
	public GameObject dialogCanvas;
	public GameObject dialogManager;
	public DialogActionManager dialogActionManager;
	public GameObject myDialogIcon;
	public bool cameraPanToFriendAtStart = true;
	public GameObject speechBubbleIcon;
	public string iconAnimationName;
	public string dialogIconName;//only needed to find object
	public AudioClip mySpeechIconSFX;
	[HideInInspector]
	public string dialogName;
	public bool autoStart;//start dialog when player gets close(without player hitting space)
	[HideInInspector]
	public DialogDefinition dialogDefiniton;
	[HideInInspector]
	public bool canTalkTo = true;
//	public bool tempBoolForActionManager;

	GameObject player;
	int spawnSpeechBubble = 0;


	void Awake(){
		player = GameObject.FindGameObjectWithTag("Player");


	}
	void OnEnable(){
		player = GameObject.FindGameObjectWithTag("Player");
		dialogName = friend.nextDialog;


	}

	
	void Update () {



			if(GlobalVariableManager.Instance.CARRYING_SOMETHING == false){
				
				if(dialogName.Length > 0 && player != null){

				//Debug.Log("Criteria met - 1" + Mathf.Abs(transform.position.x - player.transform.position.x) +"   " + Mathf.Abs(transform.position.y - player.transform.position.y));
					if(Mathf.Abs(transform.position.x - player.transform.position.x) < xDistanceThreshold &&Mathf.Abs(transform.position.y - player.transform.position.y) < yDistanceThreshold){

		//Debug.Log("Criteria met - 2");
						if(autoStart && canTalkTo){

		//Debug.Log("Criteria met - 3");
							dialogName = friend.nextDialog;
							ActivateDialog();
						}else if(canTalkTo){
							//Debug.Log("Autostart val:" + autoStart);
							//Debug.Log("canTalkTo val:" + canTalkTo);

							if(speechBubbleIcon != null && speechBubbleIcon.activeInHierarchy == false){
								SoundManager.instance.PlaySingle(mySpeechIconSFX);
								speechBubbleIcon.SetActive(true);
								spawnSpeechBubble = 1;
							}else{
								if(spawnSpeechBubble == 0){
									speechBubbleIcon = ObjectPool.Instance.GetPooledObject("speechIcon", gameObject.transform.position);
									spawnSpeechBubble = 1;
								}
							}
							if(ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT)){
								dialogManager.GetComponent<DialogManager>().canContinueDialog = true;
								dialogName = friend.nextDialog;
								ActivateDialog();
							}
						}
					}else{
						//Debug.Log("Far away from: " + gameObject.name);
						if(spawnSpeechBubble == 1 ){
							//Debug.Log("Speech Bubble should've died");
							ObjectPool.Instance.ReturnPooledObject(speechBubbleIcon);
							spawnSpeechBubble = 0;
							//speechBubbleIcon.SetActive(false);//disable speech bubble icon when far away
						}
					}
					
				}
			}//end of carry something check
		
	}

	public void SetDialog(DialogDefinition dd){
		dialogDefiniton = dd;
		Debug.Log("**MY DIALOG DEFINITION***"+dialogDefiniton.name);
		Debug.Log("**MY DIALOG DEFINITION***"+dialogDefiniton.name);
		Debug.Log("**MY DIALOG DEFINITION***"+dialogDefiniton.name);
		Debug.Log("**MY DIALOG DEFINITION***"+dialogDefiniton.name);



	}

	public void ActivateDialog(){
		GlobalVariableManager.Instance.PLAYER_CAN_MOVE = false;
		player.GetComponent<EightWayMovement>().StopMovement();
		player.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,0f);
		if(dialogCanvas.activeInHierarchy == false){
			if(cameraPanToFriendAtStart){
					dialogManager.GetComponent<DialogManager>().mainCam.GetComponent<Ev_MainCameraEffects>().CameraPan(gameObject.transform.position, "");
			}
			Debug.Log("Dialog Definition Name:"+ dialogDefiniton.name);
			myDialogIcon.GetComponent<DialogIconAnimationManager>().SwitchAni(iconAnimationName);
			dialogManager.GetComponent<DialogManager>().animationName = iconAnimationName;
			dialogManager.GetComponent<DialogManager>().myDialogDefiniton = dialogDefiniton;
			dialogManager.GetComponent<DialogManager>().dialogTitle = dialogName;
			dialogManager.GetComponent<DialogManager>().currentlySpeakingIcon = null;
			dialogManager.GetComponent<DialogManager>().canContinueDialog = true; //need this for starting after any previous dialog
			if(friend != null){ // = null for some one timers
				dialogActionManager.friend = friend;
				dialogManager.GetComponent<DialogManager>().characterName.text = friend.friendName;

			}
			dialogManager.GetComponent<DialogManager>().SetFriend(friend);
			dialogCanvas.SetActive(true);

			myDialogIcon.SetActive(true);
				if(myDialogIcon.transform.childCount>0){//if more than one icon
						List<GameObject> dialogIcons = new List<GameObject>();
						for(int i = 0; i< myDialogIcon.transform.childCount; i++){
							dialogIcons.Add(myDialogIcon.transform.GetChild(i).gameObject);
						}
						dialogManager.GetComponent<DialogManager>().dialogIcons = dialogIcons;
				}else{
					//dialogManager.GetComponent<DialogManager>().dialogIcons = new List<GameObject>{myDialogIcon};//one icon
					dialogManager.GetComponent<DialogManager>().currentlySpeakingIcon = myDialogIcon;
				}
			canTalkTo = false;
			}
	}

	public void GetData(DialogActivator friendActivator){ //given by DialogActivator.cs
		Debug.Log("ActivateWhenClose Data set properly...");
		dialogCanvas = friendActivator.dialogCanvas;
		myDialogIcon = friendActivator.dialogIcon;
		dialogActionManager = friendActivator.dialogActionManager;
		dialogManager = friendActivator.dialogManager;
		friend.calendar = friendActivator.calendar;
//		dialogManager.GetComponent<DialogManager>().tempBoolForActionManager = this.tempBoolForActionManager;
	}

}
