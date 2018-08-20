using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Friend))]
public class ActivateDialogWhenClose : MonoBehaviour {
    public Friend friend;

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
		Debug.Log(introDialogName);
	}
	
	void Update () {
		if(GlobalVariableManager.Instance.CARRYING_SOMETHING == false){
			
			if(introDialogName.Length > 0){
				
				if(Mathf.Abs(transform.position.x - player.transform.position.x) < xDistanceThreshold &&Mathf.Abs(transform.position.y - player.transform.position.y) < yDistanceThreshold){
					//INITIAL DIALOG ACTIVATED WHEN CLOSE

					GlobalVariableManager.Instance.PLAYER_CAN_MOVE = false;
					player.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,0f);
					if(dialogCanvas.activeInHierarchy == false){
						dialogCanvas.SetActive(true);
						dialogManager.GetComponent<DialogBehaviorManager>().SetDialogName(introDialogName);
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
