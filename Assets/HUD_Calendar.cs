using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD_Calendar : MonoBehaviour {

	public Sprite jumboSprite;
	public Sprite coonSprite;
	public Sprite iggySprite;
	List<FriendEvent> currentEvents;
	public AudioClip calendaMarkSfx;

	void Start () {
		currentEvents = CalendarManager.Instance.friendEvents;
		/*for(int i = 0; i < currentEvents.Count; i++){
			Debug.Log(currentEvents[i].day);
			if(!gameObject.transform.GetChild(currentEvents[i].day -1).gameObject.activeInHierarchy){
				transform.GetChild(currentEvents[i].day -1).gameObject.SetActive(true);
			}
		}*/

	}

	void Update () {
		
	}

	public void NewMarkSequence(int day, string friendName){

		gameObject.transform.localPosition = new Vector2(-23f,-1f);
		gameObject.GetComponent<SpecialEffectsBehavior>().SmoothMovementToPoint(0f,0f,.5f,true);

		GameObject daySquare = transform.GetChild(day-1).gameObject;
		if(!daySquare.activeInHierarchy){
				daySquare.SetActive(true);
		}
		GameObject newIcon = null;
		if(!daySquare.transform.GetChild(0).gameObject.activeInHierarchy){
			newIcon = daySquare.transform.GetChild(0).gameObject;
		}else if(!daySquare.transform.GetChild(1).gameObject.activeInHierarchy){
			newIcon = daySquare.transform.GetChild(1).gameObject;
		}else if(!daySquare.transform.GetChild(2).gameObject.activeInHierarchy){
			newIcon = daySquare.transform.GetChild(2).gameObject;
		}
			newIcon.SetActive(true);
			if(friendName == "Jumbo"){
				newIcon.GetComponent<Image>().sprite = jumboSprite;
			}else if(friendName == "Iggy"){
				newIcon.GetComponent<Image>().sprite = iggySprite;
			}else if(friendName == "Coonelius"){
				newIcon.GetComponent<Image>().sprite = coonSprite;
			}
			//newIcon.GetComponent<GUIEffects>().Start();//marker shrinks into position
			SoundManager.instance.PlaySingle(calendaMarkSfx);
		
	}

	public void LeaveScreen(){
		gameObject.GetComponent<SpecialEffectsBehavior>().SmoothMovementToPoint(24f,0f,.5f,true);
	}
}
