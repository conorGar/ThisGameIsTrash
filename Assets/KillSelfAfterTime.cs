using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillSelfAfterTime : MonoBehaviour {
	public float timeUntilDeath;
	public bool deactivateInsteadOfKill;
	public bool playSoundAtStart;
	public bool smokePuffAtEnd;
	public AudioClip mySound;
	public bool startCountdownImmediately = true;
	// Use this for initialization
	void Start () {
		//Invoke("Kill",timeUntilDeath);

	}

	
	public IEnumerator Kill(){
		yield return new WaitForSeconds(timeUntilDeath);
		if(deactivateInsteadOfKill){
			if(smokePuffAtEnd)
				ObjectPool.Instance.GetPooledObject("effect_landingSmoke",transform.position);
			ObjectPool.Instance.ReturnPooledObject(this.gameObject);
		}else{
			Destroy(gameObject);
		}
	}

	void OnEnable(){
		//Invoke("Kill",timeUntilDeath);
		StopAllCoroutines();
		StartCoroutine("Kill");
		if(playSoundAtStart){
			SoundManager.instance.PlaySingle(mySound);
		}
	}
}
