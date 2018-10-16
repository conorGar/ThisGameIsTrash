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
    public GameObject endDayDisplayHUD;

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

        GameStateManager.Instance.RegisterEnterEvent(typeof(OptionsState), OnEnterOptionsState);
        GameStateManager.Instance.RegisterLeaveEvent(typeof(OptionsState), OnLeaveOptionsState);

        // TODO: Unity doesn't run start on disabled gameobjects because it's lame and weird.  Is there a better way to do this
        // Then starting active and disabling it immediately?
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        GameStateManager.Instance.UnregisterEnterEvent(typeof(OptionsState), OnEnterOptionsState);
        GameStateManager.Instance.UnregisterLeaveEvent(typeof(OptionsState), OnLeaveOptionsState);
    }

    void OnEnterOptionsState()
    {
        gameObject.SetActive(true);
        SoundManager.instance.PlaySingle(paperSlide);
        Time.timeScale = 0;
    }

    void OnLeaveOptionsState()
    {
        gameObject.SetActive(false);
        SoundManager.instance.PlaySingle(paperSlide);
        Time.timeScale = 1;
    }

	// Update is called once per frame
	void Update () {
        if (GameStateManager.Instance.GetCurrentState() == typeof(OptionsState)) {
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
                    optionsMenu.SetActive(true);
                    this.enabled = false;
                }
                else if (arrowpos == 2) {//end day
                    endDayDisplayHUD.SetActive(true);
                    endDayDisplayHUD.GetComponent<GUI_OptionsPopupBehavior>().pauseMenu = this;
                    endDayDisplayHUD.GetComponent<GUI_OptionsPopupBehavior>().AtPauseScreen = true;
                    this.enabled = false;
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
