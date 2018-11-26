using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogIconAnimationManager : MonoBehaviour {
    public string ID = "icon_id";
	public Animator myAnim;

    [SerializeField] // to show in inspector!
    private bool isTalking;

	// Update is called once per frame
	void Update () {
        
    }

    public virtual void SetTalking(bool talking)
    {
        isTalking = talking;
        myAnim.SetBool("IsTalking", isTalking);
    }

    public virtual void Slide()
    {
        myAnim.SetTrigger("Slide");
    }

    public virtual void SlideBack()
    {
        myAnim.SetTrigger("SlideBack");
    }

    public virtual void SetAnimTrigger(string action)
    {
        myAnim.SetTrigger(action);
    }

    public virtual void SetAnimBool(string key, bool value)
    {
        myAnim.SetBool(key, value);
    }

    public virtual void ResetIconPositionsOnScreen()
    {
        // Don't do anything on single icon animation managers.
    }
}
