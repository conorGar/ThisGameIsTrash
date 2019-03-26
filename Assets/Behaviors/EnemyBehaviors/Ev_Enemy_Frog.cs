using UnityEngine;
using System.Collections;

public class Ev_Enemy_Frog : FollowPlayerAfterNotice
{

	protected override void NoticePlayerEvent(){
		Debug.Log("-f-f-f-f-f-f-f-  Frog notices player!   -f-f-f-f-f-f-f-f");
        gameObject.GetComponent<FireTowardPlayer>().StartCoroutine("Fire");
	}

}

