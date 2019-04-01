using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class GUI_SaveFileSelect : GUI_MenuBase
{

	// Use this for initialization
	public GameObject fadeHelper;
	public AudioClip selectSfx;
	public GameObject selectStarsPS;
	public bool debugIntroSkip = true;

	void OnEnable(){

		GetComponent<SpecialEffectsBehavior>().SmoothMovementToPoint(12f,8.4f,.2f,true);
	}
	void Update ()
	{
#if DEBUG_QUICKLOAD
        LoadSave();
#else
        if (GameStateManager.Instance.GetCurrentState() == typeof(TitleState)) {
            if (ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT)
             || ControllerManager.Instance.GetKeyDown(INPUTACTION.PAUSE)) {
            	SoundManager.instance.PlaySingle(selectSfx);
            	selectStarsPS.transform.position = optionIcons[arrowPos].transform.position;
            	selectStarsPS.SetActive(true);
                LoadSave();
                optionIcons[arrowPos].GetComponent<Image>().color = new Color(0.27f, .98f, .51f);
                Invoke("ReturnFromSelectEffect", .2f);
                //TODO: back button: enable scene event script again when do

            }

            if (ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVELEFT) && leftRightNav && arrowPos > 0) {
                optionIcons[arrowPos].GetComponent<GUI_Ev_buttonPopUp>().UnhighlightButton();
                Navigate("left");
            }
            else if (ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVERIGHT) && leftRightNav && arrowPos < maxArrowPos) {
                Debug.Log("Arrow RIGHT");
                optionIcons[arrowPos].GetComponent<GUI_Ev_buttonPopUp>().UnhighlightButton();
                Navigate("right");
            }
            else if (ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVEUP) && upDownNav && arrowPos > 0) {
                optionIcons[arrowPos].GetComponent<GUI_Ev_buttonPopUp>().UnhighlightButton();
                Navigate("up");
            }
            else if (ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVEDOWN) && upDownNav && arrowPos < maxArrowPos) {
                optionIcons[arrowPos].GetComponent<GUI_Ev_buttonPopUp>().UnhighlightButton();
                Navigate("down");
            }
            else if (ControllerManager.Instance.GetKeyDown(INPUTACTION.CALENDAR)) {
                ResetData(arrowPos);
            }


            selectionArrow.transform.position = new Vector2(optionIcons[arrowPos].transform.position.x - 5f, optionIcons[arrowPos].transform.position.y);
        }
#endif
	}
	
	public override void NavigateEffect(){
		optionIcons[arrowPos].GetComponent<GUI_Ev_buttonPopUp>().HighlightButton();

	}

	void LoadSave(){

		UserDataManager.Instance.SetSlot(arrowPos);

        // Clear out all the states.
        GameStateManager.Instance.PopAllStates();
        GlobalVariableManager.Instance.SetDefaultStats();
        SoundManager.instance.FadeMusic();
        StartCoroutine(UserDataManager.Instance.ReadAsync(
        () => {
            // After the data is read, load the scene based on which day the player is currently at.
            // Day 0 goes right into level 1.  All other days will be at the hub.
            if (GlobalVariableManager.Instance.DAY_NUMBER - 1 < 1) {
            	if(!debugIntroSkip)
                fadeHelper.GetComponent<Ev_FadeHelper>().FadeToScene("Intro");
                else
				fadeHelper.GetComponent<Ev_FadeHelper>().FadeToScene("1_1");

            }
            else {
                fadeHelper.GetComponent<Ev_FadeHelper>().FadeToScene("Hub");
            }
        }));

		

	}

    static void ResetData(int slot)
    {
        string directory_path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), "TGIT");
        if (!Directory.Exists(directory_path)) {
            return;
        }

        string fileName = Path.Combine(directory_path, "UserData_" + slot + ".json");

        if (File.Exists(fileName)) {
            File.Delete(fileName);
        }

        Debug.Log("Data in Slot: " + slot + " has been deleted!");
    }

    void ReturnFromSelectEffect(){
		optionIcons[arrowPos].GetComponent<Image>().color = Color.white;
    }
}

