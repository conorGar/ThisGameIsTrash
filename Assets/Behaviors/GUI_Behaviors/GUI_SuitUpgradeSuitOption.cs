using UnityEngine;
using System.Collections;

public class GUI_SuitUpgradeSuitOption : GUI_MenuBase
{
	public GUI_SuitUpgradeBox[] myOptions;
	bool highlighted;


	
	// Update is called once per frame
	void Update ()
	{
		if(highlighted){
			if (ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVELEFT) && arrowPos > 0) {
				Debug.Log(arrowPos);
				myOptions[arrowPos].Unhighlight();
				Navigate("left");
			}else if(ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVERIGHT) && arrowPos < myOptions.Length -1){
				myOptions[arrowPos].Unhighlight();
				Debug.Log(arrowPos);

				Navigate("right");
			}
			myOptions[arrowPos].Higlight();
		}
	}

	public void Higlight(){
		for(int i = 0; i < myOptions.Length; i++){
			myOptions[i].gameObject.SetActive(true);
		}
		gameObject.GetComponent<RectTransform>().localScale = new Vector2(1,1);
		highlighted =true;
	}

	public void Unhighlight(){
		for(int i = 0; i < myOptions.Length; i++){
			myOptions[i].gameObject.SetActive(false);
		}
		gameObject.GetComponent<RectTransform>().localScale = new Vector2(.8f,.8f);
		highlighted = false;
	}
}

