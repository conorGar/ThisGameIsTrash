using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendEvent {
    public int day = 0;
    public Friend friend = null;

	public void Execute()
    {
        friend.Execute();
    }
}
