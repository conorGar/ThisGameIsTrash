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
	public GameObject mainCam;
	public AudioClip projectorPlay;
	int numberOfActivation;
	bool movieIsPlaying;

    //public DialogDefinition myDialogDefinition;

    public override void GenerateEventData()
    {
        films.Shuffle();
        day = CalendarManager.Instance.currentDay + Mathf.Min(Random.Range(minDays, maxDays), CalendarManager.Instance.maxDays);
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

	public override void FinishDialogEvent(){
		base.FinishDialogEvent();
		gameObject.GetComponent<ActivateDialogWhenClose>().enabled = false; // needed to fix glitch where if player spammed continue button dialog would start again
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
		dialogManager.mainCam.GetComponent<PostProcessingBehaviour>().profile = null;
		if(nextDialog == "Jumbo2"){
			if(numberOfActivation == 1){//pan to jumbo hiding in bush...
				mainCam.GetComponent<Ev_MainCameraEffects>().CameraPan(gameObject.transform.position," ");
				dialogManager.textBox.SetActive(false);
				dialogManager.currentlySpeakingIcon.SetActive(false);
				dialogManager.Invoke("ReturnFromAction",2f);
			}else if(numberOfActivation == 2){//pan to audience...
				dialogManager.textBox.SetActive(false);
				mainCam.GetComponent<Ev_MainCameraEffects>().CameraPan(deadRat.transform.position," ");
				//mainCam.GetComponent<Ev_MainCameraEffects>().ZoomInOut(2f,.1f);
				StartCoroutine("FilmSetPan");
				dialogManager.Invoke("ReturnFromAction",5f);
			}else if(numberOfActivation == 3){//dead rat zoom in...
				dialogManager.ReturnFromAction();
				mainCam.GetComponent<Ev_MainCameraEffects>().CameraPan(deadRat.transform.position," ");
				mainCam.GetComponent<Ev_MainCameraEffects>().ZoomInOut(3f,.1f);
				dialogManager.currentlySpeakingIcon.SetActive(false);
				dialogManager.variableText = GetCurrentFilm().Replace('_',' ');
			}else if(numberOfActivation == 4){//return to jumbo after dead rat
				mainCam.GetComponent<Ev_MainCameraEffects>().CameraPan(gameObject.transform.position," ");
				dialogManager.Invoke("ReturnFromAction",.1f);
				mainCam.GetComponent<Ev_MainCameraEffects>().ZoomInOut(1.15f,4f);
				dialogManager.mainCam.GetComponent<PostProcessingBehaviour>().profile = dialogManager.dialogBlur;

			}
		}
	}

	public void JumboMoviePlay(){
		if(!movieIsPlaying){
			movieIsPlaying = true;
			mainCam.GetComponent<Ev_MainCameraEffects>().CameraPan(new Vector3(-87.4f,32f,-10f),"JumboMovie");

			dialogManager.textBox.SetActive(false);
			dialogManager.mainCam.GetComponent<PostProcessingBehaviour>().profile = null;//TODO: returns to NO effect, not sure if you want this, future Conor
			dialogManager.currentlySpeakingIcon.SetActive(false);


			SoundManager.instance.PlaySingle(projectorPlay);
			string filmToPlay = GetCurrentFilm();
			movieScreen.GetComponent<tk2dSpriteAnimator>().Play(filmToPlay);
			if(movieScreen.transform.GetChild(1).gameObject.activeInHierarchy){//if film color is enabled
				movieScreen.GetComponent<tk2dSpriteAnimator>().Play(filmToPlay + "_Color");
			}
			DeleteCurrentFilm();

			//determine next film
			GenerateEventData();
			FriendEvent nextMovie = GenerateEvent();
			Debug.Log("******NEXT MOVIE DAY****** = " + nextMovie.day);
			CalendarManager.Instance.AddFriendEvent(nextMovie);
			newestAddedEvent = nextMovie;
			dialogManager.variableText = GetCurrentFilm().Replace('_',' ');
			dialogManager.ChangeIcon("SurprisedAni");
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
		mainCam.GetComponent<Ev_MainCameraEffects>().CameraPan(gameObject.transform.position,"JumboMovie");
		yield return new WaitForSeconds(1f);
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		mainCam.GetComponent<Ev_MainCameraEffects>().CameraPan(player.transform.position,"JumboMovie");
		yield return new WaitForSeconds(1f);
		mainCam.GetComponent<Ev_MainCameraEffects>().CameraPan(gameObject.transform.position,"JumboMovie");
		yield return new WaitForSeconds(.5f);
		dialogManager.Invoke("ReturnFromAction",.1f);//10= length of each movie TODO:check for if this is the first movie or not, if not activate this line of code


	}

	IEnumerator FilmSetPan(){
		yield return new WaitForSeconds(4f);
		mainCam.GetComponent<Ev_MainCameraEffects>().CameraPan(gameObject.transform.position," ");

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
		mainCam= neededObjs[4];
    }
}
