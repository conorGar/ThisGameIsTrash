﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowTrash : MonoBehaviour {

	// Use this for initialization
	//tk2dSpriteAnimator myAnim;
	public AudioClip throwSfx;

	void Start () {
		if((GlobalVariableManager.Instance.BOSSES_KILLED & GlobalVariableManager.BOSSES.ONE ) != GlobalVariableManager.BOSSES.ONE){
			this.enabled = false;// cant do if haven't picked up throwing gloves yet.
		}
	}
	
	// Update is called once per frame
	void Update () {
        if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
            if (Input.GetKey(KeyCode.LeftShift)) {
                if (ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKLEFT)) {
                    StartCoroutine("Throw", "left");
                }
                else if (ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKRIGHT)) {
                    StartCoroutine("Throw", "right");
                }
                else if (ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKUP)) {
                    StartCoroutine("Throw", "up");
                }
                else if (ControllerManager.Instance.GetKeyDown(INPUTACTION.ATTACKDOWN)) {
                    StartCoroutine("Throw", "down");
                }
            }
        }
	}

	IEnumerator Throw(string direction){

		yield return new WaitForSeconds(.1f);
		GameObject thrownTrash = ObjectPool.Instance.GetPooledObject("thrownTrash",gameObject.transform.position);
		thrownTrash.GetComponent<Ev_ThrownTrash>().direction = direction;
		if(direction == "right"){
			thrownTrash.GetComponent<Rigidbody2D>().velocity = new Vector2(10f,0f);
		}else if(direction == "left"){
			thrownTrash.GetComponent<Rigidbody2D>().velocity = new Vector2(-10f,0f);
		}else if(direction == "down"){
			thrownTrash.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,-10f);
		}else if(direction == "up"){
			thrownTrash.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,10f);
		}
	}
}
