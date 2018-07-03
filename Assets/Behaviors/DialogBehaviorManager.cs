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

	public tk2dSprite dialogName;
	public Image continueIcon;
	public string currentWorldNumber;

	bool finishedDisplayingText = false; // set true when the animation is done
	bool hasWavingText;
	int currentLineNum = 1;
	string textToSwitchto;
	string speaker;
	GameObject dialogIcon;
	//List<DialogChoiceManager> dialogChoices;

	string mydialogName;//set by friend

	void Start () {
		//-------Convert csv file into list of strings-----------//
		string csvText = csvFile.text;
		Debug.Log(csvText.IndexOf("WorldNumber" + currentWorldNumber));
		Debug.Log(csvText.IndexOf("WorldNumber", csvText.IndexOf("WorldNumber" + currentWorldNumber) + 1));
		csvText = csvText.Substring(csvText.IndexOf("WorldNumber" + currentWorldNumber),Mathf.Abs(csvText.IndexOf("WorldNumber" + currentWorldNumber) - csvText.IndexOf("WorldNumber", csvText.IndexOf("WorldNumber" + currentWorldNumber) + 1)));//groups by world number
		Debug.Log(csvText);
		csvText = csvText.Substring(csvText.IndexOf(mydialogName),csvText.IndexOf("/nd", csvText.IndexOf(mydialogName) + 1)); //only get the strings for this specific dialog occuring
		string[] lines = csvText.Split('\n');


		foreach(var line in lines){
			//split line into colomns
			var colomns = line.Split(',');
			Debug.Log("colomns: " + colomns.Length);
			if(speaker == null){
				speaker = colomns[2];
				//Debug.Log(colomns[1]);
			}
			//Debug.Log(colomns[1]);
			if(colomns.Length > 3)//need check because sometimes last value wont have all colons read
				dialogLines.Add(colomns[3].Replace(';',','));//semi-colons have to be used instead of commas to begin with because of .CSV file layout
			if(colomns[4] == "YES"){
				//add dialogue choice

			}
		}

		Debug.Log(dialogLines[1]);
		//-----------------------------------------------------//

		dialogName.SetSprite("dialogNames_" + speaker.ToLower());
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
		//dialogIcon.Play("idle");//stop talking animation
		dialogIcon.GetComponent<Animator>().StopPlayback();
		//dialogIcon.GetComponent<Animator>().
		continueIcon.enabled = true;
		finishedDisplayingText = true;
	}

	void NextText(){
		finishedDisplayingText = false;
		continueIcon.enabled = false;
		if(hasWavingText){
			currentDisplayedText.GetComponent<TextAnimation>()._AnimationSlots[2]._Animation._Sequences[0]._Enabled = false;
			currentDisplayedText.GetComponent<TextAnimation>()._AnimationSlots[2]._Animation._Sequences[0]._Enabled = false;
			hasWavingText = false;
		}
		currentLineNum++;//go to next line in list
		if(dialogLines[currentLineNum].Contains("<c")){
			HighLightText();
		}
		if(dialogLines[currentLineNum].Contains("<w")){
			WaveyText();
		}
		textToSwitchto = dialogLines[currentLineNum];
		currentDisplayedText.text = textToSwitchto;
		dialogIcon.GetComponent<Animator>().StartPlayback();
		currentDisplayedText.GetComponent<TextAnimation>().PlayAnim(0);

	}

	void SetOptions(){
		int[] dialogOptionLinks = 
	}

	public void HighLightText(){ 
		
		if(dialogLines[currentLineNum].Contains("c=g")){//green color highlight
			dialogLines[currentLineNum] = dialogLines[currentLineNum].Replace("<c=g","<color=#78FF32>");
		}
		if(dialogLines[currentLineNum].Contains("c=r")){//red color highlight
			dialogLines[currentLineNum] = dialogLines[currentLineNum].Replace("<c=r","<color=#DC3232>");
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
		int rangeEnd = dialogLines[currentLineNum].IndexOf("</w>");
		dialogLines[currentLineNum] = dialogLines[currentLineNum].Replace("<w>","");
		dialogLines[currentLineNum] = dialogLines[currentLineNum].Replace("</w>","");
		currentDisplayedText.GetComponent<TextAnimation>()._AnimationSlots[2]._Animation._Sequences[0]._TargetRangeStart = rangeStart;
		currentDisplayedText.GetComponent<TextAnimation>()._AnimationSlots[2]._Animation._Sequences[0]._TargetRangeStart = rangeEnd;
		currentDisplayedText.GetComponent<TextAnimation>()._AnimationSlots[2]._Animation._Sequences[1]._TargetRangeStart = rangeStart;
		currentDisplayedText.GetComponent<TextAnimation>()._AnimationSlots[2]._Animation._Sequences[1]._TargetRangeStart = rangeEnd;
		hasWavingText = true;

	}

	public void SetIcon(GameObject icon){
		dialogIcon = icon;
	}

	public void SetDialogName(string name){
		mydialogName = name;
	}


}
