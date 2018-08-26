using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatWithHatFriend : Friend {

	// Use this for initialization
	void Start () {
		if(GlobalVariableManager.Instance.DAY_NUMBER == day && nextDialog == "Start"){
			gameObject.GetComponent<ActivateDialogWhenClose>().ActivateDialog();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
