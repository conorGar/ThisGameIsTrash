using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using I2.TextAnimation;

public class GUI_HPdisplay : MonoBehaviour {
	public TextMeshProUGUI maxHP;
	public TextMeshProUGUI currentHP;
	// Use this for initialization
	void Start () {
		maxHP.text = "/" + GlobalVariableManager.Instance.Max_HP.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void UpdateDisplay(int currentHealth){
		Debug.Log("UpdateDisplay Happened properly");
		currentHP.text = currentHealth.ToString();
		currentHP.GetComponent<TextAnimation>().PlayAnimation(0);
	}
}
