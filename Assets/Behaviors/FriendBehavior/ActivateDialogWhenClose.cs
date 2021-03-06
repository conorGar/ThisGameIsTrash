﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Friend))]
public class ActivateDialogWhenClose : MonoBehaviour {

	//** --------------Really just the general dialog activator ---------------**//



    public Friend friend;

	

	public float distanceThreshold = 5;
	//public float yDistanceThreshold;
	public bool cameraPanToFriendAtStart = true;
	public GameObject speechBubbleIcon;
	public string iconAnimationName;
	public string dialogIconName;//only needed to find object
	public AudioClip mySpeechIconSFX;
	public string startNodeName;
	public bool autoStart;//start dialog when player gets close(without player hitting space)

	[HideInInspector]
	public DialogDefinition dialogDefiniton;
	public bool canTalkTo = true;
    public bool hideDialogIconsOnStart = false;
    public bool noIcon = false;
//	public bool tempBoolForActionManager;

	int spawnSpeechBubble = 0;
	int activatePanOnce = 0;


	void OnEnable(){
        startNodeName = friend.nextDialog;
        canTalkTo = true;

	}

    // Called by other friends to test if they are close and control the dialog as it's displayed and interacted with.
    // Moved this out of Update so it can be controlled better by the friend and what state they are in.
    public void Execute(string firstIcon = "", string secondIcon = "", string thirdIcon = "")
    {
        if (PlayerManager.Instance.controller.currentState.GetState() != JimState.CARRYING)
        {
            if (startNodeName.Length > 0)
            {
                if (Vector2.Distance(PlayerManager.Instance.player.transform.position, gameObject.transform.position) <  distanceThreshold)
                {
                    if (autoStart && canTalkTo)
                    {
                        startNodeName = friend.nextDialog;
                        ActivateDialog(firstIcon, secondIcon, thirdIcon);
                    }
                    else if (canTalkTo)
                    {
                            if (spawnSpeechBubble == 0)
                            {
                                speechBubbleIcon = ObjectPool.Instance.GetPooledObject("speechIcon", new Vector2(gameObject.transform.position.x+2f,gameObject.transform.position.y));
                                spawnSpeechBubble = 1;
                            }
                        
                        if (ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT))
                        {
                        	if(dialogDefiniton == null){
                        		dialogDefiniton = friend.myDialogDefiniton;
                        	}

                            DialogManager.Instance.canContinueDialog = true;
                            startNodeName = friend.nextDialog;
                            ActivateDialog(firstIcon, secondIcon, thirdIcon);
                        }
                    }
                }
                else
                {
                    if (spawnSpeechBubble == 1)
                    {
                        ObjectPool.Instance.ReturnPooledObject(speechBubbleIcon);
                        spawnSpeechBubble = 0;
                    }
                }

            }
        }//end of carry something check
    }

    public void ResetDefaults()
    {
        autoStart = true;
        ObjectPool.Instance.ReturnPooledObject(speechBubbleIcon);
        spawnSpeechBubble = 0;
        if(!noIcon){ //if town NPC can talk to repeatedly
        GetComponent<ActivateDialogWhenClose>().canTalkTo = false;
        }else{
        canTalkTo= true; //can talk to NPC townfolk again after speaking
        }
    }

    void Update () {
		
	}

	public void SetDialog(DialogDefinition dd){
		dialogDefiniton = dd;
		Debug.Log("**MY DIALOG DEFINITION***"+dialogDefiniton.name);
	}

	public void ActivateDialog(string firstIcon = "", string secondIcon = "", string thirdIcon = "")
    {
    	if(GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)){ // added so dialog that happens right at start of scene(ratWhoFat,etc) waits until DayDisplay is done fading into the scene
    		
				
				if(cameraPanToFriendAtStart && activatePanOnce == 0){
						Debug.Log("Camera Pan to friend activated");
		                CamManager.Instance.mainCamEffects.CameraPan(gameObject.transform.position, "");
		                if(noIcon){
		                	CamManager.Instance.mainCamEffects.ZoomInOut(1.9f,1f);
		                }
                        PlayerManager.Instance.player.GetComponent<EightWayMovement>().StopMovement();
                        PlayerManager.Instance.player.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,0f);
						GameStateManager.Instance.PushState(typeof(MovieState));
		                StartCoroutine(WaitUntilCamPan(firstIcon,secondIcon,thirdIcon));
		                activatePanOnce = 1;
		               
				}else{
					StartDialog(firstIcon,secondIcon,thirdIcon);

		        }
		       

	        
        }
	}

	IEnumerator WaitUntilCamPan(string first, string second, string third){
		yield return new WaitUntil( () => Vector2.Distance(CamManager.Instance.mainCam.gameObject.transform.position, gameObject.transform.position) < 2f);
		GameStateManager.Instance.PopState(); // pop movie state(pan to character)
		StartDialog(first,second,third);
	}

	void StartDialog(string firstIcon = "", string secondIcon = "", string thirdIcon = ""){
		Debug.Log("Start Dialog Activated");
		DialogManager.Instance.gameObject.SetActive(true);
				friend.OnActivateDialog();
				if(PlayerManager.Instance.player.GetComponent<EightWayMovement>()!=null)
                    PlayerManager.Instance.player.GetComponent<EightWayMovement>().StopMovement();
				if(PlayerManager.Instance.player.GetComponent<Rigidbody2D>()!=null)
                    PlayerManager.Instance.player.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,0f);

				if(DialogManager.Instance.dialogCanvas.activeInHierarchy == false){
					
					Debug.Log("Dialog Definition Name:"+ dialogDefiniton.name);
					DialogManager.Instance.animationName = iconAnimationName;
		            DialogManager.Instance.myDialogDefiniton = dialogDefiniton;
		            DialogManager.Instance.dialogTitle = startNodeName;
		            DialogManager.Instance.canContinueDialog = true; //need this for starting after any previous dialog
					if(friend != null){ // = null for some one timers
		                DialogManager.Instance.dialogActionManager.friend = friend;
		                DialogManager.Instance.characterName.text = friend.friendName;
					}
					if(!noIcon){
		            	DialogManager.Instance.SetDialogIconByID(dialogDefiniton.dialogIconID);
						if (DialogManager.Instance.currentlySpeakingIcon.GetType() == typeof(MultipleDialogIconsManager)) {
		                // A flag to check if all dialogIcons should be hidden initially (Rocks) or visable initially (white trash army)
		                var multiIcon = (MultipleDialogIconsManager)DialogManager.Instance.currentlySpeakingIcon;
		                for (int i = 0; i < multiIcon.icons.Count; i++) {
		                    multiIcon.icons[i].gameObject.SetActive(!hideDialogIconsOnStart);
		                }

		                var multiDialog = (MultipleDialogIconsManager)DialogManager.Instance.currentlySpeakingIcon;
		                multiDialog.SetStartingIcons(firstIcon, secondIcon, thirdIcon);
		           		 }
		            }else{
		            	DialogManager.Instance.SetDialogIconAsModel(this.gameObject);
		            }
		            DialogManager.Instance.SetFriend(friend);

		            

		            if(!noIcon){
		            DialogManager.Instance.currentlySpeakingIcon.gameObject.SetActive(true);
		            DialogManager.Instance.currentlySpeakingIcon.SetTalking(true);
		            }
		            Debug.Log("Can talk to deactivated here....");
		            canTalkTo = false;

		            DialogManager.Instance.dialogCanvas.SetActive(true);
		            Debug.Log("Got here with activate dialog execute" + DialogManager.Instance.dialogCanvas.activeInHierarchy);
		            activatePanOnce = 0;
		        }
	}
}
