using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ev_StartDayButton : SE_GlowWhenClose {

	public Ev_FadeHelper fader;

	public override void Activate(){
		fader.FadeToScene("WorldSelect");
	}
}
