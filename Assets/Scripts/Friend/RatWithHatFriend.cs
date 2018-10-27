using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatWithHatFriend : Friend {

	[HideInInspector]
	public bool hadIntroDialog = false; 


	public GameObject baseStatHUD;
	// Use this for initialization
	void Start () {
		if(GlobalVariableManager.Instance.DAY_NUMBER < day){
		gameObject.SetActive(false);
		}

		if(GlobalVariableManager.Instance.DAY_NUMBER == day && nextDialog == "Start"){
			gameObject.GetComponent<ActivateDialogWhenClose>().ActivateDialog();
			Debug.Log("RatWithAHat start dialog activated");
		}
		if(hadIntroDialog){//after the rat's intro dialog, getting close goes to upgrade HUD.
			gameObject.GetComponent<ActivateDialogWhenClose>().enabled = false;
			gameObject.GetComponent<Hub_UpgradeStand>().enabled = true;
		}
	}
	
	// Update is called once per frame
	void Update () {

    }
}
