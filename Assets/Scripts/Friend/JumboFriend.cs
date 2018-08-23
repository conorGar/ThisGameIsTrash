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
}
