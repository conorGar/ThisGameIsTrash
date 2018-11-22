using UnityEngine;
using System.Collections;

public class Ev_Enemy_SpearMole : FollowPlayerAfterNotice
{

	public GameObject spearCollision;

	ParticleSystem.EmissionModule myEmission;

	void OnEnable(){
		spearCollision.SetActive(false);
		myEmission =gameObject.GetComponent<FollowPlayer>().chasePS.emission;
		myEmission.rateOverTime = 5f;
	}

	public void Update(){
		base.Update();

	}


	protected override void NoticePlayerEvent(){
		spearCollision.SetActive(true);
		this.enabled = false;
	}
}

