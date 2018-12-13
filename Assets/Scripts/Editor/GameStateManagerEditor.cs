using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameStateManager))]
public class GameStateManagerEditor : Editor {

    void OnEnable()
    {

    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        serializedObject.ApplyModifiedProperties();
        if (GameStateManager.Instance != null) {
            var manager = target as GameStateManager;
            // Show some more debug info
            EditorGUILayout.LabelField("Game State Stack Size: " + manager.GetStateStackCount());
            EditorGUILayout.LabelField("Current Game State: " + manager.GetCurrentState());

            foreach (var item in manager.GetGameStateStack()) {
                EditorGUILayout.LabelField(item.GetType().ToString());
            }
        }
        base.OnInspectorGUI();
    }

    public override bool RequiresConstantRepaint()
    {
        return true;
    }
}
