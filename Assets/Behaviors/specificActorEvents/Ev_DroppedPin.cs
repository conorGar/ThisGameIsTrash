using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Ev_DroppedPin : MonoBehaviour {

	PinDefinition pinData = null; //usually given by trash can
	int bounceCounter = 0;
	GameObject pinUnlockHud; //given by trash can
	float landingY;
	bool bouncing;
	bool canBeGrabbed = false;
	Sprite mySprite; //given by trash can, used by unlock hud
	int increaseOnce;


	// Use this for initialization
	void Start () {
		gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(1,5),ForceMode2D.Impulse);
		StartCoroutine("WaitForGrab");

	}
	
	// Update is called once per frame
	void Update () {
		if(bouncing){
			if(gameObject.transform.position.y < landingY){
				if(bounceCounter < 3){
					gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0,(5-bounceCounter)),ForceMode2D.Impulse);
					if(increaseOnce == 0){
						bounceCounter++;
						increaseOnce++;
						StartCoroutine("BounceIncreaseDelay");
					}
				}
				else{
					bouncing = false;
					gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
					gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
					canBeGrabbed = true;
				}
			}
		}
	}

	void OnTriggerEnter2D(Collider2D collider){
		Debug.Log("Can dropped pin be grabbed? = " + canBeGrabbed);
		if(canBeGrabbed && collider.gameObject.tag == "Player"){
			pinUnlockHud.SetActive(true);
			pinUnlockHud.GetComponent<GUI_PinUnlockDisplay>().SetValues(pinData.displayName, pinData.description,mySprite);
			GlobalVariableManager.Instance.PINS_DISCOVERED |= pinData.Type;
			GameStateManager.Instance.PushState(typeof(PopupState));
			gameObject.SetActive(false);
		}
	}

	public void SetPinData(PinDefinition data, GameObject unlockHUD, Sprite pinsprite){
        pinData = data;
        pinUnlockHud = unlockHUD;
        mySprite = pinsprite;
	}//end of pin data set


	IEnumerator WaitForGrab(){
		yield return new WaitForSeconds(.8f);
		landingY = gameObject.transform.position.y;
		Debug.Log("Dropped Pin force added >>>>>>>>>>>>>>>>");
		gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0,5),ForceMode2D.Impulse);
		bounceCounter = 1;
		bouncing = true;

	}
	IEnumerator BounceIncreaseDelay(){
		//needed for bounce counter to not increase all at once
		yield return new WaitForSeconds(.1f);
		bounceCounter++;
	}
}
