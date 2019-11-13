using UnityEngine;
using System.Collections;

public class DresserBehavior : SE_GlowWhenClose
{

	public GUI_SuitSelect suitSelectHud;

	void Start(){
		base.Start();
		if(suitSelectHud == null){
			suitSelectHud = GUIManager.Instance.suitSelectDisplay;
		}

	}




	public override void Activate ()
	{

		suitSelectHud.gameObject.SetActive(true);
		GameStateManager.Instance.PushState(typeof(PauseMenuState));
	}

	
	public override void GlowFunction(){

	}
	public override void StopGlowFunction(){

	}
}

