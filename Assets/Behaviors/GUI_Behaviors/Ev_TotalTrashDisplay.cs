using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ev_TotalTrashDisplay : MonoBehaviour {

	Text numberDisplay;
	bool showingStarDisplay = false;


	void Start () {
		numberDisplay = gameObject.transform.GetChild(0).GetComponent<Text>();
	}

	// Update is called once per frame
	void Update () {
		if(showingStarDisplay == false){
			numberDisplay.text = GlobalVariableManager.Instance.TOTAL_TRASH.ToString();
		}else{
			numberDisplay.text = GlobalVariableManager.Instance.characterUpgradeArray[7];
		}
	}

	public void ToggleStarDisplay(bool val){
		showingStarDisplay = val;
	}
}
