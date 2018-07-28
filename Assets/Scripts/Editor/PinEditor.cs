using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PinDefinition))]
public class PinEditor : Editor {

    public override void OnInspectorGUI()
    {
        // Unity inspector is dumb. It doesn't like 64 bit enums.  It can't seem to serialize them either.
        // Well I don't like Unity's face.
        // This kanoodles it so the inspector properly displays an enum dropdown while serializing a 64 bit number (long).
        PinDefinition pinDefinition = (PinDefinition)target;
        pinDefinition.Type = (PIN)EditorGUILayout.EnumPopup(pinDefinition.Type);
        pinDefinition.pinValue = (ulong)pinDefinition.Type;

        base.OnInspectorGUI();
    }
}
