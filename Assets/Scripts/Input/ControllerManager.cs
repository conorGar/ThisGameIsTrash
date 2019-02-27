using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct INPUTSNAPSHOT
{
    public bool isPressed;
    public float rawAxis;
}

public class ControllerManager : MonoBehaviour {
    public static ControllerManager Instance;
    public List<GAMEACTION> actions;
    public List<InputDefinition> inputDefinitions;
    public float DEADZONE = 0.5f;
    private Dictionary<INPUTACTION, INPUTSNAPSHOT> currentInputs;
    private Dictionary<INPUTACTION, INPUTSNAPSHOT> previousInputs;
    private Dictionary<string, InputDefinition> inputDefLookup;
    private List<string> unsupportedInputs = new List<string>();



    // Use this for initialization
    void Awake() {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            currentInputs = new Dictionary<INPUTACTION, INPUTSNAPSHOT>();
            previousInputs = new Dictionary<INPUTACTION, INPUTSNAPSHOT>();

            // build the previous and current input lists.
            for (int i = 0; i < actions.Count; i++)
            {
                currentInputs[actions[i].action] = new INPUTSNAPSHOT();
                previousInputs[actions[i].action] = new INPUTSNAPSHOT();
            }

            inputDefLookup = new Dictionary<string, InputDefinition>();
            for (int i = 0; i < inputDefinitions.Count; i++)
            {
                inputDefLookup[inputDefinitions[i].joystickName] = inputDefinitions[i];
            }

            string[] inputNames = Input.GetJoystickNames();
            /*for (int i = 0; i < Input.GetJoystickNames().Length; i++)
            {
                Debug.Log(i + " input: " + inputNames[i]);
            }*/
            
        }
    }

    // Inputs are updated in the late update so other gameObjects don't have differing inputs.
    void LateUpdate()
    {
        // store the last frame
        // TODO: dictionary copy every frame.  Is this too slow?
        previousInputs = new Dictionary<INPUTACTION, INPUTSNAPSHOT>(currentInputs);

        for (int i = 0; i < actions.Count; i++)
        {
            currentInputs[actions[i].action] = new INPUTSNAPSHOT();
        }

        string[] joystickNames = Input.GetJoystickNames();

        UpdateInputs("Keyboard", 0);

        for (int i = 0; i < joystickNames.Length; i++)
        {
            UpdateInputs(joystickNames[i], i+1);
        }
    }

    private void UpdateInputs(string joystickName, int position)
    {
        string joyName = "";

        // Empty string is the keyboard, I think.
        if (string.IsNullOrEmpty(joystickName))
            return;

        if (unsupportedInputs.Contains(joystickName))
            return;

        if (joystickName != "Keyboard")
            joyName = "Joy" + (position);

        InputDefinition inputDef;
        if (inputDefLookup.TryGetValue(joystickName, out inputDef))
        {
            INPUTSNAPSHOT snapshot = new INPUTSNAPSHOT();
            // snapshot inputs every frame
            for (int j = 0; j<actions.Count; j++)
            {
                INPUT input = inputDef.inputs[j];
                snapshot = currentInputs[actions[j].action];

                for (int k = 0; k<input.keys.Count; k++)
                {
                    string key = joyName + input.keys[k];
                    float rawAxis = Input.GetAxisRaw(key);
                    switch (input.type)
                    {
                        // Quick and dirty axis detection with a deadzone
                        case INPUTTYPE.AXIS:

                            // get the most extreme axis and store that.
                            if (Mathf.Abs(rawAxis) > Mathf.Abs(snapshot.rawAxis))
                                snapshot.rawAxis = rawAxis;

                            // clamp deadzone to 0f
                            if (DEADZONE > Mathf.Abs(snapshot.rawAxis))
                                snapshot.rawAxis = 0f;

                            if (input.axisDirection == INPUTDIRECTION.POSITIVE)
                                snapshot.isPressed |= snapshot.rawAxis > 0;
                            else
                                snapshot.isPressed |= snapshot.rawAxis < 0;

                            break;
                        case INPUTTYPE.BUTTON:
                            if (input.axisDirection == INPUTDIRECTION.POSITIVE)
                                snapshot.isPressed |= rawAxis > 0;
                            else
                                snapshot.isPressed |= rawAxis< 0;

                            if (Mathf.Abs(rawAxis) > Mathf.Abs(snapshot.rawAxis))
                                snapshot.rawAxis = rawAxis;

                            break;
                    }
                }

                currentInputs[input.action] = snapshot;
            }
        }
        else
        {
            Debug.Log("Unsupported Controller: " + joystickName + "!!! Write a definition for it!");
            unsupportedInputs.Add(joystickName);
        }
    }

    // Functions for retrieving button states.
    public bool GetKey(INPUTACTION action)
    {
        return currentInputs[action].isPressed;
    }

    public bool GetKeyDown(INPUTACTION action)
    {
        return currentInputs[action].isPressed && !previousInputs[action].isPressed;
    }

    public bool GetKeyUp(INPUTACTION action)
    {
        return !currentInputs[action].isPressed && previousInputs[action].isPressed;
    }

    public float GetAxis(INPUTACTION action)
    {
        return currentInputs[action].rawAxis;
    }
}
