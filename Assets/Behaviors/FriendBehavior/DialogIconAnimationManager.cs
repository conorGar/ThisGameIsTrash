using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogIconAnimationManager : MonoBehaviour {
    public string ID = "icon_id";
	public Animator myAnim;

	void Awake () {
		myAnim = gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public virtual void SwitchAni(string triggerName){
		myAnim.StopPlayback();
		int triggerHash = Animator.StringToHash(triggerName);
		myAnim.SetTrigger(triggerHash);
		myAnim.Play(triggerName);
		Debug.Log("Switch ani activated with: " + triggerName);
	}

    public virtual void EnableAnimator()
    {
        myAnim.enabled = true;
    }

    public virtual void DisableAnimator()
    {
        myAnim.enabled = false;
    }
}
