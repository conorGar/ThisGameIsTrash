using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour {
    public static GameStateManager Instance;
    public List<GameState> gameStates;

    Stack<GameState> currentGameStateStack = new Stack<GameState>();

    void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start ()
    {
        // turn off all game states.
        for (int i = 0; i < gameStates.Count; i++) {
            gameStates[i].gameObject.SetActive(false);
        }
	}

    public Type GetCurrentState()
    {
        if (currentGameStateStack.Count > 0) {
            return currentGameStateStack.Peek().GetType();
        }

        return null;
    }

    public GameState GetState(Type type)
    {
        for (int i = 0; i < gameStates.Count; i++) {
            GameState state = gameStates[i];
            if (type == state.GetType()) {
                return state;
            }
        }

        return null;
    }

    public void PushState(Type type)
    {
        for (int i = 0; i < gameStates.Count; i++) {
            GameState state = gameStates[i];
            if (type == state.GetType()) {
                EnterState(state);
            }
        }
    }

    public void PopState()
    {
        if (currentGameStateStack.Count > 0) {
            LeaveState();
        }
    }

    public void PopAllStates()
    {
        while (currentGameStateStack.Count > 0) {
            LeaveState();
        }
    }

    // Register / Unregister convenience functions
    public void RegisterEnterEvent(Type type, Action action)
    {
#if DEBUG_GAMESTATE
        Debug.Log("Registering Enter Event. Type:" + type + " Action: " + action);
#endif
        GameStateManager.Instance.GetState(type).OnEnterEvent += action;
    }

    public void RegisterLeaveEvent(Type type, Action action)
    {
#if DEBUG_GAMESTATE
        Debug.Log("Registering Leave Event. Type:" + type + " Action: " + action);
#endif
        GameStateManager.Instance.GetState(type).OnLeaveEvent += action;
    }

    public void UnregisterEnterEvent(Type type, Action action)
    {
#if DEBUG_GAMESTATE
        Debug.Log("Unregistering Enter Event. Type:" + type + " Action: " + action);
#endif
        GameStateManager.Instance.GetState(type).OnEnterEvent -= action;
    }

    public void UnregisterLeaveEvent(Type type, Action action)
    {
#if DEBUG_GAMESTATE
        Debug.Log("Unregistering Leave Event. Type:" + type + " Action: " + action);
#endif
        GameStateManager.Instance.GetState(type).OnLeaveEvent -= action;
    }

    // Helpers
    void EnterState(GameState state)
    { 
        currentGameStateStack.Push(state);
#if DEBUG_GAMESTATE
        Debug.Log("Entering Gamestate: " + state.GetType());
#endif
        state.gameObject.SetActive(true);
    }

    void LeaveState()
    {
        GameState state = currentGameStateStack.Pop();
#if DEBUG_GAMESTATE
        Debug.Log("Leaving Gamestate: " + state.GetType());
#endif
        state.gameObject.SetActive(false);
    }
}
