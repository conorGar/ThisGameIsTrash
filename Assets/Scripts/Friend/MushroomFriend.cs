using UnityEngine;
using System.Collections;

public class MushroomFriend : Friend
{
	int mushroomsCollected;
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public override void PickUpObject(CollectableFriendObject item){
		mushroomsCollected++;
	}
}

