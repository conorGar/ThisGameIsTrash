using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ev_HUD : MonoBehaviour {

	public void Create(GameObject go){
		go.SetActive(true);

		//couldnt get instantiate to work well with GUI effects, so for now
		//I just have popups disabled until needed


		//Instantiate(go,gameObject.transform.localPosition,Quaternion.identity);
		//go.transform.parent = this.gameObject.transform;
	}
}
