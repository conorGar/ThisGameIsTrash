using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Ev_BagSelect : MonoBehaviour {

	

	tk2dSpriteAnimator myAnim;

	int phase = 0;
	// Use this for initialization
	void Start () {
        // if paper is not discovered.
		if((GlobalVariableManager.Instance.GARBAGE_DISCOVERY_LIST[GlobalVariableManager.Instance.MENU_SELECT_STAGE] & GlobalVariableManager.GARBAGE.PAPER) != GlobalVariableManager.GARBAGE.PAPER) {
			gameObject.GetComponent<tk2dSprite>().color = Color.black;
		}
		myAnim = gameObject.GetComponent<tk2dSpriteAnimator>();
		if(GlobalVariableManager.Instance.MENU_SELECT_STAGE == 2){
			myAnim.Play("cassie");

		}else if(GlobalVariableManager.Instance.MENU_SELECT_STAGE == 1){
			myAnim.Play("reggie");

		}else if(GlobalVariableManager.Instance.MENU_SELECT_STAGE == 3){
			myAnim.Play("New Clip"); //its B.A.G., was just too lazy to go back and change it...
		}



	}
	
	// Update is called once per frame
	void Update () {
		if(phase == 0){//enter screen
			if(Mathf.Abs(gameObject.transform.position.x -13) < 0.5f){
				gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
				phase = 1;
			}

		}
	}

	public IEnumerator LeaveScreen(){
		yield return new WaitForSeconds(.6f);
		GameObject manager = GameObject.Find("Manager");
		manager.GetComponent<S_Ev_BagSelect>().SetNavigate(true);
		Destroy(gameObject);
	}
}
