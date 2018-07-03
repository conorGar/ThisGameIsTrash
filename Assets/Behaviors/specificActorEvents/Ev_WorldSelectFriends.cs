using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ev_WorldSelectFriends : MonoBehaviour {
/// <summary>
/// handles smooth movement to proper given position when activated, turns black if
///character asn't been discovered
/// </summary>

	public int charPosition;
	public int myPosInFriendList;
	public char charValToCheck;
	public float positionToMoveToX;
	public float positionToMoveToY;

	// Use this for initialization
	void Start () {
		//if(GlobalVariableManager.Instance.FRIEND_LIST[myPosInFriendList][charPosition] == charValToCheck){
			gameObject.GetComponent<SpriteRenderer>().color = Color.black;
			//gameObject.GetComponent<SpriteRenderer>().color = new Color(Color.black, .75f); 
		//}
		ActivateMovement();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public IEnumerator ActivateMovement(){
		gameObject.GetComponent<SpecialEffectsBehavior>().StopMovement();
		//gameObject.transform.localPosition = new Vector2(0f,0f);
		yield return new WaitForSeconds(.5f);
		Debug.Log("smooth move activate -----friend icon");
		gameObject.GetComponent<SpecialEffectsBehavior>().SmoothMovementToPoint(positionToMoveToX,positionToMoveToY,.2f);

	}
}
