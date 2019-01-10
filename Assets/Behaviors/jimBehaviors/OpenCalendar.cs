using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCalendar : MonoBehaviour {

	//public GameObject calendar;


	// Update is called once per frame
	void Update () {
		if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
            if (ControllerManager.Instance.GetKeyDown(INPUTACTION.CALENDAR)) {
                GameStateManager.Instance.PushState(typeof(CalendarState));
            }
        }
	}
}
