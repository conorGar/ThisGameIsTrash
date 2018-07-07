using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class DialogChoiceManager : MonoBehaviour {

	public TextAsset csvFile;
	public int currentWorldNumber;
	public GameObject dialogManager; // just needed for SelectOption()

	List<DialogChoice> dialogChoices= new List<DialogChoice>();
	DialogChoice currentDialogChoice;

	string mydialogName;
	int numberOfOptions;
	string csvText;
	string thisDialogText = null;
	List<int> myLinks = new List<int>(); //needed up here for proper function of SelectOption()

	//----Navigation Variables!---------//
	int arrowPos = 0;
	Color highlightedColor = new Color(255f,255f,255f,1f);
	Color unhighlightedColor = new Color(255f,255f,255f,.5f);


	//--------------------------------//




	void Start () {
	//- didnt define csv here because i think maybe it wasn't doing it before it was looking for the index of dialogname under SetDialogCHoices()
		//csvText = csvFile.text;
		//csvText = csvText.Substring(csvText.IndexOf("WorldNumber" + currentWorldNumber),Mathf.Abs(csvText.IndexOf("WorldNumber" + currentWorldNumber) - csvText.IndexOf("WorldNumber", csvText.IndexOf("WorldNumber" + currentWorldNumber) + 1)));//groups by world number
		//Debug.Log(csvText);
		//string[] lines = csvText.Split('\n');

	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.UpArrow) && arrowPos > 0){
			NavigateOptions("up");
		}else if(Input.GetKeyDown(KeyCode.DownArrow) && arrowPos <= (numberOfOptions - 1)){
			NavigateOptions("down");
		}else if(Input.GetKeyDown(KeyCode.Space)){
			SelectOption(arrowPos);
		}
	}

	public void SetDialogChoices(string dialogName, int id){//activated from DialogBehaviorManager.NextText()
		

		if(this.mydialogName != dialogName){//this is the first time being activated
			this.mydialogName = dialogName;
			csvText = csvFile.text;
			Debug.Log(csvText.IndexOf("WorldNumber", (csvText.IndexOf("WorldNumber" + currentWorldNumber) + 1)));
			Debug.Log(Mathf.Abs(csvText.IndexOf("WorldNumber", (csvText.IndexOf("WorldNumber" + currentWorldNumber) + 1))- csvText.IndexOf("WorldNumber" + currentWorldNumber)));
			csvText = csvText.Substring(csvText.IndexOf("WorldNumber" + currentWorldNumber),Mathf.Abs(csvText.IndexOf("WorldNumber", (csvText.IndexOf("WorldNumber" + currentWorldNumber) + 1))- csvText.IndexOf("WorldNumber" + currentWorldNumber)));//groups by world number
			Debug.Log(csvText);

			List<string> myChoices = new List<string>();
			Debug.Log(mydialogName);
			int myIndex = csvText.IndexOf(mydialogName);
			thisDialogText =  csvText.Substring(myIndex,(csvText.IndexOf("/nd", myIndex + 1)) - myIndex); //only get the strings for this specific dialog occuring
			string title = null;


			string[] dialogOptionInstancesInThisDialog = thisDialogText.Split(new string[]{"ID:"}, System.StringSplitOptions.None);

			for(int i = 0; i <dialogOptionInstancesInThisDialog.Length; i++){ 
				string[] lines = dialogOptionInstancesInThisDialog[i].Split('\n');
					foreach(var line in lines){
						//split line into colomns
						var colomns = line.Split(',');
							if(title == null && colomns.Length > 1 && colomns[1].Length>2){ //colomns[1] length checkis to make sure that the part before "ID" index isnt set as title.(basically, it'll be blank if you dont do this)
								title = colomns[1]; 
								Debug.Log("Title:" + title);
								myChoices.Add(colomns[2]);//1st option is in different colomn number(2) than the other ones(4). Use title set check to make sure this only happens on 1st option
								myLinks.Add(int.Parse(colomns[3]));//^basically same deal as this
							}
							Debug.Log("colomn 1:" + colomns[1]);
						if(colomns.Length > 4){
							myChoices.Add(colomns[4]);
							Debug.Log("colomn 5:" + colomns[5]);
							myLinks.Add(int.Parse(colomns[5]));
						}
					}
				dialogChoices.Add(new DialogChoice(title,myLinks,myChoices));
			}
		}

		currentDialogChoice = dialogChoices[id];
		Debug.Log("Link 0:" + currentDialogChoice.GetLink(0));
		Debug.Log("Link 1:" + currentDialogChoice.GetLink(1));
		Debug.Log("Link 2:" + currentDialogChoice.GetLink(2));


		ShowChoices(currentDialogChoice);


	}

	void ShowChoices(DialogChoice thisDialogChoiceSequence){
		//DialogChoice thisDialogChoice = dialogChoices[choiceID];
		gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = thisDialogChoiceSequence.GetTitle();//question title
		numberOfOptions = thisDialogChoiceSequence.GetNumberOfChoices();
		Debug.Log("Number of Options:" + numberOfOptions);
		Debug.Log(thisDialogChoiceSequence.GetChoices(0));
		for(int i = 0; i< numberOfOptions;i++){
			gameObject.transform.GetChild(i+1).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = thisDialogChoiceSequence.GetChoices(i);
		}

	}

	void NavigateOptions(string direction){
		gameObject.transform.GetChild(arrowPos+1).GetComponent<SpriteRenderer>().color = unhighlightedColor; 
		gameObject.transform.GetChild(arrowPos+1).transform.position = new Vector2(4f,this.gameObject.transform.GetChild(arrowPos+1).transform.position.y);
		Debug.Log(arrowPos);
		Debug.Log(this.gameObject.transform.GetChild(arrowPos+1).transform.position.y);
		if(direction == "up"){
		 	arrowPos--;
		}else if(direction == "down"){
			arrowPos++;
		} 
		gameObject.transform.GetChild(arrowPos+1).GetComponent<SpriteRenderer>().color = highlightedColor;
		gameObject.transform.GetChild(arrowPos+1).transform.position = new Vector2(5.8f,this.gameObject.transform.GetChild(arrowPos+1).transform.position.y);
		Debug.Log(arrowPos);
		Debug.Log(gameObject.transform.GetChild(arrowPos+1).name);
		Debug.Log(this.gameObject.transform.GetChild(arrowPos+1).transform.position.y);

	}

	void SelectOption(int optionNumber){
		dialogManager.GetComponent<DialogBehaviorManager>().ReturnFromDialogOption(myLinks[optionNumber]);
	}

}


//Still need:

//-Activate only a certain number of children based on how many options there are...