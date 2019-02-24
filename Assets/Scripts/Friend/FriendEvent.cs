using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendEvent {
    public int day = 0;
    public Friend friend = null;
  

    public FriendEvent(int d, Friend f){
    	day = d;
    	friend = f;
    }
    public FriendEvent(){

    }

	public void Execute()
    {
        friend.Execute();
    }
}
