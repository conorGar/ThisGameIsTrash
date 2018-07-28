using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Ev_StartScreen : MonoBehaviour {

	// Use this for initialization

	void Awake(){

	}
	void Start () {

        // TODO: Waste Warrior stuff?  How do we handle this?
		/*if(GlobalVariableManager.Instance.pinsEquipped.Count < 2){
			GlobalVariableManager.Instance.pinsEquipped[19] = 2;
		}*/

		if(GlobalVariableManager.Instance.CALENDAR.Count < 2){
			for(int i = 0; i < 30; i++){
				GlobalVariableManager.Instance.CALENDAR.Add("a");
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
