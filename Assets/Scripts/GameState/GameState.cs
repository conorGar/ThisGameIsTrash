using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour {
    public System.Action OnEnterEvent;
    public System.Action OnLeaveEvent;

    private void OnEnable()
    {
        if (OnEnterEvent != null)
            OnEnterEvent();
    }

    private void OnDisable()
    {
        if (OnLeaveEvent != null)
            OnLeaveEvent();
    }
}
