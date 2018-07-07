using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivateDialogWhenClose : MonoBehaviour {

	public int myPosInFriendList;
	public string introDialogName;

	public float xDistanceThreshold;
	public float yDistanceThreshold;
	//public GameObject myDialogIcon;
	public GameObject dialogCanvas;
	public GameObject dialogManager;

	public GameObject myDialogIcon;



	GameObject player;




	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		Debug.Log("my value at friends list" + GlobalVariableManager.Instance.FRIEND_LIST[myPosInFriendList]);
		Debug.Log(introDialogName);
		Debug.Log(introDialogName == GlobalVariableManager.Instance.FRIEND_LIST[myPosInFriendList]);
	}
	
	void Update () {
		if(GlobalVariableManager.Instance.CARRYING_SOMETHING == false){
			
			if(GlobalVariableManager.Instance.FRIEND_LIST[myPosInFriendList] == introDialogName){
				
				if(Mathf.Abs(transform.position.x - player.transform.position.x) < xDistanceThreshold &&Mathf.Abs(transform.position.y - player.transform.position.y) < yDistanceThreshold){
					//INITIAL DIALOG ACTIVATED WHEN CLOSE

					GlobalVariableManager.Instance.PLAYER_CAN_MOVE = false;
					player.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,0f);
					if(dialogCanvas.activeInHierarchy == false){
						dialogCanvas.SetActive(true);
						dialogManager.GetComponent<DialogBehaviorManager>().SetDialogName(GlobalVariableManager.Instance.FRIEND_LIST[myPosInFriendList]);
						myDialogIcon.SetActive(true);
						if(myDialogIcon.transform.childCount>0){//if more than one icon
							List<GameObject> dialogIcons = new List<GameObject>();
							for(int i = 0; i< myDialogIcon.transform.childCount; i++){
								dialogIcons.Add(myDialogIcon.transform.GetChild(i).gameObject);
							}
							dialogManager.GetComponent<DialogBehaviorManager>().SetIcon(dialogIcons);
						}else{
							dialogManager.GetComponent<DialogBehaviorManager>().SetIcon(new List<GameObject>{myDialogIcon});//one icon
						}
					}
				}
			}
		}//end of carry something check
	}
}
