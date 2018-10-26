using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_PauseMenu : MonoBehaviour {

    int arrowpos = 1;
    public GameObject optionsOption;
    public GameObject enddayOption;
    public GameObject optionsMenu;
    public GameObject returnToDumpsterOption;
    public GUI_OptionsPopupBehavior endDayPopup;

    public Sprite optionsHLspr;
    public Sprite endayHLspr;
    public Sprite returnToDumpsterHlspr;


    public AudioClip selectSound;
    public AudioClip paperSlide;
    [HideInInspector]
    public GameObject player;//set by player- PauseGame.cs
    Sprite optionStartSpr;
    Sprite endDayStartSpr;
    Sprite returnStartSpr;
    Vector3 startPos;
    // Use this for initialization
    void Start() {
        startPos = gameObject.transform.localPosition;
        endDayStartSpr = enddayOption.GetComponent<Image>().sprite;
        optionStartSpr = optionsOption.GetComponent<Image>().sprite;
        endDayStartSpr = enddayOption.GetComponent<Image>().sprite;

        optionsOption.GetComponent<Image>().sprite = optionsHLspr;

        // TODO: Unity doesn't run start on disabled gameobjects because it's lame and weird.  Is there a better way to do this
        // Then starting active and disabling it immediately?
        gameObject.SetActive(false);

        // Register Events
        endDayPopup.RegisterCloseEvent(OnPopupCloseEvent);
        endDayPopup.RegisterOptionEvent(OnPopupOptionsEvent);

        GameStateManager.Instance.RegisterEnterEvent(typeof(PauseMenuState), OnEnterPauseMenuState);
        GameStateManager.Instance.RegisterLeaveEvent(typeof(PauseMenuState), OnLeavePauseMenuState);
        GameStateManager.Instance.RegisterEnterEvent(typeof(EndDayState), OnEnterEndDayState);
    }

    private void OnDestroy()
    {
        // Unregister Events
        endDayPopup.UnregisterCloseEvent(OnPopupCloseEvent);
        endDayPopup.UnregisterOptionEvent(OnPopupOptionsEvent);

        GameStateManager.Instance.UnregisterEnterEvent(typeof(PauseMenuState), OnEnterPauseMenuState);
        GameStateManager.Instance.UnregisterLeaveEvent(typeof(PauseMenuState), OnLeavePauseMenuState);
        GameStateManager.Instance.UnregisterEnterEvent(typeof(EndDayState), OnEnterEndDayState);
    }

    void OnPopupCloseEvent()
    {
    }

    void OnPopupOptionsEvent(int optionNum)
    {
        switch (optionNum) {
            case 0:
                GameObject fadeHelp = GameObject.Find("fadeHelper");
                fadeHelp.GetComponent<Ev_FadeHelper>().EndOfDayFade();
                Destroy(endDayPopup.gameObject);
                break;
        }
    }

    void OnEnterPauseMenuState()
    {
        gameObject.SetActive(true);
        SoundManager.instance.PlaySingle(paperSlide);
        Time.timeScale = 0;
    }

    void OnLeavePauseMenuState()
    {
        gameObject.SetActive(false);
        SoundManager.instance.PlaySingle(paperSlide);
        Time.timeScale = 1;
    }

    void OnEnterEndDayState()
    {
        // Hide this if the day is over.
        gameObject.SetActive(false);
    }

	// Update is called once per frame
	void Update () {
        if (GameStateManager.Instance.GetCurrentState() == typeof(PauseMenuState)) {
            if (ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVERIGHT)
            || ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKRIGHT)) {
                if (arrowpos < 2) {
                    arrowpos++;
                    SoundManager.instance.PlaySingle(selectSound);
                    enddayOption.GetComponent<Image>().sprite = endayHLspr;
                    optionsOption.GetComponent<Image>().sprite = optionStartSpr;
                }
            }
            else if (ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVELEFT)
                 || ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKLEFT)) {
                if (arrowpos > 1) {
                    arrowpos--;
                    SoundManager.instance.PlaySingle(selectSound);
                    enddayOption.GetComponent<Image>().sprite = endDayStartSpr;
                    optionsOption.GetComponent<Image>().sprite = optionsHLspr;
                }
            }
            else if (ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT)) {
                if (arrowpos == 1) {
                    // To the options menu.
                    GameStateManager.Instance.PushState(typeof(OptionsState));
                    optionsMenu.SetActive(true);
                }
                else if (arrowpos == 2) {//end day
                    endDayPopup.gameObject.SetActive(true);
                }
            }
            if (ControllerManager.Instance.GetKeyDown(INPUTACTION.CANCEL)
            || ControllerManager.Instance.GetKeyDown(INPUTACTION.PAUSE)) {
                arrowpos = 1;
                gameObject.transform.localPosition = startPos;
                GameStateManager.Instance.PopState();
            }
        }


		gameObject.transform.localPosition = Vector3.Lerp(gameObject.transform.localPosition,Vector3.zero,.1f*(Time.realtimeSinceStartup - Time.deltaTime));
	}
}
