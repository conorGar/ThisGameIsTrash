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
	public OpenCalendar player;

	bool newMarkSequence;
	bool leavingScreen;

	void Start () {
		currentEvents = CalendarManager.Instance.friendEvents;
		/*for(int i = 0; i < currentEvents.Count; i++){
			Debug.Log(currentEvents[i].day);
			if(!gameObject.transform.GetChild(currentEvents[i].day -1).gameObject.activeInHierarchy){
				transform.GetChild(currentEvents[i].day -1).gameObject.SetActive(true);
			}
		}*/

	}

	void OnEnable(){
		leavingScreen = false;
		newMarkSequence = false;
		gameObject.transform.localPosition = new Vector2(-23f,-1f);

			gameObject.GetComponent<SpecialEffectsBehavior>().SmoothMovementToPoint(0f,0f,.5f,true);//TODO: This gets faster as time passes...fix!

	}

	void Update () {
		if((Input.GetKeyDown(KeyCode.C) || ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT)) && !newMarkSequence){
			Time.timeScale = 1f;
			LeaveScreen();
			Invoke("ReenableOpenCal",.5f);
		}
		if(!newMarkSequence && !leavingScreen){ 
			gameObject.transform.localPosition = Vector3.Lerp(gameObject.transform.localPosition,Vector3.zero,.1f*(Time.realtimeSinceStartup - Time.deltaTime));
		}
	}

	public void NewMarkSequence(int day, string friendName){
		newMarkSequence = true;
		//gameObject.transform.localPosition = new Vector2(-23f,-1f);
		//gameObject.GetComponent<SpecialEffectsBehavior>().SmoothMovementToPoint(0f,0f,.5f,true);

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
		leavingScreen = true;
		gameObject.GetComponent<SpecialEffectsBehavior>().SmoothMovementToPoint(24f,0f,.2f,true);
	}

	public void ReenableOpenCal(){
		player.enabled = true;
		this.gameObject.SetActive(false);
	}


}
