﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ev_HitStars : MonoBehaviour {
	public Sprite TwoDamage;
	public Sprite ThreeDamage;
	public Sprite FourDamage;
	public Sprite FiveDamage;
	public Sprite SixDamage;

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<SpecialEffectsBehavior>().SetGrowValues(.1f,.5f);
		gameObject.GetComponent<SpecialEffectsBehavior>().StartCoroutine("Shrink",1f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ShowProperDamage(int damageDealt){
		if(damageDealt == 2){
			this.gameObject.GetComponent<SpriteRenderer>().sprite = TwoDamage;
		}else if(damageDealt == 3){
			this.gameObject.GetComponent<SpriteRenderer>().sprite = ThreeDamage;
		}else if(damageDealt == 4){
			this.gameObject.GetComponent<SpriteRenderer>().sprite = FourDamage;
		}else if(damageDealt == 5){
			this.gameObject.GetComponent<SpriteRenderer>().sprite = FiveDamage;
		}else if(damageDealt == 6){
			this.gameObject.GetComponent<SpriteRenderer>().sprite = SixDamage;
		}
	}
}
