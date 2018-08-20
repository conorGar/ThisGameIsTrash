using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DESIRED_OBJECT
{
    NONE = 0,
    PRETTY,
    CRUNCHY,
    SOFT,
    SMELLY
}


// Covers the 3 rock-based friends (Rock, Stone, Slab)
public class RockFriend : Friend {

    [SerializeField]
    private List<DESIRED_OBJECT> desiredObject = new List<DESIRED_OBJECT>();
    private List<DESIRED_OBJECT> deliveredObjects = new List<DESIRED_OBJECT>();

    public override void GenerateEventData()
    {
        // These guys show up every day.
        day = CalendarManager.Instance.currentDay;
    }

    public void DeliverObject(DESIRED_OBJECT obj)
    {
        deliveredObjects.Add(obj);
    }
}
