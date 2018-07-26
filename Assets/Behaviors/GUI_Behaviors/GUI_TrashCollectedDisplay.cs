using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
using TMPro;
using I2.TextAnimation;

public class GUI_TrashCollectedDisplay : MonoBehaviour {
	public TextMeshProUGUI trashCollected;
	public GameObject newDiscoveryDisplay;
	// Use this for initialization
	void Start () {
		//TODO: adjust for different bag types
		trashCollected.text = GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[0].ToString();

	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void UpdateDisplay(int trashNum){
		Debug.Log("UpdateDisplay Happened properly");
		trashCollected.text = trashNum.ToString();
		trashCollected.GetComponent<TextAnimation>().PlayAnimation(0);
	}
	public void NewDiscoveryShow(string trashSprite, string trashname){
		newDiscoveryDisplay.SetActive(true);
		//newDiscoveryDisplay.GetComponent<GUIEffects>().Start();
		newDiscoveryDisplay.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = trashname;
		newDiscoveryDisplay.transform.GetChild(1).GetComponent<tk2dSprite>().SetSprite(trashSprite);
	}
}
