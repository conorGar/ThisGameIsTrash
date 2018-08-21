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

	public Camera guiCamera; //needed for icon to corner at dialog choice
	//------------Screen Blur stuff---------------//
	public PostProcessingProfile dialogBlur;
	public GameObject mainCam;
	//------------------------------------------//

	bool finishedDisplayingText = false; // set true when the text animation is done
	bool hasWavingText; 

	void Start () {
		//currentNode = myDialogDefiniton.nodes[myDialogDefiniton.rootNodeId];

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
		foreach(var node in myDialogDefiniton.nodes){
			string nodeTitle = node.Value.title;
			if(nodeTitle == dialogTitle){
				currentNode = node.Value;
				break;
			}
		}
		displayedText.text = currentNode.text;
		mainCam.GetComponent<PostProcessingBehaviour>().profile = dialogBlur;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)){
			if(finishedDisplayingText){
				NextNode();
			}
		}
	}

	void NextNode(){

		if(currentNode.type == DIALOGNODETYPE.QUESTION){
			Debug.Log("Question Dialog Node Properly Read");
			dialogOptions.SetActive(true);
			dialogOptions.GetComponent<DialogChoiceManager>().SetDialogChoices(currentNode);
			finishedDisplayingText = false;
			textBox.SetActive(false);
			currentlySpeakingIcon.transform.position = guiCamera.ViewportToWorldPoint(new Vector3(0f,0f,0f));
		}else{
			
			currentNode = myDialogDefiniton.nodes[currentNode.child_id];
			if(currentNode.text.Contains("<c")){
					Debug.Log("HIGHLIGHT TEXT() ACTIVATE");
					HighLightText();
				}
				if(currentNode.text.Contains("<w")){
					WaveyText();
				}
			finishedDisplayingText = false;
			displayedText.text = currentNode.text;
			displayedText.GetComponent<TextAnimation>().StartAgain();
			InvokeRepeating("TalkSound",0.1f,.05f);
		}
	}

	public void ReturnFromChoice(SerializableGuid link){
			textBox.SetActive(true);
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
	}


	public void FinishedDisplay(){
		Debug.Log("Finished Display activated");
		//if(currentlySpeakingIcon.GetComponent<Animator>().isActiveAndEnabled)
			//currentlySpeakingIcon.GetComponent<Animator>().StopPlayback();

		//continueIcon.enabled = true;
		CancelInvoke();
		finishedDisplayingText = true;
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
		Debug.Log("TalkSound Activate");
		SoundManager.instance.RandomizeSfx(typeSound,.8f,1.2f);
	}
}
