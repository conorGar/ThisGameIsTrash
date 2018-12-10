using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Ev_PinDisplayOption : SE_GlowWhenClose {

	//public Ev_FadeHelper fader;
	public GameObject pinEquipHUD;
	public GameObject hubDescriptionPrompt;



	public override void Activate ()
	{
		player.GetComponent<EightWayMovement>().enabled = false;
		pinEquipHUD.SetActive(true);
		this.enabled = false;
	}


	public override void GlowFunction(){
		
		hubDescriptionPrompt.GetComponent<TextMeshProUGUI>().text = "<color=#78FF32>Equip Pins</color>";
		hubDescriptionPrompt.SetActive(true);
	}
	public override void StopGlowFunction(){
		hubDescriptionPrompt.SetActive(false);

	}

}
