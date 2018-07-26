using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ev_DroppedTrash : MonoBehaviour {

	public bool isPile;
	public GameObject droppedTrashCollectedDisplay;
	// Use this for initialization
	void Start () {
		if(!isPile)
			StartCoroutine("Land");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D collider){
		if(isPile && collider.gameObject.tag == "Player"){
			droppedTrashCollectedDisplay.SetActive(true);
			droppedTrashCollectedDisplay.GetComponent<SpecialEffectsBehavior>().SmoothMovementToPoint(droppedTrashCollectedDisplay.transform.position.x, droppedTrashCollectedDisplay.transform.position.y+3,2);
			droppedTrashCollectedDisplay.GetComponent<SpecialEffectsBehavior>().SetFadeVariables(.4f,1f);
			droppedTrashCollectedDisplay.GetComponent<SpecialEffectsBehavior>().FadeOut();
			GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[0] += GlobalVariableManager.Instance.GARBAGE_HAD;
			collider.gameObject.GetComponent<PlayerTakeDamage>().trashCollectedDisplay.GetComponent<GUI_TrashCollectedDisplay>().UpdateDisplay(GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[0]);
			GlobalVariableManager.Instance.GARBAGE_HAD = 0;
			GlobalVariableManager.Instance.DROPPED_TRASH_LOCATION = Vector3.zero;
			gameObject.SetActive(false);
		}
	}

	IEnumerator Land(){
		float timeTillLand = Random.Range(.5f,.9f);
		yield return new WaitForSeconds(timeTillLand);
		gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
		gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		gameObject.transform.GetChild(0).gameObject.SetActive(true);
	}
}
