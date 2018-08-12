using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PinFunctionsManager : MonoBehaviour {

	/// <summary>
	/// Keeps a method for each pins effect that is called when needed by whatever script.
	/// </summary>
	GameObject objectToKill;
	float delay;
	int effectDisplayOverride;


	public GameObject pinEffectDisplayHUD;
	public GameObject pinObjectPool;
	public GameObject devil;

	public void HeroOfGrime(int direction, Vector3 spawnPos){//right = 1, left = 2, up = 3, down = 4
		GameObject beam = pinObjectPool.GetComponent<ObjectPool>().GetPooledObject("pObj_hogBeam",spawnPos);
		if(direction == 1){
			beam.GetComponent<Rigidbody2D>().velocity = new Vector2(15f,0f);
			beam.transform.localScale = new Vector3(.87f,beam.transform.localScale.y,beam.transform.localScale.z);

		}else if(direction == 2){
			beam.GetComponent<Rigidbody2D>().velocity = new Vector2(-15f,0f);
			beam.transform.localScale = new Vector3(-.87f,beam.transform.localScale.y,beam.transform.localScale.z);
		}else if(direction == 3){
			beam.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,15f);
			beam.transform.Rotate(0f,0f,90f);
		}else if(direction == 4){
			beam.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,-15f);
			beam.transform.Rotate(0f,0f,-90f);
		}
		SetDelay(1f);
		StartCoroutine("KillObject",beam);
	}

	public void DevilsDeal(){
		Instantiate(devil,gameObject.transform);

	}

	public void HungryForMore(){
		WorldManager.Instance.amountTrashHere += 5;
		StartCoroutine("DisplayEffectHud",PinManager.Instance.GetPin(PIN.HUNGRYFORMORE).sprite);
	}

	public IEnumerator DisplayEffectHud(Sprite pinIcon){
		if(pinEffectDisplayHUD.activeInHierarchy){//if just activated by previous pin
			pinEffectDisplayHUD.SetActive(false);
			CancelInvoke();
			StartCoroutine("DisplayEffectHud",pinIcon);
		}
		pinEffectDisplayHUD.SetActive(true);
		pinEffectDisplayHUD.GetComponent<Image>().sprite = pinIcon;
		yield return new WaitForSeconds(1.5f);
		pinEffectDisplayHUD.GetComponent<SpecialEffectsBehavior>().SmoothMovementToPoint(pinEffectDisplayHUD.transform.position.x,pinEffectDisplayHUD.transform.position.y -10f,.5f);
		yield return new WaitForSeconds(.5f);
		pinEffectDisplayHUD.SetActive(false);
	}

	void SetDelay(float thisDelay){
		delay = thisDelay;
	}


	IEnumerator KillObject(GameObject objToKill){
		
		yield return new WaitForSeconds(delay);

		objToKill.SetActive(false);
	}
	

}
