using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogIconAnimationManager : MonoBehaviour {

	Animator myAnim;

	void Start () {
		myAnim = gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SwitchAni(string triggerName){
		int triggerHash = Animator.StringToHash(triggerName);
		myAnim.SetTrigger(triggerHash);

	}
}
