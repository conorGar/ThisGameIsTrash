using System.Collections;
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

    public void EnableProperFriends(int worldNum){
    	Debug.Log("World Num:" + worldNum);
    	Debug.Log(worlds.Count);
		Debug.Log("Enable Proper Friends activated" + worlds[worldNum-1].name);
		for(int i = 0; i < worlds[worldNum-1].transform.childCount; i++){
			worlds[worldNum-1].transform.GetChild(i).gameObject.SetActive(true);
			Debug.Log(worlds[worldNum-1].transform.GetChild(i).gameObject.name);
		}

    }

    public void DisableFriends(int worldNum){ //activated by ev_results
		for(int i = 0; i < worlds[worldNum-1].transform.childCount; i++){
			worlds[worldNum-1].transform.GetChild(i).gameObject.SetActive(false);
		}
    }
}
