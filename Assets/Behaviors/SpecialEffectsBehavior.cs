using UnityEngine;
using System.Collections;

public class SpecialEffectsBehavior : MonoBehaviour
{

	//-----Variables for smooth movement to point -------//
	bool smoothMovement = false;
	Vector3 velocity;
	Vector3 targetPos;
	float timeOfMovement;
	//-----------------------------------//

	float fadeDelay;
	float fadeDuration;
	bool shake;
	float shakeIntensity;
	float startYShake;

	void Update(){
		if(smoothMovement){
			transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity,timeOfMovement);

		}

		if(shake){
			Debug.Log("shaking");
			transform.position = new Vector2(transform.position.x,startYShake + Random.Range(shakeIntensity*-1,shakeIntensity));
		}
	}
	public void SetFadeVariables(float afterTime, float fd){
		fadeDelay = afterTime;
		fadeDuration = fd;
	}
	public IEnumerator FadeOut(){
		float counter = 0;
		SpriteRenderer myRenderer = gameObject.GetComponent<SpriteRenderer>();
		Color myColor = myRenderer.color;
		while(counter < fadeDuration){
			counter += Time.deltaTime;
			float alpha = Mathf.Lerp(1,0, counter/fadeDuration);

			myRenderer.color = new Color(myColor.r,myColor.g,myColor.b,alpha);
			yield return null;
		}

	}

	public IEnumerator SelectFlash(){
		gameObject.GetComponent<SpriteRenderer>().color = new Color((96f/255f),1f,.19f);
		yield return new WaitForSeconds(.2f);
		gameObject.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,gameObject.GetComponent<SpriteRenderer>().color.a); //alpha part is to make sure alpha isnt effected if fading ouw right after selected(as the case with world select)

	}

	public IEnumerator Grow(float afterTime, float targetSize, float duration){
		float currentScale = gameObject.transform.lossyScale.x;
		bool growing = true;

		float amountToAdd = (targetSize - currentScale)/duration;
		float repeatWait = duration/60; //time divided by frame count
		yield return new WaitForSeconds(afterTime);
		while(growing){

			currentScale += amountToAdd;
			if(currentScale > targetSize){
				growing = false;
			}
			gameObject.transform.localScale = Vector3.one * currentScale;
			yield return new WaitForSeconds(repeatWait);
		}
	}

	public IEnumerator Squish(){
		yield return null;
	}


	public void SmoothMovementToPoint(float xDestination, float yDestination, float duration){
		Debug.Log("smooth move activate");
		velocity = Vector3.zero;
		targetPos = new Vector3(xDestination,yDestination, gameObject.transform.position.z);
		timeOfMovement = duration;
		smoothMovement = true;
	}


	public void StopMovement(){
		smoothMovement = false;
	}

	public IEnumerator Shake(float duration){
		shakeIntensity = 2;
		startYShake = gameObject.transform.position.y;
		shake = true;

		yield return new WaitForSeconds(duration);
		shake = false;
	}



}

