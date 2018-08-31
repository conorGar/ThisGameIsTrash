using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using I2.TextAnimation;
using UnityEngine.UI;
using UnityEngine.PostProcessing;

public class DialogManager : MonoBehaviour {

	public DialogDefinition myDialogDefiniton;//given by friend
	DialogNode currentNode;
	int currentNodeID;
	string currentSpeaker;
	public GameObject currentlySpeakingIcon;
	public Image continueIcon;
	public string dialogTitle;
	public TextMeshProUGUI displayedText;
	public List<GameObject> dialogIcons = new List<GameObject>();
	public GameObject dialogOptions;
	public GameObject textBox;
	public AudioClip typeSound;
	public DialogActionManager dialogActionManager;
	public Camera guiCamera; //needed for icon to corner at dialog choice
	public GameObject dialogCanvas;
	public TextMeshProUGUI characterName;
	[HideInInspector]
	public string animationName;
	//------------Screen Blur stuff---------------//
	public PostProcessingProfile dialogBlur;
	public GameObject mainCam;
	//------------------------------------------//

	public string variableText; //For any variable that is displayed(ex: day of return)

	bool finishedDisplayingText = false; // set true when the text animation is done
	bool hasWavingText; 
	public bool canContinueDialog = true;
	Friend friend;//needed for finish();
	string currentSpeakerName;
	MultipleDialogIconsManager multipleIconsManager;

	void Start () {

		foreach(var node in myDialogDefiniton.nodes){
			string nodeTitle = node.Value.title;
			if(nodeTitle == dialogTitle){
				currentNode = node.Value;
				break;
			}
		}

		//currentNode = myDialogDefiniton.nodes[currentDialogTitle];
		displayedText.text = currentNode.text;
	}

	void OnEnable(){
		//currentNode = myDialogDefiniton.nodes[myDialogDefiniton.rootNodeId];

		SoundManager.instance.musicSource.volume *= .5f;
		foreach(var node in myDialogDefiniton.nodes){
			string nodeTitle = node.Value.title;
			if(nodeTitle == dialogTitle){
				currentNode = node.Value;
				break;
			}
		}
		displayedText.text = currentNode.text;
		currentSpeakerName = currentNode.speakerName;
		mainCam.GetComponent<PostProcessingBehaviour>().profile = dialogBlur;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space) && canContinueDialog){
			if(finishedDisplayingText){
				NextNode();
			}else{
				displayedText.GetComponent<TextAnimation>().StopAnimation("Zoom In Front");
				//FinishedDisplay();
			}
		}
	}

	void NextNode(){
		
		if(currentNode.type == DIALOGNODETYPE.QUESTION){
			Debug.Log("Question Dialog Node Properly Read");
			textBox.SetActive(false);
			dialogOptions.SetActive(true);

			dialogOptions.GetComponent<SpecialEffectsBehavior>().SmoothMovementToPoint(-22f,-31f,.5f,true);
			Debug.Log("CURRENT NODE NAME WHEN SWITCH TO DIALOG CHOICES : " + currentNode.title);
			dialogOptions.GetComponent<DialogChoiceManager>().SetDialogChoices(currentNode);
			finishedDisplayingText = false;
			//currentlySpeakingIcon.transform.position = guiCamera.ViewportToWorldPoint(new Vector3(0f,0f,0f));
			currentlySpeakingIcon.GetComponent<Animator>().enabled = true;
			currentlySpeakingIcon.GetComponent<DialogIconAnimationManager>().SwitchAni("IconSlide");
			canContinueDialog = false;
			CancelInvoke();
		}else if(currentNode.action != null && currentNode.action.Length > 2){ //length check just to see if not blank
			if(currentNode.action == "finish"){
				canContinueDialog = false;
				FinishDialog();
			}else{
				canContinueDialog = false;
				dialogActionManager.Invoke(currentNode.action,.1f);
			}
		}else{
			
			//currentlySpeakingIcon.GetComponent<Animator>().Play("JumboAnimation");
			currentNode = myDialogDefiniton.nodes[currentNode.child_id];

			//check to see if changed speaker
			if(currentSpeakerName != currentNode.speakerName && currentNode.speakerName.Length >2)//>2 check os for if the field is blank, which it is if the speaker is the same as previous
			{
				multipleIconsManager.ChangeSpeaker(currentNode.speakerName);

			}

			if(currentNode.text.Contains("<c")){
					Debug.Log("HIGHLIGHT TEXT() ACTIVATE");
					HighLightText();
			}
			if(currentNode.text.Contains("<w")){
					WaveyText();
			}if(currentNode.text.Contains("<var>")){
				currentNode.text = currentNode.text.Replace("<var>",variableText);
			}

			displayedText.text = currentNode.text;
			displayedText.GetComponent<TextAnimation>().StartAgain();
			finishedDisplayingText = false;

			currentlySpeakingIcon.GetComponent<Animator>().enabled = true;
			InvokeRepeating("TalkSound",0.1f,.05f);
		}
	}

	public void ReturnFromChoice(SerializableGuid link){
			currentlySpeakingIcon.GetComponent<Animator>().enabled = true;
			currentlySpeakingIcon.GetComponent<DialogIconAnimationManager>().SwitchAni("IconSlideBack");
			canContinueDialog = true;
			mainCam.GetComponent<PostProcessingBehaviour>().profile = dialogBlur;
			textBox.SetActive(true);
			currentlySpeakingIcon.SetActive(true);
			dialogOptions.transform.localPosition = new Vector2(-22f,-204f); //reset choice position so it slides on screen next time.
			dialogOptions.SetActive(false);
			currentNode = myDialogDefiniton.nodes[link];
			if(currentNode.text.Contains("<c")){
					Debug.Log("HIGHLIGHT TEXT() ACTIVATE");
					HighLightText();
				}
				if(currentNode.text.Contains("<w")){
					WaveyText();
				}
			finishedDisplayingText = false;
			displayedText.text = currentNode.text;
			InvokeRepeating("TalkSound",0.1f,.05f);
	}
	public void ReturnFromAction(){
			canContinueDialog = true;
			currentNode = myDialogDefiniton.nodes[currentNode.child_id];
			currentlySpeakingIcon.SetActive(true);
			textBox.SetActive(true);
			if(currentNode.text.Contains("<c")){
					Debug.Log("HIGHLIGHT TEXT() ACTIVATE");
					HighLightText();
			}
			if(currentNode.text.Contains("<w")){
					WaveyText();
			}if(currentNode.text.Contains("<var>")){
				currentNode.text = currentNode.text.Replace("<var>",variableText);
			}
			finishedDisplayingText = false;
			displayedText.text = currentNode.text;
			displayedText.GetComponent<TextAnimation>().StartAgain();
			InvokeRepeating("TalkSound",0.1f,.05f);
	}


	public void FinishedDisplay(){
		if(finishedDisplayingText == false){
			Debug.Log("Finished Display activated");
			if(currentlySpeakingIcon.GetComponent<Animator>().isActiveAndEnabled)
				currentlySpeakingIcon.GetComponent<Animator>().enabled = false;

			continueIcon.enabled = true;
			CancelInvoke();
			finishedDisplayingText = true;
		}
	}

	public void HighLightText(){ 
		
		if(currentNode.text.Contains("c=g")){//green color highlight
			Debug.Log("contains green color");
			currentNode.text = currentNode.text.Replace("<c=g>","<color=#78FF32>");
		}
		if(currentNode.text.Contains("c=r")){//red color highlight
			currentNode.text = currentNode.text.Replace("<c=r>","<color=#DC3232>");
			Debug.Log("contains red color");
		}
		if(currentNode.text.Contains("c=b")){//red color highlight
			currentNode.text = currentNode.text.Replace("<c=b>","<color=#29B6F6>");
			Debug.Log("contains blue color");
		}
		currentNode.text = currentNode.text.Replace("</c>","</color>");

	}
	public void WaveyText(){
		int rangeStart = currentNode.text.IndexOf("<w>");
		Debug.Log("Wavey text start:" + rangeStart);
		int rangeEnd = currentNode.text.IndexOf("</w>");
		Debug.Log(currentNode.text.Substring(rangeStart));
		currentNode.text = currentNode.text.Replace("<w>","");
		currentNode.text = currentNode.text.Replace("</w>","");

		displayedText.GetComponent<TextAnimation>()._AnimationSlots[2]._Animation._Sequences[0]._Enabled = true;
		displayedText.GetComponent<TextAnimation>()._AnimationSlots[2]._Animation._Sequences[1]._Enabled = true;
		displayedText.GetComponent<TextAnimation>()._AnimationSlots[2]._Animation._Sequences[0]._TargetRangeStart = 43;
		displayedText.GetComponent<TextAnimation>()._AnimationSlots[2]._Animation._Sequences[0]._TargetRangeStart = rangeEnd;
		displayedText.GetComponent<TextAnimation>()._AnimationSlots[2]._Animation._Sequences[1]._TargetRangeStart = rangeStart;
		displayedText.GetComponent<TextAnimation>()._AnimationSlots[2]._Animation._Sequences[1]._TargetRangeStart = rangeEnd;
		hasWavingText = true;
		//Debug.Log("Target Range start:" + currentDisplayedText.GetComponent<TextAnimation>()._AnimationSlots[2]._Animation._Sequences[0]._TargetRangeStart);

	}

	public void TalkSound(){
		//Debug.Log("TalkSound Activate");
		SoundManager.instance.RandomizeSfx(typeSound,.8f,1.2f);
	}

	public void FinishDialog(){//public because of rat with a hat
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		//TODO: maybe need some way to set activateDialog's 'canTalk' to true again depending on character
		mainCam.GetComponent<PostProcessingBehaviour>().profile = null; //TODO: returns to NO effect, not sure if you want this, future Conor
		mainCam.GetComponent<Ev_MainCameraEffects>().ReturnFromCamEffect();
		Debug.Log(friend.nextDialog);
		//Debug.Log(currentNode.child_id.ToString());
		if(friend != null)//would = null for some one timers
			friend.nextDialog = myDialogDefiniton.nodes[currentNode.child_id].title;
		 //sets up dialog for next interaction
		//GlobalVariableManager.Instance.PLAYER_CAN_MOVE = false;
		SoundManager.instance.musicSource.volume *= 2; //turn music back to normal.
		player.GetComponent<EightWayMovement>().enabled = true;
		GlobalVariableManager.Instance.PLAYER_CAN_MOVE = true;
		dialogCanvas.SetActive(false);
	}

	public void ChangeIcon(string aniName){
		currentlySpeakingIcon.GetComponent<DialogIconAnimationManager>().SwitchAni(aniName);
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
	}

}
