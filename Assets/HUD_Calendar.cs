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
	public AudioClip paperSlide;

	bool newMarkSequence;
	bool leavingScreen;

	void Start () {
		currentEvents = CalendarManager.Instance.friendEvents;
		gameObject.SetActive(false);
		GameStateManager.Instance.RegisterChangeStateEvent(OnChangeState);


	}

	private void OnDestroy()
    {
       

        GameStateManager.Instance.UnregisterChangeStateEvent(OnChangeState);
    }

	void OnChangeState(System.Type stateType, bool isEntering)
    {
        if (isEntering) {
            if (stateType == typeof(CalendarState)) {
                gameObject.SetActive(true);
                SoundManager.instance.PlaySingle(paperSlide);
                SoundManager.instance.musicSource.volume = SoundManager.instance.musicSource.volume / 2;
                Time.timeScale = 0;
            }
            else if (stateType == typeof(CalendarState)) {
                gameObject.SetActive(false);
            }
        }
        else {
            if (stateType == typeof(PauseMenuState)) {
                gameObject.SetActive(false);
                SoundManager.instance.PlaySingle(paperSlide);
                SoundManager.instance.musicSource.volume = SoundManager.instance.musicSource.volume * 2;
                Time.timeScale = 1;
            }
        }
    }
	void OnEnable(){
		leavingScreen = false;
		newMarkSequence = false;
		gameObject.transform.localPosition = new Vector2(-23f,-1f);

			gameObject.GetComponent<SpecialEffectsBehavior>().SmoothMovementToPoint(0f,0f,.5f,true);//TODO: This gets faster as time passes...fix!

	}

	void Update () {
		if((ControllerManager.Instance.GetKeyDown(INPUTACTION.CALENDAR) || ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT)) && !newMarkSequence){
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
			//newIcon.GetComponent<GUIEffects>().Start();//marker shrinks into position
			SoundManager.instance.PlaySingle(calendaMarkSfx);
		
	}

	public void LeaveScreen(){
		leavingScreen = true;
		gameObject.GetComponent<SpecialEffectsBehavior>().SmoothMovementToPoint(24f,0f,.2f,true);
		GameStateManager.Instance.PopState();
        GameStateManager.Instance.PushState(typeof(GameState));
	}

	public void ReenableOpenCal(){
		
		this.gameObject.SetActive(false);
	}


}
