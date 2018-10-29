using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour {

	public GameObject pauseMenu;


	// Use this for initialization
	void Start () {
        pauseMenu.GetComponent<GUI_PauseMenu>().player = this.gameObject;
    }

    private void OnDestroy()
    {
    }

    // Update is called once per frame
    void Update () {
        if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
            if (ControllerManager.Instance.GetKeyDown(INPUTACTION.PAUSE)) {
                GameStateManager.Instance.PushState(typeof(PauseMenuState));
            }
        }
	}
}
