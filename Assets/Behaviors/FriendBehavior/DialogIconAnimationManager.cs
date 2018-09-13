using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogIconAnimationManager : MonoBehaviour {

	public Animator myAnim;

	void Awake () {
		myAnim = gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SwitchAni(string triggerName){
		int triggerHash = Animator.StringToHash(triggerName);
		myAnim.SetTrigger(triggerHash);
		Debug.Log("Switch ani activated with: " + triggerName);
	}
}
