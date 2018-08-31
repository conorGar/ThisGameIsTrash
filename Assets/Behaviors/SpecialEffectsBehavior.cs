using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class SpecialEffectsBehavior : MonoBehaviour
{	
	

	//-----Variables for smooth movement to point -------//
	bool smoothMovement = false;
	bool smoothMovementLocally = false;
	Vector3 velocity;
	Vector3 targetPos;
	float timeOfMovement;
	//-----------------------------------//

	float fadeDelay;
	float fadeDuration;
	bool shake;
	bool growing;
	float shakeIntensity;
	float startYShake;

	//grow/shrink
	bool shrinking;
	Vector3 currentScale;
	//float amountToAdd;
	float growDuration;
	Vector3 targetSize;




	void Update(){
		if(smoothMovement){
			if(!smoothMovementLocally){
				transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity,timeOfMovement);
			}else{
				transform.localPosition = Vector3.SmoothDamp(transform.localPosition, targetPos, ref velocity,timeOfMovement);

			}

		}

		if(shake){
			Debug.Log("shaking");
			transform.position = new Vector2(transform.position.x,startYShake + Random.Range(shakeIntensity*-1,shakeIntensity));
		}
		if(growing){
			//currentScale += amountToAdd;
			if(Vector3.Distance(currentScale,targetSize) < 1.5f){//TODO: 1.5 pretty specific number needed for title screen
				Debug.Log("GROWING STOPPED");
				growing = false;
			}
			Debug.Log("distance:" + Vector3.Distance(currentScale,targetSize));
			//gameObject.transform.localScale = Vector3.one * currentScale;*/
			gameObject.transform.localScale = Vector3.Lerp(currentScale,targetSize,growDuration*Time.deltaTime);
		}

		/*else if(shrinking){
			currentScale -= amountToAdd;
			if(currentScale < targetSize){
				shrinking = false;
			}
			gameObject.transform.localScale = Vector3.one * currentScale;
		}*/
	}
	public void SetFadeVariables(float afterTime, float fd){
		fadeDelay = afterTime;
		fadeDuration = fd;
	}
	public IEnumerator FadeOut(){
		float counter = 0;
		Color myColor = new Color(0f,0f,0f);
		yield return new WaitForSeconds(fadeDelay);
		if(gameObject.GetComponent<SpriteRenderer>() != null){
			SpriteRenderer myRenderer = gameObject.GetComponent<SpriteRenderer>();
			myColor = myRenderer.color;
				while(counter < fadeDuration){
					counter += Time.deltaTime;
					float alpha = Mathf.Lerp(1,0, counter/fadeDuration);

					myRenderer.color = new Color(myColor.r,myColor.g,myColor.b,alpha);
					yield return null;
				}
		}else if(gameObject.GetComponent<tk2dSprite>() != null){
			tk2dSprite myRenderer = gameObject.GetComponent<tk2dSprite>();
			myColor = myRenderer.color;
				while(counter < fadeDuration){
					counter += Time.deltaTime;
					float alpha = Mathf.Lerp(1,0, counter/fadeDuration);

					myRenderer.color = new Color(myColor.r,myColor.g,myColor.b,alpha);
					yield return null;
				}
		}else if(gameObject.GetComponent<Image>() != null){
			Image myRenderer = gameObject.GetComponent<Image>();
			myColor = myRenderer.color;
				while(counter < fadeDuration){
					counter += Time.deltaTime;
					float alpha = Mathf.Lerp(1,0, counter/fadeDuration);

					myRenderer.color = new Color(myColor.r,myColor.g,myColor.b,alpha);
					yield return null;
				}
		}
	


	}

	public IEnumerator SelectFlash(){
		gameObject.GetComponent<SpriteRenderer>().color = new Color((96f/255f),1f,.19f);
		yield return new WaitForSeconds(.2f);
		gameObject.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,gameObject.GetComponent<SpriteRenderer>().color.a); //alpha part is to make sure alpha isnt effected if fading ouw right after selected(as the case with world select)

	}

	public void SetGrowValues( float tSize, float duration){
		currentScale = gameObject.transform.localScale;
		targetSize = new Vector3(tSize,tSize,currentScale.z);
		//amountToAdd = (targetSize - currentScale)/duration;
	}
	public void SetGrowValues( Vector3 tSize, float duration){
		currentScale = gameObject.transform.localScale;
		targetSize = tSize;
		growDuration = duration;
	}


	public IEnumerator Grow(float afterTime){
		Debug.Log("target size: " + targetSize);
		Debug.Log("current size: " + currentScale);

		yield return new WaitForSeconds(afterTime);

		growing = true;

	}
	/*public IEnumerator Shrink(float afterTime){
		yield return new WaitForSeconds(afterTime);

		currentScale = gameObject.transform.localScale;

		shrinking = true;

	}*/

	public IEnumerator Squish(){
		yield return null;
	}


	public void SmoothMovementToPoint(float xDestination, float yDestination, float duration){
		Debug.Log("smooth move activate");
		velocity = Vector3.zero;
		targetPos = new Vector3(xDestination,yDestination, gameObject.transform.position.z);
		timeOfMovement = duration;
		smoothMovement = true;
		smoothMovementLocally = false;
	}
	public void SmoothMovementToPoint(float xDestination, float yDestination, float duration, bool localPosition){
		Debug.Log("smooth move activate");
		velocity = Vector3.zero;
		targetPos = new Vector3(xDestination,yDestination, gameObject.transform.position.z);
		timeOfMovement = duration;
		smoothMovement = true;
		smoothMovementLocally = true;
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

