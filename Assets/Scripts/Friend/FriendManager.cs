﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendManager : MonoBehaviour {
    public static FriendManager Instance;
    public List<Friend> friends = new List<Friend>();
	public List<GameObject> worlds = new List<GameObject>();//holds the 4 sub parents for the four worlds

    void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void OnEnable () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GenerateEvents()
    {
        CalendarManager.Instance.ClearFriendEvents();
        for (int i=0; i < friends.Count; ++i)
        {
            FriendEvent the_event = friends[i].GenerateEvent();

            if (the_event != null)
                CalendarManager.Instance.AddFriendEvent(the_event);
        }
    }

    public GameObject GetFriendObject(Friend p_friend)
    {
        var friend = GetFriend(p_friend);

        if (friend != null)
            return friend.gameObject;

        return null;
    }

    public Friend GetFriend(Friend friend)
    {
        for (int i = 0; i < friends.Count; ++i)
        {
            if (friend.tag == friends[i].tag)
                return friends[i];
        }

		Debug.LogError("GetFriend was called for a friend with no tag setup! Friend: " + friend.name);
        return null;
    }

    public Friend GetFriend(string friendName)
    {
        for (int i = 0; i < friends.Count; ++i)
        {
            if (friendName == friends[i].name)
                return friends[i];
        }

        return null;
    }

    // Activate any world stuff pertaining to friends.
    public void OnWorldStart(World world)
    {
        for (int i = 0; i < friends.Count; i++) {
            friends[i].OnWorldStart(world);
        }
    }

    public void OnWorldEnd(){ // Activated by Ev_results
		for (int i = 0; i < friends.Count; i++) {
            friends[i].OnWorldEnd();
        }
    }

    public bool IsThereDayEndEvent(){
		for (int i = 0; i < friends.Count; i++) {
            if(friends[i].DayEndEventCheck()){
            	return true;
            };
        }

        return false;
    }

    public void EnableProperFriends(int worldNum){
    	Debug.Log("World Num:" + worldNum);
    	Debug.Log("World Count: " + worlds.Count);
		Debug.Log("Enable Proper Friends For World: " + worlds[worldNum-1].name);
		for(int i = 0; i < worlds[worldNum-1].transform.childCount; i++){
			worlds[worldNum-1].transform.GetChild(i).gameObject.SetActive(true);
			Debug.Log("Friend Game Object Activated: " + worlds[worldNum-1].transform.GetChild(i).gameObject.name);
		}

    }

    public void DisableAllFriends()
    {
        for (int i = 0; i < friends.Count; i++) {
            friends[i].gameObject.SetActive(false);
        }
    }

    public void DisableFriends(int worldNum){ //activated by ev_results
		for(int i = 0; i < worlds[worldNum-1].transform.childCount; i++){
			worlds[worldNum-1].transform.GetChild(i).gameObject.SetActive(false);
		}
    }
}
