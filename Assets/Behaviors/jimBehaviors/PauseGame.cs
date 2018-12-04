using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour {
    // Update is called once per frame
    void Update () {
        if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
            if (ControllerManager.Instance.GetKeyDown(INPUTACTION.PAUSE)) {
                GameStateManager.Instance.PushState(typeof(PauseMenuState));
            }
        }
	}
}
