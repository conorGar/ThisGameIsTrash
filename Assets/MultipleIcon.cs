using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleIcon : MonoBehaviour {


	//public ActivateDialogWhenClose myFriend;
	public bool movePositionOnScreen = true;
	public string myIconAniTrigger;
    public Animator animator;
    public bool isTalking;

    // positionOnScreen should be changed by the specific friend event script's method that activates the new dialog.
    
	public int positionOnScreen; //0 = left, 1 = middle, 2 = right
    int initialPositionOnScreen; // Since we sometimes allow positions to be edited, allow the original positions to restore themselves ondisable after these special cases.

    bool movingBack;
    // Use this for initialization

    void Awake()
    {
        initialPositionOnScreen = positionOnScreen;   
    }

    //mnEnable() here that postions icon properly based on 'positionOnScreen' value
    void OnEnable(){
    	if(movePositionOnScreen){
			if(positionOnScreen == 0){
				gameObject.transform.localPosition = new Vector2(-48f,6.5f);
			}else if(positionOnScreen == 1){
				gameObject.transform.localPosition = new Vector2(0f,13f);
			}else if(positionOnScreen == 2){
				gameObject.transform.localPosition = new Vector2(39f,6.5f);
			}
		}
	}

    public void ResetPositionOnScreen()
    {
        // restore initial positions for the next time this icon is activated.
        if(this.gameObject.activeInHierarchy)
        	positionOnScreen = initialPositionOnScreen;
    }


    // Update is called once per frame
    void Update () {
		if(movingBack){
			if(positionOnScreen == 0){
				gameObject.transform.Translate(Vector2.left*Time.deltaTime);
				if(gameObject.transform.localPosition.x < -48)
					movingBack = false;
			}else if(positionOnScreen == 2){
				gameObject.transform.Translate(Vector2.right*Time.deltaTime);
				if(gameObject.transform.localPosition.x > 45)
					movingBack = false;
			}
			else{ // no change for middle icon
				movingBack = false;
			}
		}
	}

	public void ReturnToPosition(){
		Debug.Log("***RETURN TO POSITION ACTIVATED !!!!!***!!!!" + positionOnScreen);
		movingBack = true;
	}
}
