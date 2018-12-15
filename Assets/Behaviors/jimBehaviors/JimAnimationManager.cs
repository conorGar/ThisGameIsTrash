using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JimAnimationManager : MonoBehaviour {

	tk2dSpriteAnimator myAnimator;
	public GameObject aniHelper;// for motion tween animations
	bool clipOverride;

	void Start () {
		myAnimator = gameObject.GetComponent<tk2dSpriteAnimator>();
	}

	void Update(){

		if(clipOverride){
			if(myAnimator.CurrentFrame == myAnimator.CurrentClip.frames.Length){//once the clip is finished
				gameObject.GetComponent<EightWayMovement>().clipOverride = false;
				clipOverride = false;
			}
		}

	}


	public void PlayAnimation(string clip, bool overrideCurrentClip){
		
//		Debug.Log("Clip Play activated with clip:" + clip);
		myAnimator.Play(clip);

		if(overrideCurrentClip != false){

			gameObject.GetComponent<EightWayMovement>().clipOverride = true;
			clipOverride = true;
			//if this doesnt work look into triggers for tk2d animators
			/*if(myAnimator.CurrentClip.name == "ani_jimIdle"||myAnimator.CurrentClip.name == "ani_jimIdleDown" ||myAnimator.CurrentClip.name == "ani_jimIdleUP"){
				myAnimator.Play(clip);
			}else{
				clipToSwitchTo = clip;
				StartCoroutine("NextAnimation",myAnimator.ClipTimeSeconds);
			}*/

		}
	}

	public void PlayExcitedJump(){
		//aniHelper.transform.parent = null;
		//gameObject.transform.parent = aniHelper.transform;
		gameObject.GetComponent<tk2dSpriteAnimator>().Play("excitedJump");
		//gameObject.GetComponent<Animator>().enabled = true;
		//gameObject.GetComponent<Animator>().Play("JimAni_excitedJump",-1,0f);

	}

	public void StopTweenAnimation(){
		gameObject.transform.parent = null;
		aniHelper.transform.parent  = this.transform;
		//gameObject.GetComponent<Animator>().StopPlayback();
		//gameObject.GetComponent<Animator>().enabled = false;
	}

	/*IEnumerator NextAnimation(float duration){
		yield return new WaitForSeconds(duration);
		myAnimator.Play(clipToSwitchTo);
	}*/
}
