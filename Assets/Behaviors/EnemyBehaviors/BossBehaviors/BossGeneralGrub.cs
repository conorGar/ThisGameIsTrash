using UnityEngine;
using System.Collections;

public class BossGeneralGrub : Boss
{
	public GameObject tankParent;
	[HideInInspector]
	public GeneralGrubFriend grubFriend; //given at .Only used for final death dialog


	// Use this for initialization


	public override void BossDeathEvent(){
		Debug.Log("General Grub Death Event Activated");
		grubFriend.SetFriendState("GENERAL_FIGHT_END");
		grubFriend.GetComponent<ActivateDialogWhenClose>().canTalkTo = true;
		grubFriend.GetComponent<ActivateDialogWhenClose>().distanceThreshold =42;
		grubFriend.gameObject.SetActive(true);

        DeactivateHpDisplay();
        SoundManager.instance.musicSource.Stop();
        tankParent.SetActive(false);
    }
}

