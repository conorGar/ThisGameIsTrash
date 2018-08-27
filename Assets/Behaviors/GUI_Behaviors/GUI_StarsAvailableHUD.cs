using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class GUI_StarsAvailableHUD : MonoBehaviour {

	public TextMeshProUGUI myText;

	void Start () {
		
	}
	void OnEnable(){
		myText.text = GlobalVariableManager.Instance.STAR_POINTS.ToString();
	}
	// Update is called once per frame
	void Update () {
		
	}

	public void UpdateDisplay(){
		myText.text = GlobalVariableManager.Instance.STAR_POINTS.ToString();
	}
}
