using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ev_Dumpster : MonoBehaviour {
	//if activate when close, dumpster check. If carrying a piece of l trash, activate Return in LargeTrashBahvior,
	//otherwise, bring up end day popup.

	public GameObject endDayPopUp;
	GameObject thisHud;
	// Use this for initialization
	void Start () {
		thisHud = GameObject.Find("HUD");
		if(GlobalVariableManager.Instance.DAY_NUMBER == 1){
			gameObject.GetComponent<SE_GlowWhenClose>().enabled = false; // cannot instantly end day on first day.
			StartCoroutine("FirstDayActivateDelay");
		}
	}


	public void Activate(){
		
		thisHud.GetComponent<Ev_HUD>().Create(endDayPopUp);
	}

	IEnumerator FirstDayActivateDelay(){
		yield return new WaitForSeconds(10f);
		gameObject.GetComponent<SE_GlowWhenClose>().enabled = true;
	}
}
