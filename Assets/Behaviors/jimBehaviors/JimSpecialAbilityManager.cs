﻿using UnityEngine;
using System.Collections;

public class JimSpecialAbilityManager : MonoBehaviour
{

	bool chargingSpin;
	public GameObject spinAttack;

	public int trashCost = 1;

	 void Start ()
	{
	
	}
	
	void Update(){

		if(ControllerManager.Instance.GetKeyDown(INPUTACTION.SPECIAL)){
			Debug.Log("Special Button Pressed");
			//Link To The Trash

			if(GlobalVariableManager.Instance.IsPinEquipped(PIN.LINKTOTRASH) && GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[0] >= trashCost){
				Debug.Log("Link To The Trash Activate");
				if(!chargingSpin){
					chargingSpin = true;
					StartCoroutine("SpinAttack");
				}
			}


		}
	


		if(chargingSpin && ControllerManager.Instance.GetKeyUp(INPUTACTION.SPECIAL)){
			chargingSpin = false;
			StopCoroutine("SpinAttack");
			gameObject.GetComponent<MeleeAttack>().cantAttack = false;
			gameObject.GetComponent<MeleeAttack>().ReturnFromSwing();

		}

	}


	public IEnumerator SpinAttack(){ //called at Swing() in 'MeleeAttack.cs'
		Debug.Log("Spin Attack Coroutine Started");
		gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		gameObject.GetComponent<JimAnimationManager>().PlayAnimation("spinAttack",true);

		//givenKey = INPUTACTION.SPECIAL;
		yield return new WaitForSeconds(.3f);
		spinAttack.SetActive(true);
		DepleteTrash();
		chargingSpin = false;
		yield return new WaitForSeconds(.5f);
		gameObject.GetComponent<MeleeAttack>().cantAttack = false;
		gameObject.GetComponent<MeleeAttack>().ReturnFromSwing();
		spinAttack.SetActive(false);
	}


	void DepleteTrash(){
		CancelInvoke("LostTrashDeactivate");
		GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[0] -= trashCost;
		GUIManager.Instance.lostTrash.text = "-" + trashCost.ToString();
		GUIManager.Instance.lostTrash.gameObject.SetActive(true);
		GUIManager.Instance.TrashCollectedDisplayGameplay.UpdateDisplay(GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[0]);
		Invoke("LostTrashDeactivate",1f);
	}

	void LostTrashDeactivate(){
		GUIManager.Instance.lostTrash.gameObject.SetActive(false);
	}
}
