using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
