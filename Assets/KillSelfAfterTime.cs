using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillSelfAfterTime : MonoBehaviour {
	public float timeUntilDeath;
	public bool deactivateInsteadOfKill;
	public bool playSoundAtStart;
	public AudioClip mySound;
	// Use this for initialization
	void Start () {
		Invoke("Kill",timeUntilDeath);

	}

	
	void Kill(){
		if(deactivateInsteadOfKill){
			this.gameObject.SetActive(false);
		}else{
			Destroy(gameObject);
		}
	}

	void OnEnable(){
		Invoke("Kill",timeUntilDeath);
		if(playSoundAtStart){
			SoundManager.instance.PlaySingle(mySound);
		}
	}
}
