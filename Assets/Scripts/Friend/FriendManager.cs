using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendManager : MonoBehaviour {
    public static FriendManager Instance;
    public List<Friend> friends = new List<Friend>();

    void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GenerateEvents()
    {
        for (int i=0; i < friends.Count; ++i)
        {
            FriendEvent the_event = friends[i].GenerateEvent();

            if (the_event != null)
                CalendarManager.Instance.AddFriendEvent(the_event);
        }
    }

    public GameObject GetFriendObject(Friend friend)
    {
        for (int i = 0; i < friends.Count; ++i)
        {
            if (friend.tag == friends[i].tag)
                return friends[i].gameObject;
        }

        return null;
    }
}
