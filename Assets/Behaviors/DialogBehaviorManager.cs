using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using I2.TextAnimation;


public class DialogBehaviorManager : MonoBehaviour {

	public TextAsset csvFile;
	List<string> dialogLines = new List<string>();

	public TextMeshProUGUI currentDisplayedText;

	public tk2dSprite speakerName;
	public Image continueIcon;
	public string currentWorldNumber;
	public GameObject textBox; //only needed so it can be disabled when doing dialog option spawning
	public List<GameObject> dialogIcons = new List<GameObject>();


	//------stuff for dialog options---------//
	public GameObject dialogOptions;
	bool spawnDialogChoiceWhenContinue = false;
	List<int> lineNumbersThatActivateOptions = new List<int>();
	List<int> optionIDCodes = new List<int>();

	//--------------------------------------//

	bool finishedDisplayingText = false; // set true when the animation is done
	bool hasWavingText;
	bool justReturnedFromChoice = false;
	int currentLineNum = 1;
	string textToSwitchto;
	List<string> speakerList = new List<string>();
	string currentSpeaker;
	GameObject currentlySpeakingIcon;

	List<int> idsOfJumpingLines = new List<int>();//the id numbers of dialog lines that have a number to go to after that isnt just the next line(aka, have a colomn[6])
	List<int> dialogIdsToJumpTo = new List<int>();// ^ the dialog id that the respective line in 'idsOfJumpingLines' will go to...


	string mydialogName;//set by friend

	void Start () {
		//-------Convert csv file into list of strings-----------//
		string csvText = csvFile.text;
		//Debug.Log(csvText.IndexOf("WorldNumber" + currentWorldNumber));
		//Debug.Log(csvText.IndexOf("WorldNumber", csvText.IndexOf("WorldNumber" + currentWorldNumber) + 1));
		csvText = csvText.Substring(csvText.IndexOf("WorldNumber" + currentWorldNumber),Mathf.Abs(csvText.IndexOf("WorldNumber" + currentWorldNumber) - csvText.IndexOf("WorldNumber", csvText.IndexOf("WorldNumber" + currentWorldNumber) + 1)));//groups by world number
		//Debug.Log(csvText);

		//***Part after here will likely have to be moved to it's own method that can be activated when needed by various friends(with different "myDialogName" values)
	
		int myIndex = csvText.IndexOf(mydialogName); 

		csvText = csvText.Substring(myIndex, (csvText.IndexOf("/nd", (csvText.IndexOf(mydialogName) + 1))) - myIndex); //only get the strings for this specific dialog occuring
		string[] lines = csvText.Split('\n');

		int counter = 0;//used to determine which lines have dialog options
		foreach(var line in lines){
			//split line into colomns
			var colomns = line.Split(',');
			//Debug.Log("colomns: " + colomns.Length);

			if(colomns.Length > 2){
				speakerList.Add(colomns[2]);
				//Debug.Log(colomns[2]);
			}
			/*if(currentSpeaker == null){
				currentSpeaker = colomns[1];//set first speaker
			}*/
			
			//Debug.Log(colomns[1]);
			if(colomns.Length > 3)//need check because sometimes last value wont have all colons read
				dialogLines.Add(colomns[3].Replace(';',','));//semi-colons have to be used instead of commas to begin with because of .CSV file layout
			if(colomns.Length > 4){
				//Debug.Log("COLOMN 4:" + colomns[4]);
			 if(colomns[4].Trim() == "YES:"){
				Debug.Log("********Added dialogOption to list*****");
				lineNumbersThatActivateOptions.Add(counter);//adds the current line to the options spawn line list
				optionIDCodes.Add(int.Parse(colomns[5])); //adds the ID to give to the DialogChoiceManager when needed.
				}
			}
			if(colomns.Length>6 && colomns[6].Trim().Length > 0){//parts of dialog that don't jump to next id but jump to a set number
				//Debug.Log(colomns[6].Trim().Length);
				//Debug.Log(colomns[1]);
				idsOfJumpingLines.Add(int.Parse(colomns[1]));//id of line that's GOING TO jump.
				dialogIdsToJumpTo.Add(int.Parse(colomns[6]));//^ id that this is going to jump to.
			}
			counter++;
		}
		for(int i = 0; i<idsOfJumpingLines.Count;i++){
			Debug.Log("ids of jumping lines: " + idsOfJumpingLines[i]);
			Debug.Log("dialog IDs to jump to: " + dialogIdsToJumpTo[i]);
		}
		Debug.Log(dialogLines[1]);
		//-----------------------------------------------------//
		ChangeSpeaker();
		if(dialogLines[1].Contains("<c")){
			HighLightText();
		}
		if(dialogLines[1].Contains("<w")){
			WaveyText();
		}
		currentDisplayedText.text = dialogLines[1];

	}
	
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)){
			if(finishedDisplayingText){
				NextText();
			}
		}
	}

	public void FinishedDisplay(){
		Debug.Log("Finished Display activated");
		if(currentlySpeakingIcon.GetComponent<Animator>().isActiveAndEnabled)
			currentlySpeakingIcon.GetComponent<Animator>().StopPlayback();

		continueIcon.enabled = true;
		finishedDisplayingText = true;
	}

	void NextText(){
		if(lineNumbersThatActivateOptions.Count > 0 && currentLineNum != lineNumbersThatActivateOptions[0] ||lineNumbersThatActivateOptions.Count == 0 ){ //*** For now, options only checks for next available dialogOption sequence in list, might not work when skipping around in more complex dialog systems....
			Debug.Log("NextText() happened properly");
			finishedDisplayingText = false;
			continueIcon.enabled = false;

			if(hasWavingText){
				currentDisplayedText.GetComponent<TextAnimation>()._AnimationSlots[2]._Animation._Sequences[0]._Enabled = false;
				currentDisplayedText.GetComponent<TextAnimation>()._AnimationSlots[2]._Animation._Sequences[1]._Enabled = false;
				hasWavingText = false;
			}
			if(idsOfJumpingLines.Contains(currentLineNum) && !justReturnedFromChoice){
				currentLineNum = dialogIdsToJumpTo[idsOfJumpingLines.IndexOf(currentLineNum)];
			}else{
				currentLineNum++;//go to next line in list
			}

			if(speakerList[currentLineNum] != currentSpeaker){
				ChangeSpeaker();
			}

			if(dialogLines[currentLineNum].Contains("<c")){
				Debug.Log("HIGHLIGHT TEXT() ACTIVATE");
				HighLightText();
			}
			if(dialogLines[currentLineNum].Contains("<w")){
				WaveyText();
			}

			if(justReturnedFromChoice){
				justReturnedFromChoice = false;
			}

			textToSwitchto = dialogLines[currentLineNum];
			currentDisplayedText.text = textToSwitchto;
			currentlySpeakingIcon.GetComponent<Animator>().StartPlayback();
			currentDisplayedText.GetComponent<TextAnimation>().PlayAnim(0);
		}else if(lineNumbersThatActivateOptions.Count > 0){ //if spawn dialog
			for(int i = 0; i < dialogIcons.Count;i++){
				dialogIcons[i].GetComponent<SpecialEffectsBehavior>().SmoothMovementToPoint(-11f,7f,.3f);//move to corner of the screen
			}
			dialogOptions.SetActive(true);
			//dialogOptions.GetComponent<DialogChoiceManager>().SetDialogChoices(mydialogName,1);//mydialogName,optionIDCodes[0]);
			lineNumbersThatActivateOptions.RemoveAt(0);// ******just removes 1st index, again, wont work if complex dialogs jump around, probably needs to be changed
			optionIDCodes.RemoveAt(0);				  // ******just removes 1st index, again, wont work if complex dialogs jump around, probably needs to be changed
			currentDisplayedText.enabled = false;
			textBox.SetActive(false);

		}
	}


	public void HighLightText(){ 
		
		if(dialogLines[currentLineNum].Contains("c=g")){//green color highlight
			Debug.Log("contains green color");
			dialogLines[currentLineNum] = dialogLines[currentLineNum].Replace("<c=g>","<color=#78FF32>");
		}
		if(dialogLines[currentLineNum].Contains("c=r")){//red color highlight
			dialogLines[currentLineNum] = dialogLines[currentLineNum].Replace("<c=r>","<color=#DC3232>");
			Debug.Log("contains red color");
		}
		if(dialogLines[currentLineNum].Contains("c=b")){//red color highlight
			dialogLines[currentLineNum] = dialogLines[currentLineNum].Replace("<c=b>","<color=#29B6F6>");
			Debug.Log("contains blue color");
		}
		dialogLines[currentLineNum] = dialogLines[currentLineNum].Replace("</c>","</color>");

	}

	public void IncreaseTextSize(string textToHL, int size){
		//not finished....
		string sizeCode = "<size=" + size.ToString() + ">";
		textToSwitchto =  textToSwitchto.Insert(textToSwitchto.IndexOf(textToHL),sizeCode);
		textToSwitchto =  textToSwitchto.Insert(textToSwitchto.IndexOf("]"),"</size>");
		textToSwitchto =  textToSwitchto.Replace('[',' ');
		textToSwitchto =  textToSwitchto.Replace(']',' ');

	}

	public void WaveyText(){
		int rangeStart = dialogLines[currentLineNum].IndexOf("<w>");
		Debug.Log("Wavey text start:" + rangeStart);
		int rangeEnd = dialogLines[currentLineNum].IndexOf("</w>");
		Debug.Log(dialogLines[currentLineNum].Substring(rangeStart));
		dialogLines[currentLineNum] = dialogLines[currentLineNum].Replace("<w>","");
		dialogLines[currentLineNum] = dialogLines[currentLineNum].Replace("</w>","");

		currentDisplayedText.GetComponent<TextAnimation>()._AnimationSlots[2]._Animation._Sequences[0]._Enabled = true;
		currentDisplayedText.GetComponent<TextAnimation>()._AnimationSlots[2]._Animation._Sequences[1]._Enabled = true;
		currentDisplayedText.GetComponent<TextAnimation>()._AnimationSlots[2]._Animation._Sequences[0]._TargetRangeStart = 43;
		currentDisplayedText.GetComponent<TextAnimation>()._AnimationSlots[2]._Animation._Sequences[0]._TargetRangeStart = rangeEnd;
		currentDisplayedText.GetComponent<TextAnimation>()._AnimationSlots[2]._Animation._Sequences[1]._TargetRangeStart = rangeStart;
		currentDisplayedText.GetComponent<TextAnimation>()._AnimationSlots[2]._Animation._Sequences[1]._TargetRangeStart = rangeEnd;
		hasWavingText = true;
		Debug.Log("Target Range start:" + currentDisplayedText.GetComponent<TextAnimation>()._AnimationSlots[2]._Animation._Sequences[0]._TargetRangeStart);

	}

	public void SetIcon(List<GameObject> icons){

		dialogIcons = icons; //gets from friends :ActivateWhenClose.cs
	}



	public void SetDialogName(string name){
		mydialogName = name;
	}

	public void ReturnFromDialogOption(int linkActivated){
		Debug.Log("Recieved Link:" + linkActivated);
		Debug.Log(dialogLines[linkActivated]);
		currentLineNum = linkActivated -1; //- 1 because NextText() inreases current line num by 1 when activated
		dialogOptions.SetActive(false);
		currentDisplayedText.enabled = true;
		textBox.SetActive(true);
		for(int i = 0; i < dialogIcons.Count;i++){
			dialogIcons[i].GetComponent<SpecialEffectsBehavior>().SmoothMovementToPoint(-1f,14f,.3f);//move to corner of the screen
		}
		justReturnedFromChoice = true; // this is needed so that lines that 'link' to other lines don't go straight to their linked line when return. (ex: if the returning dialog line had a link to id 8, without this bool it would go staright to the line at ID 8)
		NextText();
	}

	void ChangeSpeaker(){
		
		currentSpeaker = speakerList[currentLineNum].ToLower();
		speakerName.SetSprite("dialogNames_" + currentSpeaker.ToLower());

		//-------------fade/darken non speakers, highlight current speaker----------//
		for(int i = 0; i < dialogIcons.Count; i++){
			Debug.Log("Change Speaker:" + dialogIcons[i].name);
			Debug.Log("Change Speaker:" + dialogIcons[i].name.Substring(4));
			if(dialogIcons[i].name.Substring(5) == currentSpeaker){//substring because of "icon_" infront of each icon's name
				dialogIcons[i].GetComponent<Image>().color = new Color(255f,255f,255f,1f);
				currentlySpeakingIcon = dialogIcons[i];
			}else{
				dialogIcons[i].GetComponent<Image>().color = new Color(108f,108f,108f,.75f);
			}
		}
		//-----------------------------------------------------------------------//
	}


}
