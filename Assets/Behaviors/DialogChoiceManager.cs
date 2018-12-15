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
	//DialogChoice currentDialogChoice;
	public GameObject chooseHighlighter;

	string mydialogName;
	int numberOfOptions;
	//string csvText;
	string thisDialogText = null;
	List<int> myLinks = new List<int>(); //needed up here for proper function of SelectOption()
    bool canSelect = false;
	float popupSecondsPassed = 0f;
    float popupWaitTime = 1f; // Time to wait for the player is allowed to select a dialog choice
    float smoothMovementTime = .5f; // Time it takes for the dialog to transition smoothly into view


	//----Navigation Variables!---------//
	int arrowPos = 0;
	Color highlightedColor = new Color(255f,255f,255f,1f);
	Color unhighlightedColor = new Color(255f,255f,255f,.5f);


	//--------------------------------//




	void Start () {

	}

	void OnEnable(){
		chooseHighlighter.SetActive(false);
		arrowPos = 0;
        GetComponent<SpecialEffectsBehavior>().SmoothMovementToPoint(-22f, -31f, smoothMovementTime, true);
        popupSecondsPassed = 0f;
        canSelect = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (GameStateManager.Instance.GetCurrentState() == typeof(DialogState)) {
            if (ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVEUP)
            || ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKUP) && arrowPos > 0) {
                SoundManager.instance.PlaySingle(navUpSfx);
                NavigateOptions("up");
            }
            else if (ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVEDOWN)
            || ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKDOWN) && arrowPos <= (numberOfOptions - 1)) {
                SoundManager.instance.PlaySingle(navDownSfx);
                NavigateOptions("down");
            }
            else if (ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT)) {
                if (canSelect && popupSecondsPassed > popupWaitTime) {
                    SoundManager.instance.PlaySingle(selectChoice);
                    StartCoroutine("SelectOption", arrowPos);
                    canSelect = false;
                }
            }

            popupSecondsPassed += Time.deltaTime;
        }
	}

	public void SetDialogChoices(DialogNode currentNode){//activated from DialogBehaviorManager.NextText()
        arrowPos = 0;
		dialogResponses.Clear();
		numberOfOptions = currentNode.responses.Count;
		//Debug.Log("Number of Options: " + numberOfOptions);
		//Debug.Log(currentNode.responses[0].text);
		//Debug.Log(currentNode.responses[1].text);
		choiceTitle.text = currentNode.question.ToString();
		for(int i = 0; i < numberOfOptions; i++){
			//Debug.Log(currentNode.responses[i].text);
			//currentNode.responses[i].text.Replace("image","");
			//Debug.Log(currentNode.responses[i].text);
			dialogResponses.Add(currentNode.responses[i]);
			//dialogResponses[i].text.Replace("[image]"," ");
			dialogChoiceBoxes[i].SetActive(true);
			dialogChoiceBoxes[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = dialogResponses[i].text;

			//enable images on special image dialog choices
			if(dialogResponses[i].text.Contains(":")){
				//Debug.Log("vvvvvv Dialog Choice Icons Activated Here vvvv");
				//int index = dialogChoiceBoxes[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text.IndexOf("[image]");
				//Debug.Log(index);
				//dialogChoiceBoxes[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text.Replace("[image]",".");
				//dialogChoiceBoxes[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text.Remove(index,6);
				//Debug.Log(dialogChoiceBoxes[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text);
				dialogChoiceBoxes[i].transform.GetChild(1).gameObject.SetActive(true);
			}
		}

		dialogChoiceBoxes[arrowPos].GetComponent<Image>().color = highlightedColor;
		dialogChoiceBoxes[arrowPos].transform.position = new Vector2(5.8f,dialogChoiceBoxes[arrowPos].transform.position.y);

		//text change
		dialogChoiceBoxes[arrowPos].transform.GetChild(0).gameObject.GetComponent<TextAnimation>().enabled = true;
		dialogChoiceBoxes[arrowPos].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().color = Color.green;
					



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

	IEnumerator SelectOption(int optionNumber){
		//deactivate dialog boxes
		chooseHighlighter.transform.parent = dialogChoiceBoxes[arrowPos].transform;
		chooseHighlighter.transform.localPosition = new Vector2(.87f,1.9f);
		chooseHighlighter.SetActive(true);
		dialogChoiceBoxes[arrowPos].GetComponent<Animator>().Play("choicePopupSelect",-1,0f);

		yield return new WaitForSeconds(.35f);
		dialogChoiceBoxes[arrowPos].GetComponent<Image>().color = unhighlightedColor; 
		dialogChoiceBoxes[arrowPos].transform.position = new Vector2(4.6f,dialogChoiceBoxes[arrowPos].transform.position.y);
		dialogChoiceBoxes[arrowPos].transform.GetChild(0).gameObject.GetComponent<TextAnimation>().enabled = false;
		dialogChoiceBoxes[arrowPos].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().color = Color.white;
		for(int i = 0; i < dialogChoiceBoxes.Count; i++){
			if(dialogChoiceBoxes[i].transform.childCount >1)
				dialogChoiceBoxes[i].transform.GetChild(1).gameObject.SetActive(false); // make sure choice images are deactivated
			dialogChoiceBoxes[i].SetActive(false);
		}
		Debug.Log("number of choices: " + dialogChoiceBoxes.Count);
		Debug.Log("Option number selected:" + optionNumber);
		Debug.Log(dialogResponses.Count);
		chooseHighlighter.SetActive(false);
		dialogManager.GetComponent<DialogManager>().ReturnFromChoice(dialogResponses[optionNumber].node_id);
	}

}


//Still need:

//-Activate only a certain number of children based on how many options there are...