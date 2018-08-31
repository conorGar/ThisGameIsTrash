using System.Collections;
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
		gameObject.GetComponent<SpecialEffectsBehavior>().StartCoroutine("Grow",1f);
		Debug.Log("~~~~~~~~Hit Star Created!~~~~~~~~"); // Removed the < and > here so it doesn't come up on merges :p
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnEnable(){
		gameObject.GetComponent<SpecialEffectsBehavior>().StopCoroutine("Shrink");
		transform.localScale = new Vector3(1,1,1);
		gameObject.GetComponent<SpecialEffectsBehavior>().SetGrowValues(.1f,.5f);
		gameObject.GetComponent<SpecialEffectsBehavior>().StartCoroutine("Grow",1f);
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
