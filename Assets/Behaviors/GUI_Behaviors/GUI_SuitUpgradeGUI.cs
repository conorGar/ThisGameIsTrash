using UnityEngine;
using System.Collections;

public class GUI_SuitUpgradeGUI : GUI_MenuBase
{


	/** How this works:

	SuitUpgradeGUI handles up/down naviagation between different SuitUpgradeSuit

	SuitUpgradeSuit handles left/right navigation between different SuitUpgradeBox's

	SuitUpgradeBox handles info display and purchasing
	**/

	public GUI_SuitUpgradeSuitOption[] mySuitOptions; 
	 int upDownArrowPos = 0;

	// Use this for initialization
	void OnEnable(){

        GameStateManager.Instance.PushState(typeof(ShopState));
        CamManager.Instance.mainCamPostProcessor.profile = blur;
	}

	void OnDisable()
    {
        CamManager.Instance.mainCamPostProcessor.profile = null;
        GameStateManager.Instance.PopState();
    }

	// Update is called once per frame
	void Update ()
	{
		if (ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVEUP) && arrowPos > 0) {
			mySuitOptions[arrowPos].Unhighlight();
			Debug.Log("Main GUI arrow move" + arrowPos);
			Navigate("up");
		}else if(ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVEDOWN) && arrowPos <= mySuitOptions.Length){
			mySuitOptions[arrowPos].Unhighlight();
			Navigate("down");
			Debug.Log("Main GUI arrow move" + arrowPos);

		}else if(ControllerManager.Instance.GetKeyDown(INPUTACTION.PAUSE)){
			Close();
		}
		mySuitOptions[arrowPos].Higlight();

	}

	void Close(){
		gameObject.SetActive(false);
		//Runs 'OnDisable' above
	}
}

