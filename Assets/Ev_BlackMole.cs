using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ev_BlackMole : MonoBehaviour {

	tk2dSpriteAnimator myAnim;
	// Use this for initialization
	void Start () {
		myAnim = gameObject.GetComponent<tk2dSpriteAnimator>();
	}
	
	// Update is called once per frame
	void Update () {
		if(myAnim.CurrentClip.name == "throwL" && myAnim.CurrentFrame == 30){
			myAnim.Play("idle");
		}else if(myAnim.CurrentClip.name == "popUp" && myAnim.CurrentFrame == 13){
			myAnim.Play("idle");
		}
	}
}
