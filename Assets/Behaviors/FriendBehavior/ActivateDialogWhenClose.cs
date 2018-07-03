using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivateDialogWhenClose : MonoBehaviour {

	public int myPosInFriendList;
	public string introDialogName;

	public float xDistanceThreshold;
	public float yDistanceThreshold;
	public GameObject myDialogIcon;
	public GameObject dialogCanvas;



	GameObject player;




	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	void Update () {
		if(GlobalVariableManager.Instance.CARRYING_SOMETHING == false){
			if(GlobalVariableManager.Instance.FRIEND_LIST[myPosInFriendList] == introDialogName){
				if(Mathf.Abs(transform.position.x - player.transform.position.x) < 3f &&Mathf.Abs(transform.position.y - player.transform.position.y) < 3f){
					//INITIAL DIALOG ACTIVATED WHEN CLOSE

					GlobalVariableManager.Instance.PLAYER_CAN_MOVE = false;
					player.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,0f);
		
					dialogCanvas.SetActive(true);
					//dialogCanvas.transform.GetChild(3).GetComponent<DialogBehaviorManager>().SetIcon(myDialogIcon);
					//dialogCanvas.transform.GetChild(3).GetComponent<DialogBehaviorManager>().SetDialogName(GlobalVariableManager.Instance.FRIEND_LIST[myPosInFriendList]);
						//dialogCanvas.transform.GetChild(0).GetComponent<Image>().sprite = myDialogIcon;
						//dialogCanvas.transform.GetChild(0).GetComponent<GUIEffects>().Start();//Icon flies in
						//dialogCanvas.transform.GetChild(1).GetComponent<GUIEffects>().Start();//textbox flies in

				}
			}
		}//end of carry something check
	}
}
