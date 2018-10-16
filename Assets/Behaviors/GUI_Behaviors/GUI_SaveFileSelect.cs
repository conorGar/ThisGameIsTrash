using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using UnityEngine;

public class GUI_SaveFileSelect : GUI_MenuBase
{

	// Use this for initialization
	public GameObject fadeHelper;

	void OnEnable(){

		GetComponent<SpecialEffectsBehavior>().SmoothMovementToPoint(12f,8.4f,.2f,true);
	}
	void Update ()
	{
		if(ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT)){
			LoadSave();
			//TODO: back button: enable scene event script again when do
		}

        if (ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVELEFT) && leftRightNav && arrowPos > 0) {
            optionIcons[arrowPos].GetComponent<GUI_Ev_buttonPopUp>().UnhighlightButton();
            Navigate("left");
        } else if (ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVERIGHT) && leftRightNav && arrowPos < maxArrowPos) {
            Debug.Log("Arrow RIGHT");
            optionIcons[arrowPos].GetComponent<GUI_Ev_buttonPopUp>().UnhighlightButton();
            Navigate("right");
        } else if (ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVEUP) && upDownNav && arrowPos > 0) {
            optionIcons[arrowPos].GetComponent<GUI_Ev_buttonPopUp>().UnhighlightButton();
            Navigate("up");
        } else if (ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVEDOWN) && upDownNav && arrowPos < maxArrowPos) {
            optionIcons[arrowPos].GetComponent<GUI_Ev_buttonPopUp>().UnhighlightButton();
            Navigate("down");
        } else if (ControllerManager.Instance.GetKeyDown(INPUTACTION.CALENDAR)){
            ResetData(arrowPos);
        }


        selectionArrow.transform.position = new Vector2(optionIcons[arrowPos].transform.position.x-5f,optionIcons[arrowPos].transform.position.y);

	}
	
	public override void NavigateEffect(){
		optionIcons[arrowPos].GetComponent<GUI_Ev_buttonPopUp>().HighlightButton();

	}

	void LoadSave(){

		UserDataManager.Instance.SetSlot(arrowPos);
		StartCoroutine(UserDataManager.Instance.ReadAsync());

        // Switch to the Gameplay State.
        GameStateManager.Instance.PopAllStates();
        GameStateManager.Instance.PushState(typeof(GameplayState));

		fadeHelper.GetComponent<Ev_FadeHelper>().FadeToScene("1_1");

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
}

