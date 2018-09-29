using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ev_PPDisplay : MonoBehaviour {

	Transform myTransform;
	void Start () {
		myTransform = gameObject.transform;
	}
	
	public void SetDisplayedIcons(int value){
		value += 1; // +1 because of 'PP:' text object
		for(int i = 0; i< myTransform.childCount; i++){
            if (i < value)
                myTransform.GetChild(i).gameObject.SetActive(true); 
            else
                myTransform.GetChild(i).gameObject.SetActive(false);
		}
	}

	public void Clear(){
		for(int i = 0; i<myTransform.childCount;i++){
			transform.GetChild(i).gameObject.SetActive(false);
		}
	}
}
