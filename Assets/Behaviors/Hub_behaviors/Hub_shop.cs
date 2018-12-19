using UnityEngine;
using System.Collections;

public class Hub_shop : SE_GlowWhenClose
{
	public GameObject tutorial;




	public override void Activate(){
	 	tutorial.SetActive(true);
		tutorial.GetComponent<GUI_TutPopup>().SetData("Shop");
		GameStateManager.Instance.PushState(typeof(DialogState));
	}
}

