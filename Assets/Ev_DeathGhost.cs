using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ev_DeathGhost : MonoBehaviour {
	SpecialEffectsBehavior myEffectBehavior;
	// Use this for initialization
	public void OnSpawn () {
		myEffectBehavior = gameObject.GetComponent<SpecialEffectsBehavior>();
		myEffectBehavior.SetFadeVariables(.6f,.4f);
		gameObject.GetComponent<tk2dSprite>().color = new Color(1,1,1,1); // return from a previous fade

		myEffectBehavior.StartCoroutine("FadeOut");
		myEffectBehavior.SmoothMovementToPoint(gameObject.transform.position.x,gameObject.transform.position.y +5f,1f);
		Invoke("Return",1.5f);
	}
	
	// Update is called once per frame
	void Return () {
		myEffectBehavior.StopMovement();
		ObjectPool.Instance.ReturnPooledObject(this.gameObject);
	}
}
