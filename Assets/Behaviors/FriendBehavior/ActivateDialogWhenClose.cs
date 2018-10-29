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
//	public bool tempBoolForActionManager;

	GameObject player;
	int spawnSpeechBubble = 0;


	void Awake(){
		player = GameObject.FindGameObjectWithTag("Player");


	}
	void OnEnable(){
		player = GameObject.FindGameObjectWithTag("Player");
        startNodeName = friend.nextDialog;


	}

    // Called by other friends to test if they are close and control the dialog as it's displayed and interacted with.
    // Moved this out of Update so it can be controlled better by the friend and what state they are in.
    public void Execute()
    {
        if (GlobalVariableManager.Instance.CARRYING_SOMETHING == false)
        {

            if (startNodeName.Length > 0 && player != null)
            {

                //Debug.Log("Criteria met - 1" + Mathf.Abs(transform.position.x - player.transform.position.x) +"   " + Mathf.Abs(transform.position.y - player.transform.position.y));
                if (Mathf.Abs(transform.position.x - player.transform.position.x) < xDistanceThreshold && Mathf.Abs(transform.position.y - player.transform.position.y) < yDistanceThreshold)
                {

                    //Debug.Log("Criteria met - 2");
                    if (autoStart && canTalkTo)
                    {

                        //Debug.Log("Criteria met - 3");
                        startNodeName = friend.nextDialog;
                        ActivateDialog();
                    }
                    else if (canTalkTo)
                    {
                        //Debug.Log("Autostart val:" + autoStart);
                        //Debug.Log("canTalkTo val:" + canTalkTo);

                        if (speechBubbleIcon != null && speechBubbleIcon.activeInHierarchy == false)
                        {
                            SoundManager.instance.PlaySingle(mySpeechIconSFX);
                            speechBubbleIcon.SetActive(true);
                            spawnSpeechBubble = 1;
                        }
                        else
                        {
                            if (spawnSpeechBubble == 0)
                            {
                                speechBubbleIcon = ObjectPool.Instance.GetPooledObject("speechIcon", gameObject.transform.position);
                                spawnSpeechBubble = 1;
                            }
                        }
                        if (ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT))
                        {
                        	if(dialogDefiniton == null){
                        		dialogDefiniton = friend.myDialogDefiniton;
                        	}

                            DialogManager.Instance.canContinueDialog = true;
                            startNodeName = friend.nextDialog;
                            ActivateDialog();
                        }
                    }
                }
                else
                {
                    //Debug.Log("Far away from: " + gameObject.name);
                    if (spawnSpeechBubble == 1)
                    {
                        //Debug.Log("Speech Bubble should've died");
                        ObjectPool.Instance.ReturnPooledObject(speechBubbleIcon);
                        spawnSpeechBubble = 0;
                        //speechBubbleIcon.SetActive(false);//disable speech bubble icon when far away
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
        GetComponent<ActivateDialogWhenClose>().canTalkTo = false;
    }

    void Update () {
		
	}

	public void SetDialog(DialogDefinition dd){
		dialogDefiniton = dd;
		Debug.Log("**MY DIALOG DEFINITION***"+dialogDefiniton.name);
	}

	public void ActivateDialog(){
		DialogManager.Instance.gameObject.SetActive(true);

		GlobalVariableManager.Instance.PLAYER_CAN_MOVE = false;
		player.GetComponent<EightWayMovement>().StopMovement();
		player.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,0f);
		if(DialogManager.Instance.dialogCanvas.activeInHierarchy == false){
			if(cameraPanToFriendAtStart){
                CamManager.Instance.mainCamEffects.CameraPan(gameObject.transform.position, "");
			}
			Debug.Log("Dialog Definition Name:"+ dialogDefiniton.name);
			DialogManager.Instance.animationName = iconAnimationName;
            DialogManager.Instance.myDialogDefiniton = dialogDefiniton;
            DialogManager.Instance.dialogTitle = startNodeName;
            DialogManager.Instance.canContinueDialog = true; //need this for starting after any previous dialog
			if(friend != null){ // = null for some one timers
                DialogManager.Instance.dialogActionManager.friend = friend;
                DialogManager.Instance.characterName.text = friend.friendName;
			}

            DialogManager.Instance.SetDialogIconByID(dialogDefiniton.dialogIconID);
            DialogManager.Instance.SetFriend(friend);
			
            DialogManager.Instance.dialogCanvas.SetActive(true);
			Debug.Log("Got here with activate dialog execute" + DialogManager.Instance.dialogCanvas.activeInHierarchy);
			DialogManager.Instance.currentlySpeakingIcon.gameObject.SetActive(true);
            DialogManager.Instance.currentlySpeakingIcon.SwitchAni(iconAnimationName);
         

			canTalkTo = false;
		}
	}
}
