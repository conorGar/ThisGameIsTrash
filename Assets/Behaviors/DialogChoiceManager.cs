using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using I2.TextAnimation;

public class DialogChoiceManager : MonoBehaviour {

	public TextAsset csvFile;
	public int currentWorldNumber;
	public GameObject dialogManager; // just needed for SelectOption()

	//List<DialogChoice> dialogChoices= new List<DialogChoice>();
	public List<GameObject> dialogChoiceBoxes = new List<GameObject>();
	List<DialogResponse> dialogResponses = new List<DialogResponse>();
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

	public void SetDialogChoices(DialogNode currentNode){//activated from DialogBehaviorManager.NextText()
		

		numberOfOptions = currentNode.responses.Count;
		for(int i = 0; i < numberOfOptions; i++){
			dialogResponses.Add(currentNode.responses[i]);
			dialogChoiceBoxes[i].SetActive(true);
			dialogChoiceBoxes[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = dialogResponses[i].text;
		}
					



	}

	/*void ShowChoices(DialogChoice thisDialogChoiceSequence){
		//DialogChoice thisDialogChoice = dialogChoices[choiceID];
		gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = thisDialogChoiceSequence.GetTitle();//question title
		numberOfOptions = thisDialogChoiceSequence.GetNumberOfChoices();
		Debug.Log("Number of Options:" + numberOfOptions);
		Debug.Log(thisDialogChoiceSequence.GetChoices(0));
		for(int i = 0; i< numberOfOptions;i++){
			gameObject.transform.GetChild(i+1).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = thisDialogChoiceSequence.GetChoices(i);
		}

	}*/

	void NavigateOptions(string direction){


		dialogChoiceBoxes[arrowPos+1].GetComponent<SpriteRenderer>().color = unhighlightedColor; 
		dialogChoiceBoxes[arrowPos+1].transform.position = new Vector2(4f,dialogChoiceBoxes[arrowPos+1].transform.position.y);
		dialogChoiceBoxes[arrowPos+1].transform.GetChild(0).gameObject.GetComponent<TextAnimation>().enabled = false;
		dialogChoiceBoxes[arrowPos+1].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().color = Color.white;


		Debug.Log(arrowPos);

		//Debug.Log(this.gameObject.transform.GetChild(arrowPos+1).transform.position.y);
		if(direction == "up"){
		 	arrowPos--;
		}else if(direction == "down"){
			arrowPos++;
		} 
		dialogChoiceBoxes[arrowPos+1].GetComponent<SpriteRenderer>().color = highlightedColor;
		dialogChoiceBoxes[arrowPos+1].transform.position = new Vector2(5.8f,this.gameObject.transform.GetChild(arrowPos+1).transform.position.y);

		//text change
		dialogChoiceBoxes[arrowPos+1].transform.GetChild(0).gameObject.GetComponent<TextAnimation>().enabled = true;
		dialogChoiceBoxes[arrowPos+1].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().color = Color.green;

		Debug.Log(arrowPos);
		Debug.Log(gameObject.transform.GetChild(arrowPos+1).name);
		Debug.Log(this.gameObject.transform.GetChild(arrowPos+1).transform.position.y);

	}

	void SelectOption(int optionNumber){
		dialogManager.GetComponent<DialogManager>().ReturnFromChoice(dialogResponses[optionNumber -1].node_id);
	}

}


//Still need:

//-Activate only a certain number of children based on how many options there are...