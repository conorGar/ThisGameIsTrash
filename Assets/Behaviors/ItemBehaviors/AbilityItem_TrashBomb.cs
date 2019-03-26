using UnityEngine;
using System.Collections;

public class AbilityItem_TrashBomb : MonoBehaviour
{
	public GameObject explosion;
	public CircleCollider2D damageZone;
	// Use this for initialization
	void OnEnable(){
		damageZone.enabled = true;
		StartCoroutine("Timer");
	}

	IEnumerator Timer(){
		yield return new WaitForSeconds(3f);
		Explode();
		yield return new WaitForSeconds(.5f);
		damageZone.enabled = false;
		yield return new WaitForSeconds(.5f);
		explosion.SetActive(false);
		gameObject.SetActive(false);
	}

	void Explode(){
		explosion.GetComponent<tk2dSpriteAnimator>().Play();
		explosion.SetActive(true);
	}



}

