using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Serializable template instance to save DialogsNodes.
[Serializable]
public class CheckpointDictionary : SerializableDictionary<string, string> { }

[CreateAssetMenu(fileName = "CheckpointDebugConfig", menuName = "TGIT/CheckpointDebugConfig", order = 2)]
public class CheckpointDebugConfig : ScriptableObject
{
    // Scene path -> checkpoint name
    public bool isOn = false;
    [SerializeField]
    public CheckpointDictionary checkpointLookup = new CheckpointDictionary();
}
