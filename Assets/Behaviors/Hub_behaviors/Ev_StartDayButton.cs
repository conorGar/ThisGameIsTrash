﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Ev_StartDayButton : SE_GlowWhenClose {

	public Ev_FadeHelper fader;
	public GameObject hubDescriptionPrompt;
	public GameObject demoEndFader;
	public GameObject demoEndText;

	public override void Activate(){
       

        if(GlobalVariableManager.Instance.DAY_NUMBER < 10){
			GameStateManager.Instance.PopAllStates();
        FriendManager.Instance.DisableAllFriends();

        // Save game data in case the player did some upgrades.
        // TODO: Maybe save after every store option if the player wants to do a few upgrades but not start a new day?
        UserDataManager.Instance.SetDirty();
			fader.FadeToScene("WorldSelect");
		}
	}

	public override void GlowFunction(){
		gameObject.GetComponent<tk2dSpriteAnimator>().Play();
		if(GlobalVariableManager.Instance.DAY_NUMBER < 10){
		hubDescriptionPrompt.GetComponent<TextMeshProUGUI>().text = "<color=#78FF32>Start Next Day</color>";
		}else{
		hubDescriptionPrompt.GetComponent<TextMeshProUGUI>().text = "Reached Demo Day Limit, go to<color=#78FF32>Polluted Peak!</color>";

		}
		hubDescriptionPrompt.SetActive(true);
	}
	public override void StopGlowFunction(){
		gameObject.GetComponent<tk2dSpriteAnimator>().Stop();
		hubDescriptionPrompt.SetActive(false);

	}

	IEnumerator DemoEnd(){
		demoEndFader.SetActive(true);
		yield return new WaitForSeconds(2f);
		demoEndText.SetActive(true);
		yield return new WaitForSeconds(2f);
		GameStateManager.Instance.PopAllStates();
		GameStateManager.Instance.PushState(typeof(TitleState));
		SoundManager.instance.backupMusicSource.Stop();

		fader.FadeToScene("TitleScreen2");

	}

}
