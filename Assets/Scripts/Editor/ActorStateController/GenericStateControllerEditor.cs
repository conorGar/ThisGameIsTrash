using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(JimStateController))]
public class JimStateControllerEditor : GenericStateControllerEditor<JimState, JimTrigger, JimFlag>
{
}

[CustomEditor(typeof(RatStateController))]
public class RatStateControllerEditor : GenericStateControllerEditor<EnemyState, EnemyTrigger, EnemyFlag>
{
}

[CustomEditor(typeof(DuckStateController))]
public class DuckStateControllerEditor : GenericStateControllerEditor<EnemyState, EnemyTrigger, EnemyFlag>
{
}

public class GenericStateControllerEditor<State_Type, Trigger_Type, Flag_Type> : Editor
{
    public override void OnInspectorGUI()
    {
        var controller = (ActorStateController<State_Type, Trigger_Type>)target;

        base.OnInspectorGUI();

        if (controller.currentState != null) {
            EditorGUILayout.LabelField("Current State: ", controller.GetCurrentState().ToString());

            System.Type genericType = typeof(Flag_Type);
            if (genericType.IsEnum) {
                foreach (Flag_Type flag in Enum.GetValues(typeof(Flag_Type))) {
                    Enum e = Enum.Parse(typeof(Flag_Type), flag.ToString()) as Enum;
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