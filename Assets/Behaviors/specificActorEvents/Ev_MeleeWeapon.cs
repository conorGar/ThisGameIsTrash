using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ev_MeleeWeapon : MonoBehaviour {

	tk2dSpriteAnimator anim;
	// Use this for initialization
	void Start () {
		anim = gameObject.GetComponent<tk2dSpriteAnimator>();


			
		/*if(anim.IsPlaying("stickUp")){
			if(GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[1] > 12 && GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[1] < 18){
				anim.Play("poleUp");
			}else if(GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[1] > 6 && GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[1] <= 12){
				anim.Play("clawUp");
			}else if(GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[1] >= 18){
				anim.Play("broomUp");
			}
		}else if(anim.IsPlaying("stickDown")){
			if(GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[1] > 12 && GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[1] < 18){
				anim.Play("poleDown");
			}else if(GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[1] > 6 && GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[1] <= 12){
				anim.Play("clawDown");
			}else if(GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[1] >= 18){
				anim.Play("broomDown");
			}
		}else{
			if(GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[1] > 12 && GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[1] < 18){
				anim.Play("poleSwing");
				gameObject.transform.position = new Vector2(transform.position.x + 1f, transform.position.y);

				//Debug.Log("P O L E");
			}else if(GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[1] > 6 && GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[1] <= 12){
				anim.Play("clawSwing");
				gameObject.transform.position = new Vector2(transform.position.x + 1f, transform.position.y);
				//Debug.Log(" C L A  W");
			}else if(GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[1] >= 18){
				anim.Play("broomSwing");
			}
		}*/
	}
	
	// Update is called once per frame
	void Update () {
		
	
	}
}
