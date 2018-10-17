﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Ev_Hub : MonoBehaviour {

	public AudioClip hubMusic;

	// Use this for initialization
	void Start () {
		UserDataManager.Instance.SetDirty();
		SoundManager.instance.musicSource.clip = hubMusic;
		SoundManager.instance.musicSource.volume = GlobalVariableManager.Instance.MASTER_MUSIC_VOL;
		SoundManager.instance.musicSource.Play();
		GlobalVariableManager.Instance.MENU_SELECT_STAGE = 2;
		GlobalVariableManager.Instance.ARROW_POSITION = 0;
		//create jim
		GlobalVariableManager.Instance.ROOM_NUM = 101;
		GlobalVariableManager.Instance.PLAYER_CAN_MOVE = true;

        GlobalVariableManager.Instance.LARGE_TRASH_LIST.Clear();

		


		//disable melee swing at hub
		GameObject.Find("Jim").GetComponent<MeleeAttack>().enabled = false;

        //LargeTrashSpawn();
        GameStateManager.Instance.PopAllStates();
        GameStateManager.Instance.PushState(typeof(GameplayState));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void LargeTrashSpawn(){

	//all large trash is already placed in hub but deactivated. Maybe each large trash in hub(hub large trash
	//should  maybe be its own object..) has a number as its name. Com runs through all positions in
	//unlocked string and if = 'o' then searches scene for gameobject with a name = the index it's currently
	//at and enables it...
        
        for (int i=0; i < sizeof(LARGEGARBAGE); ++i)
        {
            LARGEGARBAGE largeGarbageType = LargeGarbage.ByIndex(i);
            if ((GlobalVariableManager.Instance.LARGE_GARBAGE_DISCOVERED & largeGarbageType) == largeGarbageType)
            {
                GameObject.Find(i.ToString()).SetActive(true);
            }
        }
	}
}
