using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class JumboFriend : Friend {
    // "05-bq1,(chijkl"

    public int minDays = 0;
    public int maxDays = 1;
    public List<string> films;//should be defined in inspector
    private List<Friend> friendsAskedToWatch = new List<Friend>();
    public string movieEnhancement;
    public GameObject deadRat;
    public GameObject moviePosters;
    public GameObject filmColor;
    public GameObject movieScreen;
	public AudioClip projectorPlay;

	public GameObject largeTrashProjector;
	int numberOfActivation;
	bool movieIsPlaying;
    bool isFilmDateSet = false;

    //public DialogDefinition myDialogDefinition;

    public override void GenerateEventData()
    {
        if (!isFilmDateSet) {
            films.Shuffle();

            // Always play the first day.
            if (CalendarManager.Instance.currentDay == 0) {
                day = 0;
            }
            // Pick a date in the future to screen a new movie.
            else {
                day = GenerateNextFilmDay();
            }

            isFilmDateSet = true;
        }
		switch (GetFriendState()) {
            case "START":
				day = CalendarManager.Instance.currentDay; //1st meeting can happen any day
                break;
            case "END":
                break;
        }

    }

    public int GenerateNextFilmDay()
    {
        return CalendarManager.Instance.currentDay + Mathf.Min(Random.Range(minDays, maxDays), CalendarManager.Instance.maxDays);
    }

    public new void OnEnable()
    {
        switch (GetFriendState()) {
            case "START":
				day = CalendarManager.Instance.currentDay; //1st meeting can happen any day
                break;
            case "END":
                break;
        }
    }

    private void Update()
    {
        OnUpdate();
    }

    public override void OnUpdate()
    {
        switch (GetFriendState()) {
            case "START":
                nextDialog = "Start";
                GetComponent<ActivateDialogWhenClose>().Execute();
                break;
            case "INVITE_TO_SECOND_SCREENING":
                nextDialog = "Jumbo2";
                GetComponent<ActivateDialogWhenClose>().Execute();
                break;
            case "END":
                break;
        }
    }

    public override IEnumerator OnFinishDialogEnumerator()
    {
        yield return new WaitForSeconds(.3f);

        switch (GetFriendState()) {
            case "START":
                // Jumbo gave jim an invitation to the the second film.  This event is finished so Jumbo is no longer visiting today, he's editing.
                IsVisiting = false;
                SetFriendState("INVITE_TO_SECOND_SCREENING");
                break;
			case "INVITE_TO_SECOND_SCREENING":
				largeTrashProjector.SetActive(true);
				break;
            case "END":
                break;
        }

        yield return base.OnFinishDialogEnumerator();

    }

    // TODO: Ugly names :(
    public void AddFriendAskedToWatch(Friend friend)
    {
        friendsAskedToWatch.Add(friend);
    }

    // current film is just the first element in the shuffled list.
    public string GetCurrentFilm()
    {
        if (films.Count > 0)
        {
            return films[0];
        }
        else
        {
            return null;
        }
    }

    // delete the current film so a new one can be selected.
    public void DeleteCurrentFilm()
    {
        if (films.Count > 0)
            films.RemoveAt(0);
    }

    public override void StartingEvents(){
    	Debug.Log("Starting Events function happened properly");
    	if(GlobalVariableManager.Instance.DAY_NUMBER == day && nextDialog == "Jumbo2"){
    		gameObject.transform.position = new Vector2(-95f,8.5f);
    		deadRat.SetActive(true);
    	}
		if(nextDialog == "Start"){
			day = GlobalVariableManager.Instance.DAY_NUMBER; 
    	}

    	if(movieEnhancement == "marketing"){
    		moviePosters.SetActive(true);
    		for(int i = 0; i < moviePosters.transform.childCount; i++){
    			moviePosters.transform.GetChild(i).GetComponent<Ev_JumboPoster>().SetSprite(films[0]);
    		}
    	}else if(movieEnhancement == "color"){
    		filmColor.SetActive(true);
    	}else if(movieEnhancement == "sound"){
    		movieScreen.GetComponent<Ev_JumboFilmSFXHandler>().enabled = true;
    	}

    }
	public void CurrentDialogAction(){
		numberOfActivation++;
        CamManager.Instance.mainCamPostProcessor.profile = null;
		if(nextDialog == "Jumbo2"){
			if(numberOfActivation == 1){//pan to jumbo hiding in bush...
                CamManager.Instance.mainCamEffects.CameraPan(gameObject.transform.position," ");
				dialogManager.textBox.SetActive(false);
				dialogManager.currentlySpeakingIcon.gameObject.SetActive(false);
				dialogManager.Invoke("ReturnFromAction",2f);
			}else if(numberOfActivation == 2){//pan to audience...
				dialogManager.textBox.SetActive(false);
                CamManager.Instance.mainCamEffects.CameraPan(deadRat.transform.position," ");
                //CamManager.Instance.mainCamEffects.ZoomInOut(2f,.1f);
                StartCoroutine("FilmSetPan");
				dialogManager.Invoke("ReturnFromAction",5f);
			}else if(numberOfActivation == 3){//dead rat zoom in...
				dialogManager.ReturnFromAction();
                CamManager.Instance.mainCamEffects.CameraPan(deadRat.transform.position," ");
                CamManager.Instance.mainCamEffects.ZoomInOut(3f,.1f);
				dialogManager.currentlySpeakingIcon.gameObject.SetActive(false);
				dialogManager.variableText = GetCurrentFilm().Replace('_',' ');
			}else if(numberOfActivation == 4){//return to jumbo after dead rat
                CamManager.Instance.mainCamEffects.CameraPan(gameObject.transform.position," ");
				dialogManager.Invoke("ReturnFromAction",.1f);
                CamManager.Instance.mainCamEffects.ZoomInOut(1.15f,4f);
                CamManager.Instance.mainCamPostProcessor.profile = dialogManager.dialogBlur;

			}
		}
	}

	public void JumboMoviePlay(){
		if(!movieIsPlaying){
			movieIsPlaying = true;
            CamManager.Instance.mainCamEffects.CameraPan(new Vector3(-87.4f,32f,-10f),"JumboMovie");

			dialogManager.textBox.SetActive(false);
            CamManager.Instance.mainCamPostProcessor.profile = null;//TODO: returns to NO effect, not sure if you want this, future Conor
			dialogManager.currentlySpeakingIcon.gameObject.SetActive(false);


			SoundManager.instance.PlaySingle(projectorPlay);
			string filmToPlay = GetCurrentFilm();
			movieScreen.GetComponent<tk2dSpriteAnimator>().Play(filmToPlay);
			if(movieScreen.transform.GetChild(1).gameObject.activeInHierarchy){//if film color is enabled
				movieScreen.GetComponent<tk2dSpriteAnimator>().Play(filmToPlay + "_Color");
			}

            DeleteCurrentFilm();

            //determine next film
            isFilmDateSet = false;
            FriendEvent nextMovie = GenerateEvent();
            day = GenerateNextFilmDay(); // readjust the day because on the first day it gets screwy.
            nextMovie.day = day;

			Debug.Log("******NEXT MOVIE DAY****** = " + nextMovie.day);
			CalendarManager.Instance.AddFriendEvent(nextMovie);
			newestAddedEvent = nextMovie;
			dialogManager.variableText = GetCurrentFilm().Replace('_',' ');
			Debug.Log("***SET VARIABLE TEXT TO: " + GetCurrentFilm());

			if(nextDialog == "Start"){
				StartCoroutine("AfterFirstMovie");
			}else{
				dialogManager.Invoke("ReturnFromAction",10f);//10= length of each movie 
			}
		}
	}

	IEnumerator AfterFirstMovie(){
		yield return new WaitForSeconds(10f);
        CamManager.Instance.mainCamEffects.CameraPan(gameObject.transform.position,"JumboMovie");
		yield return new WaitForSeconds(1f);
		GameObject player = GameObject.FindGameObjectWithTag("Player");
        CamManager.Instance.mainCamEffects.CameraPan(player.transform.position,"JumboMovie");
		yield return new WaitForSeconds(1f);
        CamManager.Instance.mainCamEffects.CameraPan(gameObject.transform.position,"JumboMovie");
		yield return new WaitForSeconds(.5f);
        DialogManager.Instance.ReturnFromAction(); //10= length of each movie TODO:check for if this is the first movie or not, if not activate this line of code
        DialogManager.Instance.currentlySpeakingIcon.SetAnimBool("IsSurprised", true);
    }

    IEnumerator FilmSetPan(){
		yield return new WaitForSeconds(4f);
        CamManager.Instance.mainCamEffects.CameraPan(gameObject.transform.position," ");

	}
	public void JumboMovieColor(){
		//set enhancement
		movieEnhancement = "color";
		dialogManager.Invoke("ReturnFromAction",.1f);
	}public void JumboMovieSound(){
		//set enhancement
		movieEnhancement = "sound";
		dialogManager.Invoke("ReturnFromAction",.1f);
	}public void JumboMovieMarketing(){
		//set enhancement
		movieEnhancement = "marketing";
		dialogManager.Invoke("ReturnFromAction",.1f);
	}

	public override void GiveData(List<GameObject> neededObjs){

		deadRat = neededObjs[0];
		moviePosters= neededObjs[1];
		filmColor= neededObjs[2];
		movieScreen= neededObjs[3];
    }

    public void Surprise()
    {
        DialogManager.Instance.currentlySpeakingIcon.SetAnimBool("IsSurprised", true);
        DialogManager.Instance.ReturnFromAction();
    }

    public void Calm()
    {
        DialogManager.Instance.currentlySpeakingIcon.SetAnimBool("IsSurprised", false);
        DialogManager.Instance.ReturnFromAction();
    }

    // User Data implementation
    public override string UserDataKey()
    {
        return "Jumbo";
    }

    public override SimpleJSON.JSONObject Save()
    {
        var json_data = new SimpleJSON.JSONObject();

        json_data["friendState"] = friendState;
        json_data["isFilmDateSet"] = isFilmDateSet;
        json_data["day"] = day; // keeping hold of the day for the next film.

        return json_data;
    }

    public override void Load(SimpleJSON.JSONObject json_data)
    {
        friendState = json_data["friendState"].AsInt;
        isFilmDateSet = json_data["isFilmDateSet"].AsBool;
        day = json_data["day"].AsInt;
    }
}
