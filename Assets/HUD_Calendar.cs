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
		for(int i = 0; i < currentEvents.Count; i++){
			if(!transform.GetChild(currentEvents[i].day).gameObject.activeInHierarchy){
				transform.GetChild(currentEvents[i].day).gameObject.SetActive(true);
			}
		}

	}

	void Update () {
		
	}

	public void NewMarkSequence(int day, string friendName){
		GameObject daySquare = transform.GetChild(day).gameObject;
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
			newIcon.GetComponent<GUIEffects>().Start();//marker shrinks into position
			SoundManager.instance.PlaySingle(calendaMarkSfx);
		
	}
}
