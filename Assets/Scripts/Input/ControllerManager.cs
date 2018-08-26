using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum INPUTTYPE
{
    BUTTON,
    AXIS
}

public enum INPUTACTION
{
    MOVEUP,
    MOVEDOWN,
    MOVELEFT,
    MOVERIGHT,
    ATTACKUP,
    ATTACKDOWN,
    ATTACKLEFT,
    ATTACKRIGHT,
    INTERACT,
    CANCEL,
    SWITCHWEAPON,
    PAUSE
}

public enum INPUTDIRECTION
{
    POSITIVE,
    NEGATIVE
}

[System.Serializable]
public struct INPUT
{
    public INPUTTYPE type;
    public INPUTACTION action;
    public INPUTDIRECTION direction;
    public string inputkey;
}

public struct INPUTSNAPSHOT
{
    public bool isPressed;
    public float rawAxis;
}

public class ControllerManager : MonoBehaviour {
    public static ControllerManager Instance;
    public float DEADZONE = 0.5f;
    public List<INPUT> inputs = new List<INPUT>();
    private Dictionary<INPUTACTION, INPUTSNAPSHOT> currentInputs;
    private Dictionary<INPUTACTION, INPUTSNAPSHOT> previousInputs;



    // Use this for initialization
    void Awake () {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            currentInputs = new Dictionary<INPUTACTION, INPUTSNAPSHOT>();
            previousInputs = new Dictionary<INPUTACTION, INPUTSNAPSHOT>();

            // Build the inputLookup from the list since it should be quicker during runtime and doesn't have to be serialized.
            // build the previous and current input lists.
            for (int i = 0; i < inputs.Count; i++)
            {
                currentInputs[inputs[i].action] = new INPUTSNAPSHOT();
                previousInputs[inputs[i].action] = new INPUTSNAPSHOT();
            }
        }
	}

    void Update()
    {
        INPUTSNAPSHOT new_input = new INPUTSNAPSHOT();
        // store the last frame
        // TODO: dictionary copy every frame.  Is this too slow?
        previousInputs = new Dictionary<INPUTACTION,INPUTSNAPSHOT>(currentInputs);


        // snapshot inputs every frame
        for (int i = 0; i < inputs.Count; i++)
        {
            INPUT input = inputs[i];

            switch (input.type)
            {
                // Quick and dirty axis detection with a deadzone
                case INPUTTYPE.AXIS:
                    new_input.rawAxis = Input.GetAxisRaw(input.inputkey);

                    // clamp deadzone to 0f
                    if (DEADZONE > new_input.rawAxis && new_input.rawAxis > -DEADZONE)
                        new_input.rawAxis = 0f;

                    if (input.direction == INPUTDIRECTION.POSITIVE)
                        new_input.isPressed = new_input.rawAxis > 0;
                    else
                        new_input.isPressed = new_input.rawAxis < 0;
                    break;
                case INPUTTYPE.BUTTON:
                    new_input.isPressed = Input.GetButton(input.inputkey);
                    if (input.direction == INPUTDIRECTION.POSITIVE)
                        new_input.rawAxis = 1f;
                    else
                        new_input.rawAxis = -1f;
                    break;
            }

            currentInputs[input.action] = new_input;
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
