using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(JimStateController))]
public class JimStateControllerEditor : GenericStateControllerEditor<JimFlag>
{
}

public class GenericStateControllerEditor<T> : Editor
{
    public override void OnInspectorGUI()
    {
        var controller = (JimStateController)target;

        base.OnInspectorGUI();

        if (controller.currentState != null) {
            EditorGUILayout.LabelField("Current State: ", controller.GetCurrentState().ToString());

            System.Type genericType = typeof(T);
            if (genericType.IsEnum) {
                foreach (T flag in Enum.GetValues(typeof(T))) {
                    Enum e = Enum.Parse(typeof(T), flag.ToString()) as Enum;
                    if (Convert.ToInt32(e) == 0)
                        continue;

                    EditorGUILayout.LabelField(flag.ToString() + ": ", controller.IsFlag(Convert.ToInt32(e)).ToString());
                }
            }
        } else {
            EditorGUILayout.LabelField("Current State: NULL");
        }
    }
}