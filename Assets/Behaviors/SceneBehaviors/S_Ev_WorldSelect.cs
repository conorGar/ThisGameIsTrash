using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Ev_WorldSelect : MonoBehaviour {

	public GameObject w1Icon;
	public GameObject w2Icon;
	public GameObject w3Icon;
	public GameObject w4Icon;

	public AudioClip navRight;
	public AudioClip navLeft;

	int arrowPos;
	bool canNavigate = true;
	/*
		handles the naviagation. Every time navigate, checks what the front icon
		is based on the menuselectstage value. For every Icon, activates Navigate(direction)
		in 'Ev_worldSelect'. Which just moves the icons and grows/shrinks them.
	*/


	void Start () {
		
	}

	void Update () {
		if(canNavigate){
			if(ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVERIGHT)
            || ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKRIGHT))
            {
            	SoundManager.instance.PlaySingle(navRight);
				if(arrowPos < 3){
					arrowPos++;
				}else{
					arrowPos = 0;
				}
				MoveIcons("right",arrowPos);
			}else if(ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVELEFT)
                  || ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKLEFT))
            {
            	SoundManager.instance.PlaySingle(navLeft);
				if(arrowPos > 0){
					arrowPos--;
				}else{
					arrowPos =  3;
				}
				MoveIcons("left",arrowPos);
			}else if(ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT))
            {
				Select();
			}
		}
	}

	void MoveIcons(string dir, int thisArrowPos){
		w1Icon.GetComponent<Ev_WorldSelect>().Navigate(dir,thisArrowPos);
		w2Icon.GetComponent<Ev_WorldSelect>().Navigate(dir,thisArrowPos);
		w3Icon.GetComponent<Ev_WorldSelect>().Navigate(dir,thisArrowPos);
		w4Icon.GetComponent<Ev_WorldSelect>().Navigate(dir,thisArrowPos);
	}

	public void TriggeredMovement(int repeat){
		//for unlock sequence
		canNavigate = false;
		for(int i = 0; i<repeat; i++){
			if(arrowPos < 3){
					arrowPos++;
			}else{
					arrowPos = 0;
			}
			MoveIcons("right",arrowPos);
		}
	}

	void Select(){
		canNavigate = false;
		if(arrowPos == 0){
			w1Icon.GetComponent<Ev_WorldSelect>().Select();
		}else if(arrowPos == 1){
			w2Icon.GetComponent<Ev_WorldSelect>().Select();
		}else if(arrowPos == 2){
			w3Icon.GetComponent<Ev_WorldSelect>().Select();
		}else if(arrowPos == 4){
			w4Icon.GetComponent<Ev_WorldSelect>().Select();
		}

	}
	public void SetNavigate(bool b){//created for returning navigation after world unlock sequence
		canNavigate = b;
	}
}
