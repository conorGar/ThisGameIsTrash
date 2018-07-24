using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PinConfig", menuName = "TGIT/PinConfig", order = 1)]
public class PinConfig : ScriptableObject
{
    public List<PinDefinition> pinList;
}
