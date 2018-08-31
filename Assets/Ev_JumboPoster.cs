using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ev_JumboPoster : MonoBehaviour {
	public Sprite pupFiction;
	public Sprite pawShankRedemption;
	public Sprite citizenDane;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetSprite(string spriteName){
		if(spriteName == "Pup_Fiction"){
			gameObject.GetComponent<SpriteRenderer>().sprite = pupFiction;
		}else if(spriteName == "The_Pawshank_Redemption"){
			gameObject.GetComponent<SpriteRenderer>().sprite = pawShankRedemption;
		}else if(spriteName == "Citizen_Dane"){
			gameObject.GetComponent<SpriteRenderer>().sprite = citizenDane;
		}
	}
}
