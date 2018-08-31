using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUI_GeneralPopup : MonoBehaviour {

	public AudioClip myPopupSound;


	void Start () {
		Time.timeScale = 0;
		SoundManager.instance.PlaySingle(myPopupSound);
	}
	
	// Update is called once per frame
	void Update () {
		if(ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT))
        {
			
			Time.timeScale = 1;
			gameObject.SetActive(false);
		}
	}
}
