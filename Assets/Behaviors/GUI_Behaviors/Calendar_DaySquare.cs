﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Calendar_DaySquare : MonoBehaviour
{
	List<GameObject> myIcons = new List<GameObject>();

	//TODO: Determine positioning of new icon based on how many icons are in the box already
	//TODO: ^ determine positioning also based on whether this 'Calendar_DaySquare' Object is one of the two other boxes on the side
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public GameObject AddIcon(FriendEvent fEvent){
		GameObject newIcon = ObjectPool.Instance.GetPooledObject("calendarIcon",gameObject.transform.position);
		newIcon.GetComponent<SpriteRenderer>().sprite = fEvent.friend.calendarIcon;
		newIcon.transform.parent = this.transform;
		myIcons.Add(newIcon);
		return newIcon;
	}
}
