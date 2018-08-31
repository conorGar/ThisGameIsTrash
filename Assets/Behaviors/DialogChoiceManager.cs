using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using I2.TextAnimation;

public class DialogChoiceManager : MonoBehaviour {

	//public TextAsset csvFile;
	//public int currentWorldNumber;
	public GameObject dialogManager; // just needed for SelectOption()
	public AudioClip navDownSfx;
	public AudioClip navUpSfx;
	public AudioClip selectChoice;
	//List<DialogChoice> dialogChoices= new List<DialogChoice>();
	public List<GameObject> dialogChoiceBoxes = new List<GameObject>();
	List<DialogResponse> dialogResponses = new List<DialogResponse>();
	public TextMeshProUGUI choiceTitle;
	DialogChoice currentDialogChoice;

	string mydialogName;
	int numberOfOptions;
	//string csvText;
	string thisDialogText = null;
	List<int> myLinks = new List<int>(); //needed up here for proper function of SelectOption()

	//----Navigation Variables!---------//
	int arrowPos = 0;
	Color highlightedColor = new Color(255f,255f,255f,1f);
	Color unhighlightedColor = new Color(255f,255f,255f,.5f);


	//--------------------------------//




	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.UpArrow) && arrowPos > 0){
			SoundManager.instance.PlaySingle(navUpSfx);
			NavigateOptions("up");
		}else if(Input.GetKeyDown(KeyCode.DownArrow) && arrowPos <= (numberOfOptions - 1)){
			SoundManager.instance.PlaySingle(navDownSfx);
			NavigateOptions("down");
		}else if(Input.GetKeyDown(KeyCode.Space)){
			SoundManager.instance.PlaySingle(selectChoice);
			SelectOption(arrowPos);
		}
	}

	public void SetDialogChoices(DialogNode currentNode){//activated from DialogBehaviorManager.NextText()

		arrowPos = 0;
		dialogResponses.Clear();
		numberOfOptions = currentNode.responses.Count;
		for(int i = 0; i < numberOfOptions; i++){
			dialogResponses.Add(currentNode.responses[i]);
			dialogChoiceBoxes[i].SetActive(true);
			dialogChoiceBoxes[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = dialogResponses[i].text;

			//enable images on special image dialog choices
			if(dialogResponses[i].text.Contains("[Image]")){
				dialogChoiceBoxes[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text.Replace("[Image]","");
				dialogChoiceBoxes[i].transform.GetChild(1).gameObject.SetActive(true);
			}
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


		dialogChoiceBoxes[arrowPos].GetComponent<Image>().color = unhighlightedColor; 
		dialogChoiceBoxes[arrowPos].transform.position = new Vector2(4.6f,dialogChoiceBoxes[arrowPos].transform.position.y);
		dialogChoiceBoxes[arrowPos].transform.GetChild(0).gameObject.GetComponent<TextAnimation>().enabled = false;
		dialogChoiceBoxes[arrowPos].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().color = Color.white;


		Debug.Log(arrowPos);

		//Debug.Log(this.gameObject.transform.GetChild(arrowPos+1).transform.position.y);
		if(direction == "up" && arrowPos > 0){
		 	arrowPos--;
		}else if(direction == "down" && arrowPos < (numberOfOptions-1)){
			arrowPos++;
		} 
		dialogChoiceBoxes[arrowPos].GetComponent<Image>().color = highlightedColor;
		dialogChoiceBoxes[arrowPos].transform.position = new Vector2(5.8f,dialogChoiceBoxes[arrowPos].transform.position.y);

		//text change
		dialogChoiceBoxes[arrowPos].transform.GetChild(0).gameObject.GetComponent<TextAnimation>().enabled = true;
		dialogChoiceBoxes[arrowPos].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().color = Color.green;

		Debug.Log(arrowPos);
		//Debug.Log(gameObject.transform.GetChild(arrowPos+1).name);
		//Debug.Log(this.gameObject.transform.GetChild(arrowPos+1).transform.position.y);

	}

	void SelectOption(int optionNumber){
		//deactivate dialog boxes
		for(int i = 0; i < dialogChoiceBoxes.Count; i++){
			dialogChoiceBoxes[i].transform.GetChild(1).gameObject.SetActive(false); // make sure choice images are deactivated
			dialogChoiceBoxes[i].SetActive(false);
		}


		dialogManager.GetComponent<DialogManager>().ReturnFromChoice(dialogResponses[optionNumber].node_id);
	}

}


//Still need:

//-Activate only a certain number of children based on how many options there are...