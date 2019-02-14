using UnityEngine;
using System.Collections;

public class JimSpecialAbilityManager : MonoBehaviour
{

	bool chargingSpin;
	public GameObject spinAttack;


	int whichAbilityActivated;
	//public int trashCost = 1;

	 void Start ()
	{
	
	}
	
	void Update(){

		if(ControllerManager.Instance.GetKeyDown(INPUTACTION.SPECIAL) || ControllerManager.Instance.GetKeyDown(INPUTACTION.SPECIAL2)){
			Debug.Log("Special Button Pressed");
			//Link To The Trash
			if(ControllerManager.Instance.GetKeyDown(INPUTACTION.SPECIAL)){
				whichAbilityActivated = 0;
			}else if(ControllerManager.Instance.GetKeyDown(INPUTACTION.SPECIAL2)){
				whichAbilityActivated = 1;
			}
			if(GlobalVariableManager.Instance.EquippedAbilityPins[whichAbilityActivated] == PIN.LINKTOTRASH){
							Debug.Log("Link To The Trash Activate");
				if(!chargingSpin){
					chargingSpin = true;
					StartCoroutine("SpinAttack");
				}
			}

			//if(GlobalVariableManager.Instance.IsPinEquipped(PIN.A_TRASHBOMB) && GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[0] >= 1){
			if(GlobalVariableManager.Instance.EquippedAbilityPins[whichAbilityActivated] == PIN.A_TRASHBOMB&& GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[0] >= 1){
				Debug.Log("Trash Bomb activate");
				DropBomb();
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
		CamManager.Instance.mainCamEffects.ZoomInOut(1.4f,1f);
		//givenKey = INPUTACTION.SPECIAL;
		yield return new WaitForSeconds(.3f);
		spinAttack.SetActive(true);
		//DepleteTrash();
		chargingSpin = false;
		CamManager.Instance.mainCamEffects.ReturnFromCamEffect();
		yield return new WaitForSeconds(.5f);
		gameObject.GetComponent<MeleeAttack>().cantAttack = false;
		gameObject.GetComponent<MeleeAttack>().ReturnFromSwing();
		spinAttack.SetActive(false);
	}

	void DropBomb(){
		DepleteTrash(1);
		ObjectPool.Instance.GetPooledObject("TrashBomb", gameObject.transform.position);
	}



	void DepleteTrash(int trashCost){
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

