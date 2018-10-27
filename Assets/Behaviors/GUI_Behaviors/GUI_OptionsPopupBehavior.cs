using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_OptionsPopupBehavior : MonoBehaviour {


	public int howManyOptions = 2;
    public Text option1;
	public Text option2;
	public int closeOptionNumber;

	[HideInInspector]
	int arrowPos = 1;
	Color startColor;
	GameObject objectToActivate;

    public Action OnOpenEvent;
    public Action OnCloseEvent;
    public Action<int> OnOptionEvent;

	void Start () {
		startColor = option1.GetComponent<Text>().color;
	}

	void OnEnable(){
        GameStateManager.Instance.PushState(typeof(PopupState));
        OnOpenEvent();
	}

    private void OnDisable()
    {
        GameStateManager.Instance.PopState();
    }

    public void RegisterOpenEvent(Action openEvent)
    {
        OnOpenEvent += openEvent;
    }

    public void UnregisterOpenEvent(Action openEvent)
    {
        OnOpenEvent -= openEvent;
    }

    public void RegisterCloseEvent(Action closeEvent)
    {
        OnCloseEvent += closeEvent;
    }

    public void UnregisterCloseEvent(Action closeEvent)
    {
        OnCloseEvent -= closeEvent;
    }

    public void RegisterOptionEvent(Action<int> optionEvent)
    {
        OnOptionEvent += optionEvent;
    }

    public void UnregisterOptionEvent(Action<int> optionEvent)
    {
        OnOptionEvent -= optionEvent;
    }

    // Update is called once per frame
    void Update () {
        if (GameStateManager.Instance.GetCurrentState() == typeof(PopupState)) {
            if (ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVERIGHT)
            || ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKRIGHT)) {
                if (arrowPos < howManyOptions) {
                    arrowPos++;
                    option1.GetComponent<Text>().color = new Color(startColor.r, startColor.b, startColor.g, .3f);
                    option2.GetComponent<Text>().color = new Color(startColor.r, startColor.b, startColor.g, 1f);

                }
            } else if (ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVELEFT)
                   || ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKLEFT)) {
                if (1 < arrowPos) {
                    arrowPos--;
                    option1.GetComponent<Text>().color = new Color(startColor.r, startColor.b, startColor.g, 1f); ;
                    option2.GetComponent<Text>().color = new Color(startColor.r, startColor.b, startColor.g, .3f);

                }
            } else if (ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT)) {
                if (arrowPos == closeOptionNumber) {
                    gameObject.SetActive(false); // No longer in the popup state.
                    OnCloseEvent();
                } else {
                    OnOptionEvent(0);
                }
            }
        }
	}
}
