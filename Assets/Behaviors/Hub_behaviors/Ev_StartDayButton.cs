using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ev_StartDayButton : SE_GlowWhenClose {

	public Ev_FadeHelper fader;

	public override void Activate(){
        GameStateManager.Instance.PopAllStates();
        FriendManager.Instance.DisableAllFriends();
		fader.FadeToScene("WorldSelect");
	}


}
