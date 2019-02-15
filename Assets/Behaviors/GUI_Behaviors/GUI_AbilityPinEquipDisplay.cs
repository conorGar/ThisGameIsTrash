using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUI_AbilityPinEquipDisplay : MonoBehaviour
{
	int arrowPos;
	public PIN selectedPin;
	public List<GameObject> equippedAbilityPins = new List<GameObject>();
	public GameObject selectionArrow;
	public List<GameObject> arrowPositions = new List<GameObject>();
	PinDefinition pinData;
	// Use this for initialization
	void Start ()
	{
		
	}

	void OnEnable(){

		if(GlobalVariableManager.Instance.EquippedAbilityPins[0] != PIN.NONE){
			pinData = PinManager.Instance.GetPin(GlobalVariableManager.Instance.EquippedAbilityPins[0]);
			equippedAbilityPins[0].SetActive(true);
			equippedAbilityPins[0].GetComponent<tk2dSprite>().SetSprite(pinData.sprite);
		}else{
			equippedAbilityPins[0].SetActive(false);
		}


		if(GlobalVariableManager.Instance.EquippedAbilityPins[1] != PIN.NONE){
			pinData = PinManager.Instance.GetPin(GlobalVariableManager.Instance.EquippedAbilityPins[1]);
			equippedAbilityPins[1].SetActive(true);
			equippedAbilityPins[1].GetComponent<tk2dSprite>().SetSprite(pinData.sprite);
		}else{
			equippedAbilityPins[1].SetActive(false);
		}

		GameStateManager.Instance.PushState(typeof(MovieState));
	}

	// Update is called once per frame
	void Update ()
	{
		if(ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVELEFT) && arrowPos > 0){
			arrowPos --;
		}else if(ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVERIGHT) && arrowPos < 1){
			arrowPos ++;
		}else if(ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT)){
			//TODO: check if slot is filled already
			/*if(GlobalVariableManager.Instance.EquippedAbilityPins[arrowPos] != null){
				GlobalVariableManager.Instance.PP_STAT -= GlobalVariableManager.Instance.EquippedAbilityPins[arrowPos].GetData().ppValue; //give back pp from previously equipped pin
			}*/
			pinData = PinManager.Instance.GetPin(selectedPin);
			StartCoroutine("EquipSequence");

		}
		selectionArrow.transform.position = arrowPositions[arrowPos].transform.position;
	}


	IEnumerator EquipSequence(){
			GlobalVariableManager.Instance.EquippedAbilityPins[arrowPos] = selectedPin;

			//GlobalVariableManager.Instance.ACTIVE_ABILITY_PIN_ONE = selectedPin.GetData().Type;

			equippedAbilityPins[arrowPos].SetActive(true);
			equippedAbilityPins[arrowPos].GetComponent<tk2dSprite>().SetSprite(pinData.sprite);
			yield return new WaitForSeconds(.4f);
			GameStateManager.Instance.PopState();
	
			gameObject.SetActive(false);
	}


	void RemoveEquippedPin(){

	}
}

