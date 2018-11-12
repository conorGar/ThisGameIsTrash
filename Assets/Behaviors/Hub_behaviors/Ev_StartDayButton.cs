using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ev_StartDayButton : SE_GlowWhenClose {

	public Ev_FadeHelper fader;

	public override void Activate(){
        GameStateManager.Instance.PopAllStates();
        FriendManager.Instance.DisableAllFriends();

        // Save game data in case the player did some upgrades.
        // TODO: Maybe save after every store option if the player wants to do a few upgrades but not start a new day?
        UserDataManager.Instance.SetDirty();

		fader.FadeToScene("WorldSelect");
	}


}
