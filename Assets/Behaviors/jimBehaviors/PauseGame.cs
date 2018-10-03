using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour {

	public GameObject pauseMenu;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(ControllerManager.Instance.GetKeyDown(INPUTACTION.PAUSE))
        {
        	gameObject.GetComponent<MeleeAttack>().enabled = false;
        	gameObject.GetComponent<ThrowTrash>().enabled = false;
        	pauseMenu.GetComponent<GUI_PauseMenu>().player = this.gameObject;
			pauseMenu.SetActive(true);
		}
	}

	public void ReturnFromPause(){//Activated by GUI_PauseMenu
		gameObject.GetComponent<MeleeAttack>().enabled = true;
        gameObject.GetComponent<ThrowTrash>().enabled = true;

	}
}
