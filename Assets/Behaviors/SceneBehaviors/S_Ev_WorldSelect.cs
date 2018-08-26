using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Ev_WorldSelect : MonoBehaviour {

	public GameObject w1Icon;
	public GameObject w2Icon;
	public GameObject w3Icon;
	public GameObject w4Icon;


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
				if(GlobalVariableManager.Instance.MENU_SELECT_STAGE < 3){
					GlobalVariableManager.Instance.MENU_SELECT_STAGE++;
				}else{
					GlobalVariableManager.Instance.MENU_SELECT_STAGE = 0;
				}
				MoveIcons("right");
			}else if(ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVELEFT)
                  || ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKLEFT))
            {
				if(GlobalVariableManager.Instance.MENU_SELECT_STAGE > 0){
					GlobalVariableManager.Instance.MENU_SELECT_STAGE--;
				}else{
					GlobalVariableManager.Instance.MENU_SELECT_STAGE = 3;
				}
				MoveIcons("left");
			}else if(ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT))
            {
				Select();
			}
		}
	}

	void MoveIcons(string dir){
		w1Icon.GetComponent<Ev_WorldSelect>().Navigate(dir);
		w2Icon.GetComponent<Ev_WorldSelect>().Navigate(dir);
		w3Icon.GetComponent<Ev_WorldSelect>().Navigate(dir);
		w4Icon.GetComponent<Ev_WorldSelect>().Navigate(dir);
	}

	public void TriggeredMovement(int repeat){
		//for unlock sequence
		canNavigate = false;
		for(int i = 0; i<repeat; i++){
			if(GlobalVariableManager.Instance.MENU_SELECT_STAGE < 3){
					GlobalVariableManager.Instance.MENU_SELECT_STAGE++;
			}else{
					GlobalVariableManager.Instance.MENU_SELECT_STAGE = 0;
			}
			MoveIcons("right");
		}
	}

	void Select(){
		canNavigate = false;
		if(GlobalVariableManager.Instance.MENU_SELECT_STAGE == 0){
			w1Icon.GetComponent<Ev_WorldSelect>().Select();
		}else if(GlobalVariableManager.Instance.MENU_SELECT_STAGE == 1){
			w2Icon.GetComponent<Ev_WorldSelect>().Select();
		}else if(GlobalVariableManager.Instance.MENU_SELECT_STAGE == 2){
			w3Icon.GetComponent<Ev_WorldSelect>().Select();
		}else if(GlobalVariableManager.Instance.MENU_SELECT_STAGE == 4){
			w4Icon.GetComponent<Ev_WorldSelect>().Select();
		}

	}
	public void SetNavigate(bool b){//created for returning navigation after world unlock sequence
		canNavigate = b;
	}
}
