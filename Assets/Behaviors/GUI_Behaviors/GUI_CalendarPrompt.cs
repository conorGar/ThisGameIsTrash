using UnityEngine;
using System.Collections;

public class GUI_CalendarPrompt : MonoBehaviour
{
	public GameObject notifyIcon;
	// Use this for initialization
	void Start ()
	{
		for(int i = 0; i < CalendarManager.Instance.friendEvents.Count; i++){
			if(CalendarManager.Instance.friendEvents[i].day == GlobalVariableManager.Instance.DAY_NUMBER){
				if(CalendarManager.Instance.friendEvents[i].friend.myFriendType == Friend.FriendType.ScheduleFriend){
					notifyIcon.SetActive(true);
					break;
				}

			}
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}

