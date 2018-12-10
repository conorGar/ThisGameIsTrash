﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Ev_StartDayButton : SE_GlowWhenClose {

	public Ev_FadeHelper fader;
	public GameObject hubDescriptionPrompt;


	public override void Activate(){
        GameStateManager.Instance.PopAllStates();
        FriendManager.Instance.DisableAllFriends();

        // Save game data in case the player did some upgrades.
        // TODO: Maybe save after every store option if the player wants to do a few upgrades but not start a new day?
        UserDataManager.Instance.SetDirty();

		fader.FadeToScene("WorldSelect");
	}

	public override void GlowFunction(){
		gameObject.GetComponent<tk2dSpriteAnimator>().Play();
		hubDescriptionPrompt.GetComponent<TextMeshProUGUI>().text = "<color=#78FF32>Start Next Day</color>";
		hubDescriptionPrompt.SetActive(true);
	}
	public override void StopGlowFunction(){
		gameObject.GetComponent<tk2dSpriteAnimator>().Stop();
		hubDescriptionPrompt.SetActive(false);

	}

}
