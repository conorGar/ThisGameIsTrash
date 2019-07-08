using UnityEngine;
using System.Collections;

public class BossBeetleSteve : Boss
{
	public GeneralGrubFriend friend; // set by "GeneralGrubFriend"
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public override void ActivateBoss ()
	{
		ActivateHpDisplay();
		//do nothing
	}

	public override void BossDeathEvent(){
		friend.SetFriendState("CICADA_SAM");
		CamManager.Instance.mainCamEffects.ReturnFromCamEffect();
		gameObject.SetActive(false);
	}
}

