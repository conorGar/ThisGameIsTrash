using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using JimEditor = GenericStateControllerEditor<JimState, JimTrigger, JimFlag>;
using EnemyEditor = GenericStateControllerEditor<EnemyState, EnemyTrigger, EnemyFlag>;

// Hooks up a nice custom inspector on any state controllers we build.
[CustomEditor(typeof(JimStateController))]
public class JimStateControllerEditor : JimEditor { }

[CustomEditor(typeof(RatStateController))]
public class RatStateControllerEditor : EnemyEditor { }

[CustomEditor(typeof(BoomerangSlimeStateController))]
public class BoomerangSlimeStateControllerEditor : EnemyEditor { }

[CustomEditor(typeof(DuckStateController))]
public class DuckStateControllerEditor : EnemyEditor { }

[CustomEditor(typeof(DogStateController))]
public class DogStateControllerEditor : EnemyEditor { }

[CustomEditor(typeof(BlackMoleStateController))]
public class BlackMoleStateControllerEditor : EnemyEditor { }

[CustomEditor(typeof(SlimeStateController))]
public class SlimeStateControllerEditor : EnemyEditor { }

// Bosses
[CustomEditor(typeof(StuartStateController))]
public class StuartStateControllerEditor : EnemyEditor { }

[CustomEditor(typeof(ExStateController))]
public class ExStateControllerEditor : EnemyEditor { }

[CustomEditor(typeof(QuestioStateController))]
public class QuestioStateControllerEditor : EnemyEditor { }

[CustomEditor(typeof(HashStateController))]
public class HashStateControllerEditor : EnemyEditor { }

[CustomEditor(typeof(GrubStateController))]
public class GrubStateControllerEditor : EnemyEditor { }

[CustomEditor(typeof(RhinoBeetleStateController))]
public class RhinoBeetleStateControllerEditor : EnemyEditor { }

[CustomEditor(typeof(OpossumStateController))]
public class OpossumStateControllerEditor : EnemyEditor { }

[CustomEditor(typeof(HeronStateController))]
public class HeronStateControllerEditor : EnemyEditor { }

[CustomEditor(typeof(SandCrabStateController))]
public class SandCrabStateControllerEditor : EnemyEditor { }

[CustomEditor(typeof(CrabStateController))]
public class CrabStateControllerEditor : EnemyEditor { }





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