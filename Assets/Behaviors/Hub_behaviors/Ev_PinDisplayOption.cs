using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ev_PinDisplayOption : SE_GlowWhenClose {

	//public Ev_FadeHelper fader;
	public GameObject pinEquipHUD;
	// Use this for initialization


	public override void Activate ()
	{
		pinEquipHUD.SetActive(true);
		this.enabled = false;
	}
}
