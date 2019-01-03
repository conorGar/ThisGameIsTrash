using UnityEngine;
using System.Collections;

public class Ev_Enemy_SpearMole : FollowPlayerAfterNotice
{

	public GameObject spearCollision;
	public bool armoredVersion;
	ParticleSystem.EmissionModule myEmission;

	void OnEnable(){
		if(!armoredVersion){
			spearCollision.SetActive(false);
		}
		myEmission =gameObject.GetComponent<FollowPlayer>().chasePS.emission;
		myEmission.rateOverTime = 5f;
		base.OnEnable();
	}

	public void Update(){
		base.Update();

	}


	protected override void NoticePlayerEvent(){
		base.NoticePlayerEvent();
		spearCollision.SetActive(true);
		this.enabled = false;
	}
}

