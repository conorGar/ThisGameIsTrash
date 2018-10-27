using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_BossHpDisplay : MonoBehaviour {

	float fillAmount;
	float myWidth;
	//int myBossNumber;
	public Image myBar;
	public int maxHp;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void UpdateBossHp(float currentHp){
//		Debug.Log("maxHP:" + currentHp + " " + maxHp + " " + currentHp/maxHp);
		fillAmount = ((float)currentHp/(float)maxHp);
		//Debug.Log("fillamount:" + fillAmount);

		myBar.fillAmount = fillAmount;
	}
}
