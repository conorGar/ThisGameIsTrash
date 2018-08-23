using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Friend))]
public class ActivateDialogWhenClose : MonoBehaviour {
    public Friend friend;

	public string dialogName;
	public DialogDefinition dialogDefiniton;
	public float xDistanceThreshold;
	public float yDistanceThreshold;
	//public GameObject myDialogIcon;
	public GameObject dialogCanvas;
	public GameObject dialogManager;
	public DialogActionManager dialogActionManager;
	public bool autoStart;//start dialog when player gets close(without player hitting space)
	public GameObject myDialogIcon;



	GameObject player;




	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		//Debug.Log(introDialogName);
	}
	
	void Update () {

		if(autoStart){
			//Debug.Log("**MY DIALOG DEFINITION***"+dialogDefiniton.name);

			if(GlobalVariableManager.Instance.CARRYING_SOMETHING == false){
				
				if(dialogName.Length > 0){
					
					if(Mathf.Abs(transform.position.x - player.transform.position.x) < xDistanceThreshold &&Mathf.Abs(transform.position.y - player.transform.position.y) < yDistanceThreshold){
						//INITIAL DIALOG ACTIVATED WHEN CLOSE

						GlobalVariableManager.Instance.PLAYER_CAN_MOVE = false;
						player.GetComponent<EightWayMovement>().enabled = false;
						player.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,0f);
						if(dialogCanvas.activeInHierarchy == false){
							Debug.Log("Dialog Definition Name:"+ dialogDefiniton.name);

							dialogManager.GetComponent<DialogManager>().myDialogDefiniton = dialogDefiniton;
							dialogManager.GetComponent<DialogManager>().dialogTitle = dialogName;
							dialogActionManager.friend = friend;
							dialogCanvas.SetActive(true);
							myDialogIcon.SetActive(true);
							if(myDialogIcon.transform.childCount>0){//if more than one icon
								List<GameObject> dialogIcons = new List<GameObject>();
								for(int i = 0; i< myDialogIcon.transform.childCount; i++){
									dialogIcons.Add(myDialogIcon.transform.GetChild(i).gameObject);
								}
								dialogManager.GetComponent<DialogManager>().dialogIcons = dialogIcons;
							}else{
								dialogManager.GetComponent<DialogManager>().dialogIcons = new List<GameObject>{myDialogIcon};//one icon
							}
						}
					}
				}
			}//end of carry something check
		}else{
		// TODO: create space bar icon and wait for player to hit space
		}
	}

	public void SetDialog(DialogDefinition dd){
		dialogDefiniton = dd;
		Debug.Log("**MY DIALOG DEFINITION***"+dialogDefiniton.name);
		Debug.Log("**MY DIALOG DEFINITION***"+dialogDefiniton.name);
		Debug.Log("**MY DIALOG DEFINITION***"+dialogDefiniton.name);
		Debug.Log("**MY DIALOG DEFINITION***"+dialogDefiniton.name);



	}
}
