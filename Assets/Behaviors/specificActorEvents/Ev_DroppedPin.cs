﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Ev_DroppedPin : MonoBehaviour {

	PinDefinition pinData = null; //usually given by trash can

    [SerializeField] int bounceCounter = 0;
	GameObject pinUnlockHud; //given by trash can

    [SerializeField] float landingY;
    [SerializeField] bool bouncing;
    [SerializeField] bool canBeGrabbed = false;
	Sprite mySprite; //given by trash can, used by unlock hud
	[SerializeField] int increaseOnce;

    void Start()
    {
        GameStateManager.Instance.RegisterChangeStateEvent(OnChangeState);
    }

    void OnDestroy()
    {
        GameStateManager.Instance.UnregisterChangeStateEvent(OnChangeState);
    }

    // Use this for initialization
    void OnEnable () {
		gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(1,5),ForceMode2D.Impulse);
        bounceCounter = 0;
        bouncing = false;
        canBeGrabbed = false;
        increaseOnce = 0;
        landingY = 0;
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 2;
        StartCoroutine("WaitForGrab");

	}
	
	// Update is called once per frame
	void Update () {
        if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
            if (bouncing) {
                if (gameObject.transform.position.y < landingY) {
                    if (bounceCounter < 3) {
                        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, (5 - bounceCounter)), ForceMode2D.Impulse);
                        if (increaseOnce == 0) {
                            bounceCounter++;
                            increaseOnce++;
                            StartCoroutine("BounceIncreaseDelay");
                        }
                    } else {
                        bouncing = false;
                        gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
                        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                        canBeGrabbed = true;
                    }
                }
            }
        }
	}

	void OnTriggerStay2D(Collider2D collider){
		if(canBeGrabbed && collider.gameObject.tag == "Player"){
            if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
                pinUnlockHud.SetActive(true);
                pinUnlockHud.GetComponent<GUI_PinUnlockDisplay>().SetValues(pinData.displayName, pinData.description, mySprite);
                GlobalVariableManager.Instance.PINS_DISCOVERED |= pinData.Type;
                GameStateManager.Instance.PushState(typeof(PopupState));
                gameObject.SetActive(false);
            }
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

    void OnChangeState(System.Type stateType, bool isEntering)
    {
        // Only simulate physics during gameplay.
        if (GameStateManager.Instance.GetCurrentState() != typeof(GameplayState)) {
            gameObject.GetComponent<Rigidbody2D>().simulated = false;
        } else {
            gameObject.GetComponent<Rigidbody2D>().simulated = true;
        }
    }
}
