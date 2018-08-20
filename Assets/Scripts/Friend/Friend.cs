using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friend : MonoBehaviour {
    public string friendName = "Unknown Friend";
    public int day = 0;

    // Is the friend visiting the level on this day.
    public bool IsVisiting = false;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    // virtual function to generate an event for this friend so it can be added to the calendar.  Let the friend can decide when they want to show up.
    public virtual FriendEvent GenerateEvent()
    {
        var the_event = new FriendEvent();
        GenerateEventData();
        the_event.friend = this;
        the_event.day = day;

        Debug.Log("New Friend Event Generated:\n" +
                  "Friend: " + the_event.friend.name + "\n" +
                  "Day: " + the_event.day);

        return the_event;
    }

    public virtual void GenerateEventData()
    {
        // nothing to do for a basic friend.
    }

    public virtual void Execute()
    {
        IsVisiting = true;
    }
}
