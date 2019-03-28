using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(ObjectPool))]
public class ObjectPoolEditor : Editor {
    public override void OnInspectorGUI()
    {
        // Style for when the tag is a duplicate
        GUIStyle dupTagStyle = new GUIStyle();
        dupTagStyle.normal.textColor = Color.red;
        dupTagStyle.fixedWidth = 100f;

        // Style for when the tag is fine
        GUIStyle tagStyle = new GUIStyle();
        tagStyle.fixedWidth = 100f;

        List<string> duptags = new List<string>();
        serializedObject.Update();

        var property = serializedObject.FindProperty("itemsToPool");
        if (property.isExpanded) {
            EditorGUILayout.PropertyField(property.FindPropertyRelative("Array.size"));
            for (int i = 0; i < property.arraySize; i++) {
                EditorGUIUtility.labelWidth = 55f;
                var poolDef = property.GetArrayElementAtIndex(i);
                GUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(poolDef.FindPropertyRelative("IsExpandable"), new GUIContent("Expand:"));
                EditorGUIUtility.fieldWidth = 20f;
                EditorGUILayout.PropertyField(poolDef.FindPropertyRelative("poolSize"), new GUIContent("Size:"));
                EditorGUIUtility.fieldWidth = 180f;
                var poolObject = poolDef.FindPropertyRelative("poolObject");
                if (poolObject != null) {
                    var obj = poolObject.objectReferenceValue as GameObject;
                    if (obj != null) {
                        if (duptags.Contains(obj.tag)) {
                            GUILayout.Label(obj.tag, dupTagStyle);
                        } else {
                            GUILayout.Label(obj.tag, tagStyle);
                            duptags.Add(obj.tag);
                        }
                    }
                }
                EditorGUILayout.PropertyField(poolDef.FindPropertyRelative("poolObject"), GUIContent.none);
                EditorGUILayout.PropertyField(poolDef.FindPropertyRelative("parentObject"), new GUIContent("Parent:"));
                GUILayout.EndHorizontal();
            }
            EditorGUIUtility.labelWidth = 0f;
            EditorGUIUtility.fieldWidth = 0f;
        }
        serializedObject.ApplyModifiedProperties();
    }
}
