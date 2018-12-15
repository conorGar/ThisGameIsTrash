using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour {
    public static GameStateManager Instance;
    public List<GameState> gameStates;
    public System.Action<System.Type, bool> OnChangeStateEvent;

    Stack<GameState> currentGameStateStack = new Stack<GameState>();

    public bool debugMode; //just pushes gameplay state at start for test rooms

    void Awake()
    {
#if RUN_IN_BACKGROUND // To make debugging less annoying!  This will allow the game to run even when it's lost focus in windows.
        Application.runInBackground = true;
#endif

        Instance = this;
        if(debugMode)
        	GameStateManager.Instance.PushState(typeof(GameplayState));
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

    public Stack<GameState> GetGameStateStack()
    {
        return currentGameStateStack;
    }

    public int GetStateStackCount()
    {
        return currentGameStateStack.Count;
    }

    public void PushState(Type type)
    {
        for (int i = 0; i < gameStates.Count; i++) {
            GameState state = gameStates[i];
            if (type == state.GetType()) {
                EnterState(state);
                return;
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
    public void RegisterChangeStateEvent(Action<System.Type, bool> action)
    {
#if DEBUG_GAMESTATE
        Debug.Log("Registering Change State Event. Action: " + action);
#endif
        GameStateManager.Instance.OnChangeStateEvent += action;
    }
    public void UnregisterChangeStateEvent(Action<System.Type, bool> action)
    {
#if DEBUG_GAMESTATE
        Debug.Log("Unregistering Change State Event. Action: " + action);
#endif
        GameStateManager.Instance.OnChangeStateEvent -= action;
    }

    // Helpers
    void EnterState(GameState state)
    { 
        currentGameStateStack.Push(state);
#if DEBUG_GAMESTATE
        Debug.Log("Entering Gamestate: " + state.GetType());
#endif
        state.gameObject.SetActive(true);
        if (OnChangeStateEvent != null)
            OnChangeStateEvent(state.GetType(), true);
    }

    void LeaveState()
    {
        GameState state = currentGameStateStack.Pop();
#if DEBUG_GAMESTATE
        Debug.Log("Leaving Gamestate: " + state.GetType());
#endif
        state.gameObject.SetActive(false);
        if (OnChangeStateEvent != null)
            OnChangeStateEvent(state.GetType(), false);
    }
}
