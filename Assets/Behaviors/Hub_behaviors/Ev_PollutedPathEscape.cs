﻿using UnityEngine;
using System.Collections;

public class Ev_PollutedPathEscape : SE_GlowWhenClose
{

	public Ev_FadeHelper fader;
	public bool atHubSide;
	//public GameObject player;

	int playerGoToStairs;

	public override void Activate(){
		//GameStateManager.Instance.PopAllStates();
		player.layer = 0; //set to layer that wont collide
		GameStateManager.Instance.PushState(typeof(DialogState));
        FriendManager.Instance.DisableAllFriends();
        if(atHubSide){
			fader.FadeToScene("PollutedPeakPath");
        }else{
			fader.FadeToScene("Hub");
		}
	}

	void Update(){
		base.Update();
		if(playerGoToStairs == 1){
			player.transform.position = Vector2.MoveTowards(player.transform.position,new Vector2(39.8f,13.7f),1*Time.deltaTime);
		}
	}
}
