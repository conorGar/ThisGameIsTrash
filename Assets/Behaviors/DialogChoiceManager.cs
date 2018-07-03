using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogChoiceManager : MonoBehaviour {

	/*public TextAsset csvFile;
	List<DialogChoice> dialogChoices= new List<DialogChoice>();

	string mydialogName;
	int currentWorldNumber;
	int numberOfOptions;
	string csvText;



	//Hello! Start today by going through the process of setting/getting dialog choice values and determining what still needs to be done!



	void Start () {
		csvText = csvFile.text;
		csvText = csvText.Substring(csvText.IndexOf("WorldNumber" + currentWorldNumber),Mathf.Abs(csvText.IndexOf("WorldNumber" + currentWorldNumber) - csvText.IndexOf("WorldNumber", csvText.IndexOf("WorldNumber" + currentWorldNumber) + 1)));//groups by world number
		//string[] lines = csvText.Split('\n');

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void SetDialogChoices(string dialogName, int id){
		this.mydialogName = dialogName;
		string tempText =  csvText.Substring(csvText.IndexOf(mydialogName),csvText.IndexOf("/nd", csvText.IndexOf(mydialogName) + 1)); //only get the strings for this specific dialog occuring
		List<int> myLinks = new List<int>();
		List<string> myChoices = new List<string>();
		string title = null;


		string[] choices = tempText.Split("ID:");

		for(int i = 0; i <choices.Length; i++){
			string[] lines = choices[i].Split('\n');
			foreach(var line in lines){
				//split line into colomns
				var colomns = line.Split(',');
					if(title == null){
						title = colomns[3]; //****might not be proper colomn numbers if splitting at ID, look out for this***(delet this comment if everything's fine...)
					}
				myChoices.Add(colomns[4]);
				myLinks.Add(colomns[5]);
			}
			dialogChoices.Add(new DialogChoice(title,myLinks,myChoices));
		}

	}

	void ShowChoices(int choiceID){
		DialogChoice thisDialogChoice = dialogChoices[choiceID];
		gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = thisDialogChoice.GetTitle();//question title
		numberOfOptions = thisDialogChoice.GetNumberOfChoices();
		for(int i = 0; i< numberOfOptions;i++){
			gameObject.transform.GetChild(i+1).GetComponent<TextMeshProUGUI>().text = thisDialogChoice.GetChoices(i+1);
		}

	}
	*/
}


//Still need:

//-Activate only a certain number of children based on how many options there are...