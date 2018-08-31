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

[System.Serializable]
public struct GAMEACTION
{
    public INPUTACTION action;
}

public enum INPUTDIRECTION
{
    POSITIVE,
    NEGATIVE
}

[System.Serializable]
public struct INPUT
{
    public INPUTACTION action;
    public INPUTTYPE type;
    public INPUTDIRECTION axisDirection;
    public List<string> keys;
}

[CreateAssetMenu(fileName = "New Input", menuName = "TGIT/Input", order = 2)]
public class InputDefinition : ScriptableObject {
    public string joystickName = "";
    public List<INPUT> inputs = new List<INPUT>();
}
