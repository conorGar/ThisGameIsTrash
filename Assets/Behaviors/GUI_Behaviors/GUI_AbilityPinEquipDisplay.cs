using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUI_AbilityPinEquipDisplay : MonoBehaviour
{
	int arrowPos;
	public Ev_PinBehavior selectedPin;
	public List<GameObject> equippedAbilityPins = new List<GameObject>();
	public GameObject selectionArrow;
	public List<GameObject> arrowPositions = new List<GameObject>();

	// Use this for initialization
	void Start ()
	{
		for (int i = 0; i < GlobalVariableManager.Instance.EquippedAbilityPins.Count; i++) {
			if(GlobalVariableManager.Instance.EquippedAbilityPins[i] != null){
				equippedAbilityPins[i].SetActive(true);
				var definition = GlobalVariableManager.Instance.EquippedAbilityPins[i];


            	equippedAbilityPins[i].GetComponent<tk2dSprite>().SetSprite(definition.GetData().sprite);
			}
		
            
        }
	}

	void OnEnable(){
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
			StartCoroutine("EquipSequence");

		}
		selectionArrow.transform.position = arrowPositions[arrowPos].transform.position;
	}


	IEnumerator EquipSequence(){
			GlobalVariableManager.Instance.EquippedAbilityPins[arrowPos] = selectedPin;

			equippedAbilityPins[arrowPos].SetActive(true);
			equippedAbilityPins[arrowPos].GetComponent<tk2dSprite>().SetSprite(selectedPin.GetData().sprite);
			yield return new WaitForSeconds(.4f);
			GameStateManager.Instance.PopState();
	
			gameObject.SetActive(false);
	}


	void RemoveEquippedPin(){

	}
}

