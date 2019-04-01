using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ev_DroppedTrash : MonoBehaviour {

	public bool isPile;
	public GameObject droppedTrashCollectedDisplay;
	public AudioClip spawn;
	// Use this for initialization
	void OnEnable () {
		if(!isPile)
			StartCoroutine("Land");
	}
	public void PlaySound(){
		SoundManager.instance.RandomizeSfx(spawn,1f,1.1f);
	}
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D collider){
		if(collider.gameObject.tag == "Player" && GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[0] <= GlobalVariableManager.Instance.BAG_SIZE_STAT.GetMax()){
			/*droppedTrashCollectedDisplay.SetActive(true);
			droppedTrashCollectedDisplay.GetComponent<SpecialEffectsBehavior>().SmoothMovementToPoint(droppedTrashCollectedDisplay.transform.position.x, droppedTrashCollectedDisplay.transform.position.y+3,2);
			droppedTrashCollectedDisplay.GetComponent<SpecialEffectsBehavior>().SetFadeVariables(.4f,1f);
			droppedTrashCollectedDisplay.GetComponent<SpecialEffectsBehavior>().FadeOut();*/
			//GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[0] += GlobalVariableManager.Instance.GARBAGE_HAD;
			GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[0]++;
            GUIManager.Instance.TrashCollectedDisplayGameplay.UpdateDisplay(GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[0]);
			//GlobalVariableManager.Instance.GARBAGE_HAD = 0;
			//GlobalVariableManager.Instance.DROPPED_TRASH_LOCATION = Vector3.zero;
			gameObject.SetActive(false);
		}
	}

	IEnumerator Land(){
		float timeTillLand = Random.Range(.5f,.9f);
		yield return new WaitForSeconds(timeTillLand);
		gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
		gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		if(gameObject.GetComponent<Ev_ProjectileTowrdPlayer>() == null){ // homing trash no have shadow
			gameObject.transform.GetChild(0).gameObject.SetActive(true);
		}
	}
}
