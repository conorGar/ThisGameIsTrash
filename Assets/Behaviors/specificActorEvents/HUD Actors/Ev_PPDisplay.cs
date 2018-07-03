using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ev_PPDisplay : MonoBehaviour {

	Transform myTransform;
	void Start () {
		myTransform = gameObject.transform;
	}
	
	public void SetDisplayedIcons(int value){
		for(int i = 0; i<value;i++){
			myTransform.GetChild(i).gameObject.SetActive(true);
		}
	}

	public void Clear(){
		for(int i = 0; i<myTransform.childCount;i++){
			transform.GetChild(i).gameObject.SetActive(false);
		}
	}
}
