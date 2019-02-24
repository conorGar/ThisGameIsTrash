using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD_Calendar : MonoBehaviour {

	//public Sprite jumboSprite;
	//public Sprite coonSprite;
	//public Sprite iggySprite;
	List<FriendEvent> currentEvents;
	public AudioClip calendaMarkSfx;
	public OpenCalendar player;
	public AudioClip paperSlide;
	public List<Calendar_DaySquare> daySquares;
	public GameObject navArrow;
	public TextMeshProUGUI eventDescription;
	public GameObject calendarBack;
	public Calendar_DaySquare activeQuestsBack;
	public Calendar_DaySquare readyToTalkBack;
	public GameObject calendarHighlightPS;
	public GameObject calendarPrompt;

	List<GameObject> selectableIcons = new List<GameObject>();
	List<GameObject> activeRequestIcons = new List<GameObject>();
	bool newMarkSequence;
	bool leavingScreen;
	int navArrowPos;
	int currentSection;
	Color unhighlightedColor = new Color(1,1,1,.4f);

	/*void OnAwake(){
		currentEvents = CalendarManager.Instance.friendEvents;
		FillCalendar();
		FillActiveQuestsBox();
	}*/
	void Start () {
		//currentEvents = CalendarManager.Instance.friendEvents;
		currentEvents = CalendarManager.Instance.friendEvents;
		FillCalendar();
		FillActiveQuestsBox();
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
               // Time.timeScale = 0;
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
                //Time.timeScale = 1;
            }
        }
    }
	void OnEnable(){
		leavingScreen = false;
		newMarkSequence = false;
		//gameObject.transform.localPosition = new Vector2(-23f,-1f);

		//gameObject.GetComponent<SpecialEffectsBehavior>().SmoothMovementToPoint(0f,0f,.5f,true);//TODO: This gets faster as time passes...fix!

	}

	void Update () {
		if((ControllerManager.Instance.GetKeyDown(INPUTACTION.CALENDAR) || ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT)) && !newMarkSequence){
			Time.timeScale = 1f;
			ReenableOpenCal();
			GameStateManager.Instance.PopState();
			LeaveScreen();
			//Invoke("ReenableOpenCal",.5f);
		}
		if(!newMarkSequence && !leavingScreen){ 
			if(currentSection == 0){
				if(selectableIcons.Count >0){
					if(calendarHighlightPS.activeInHierarchy == false)
						calendarHighlightPS.SetActive(true);

					//gameObject.transform.localPosition = Vector3.Lerp(gameObject.transform.localPosition,Vector3.zero,.1f*(Time.realtimeSinceStartup - Time.deltaTime));
					selectableIcons[navArrowPos].GetComponent<Animator>().enabled = false;

					if(ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVERIGHT) && navArrowPos <selectableIcons.Count){
						navArrowPos++;
					}else if(ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVELEFT) && navArrowPos > 0){
						navArrowPos--;
					}else if(ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVEDOWN) && navArrowPos < selectableIcons.Count){
						navArrowPos++;
					}else if(ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVEDOWN) && navArrowPos > 0){
						navArrowPos--;
					}
					Debug.Log(selectableIcons.Count);
					navArrow.transform.position = selectableIcons[navArrowPos].transform.position;
					calendarHighlightPS.transform.position = selectableIcons[navArrowPos].transform.position;
					selectableIcons[navArrowPos].GetComponent<Animator>().enabled = true; //highlight animation
					eventDescription.text = currentEvents[navArrowPos].friend.GetEventDescription();
				}
			}else if(currentSection == 1){//active requests
				if(activeRequestIcons.Count >0){
					navArrow.transform.position = activeRequestIcons[navArrowPos].transform.position;
					calendarHighlightPS.transform.position = activeRequestIcons[navArrowPos].transform.position;
					activeRequestIcons[navArrowPos].GetComponent<Animator>().enabled = true; //highlight animation
				}else{
					navArrow.transform.position = activeQuestsBack.transform.position;
				}
			}else if(currentSection == 2){//ready to talk

			}



			//Switch between the different sections
			if(ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT)){ //switch to active requests
				currentSection = 1;
				calendarBack.GetComponent<SpriteRenderer>().color = unhighlightedColor;
				readyToTalkBack.GetComponent<SpriteRenderer>().color = unhighlightedColor;
				activeQuestsBack.GetComponent<SpriteRenderer>().color = Color.white;
			}else if(ControllerManager.Instance.GetKeyDown(INPUTACTION.CANCEL)){//switch to readyToTalk
				currentSection = 2;
				calendarBack.GetComponent<SpriteRenderer>().color = unhighlightedColor;
				activeQuestsBack.GetComponent<SpriteRenderer>().color = unhighlightedColor;
				readyToTalkBack.GetComponent<SpriteRenderer>().color = Color.white;
			}else if(ControllerManager.Instance.GetKeyDown(INPUTACTION.PAUSE)){//switch to calendar
				currentSection = 0;
				readyToTalkBack.GetComponent<SpriteRenderer>().color = unhighlightedColor;
				activeQuestsBack.GetComponent<SpriteRenderer>().color = unhighlightedColor;
				calendarBack.GetComponent<SpriteRenderer>().color = Color.white;
			}




		}


	}

	public void NewMarkSequence(int day, FriendEvent fEvent){

		newMarkSequence = true;


		GameObject icon = daySquares[day].AddIcon(fEvent);
		selectableIcons.Add(icon);

		SoundManager.instance.PlaySingle(calendaMarkSfx);
		
	}


	public void NewActiveQuestsMark(FriendEvent fEvent){

	}

	public void LeaveScreen(){
		leavingScreen = true;
		//GameStateManager.Instance.PopState();
        //GameStateManager.Instance.PushState(typeof(GameState));
		gameObject.SetActive(false);

	}

	public void ReenableOpenCal(){
		
		this.gameObject.SetActive(false);
	}


	void FillCalendar(){
		Debug.Log("Fill Calendar Activated");
		for(int i = 0 ; i < currentEvents.Count; i++){
			if(currentEvents[i].friend.myFriendType == Friend.FriendType.ScheduleFriend){
				GameObject icon = daySquares[currentEvents[i].day].AddIcon(currentEvents[i]);
				Debug.Log("Fill Calendar - selectableIcon added");
				selectableIcons.Add(icon);
			}
		}
	}

	void FillActiveQuestsBox(){
		for(int i = 0 ; i < currentEvents.Count; i++){
			if(currentEvents[i].friend.myFriendType == Friend.FriendType.GatheringFriend){
				GameObject icon = activeQuestsBack.AddIcon(currentEvents[i]);
				Debug.Log("Fill Calendar - selectableIcon added");
				activeRequestIcons.Add(icon);
			}
		}
	}

	void FillConversationFriendBox(){

	}


}
