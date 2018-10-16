using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour {

	public GameObject pauseMenu;


	// Use this for initialization
	void Start () {
        GameStateManager.Instance.RegisterEnterEvent(typeof(OptionsState), OnEnterOptionsState);
        GameStateManager.Instance.RegisterLeaveEvent(typeof(OptionsState), OnLeaveOptionsState);

        pauseMenu.GetComponent<GUI_PauseMenu>().player = this.gameObject;
    }

    private void OnDestroy()
    {
        GameStateManager.Instance.UnregisterEnterEvent(typeof(OptionsState), OnEnterOptionsState);
        GameStateManager.Instance.UnregisterLeaveEvent(typeof(OptionsState), OnLeaveOptionsState);
    }

    // Update is called once per frame
    void Update () {
        if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
            if (ControllerManager.Instance.GetKeyDown(INPUTACTION.PAUSE)) {
                GameStateManager.Instance.PushState(typeof(OptionsState));
            }
        }
	}

    void OnEnterOptionsState()
    {
    }

    void OnLeaveOptionsState()
    {
    }
}
