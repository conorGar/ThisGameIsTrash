using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class GUI_AbilityPinsHUD : MonoBehaviour
{
	public List<GameObject> equippedAbilityPins = new List<GameObject>();
	public TextMeshProUGUI pinOneCost;
	public TextMeshProUGUI pinTwoCost;
	public PinConfig pinConfig;
	// Use this for initialization
	void Start ()
	{
		for (int i = 0; i < GlobalVariableManager.Instance.EquippedAbilityPins.Count; i++) {
			if(GlobalVariableManager.Instance.EquippedAbilityPins[i] != null){
				equippedAbilityPins[i].SetActive(true);
			}
			var definition = GlobalVariableManager.Instance.EquippedAbilityPins[i];


            equippedAbilityPins[i].GetComponent<tk2dSprite>().SetSprite(definition.GetData().sprite);
            
        }
		/*for (int i = 0; i < pinConfig.pinList.Count; i++) {
            var definition = pinConfig.pinList[i];

            if (definition.abilityPin == true && GlobalVariableManager.Instance.IsPinEquipped((PIN)definition.pinValue)) {
                count += definition.ppValue;
            }
        }
        */
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}

