using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ev_Dumpster : MonoBehaviour {
	//if activate when close, dumpster check. If carrying a piece of l trash, activate Return in LargeTrashBahvior,
	//otherwise, bring up end day popup.

	public GUI_OptionsPopupBehavior endDayPopUp;
	public GameObject largeTrashDiscoveredDisplay; //enabled/disabled by ev_largeTrash
    // Use this for initialization
    void Start() {
        if (GlobalVariableManager.Instance.DAY_NUMBER == 1) {
            gameObject.GetComponent<SE_GlowWhenClose>().enabled = false; // cannot instantly end day on first day.
            StartCoroutine("FirstDayActivateDelay");
        }

        // Register Events
        if (endDayPopUp != null) { // The dumpster on the title screen doesn't have a popup or care about one.
            endDayPopUp.RegisterOpenEvent(OnOpenEvent);
            endDayPopUp.RegisterCloseEvent(OnCloseEvent);
            endDayPopUp.RegisterOptionEvent(OnOptionsEvent);
        }
	}

    void OnDestroy()
    {
        // Unregister Events
        if (endDayPopUp != null) {
            endDayPopUp.UnregisterOpenEvent(OnOpenEvent);
            endDayPopUp.UnregisterCloseEvent(OnCloseEvent);
            endDayPopUp.UnregisterOptionEvent(OnOptionsEvent);
        }
    }

    void OnOpenEvent()
    {
        Time.timeScale = 0f;
    }

    void OnCloseEvent()
    {
        if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
            gameObject.GetComponent<SE_GlowWhenClose>().enabled = true;
            gameObject.GetComponent<SE_GlowWhenClose>().SetGlowCheck(0);
            Time.timeScale = 1f;
        }
    }

    void OnOptionsEvent(int optionNum)
    {
        switch (optionNum) {
            case 0:
                GameObject fadeHelp = GameObject.Find("fadeHelper");
                fadeHelp.GetComponent<Ev_FadeHelper>().EndOfDayFade();
                Destroy(endDayPopUp.gameObject);
                Time.timeScale = 1f;
                break;
            case 1:
                // Here's where other options would go.
                break;
            case 2:
                // Here's where other options would go.
                break;
        }
    }

	public void Activate(){
		GUIManager.Instance.HUD.Create(endDayPopUp.gameObject);
	}

	IEnumerator FirstDayActivateDelay(){
		yield return new WaitForSeconds(10f);
		gameObject.GetComponent<SE_GlowWhenClose>().enabled = true;
	}
}
