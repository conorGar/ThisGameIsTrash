using UnityEngine;
using System.Collections;

public class BossBeetleSteve : Boss
{
	public GeneralGrubFriend friend; // set by "GeneralGrubFriend"
	public GameObject bossBlockades;


	// Use this for initialization

	void OnEnable(){
		bossBlockades.SetActive(true);
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
		friend.SetFriendState("CICADA_SAM_INTRO");
		bossBlockades.SetActive(false);
		CamManager.Instance.mainCamEffects.ReturnFromCamEffect();
		gameObject.SetActive(false);
	}
}

