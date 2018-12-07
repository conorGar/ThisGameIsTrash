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
		maxHP.text = "/" + GlobalVariableManager.Instance.HP_STAT.GetMax().ToString();
        currentHP.text = GlobalVariableManager.Instance.HP_STAT.GetCurrent().ToString();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void UpdateDisplay(){
		Debug.Log("UpdateDisplay Happened properly");
		currentHP.text = GlobalVariableManager.Instance.HP_STAT.GetCurrent().ToString();
		currentHP.GetComponent<TextAnimation>().PlayAnimation(0);
	}
}
