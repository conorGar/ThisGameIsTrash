using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using TMPro;
using I2.TextAnimation;
using UnityEngine.UI;
using UnityEngine.PostProcessing;

public class DialogManager : MonoBehaviour {
    public static DialogManager Instance;
	public DialogDefinition myDialogDefiniton;//given by friend
	DialogNode currentNode;
	int currentNodeID;
	string currentSpeaker;
	
	public Image continueIcon;
	public string dialogTitle;
	public TextMeshProUGUI displayedText;
	public List<DialogIconAnimationManager> dialogIcons = new List<DialogIconAnimationManager>();
	public GameObject dialogOptions;
	public GameObject textBox;
	//public AudioClip typeSound;
	public AudioClip continueDialogSfx;
	public DialogActionManager dialogActionManager;
	public Camera guiCamera; //needed for icon to corner at dialog choice
	public GameObject dialogCanvas;
	public GUI_CinematicBars blackBarCanvas;
	public TextMeshProUGUI characterName;
	[HideInInspector]
	public string animationName;
    public DialogIconAnimationManager currentlySpeakingIcon;
    private GameObject currentlySpeakingModel; //NPC is using model, not icon
	//------------Screen Blur stuff---------------//
	public PostProcessingProfile dialogBlur;
	//------------------------------------------//

	public string variableText; //For any variable that is displayed(ex: day of return)


	public AudioClip choiceBoxAppear;

	bool finishedDisplayingText = false; // set true when the text animation is done
	bool hasWavingText; 
	public bool canContinueDialog = true;
	Friend friend;//needed for finish();
                  //string currentSpeakerName;
                  // set by MultipleIconsManager.cs
   	bool guiCamShake;

    private void Awake()
    {
        Instance = this;
    }

    void Start () {
        dialogCanvas.SetActive(false);
        dialogTitle = "";
    }

	void OnEnable(){
        //currentNode = myDialogDefiniton.nodes[currentDialogTitle];
        if (currentNode != null) {
            displayedText.text = currentNode.text;
            displayedText.GetComponent<TextAnimation>().SetWidgetColor(displayedText.color);
        }


        SoundManager.instance.musicSource.volume *= .5f;
		foreach(var node in myDialogDefiniton.nodes){
			string nodeTitle = node.Value.title;
			if(nodeTitle == dialogTitle){
				currentNode = node.Value;
				Debug.Log("Found proper title - next node");

				break;
			}
		}

        // deactivate all the icons and start fresh.
        for (int i = 0; i < dialogIcons.Count; i++) {
            dialogIcons[i].gameObject.SetActive(false);
        }

        if (currentNode != null) {
            characterName.text = currentNode.speakerName;
            CamManager.Instance.mainCamPostProcessor.profile = dialogBlur;

            // Set up the initial dialog icon
            if (currentlySpeakingIcon != null) {
                currentlySpeakingIcon.gameObject.SetActive(true);

                if (currentlySpeakingIcon.GetType() == typeof(MultipleDialogIconsManager) && !string.IsNullOrEmpty(currentNode.speakerName)) {
                    ((MultipleDialogIconsManager)currentlySpeakingIcon).ChangeSpeaker(currentNode.speakerName);
                }
            }
            GameStateManager.Instance.PushState(typeof(DialogState));
//           	blackBarCanvas.Show(100,.3f);
            StartDisplay();
        }
	}
	
	// Update is called once per frame
	void Update () {
		if(ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT) && canContinueDialog)
        {
			if(finishedDisplayingText){
				NextNode();
			}else{
				displayedText.GetComponent<TextAnimation>().StopAnimation("Zoom In Front");
				//FinishedDisplay();
			}
		}
		if(guiCamShake){
			dialogCanvas.transform.localPosition = new Vector3(14+Random.Range(0.2f,1f), 11 + Random.Range(0.2f,1f),10f); //14 & 11 = starting local position of dialog canvas
		}
	}

	void NextNode(){
		if(guiCamShake){
			guiCamShake = false;
		}
		continueIcon.gameObject.SetActive(false);
		SoundManager.instance.PlaySingle(continueDialogSfx);
		if(currentNode.type == DIALOGNODETYPE.QUESTION){
			Debug.Log("Question Dialog Node Properly Read");
			textBox.SetActive(false);
			dialogOptions.SetActive(true);

			SoundManager.instance.PlaySingle(choiceBoxAppear);

			Debug.Log("CURRENT NODE NAME WHEN SWITCH TO DIALOG CHOICES : " + currentNode.title);
			dialogOptions.GetComponent<DialogChoiceManager>().SetDialogChoices(currentNode);
			finishedDisplayingText = false;
            //currentlySpeakingIcon.transform.position = guiCamera.ViewportToWorldPoint(new Vector3(0f,0f,0f));
            currentlySpeakingIcon.Slide();

            canContinueDialog = false;
			CancelInvoke();
		}else if(!currentNode.action.IsNullOrWhiteSpace()){ //length check just to see if not blank
            // TODO: Maybe get rid of this?  We can infer the dialog as ended if it doesn't have a child node.
            if (currentNode.action == "finish"){
				canContinueDialog = false;

                if (!currentNode.friendState.IsNullOrWhiteSpace()) {
                    Debug.Log("Setting Friend State to: " + currentNode.friendState);
                    friend.SetFriendState(currentNode.friendState);
                }

                FinishDialog();
			}else if(currentNode.action == "IconLeave"){
				currentlySpeakingIcon.gameObject.SetActive(false);
				ReturnFromAction();
			}else{
				if(canContinueDialog){
                    // if this isn't a dialog action, go to the game scene to show the more detailed game scene action.
                    if (!currentNode.isDialogAction) {
                        canContinueDialog = false;
                        textBox.SetActive(false);
                        for (int i = 0; i < dialogIcons.Count; i++) {
                            dialogIcons[i].gameObject.SetActive(false);
                        }

                        if (friend.dialogManager == null)
                            friend.dialogManager = this;
                        friend.Invoke(currentNode.action, .1f);
                    }
                    else {
                        if (friend.dialogManager == null)
                            friend.dialogManager = this;
                        friend.Invoke(currentNode.action, .1f);
                    }
				}
			}
		}else{
			if(displayedText.fontSize != 16.5f){//return to normal font size after small text
				displayedText.fontSize = 16.5f;
			}

            // No more nodes.  End Dialog.
            if (currentNode.child_id.IsNullOrEmpty()) {
                canContinueDialog = false;

                if (!currentNode.friendState.IsNullOrWhiteSpace()) {
                    Debug.Log("Setting Friend State to: " + currentNode.friendState);
                    friend.SetFriendState(currentNode.friendState);
                }

                FinishDialog();
                return;
            }
            //currentlySpeakingIcon.GetComponent<Animator>().Play("JumboAnimation");
            currentNode = myDialogDefiniton.nodes[currentNode.child_id];

            if (characterName.text != currentNode.speakerName && currentNode.speakerName.Length > 1) { // if the speaker is a new name, update it.
                characterName.text = currentNode.speakerName;

                if (currentlySpeakingIcon.GetType() == typeof(MultipleDialogIconsManager)) // change the speaker icon for multi dialogs.
                    ((MultipleDialogIconsManager)currentlySpeakingIcon).ChangeSpeaker(currentNode.speakerName);
            }

            StartDisplay();
		}
	}

	public void ReturnFromChoice(SerializableGuid link){
		Debug.Log("Return From Choice activated");
        currentlySpeakingIcon.SlideBack();

        canContinueDialog = true;
        CamManager.Instance.mainCamPostProcessor.profile = dialogBlur;
		

		textBox.SetActive(true);
		dialogOptions.transform.localPosition = new Vector2(-22f,-204f); //reset choice position so it slides on screen next time.
		dialogOptions.SetActive(false);

		currentNode = myDialogDefiniton.nodes[link];
		if(currentlySpeakingIcon.GetType() == typeof(MultipleDialogIconsManager) &&
            characterName.text != currentNode.speakerName && currentNode.speakerName.Length >1)//>2 check os for if the field is blank, which it is if the speaker is the same as previous
		{
			Debug.Log("NAMES DONT MATCHx-x-x-x--x-x-x-x-x-x-");
			((MultipleDialogIconsManager)currentlySpeakingIcon).ChangeSpeaker(currentNode.speakerName);
			characterName.text = currentNode.speakerName;
		}

        StartDisplay();
	}

    public void ReturnFromAction(){
        ReturnFromAction(true);
    }

    public void ReturnFromActionOnSameNode(){
        ReturnFromAction(false);
    }

	public void ReturnFromAction(bool useNextNode){
		canContinueDialog = true;

        if (useNextNode)
			currentNode = myDialogDefiniton.nodes[currentNode.child_id];
            
        if(currentlySpeakingIcon != null)
			currentlySpeakingIcon.gameObject.SetActive(true);

		textBox.SetActive(true);
        if (currentlySpeakingIcon.GetType() == typeof(MultipleDialogIconsManager) &&
            characterName.text != currentNode.speakerName && currentNode.speakerName.Length > 1)//>2 check os for if the field is blank, which it is if the speaker is the same as previous
        {
            Debug.Log("NAMES DONT MATCHx-x-x-x--x-x-x-x-x-x-");
            ((MultipleDialogIconsManager)currentlySpeakingIcon).ChangeSpeaker(currentNode.speakerName);
            characterName.text = currentNode.speakerName;
        }

        StartDisplay();
	}

    // Starts the text scrolling
    private void StartDisplay()
    {
        displayedText.text = currentNode.text;
        if (displayedText.text.Contains("<c")) {
            HighLightText();
        }
        if (displayedText.text.Contains("<w")) {
            WaveyText();
        }

        // Go through all the var elements and clip out the varText so we can replace them.
        var matchTag = new Regex(@"<var='(.+?)'>");
        foreach (Match match in matchTag.Matches(displayedText.text)) {
            var varText = match.Groups[1].Value;
            displayedText.text = matchTag.Replace(displayedText.text, friend.GetVariableText(varText), 1);
        }

        if (displayedText.text.Contains("<shake>")) {
			displayedText.text = displayedText.text.Substring(0 ,displayedText.text.IndexOf("<shake>")) + displayedText.text.Substring(displayedText.text.IndexOf("<shake>"),displayedText.text.Length-7) ; // skips "shake"
            guiCamShake = true;
        }
        else if (displayedText.text.Contains("<s")) {
            SmallText();
        }
        if (displayedText.text.Contains("<l")) {
            LargeText();
        }
#if DEBUG_DIALOG
        displayedText.text += " (" + currentNode.title + ")";
#endif
        displayedText.GetComponent<TextAnimation>().StartAgain();
        finishedDisplayingText = false;

        if (!currentNode.isThought) {
            currentlySpeakingIcon.SetTalking(true);
        }

        if (!string.IsNullOrEmpty(currentNode.animTrigger)) {
        	if(currentlySpeakingIcon){
            	Debug.Log("Firing Animation Trigger: " + currentNode.animTrigger);
            	currentlySpeakingIcon.SetAnimTrigger(currentNode.animTrigger);
            }else if(currentlySpeakingModel){
				Debug.Log("Firing Animation Trigger For Model: " + currentNode.animTrigger);
				currentlySpeakingModel.GetComponent<tk2dSpriteAnimator>().Play(currentNode.animTrigger);
            }
        }

        PlayTalkSounds();
    }

    // End of the teext scrolling
	public void FinishedDisplay(){
		if(finishedDisplayingText == false){
			if(currentlySpeakingIcon != null)
				currentlySpeakingIcon.SetTalking(false);

			continueIcon.gameObject.SetActive(true);
			CancelInvoke();
			finishedDisplayingText = true;
		}
	}

	public void HighLightText(){ 
		
		if(displayedText.text.Contains("c=g")){//green color highlight
			Debug.Log("contains green color");
            displayedText.text = displayedText.text.Replace("<c=g>","<color=#78FF32>");
		}
		if(displayedText.text.Contains("c=r")){//red color highlight
            displayedText.text = displayedText.text.Replace("<c=r>","<color=#DC3232>");
			Debug.Log("contains red color");
		}
		if(displayedText.text.Contains("c=b")){//red color highlight
            displayedText.text = displayedText.text.Replace("<c=b>","<color=#29B6F6>");
			Debug.Log("contains blue color");
		}
        displayedText.text = displayedText.text.Replace("</c>","</color>");

	}
	public void WaveyText(){
		int rangeStart = displayedText.text.IndexOf("<w>");
		Debug.Log("Wavey text start:" + rangeStart);
		int rangeEnd = displayedText.text.IndexOf("</w>");
		Debug.Log(displayedText.text.Substring(rangeStart));
        displayedText.text = displayedText.text.Replace("<w>","");
        displayedText.text = displayedText.text.Replace("</w>","");

		displayedText.GetComponent<TextAnimation>()._AnimationSlots[2]._Animation._Sequences[0]._Enabled = true;
		displayedText.GetComponent<TextAnimation>()._AnimationSlots[2]._Animation._Sequences[1]._Enabled = true;
		displayedText.GetComponent<TextAnimation>()._AnimationSlots[2]._Animation._Sequences[0]._TargetRangeStart = 43;
		displayedText.GetComponent<TextAnimation>()._AnimationSlots[2]._Animation._Sequences[0]._TargetRangeStart = rangeEnd; // wait does all this work? Looked like it didnt when reviewing for intro setup
		displayedText.GetComponent<TextAnimation>()._AnimationSlots[2]._Animation._Sequences[1]._TargetRangeStart = rangeStart;
		displayedText.GetComponent<TextAnimation>()._AnimationSlots[2]._Animation._Sequences[1]._TargetRangeAmount= rangeEnd - rangeStart;
		hasWavingText = true;
		//Debug.Log("Target Range start:" + currentDisplayedText.GetComponent<TextAnimation>()._AnimationSlots[2]._Animation._Sequences[0]._TargetRangeStart);

	}

	public void SmallText(){
		//for now just makes whole speech one font size, not sure if making only certain text small is possible with TMpro
		displayedText.fontSize = 8;
		displayedText.text = displayedText.text.Substring(3,displayedText.text.Length-3); // skips the '<s>' note that <s> has to come at start
       // displayedText.text = displayedText.text.Replace("<s>","");
	}
	public void LargeText(){
		displayedText.fontSize = 35;
		displayedText.text = displayedText.text.Substring(3,displayedText.text.Length-3);
        //displayedText.text = displayedText.text.Replace("<l>","");
	}

    private void PlayTalkSounds()
    {
        CancelInvoke();
        InvokeRepeating("TalkSound", 0.1f, .05f);
    }

    private void TalkSound(){
		SoundManager.instance.RandomizeSfx(SFXBANK.VOICE_TICK,.8f,1.2f);
	}

	public void FinishDialog(){//public because of rat with a hat
		GameObject player = GameObject.FindGameObjectWithTag("Player");
        //TODO: maybe need some way to set activateDialog's 'canTalk' to true again depending on character
        CamManager.Instance.mainCamPostProcessor.profile = null; //TODO: returns to NO effect, not sure if you want this, future Conor

		Debug.Log(friend.nextDialog);
		//Debug.Log(currentNode.child_id.ToString());
		if(friend != null && !currentNode.child_id.IsNullOrEmpty()) //would = null for some one timers
			friend.nextDialog = myDialogDefiniton.nodes[currentNode.child_id].title;
		 //sets up dialog for next interaction
		//GlobalVariableManager.Instance.PLAYER_CAN_MOVE = false;
		SoundManager.instance.musicSource.volume *= 2; //turn music back to normal.
		//player.GetComponent<EightWayMovement>().enabled = true;
        //GameStateManager.Instance.PopState();

        // Handle when movie type stuff plays at the end of a dialog.
        //sGameStateManager.Instance.PushState(typeof(MovieState));
        friend.OnFinishDialog();
   //     blackBarCanvas.Hide(.3f);
		dialogCanvas.SetActive(false);
    }

    public void SetDialogIconByID(string p_dialogIconID)
    {
        for (int i = 0; i < dialogIcons.Count; i++) {
            if (dialogIcons[i].ID == p_dialogIconID) {
                currentlySpeakingIcon = dialogIcons[i];
                return;
            }
        }

        Debug.LogError("Could not find Dialog Icon ID: " + p_dialogIconID + "!  Make sure it's defined on the friend icon!");
        currentlySpeakingIcon = null;
    }

    public void SetDialogIconAsModel(GameObject model){
    	currentlySpeakingModel = model;
    }

	public void SetFriend(Friend thisFriend){//set by activateDialogWhenClose
		friend = thisFriend;
	}

	public void JumpToNewNode(string nodeName){//for use in situations where needs to break from current dialog into a new one, such as a check to see if go the friend's final dialog(if you complete their quest)
		foreach(var node in myDialogDefiniton.nodes){
			string nodeTitle = node.Value.title;
			if(nodeTitle == nodeName){
				currentNode = node.Value;
				break;
			}
		}
		ReturnFromAction(false);
	}
}
