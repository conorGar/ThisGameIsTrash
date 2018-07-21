using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ev_DeathGhost : MonoBehaviour {
	SpecialEffectsBehavior myEffectBehavior;
	// Use this for initialization
	void Start () {
		myEffectBehavior = gameObject.GetComponent<SpecialEffectsBehavior>();
		myEffectBehavior.SetFadeVariables(.6f,.4f);
		myEffectBehavior.StartCoroutine("FadeOut");
		myEffectBehavior.SmoothMovementToPoint(gameObject.transform.position.x,gameObject.transform.position.y +5f,1f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
