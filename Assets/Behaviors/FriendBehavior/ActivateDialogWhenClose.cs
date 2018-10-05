using System.Collections;
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
	public DialogManager dialogManager;
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

	GameObject player;



	void Awake(){
		player = GameObject.FindGameObjectWithTag("Player");


	}
	void OnEnable(){
		player = GameObject.FindGameObjectWithTag("Player");


	}

    // Called by other friends to test if they are close and control the dialog as it's displayed and interacted with.
    // Moved this out of Update so it can be controlled better by the friend and what state they are in.
    public void Execute()
    {
        //Debug.Log("**MY DIALOG DEFINITION***"+dialogDefiniton.name);
        if (GlobalVariableManager.Instance.CARRYING_SOMETHING == false)
        {

            if (dialogName.Length > 0 && player != null)
            {

                if (Mathf.Abs(transform.position.x - player.transform.position.x) < xDistanceThreshold && Mathf.Abs(transform.position.y - player.transform.position.y) < yDistanceThreshold)
                {

                    if (autoStart && canTalkTo)
                    {
                        dialogName = friend.nextDialog;
                        ActivateDialog();
                    }
                    else
                    {
                        //Debug.Log("Autostart val:" + autoStart);
                        //Debug.Log("canTalkTo val:" + canTalkTo);

                        if (speechBubbleIcon != null && speechBubbleIcon.activeInHierarchy == false)
                        {
                            SoundManager.instance.PlaySingle(mySpeechIconSFX);
                            speechBubbleIcon.SetActive(true);
                        }
                        if (ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT))
                        {
                            dialogManager.canContinueDialog = true;
                            dialogName = friend.nextDialog;
                            ActivateDialog();
                        }
                    }
                }
                else if (!autoStart && speechBubbleIcon != null && speechBubbleIcon.activeInHierarchy)
                {
                    speechBubbleIcon.SetActive(false);//disable speech bubble icon when far away
                }
            }
        }//end of carry something check
    }
	
	void Update () {
		
	}

	public void SetDialog(DialogDefinition dd){
		dialogDefiniton = dd;
		Debug.Log("**MY DIALOG DEFINITION***"+dialogDefiniton.name);
	}

	public void ActivateDialog(){
		GlobalVariableManager.Instance.PLAYER_CAN_MOVE = false;
		player.GetComponent<EightWayMovement>().enabled = false;
		player.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,0f);
		if(dialogCanvas.activeInHierarchy == false){
			if(cameraPanToFriendAtStart){
					dialogManager.mainCam.GetComponent<Ev_MainCameraEffects>().CameraPan(gameObject.transform.position, "");
			}
			Debug.Log("Dialog Definition Name:"+ dialogDefiniton.name);
			myDialogIcon.GetComponent<DialogIconAnimationManager>().SwitchAni(iconAnimationName);
			dialogManager.animationName = iconAnimationName;
			dialogManager.myDialogDefiniton = dialogDefiniton;
			dialogManager.dialogTitle = dialogName;
			if(friend != null){ // = null for some one timers
				dialogActionManager.friend = friend;
				dialogManager.characterName.text = friend.friendName;

			}
			dialogManager.SetFriend(friend);
			dialogCanvas.SetActive(true);
			myDialogIcon.SetActive(true);
				if(myDialogIcon.transform.childCount>0){//if more than one icon
						List<GameObject> dialogIcons = new List<GameObject>();
						for(int i = 0; i< myDialogIcon.transform.childCount; i++){
							dialogIcons.Add(myDialogIcon.transform.GetChild(i).gameObject);
						}
						dialogManager.dialogIcons = dialogIcons;
				}else{
					//dialogManager.dialogIcons = new List<GameObject>{myDialogIcon};//one icon
					dialogManager.currentlySpeakingIcon = myDialogIcon;
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
	}

}
