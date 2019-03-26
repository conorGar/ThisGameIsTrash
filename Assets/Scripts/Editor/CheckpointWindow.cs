using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class CheckpointWindow : EditorWindow
{
    static CheckpointDebugConfig config;
    static int checkpointIndex = 0;
    static Checkpoint[] checkpointList;
    static string[] checkpointStrings;

    [MenuItem("TGIT/Start From Checkpoint")]
    static void Init()
    {
        CheckpointWindow window = (CheckpointWindow)EditorWindow.GetWindow(typeof(CheckpointWindow));
        window.Show();
    }

    private void OnFocus()
    {
        // Get the CheckpointDebugConfig that has all the info on which debug checkpoint is active.
        var configs = Resources.FindObjectsOfTypeAll<CheckpointDebugConfig>();
        if (configs.Length > 0) {
            config = configs[0];
        } else {
            // Generate the config
            CheckpointDebugConfig newConfig = new CheckpointDebugConfig();
            AssetDatabase.CreateAsset(newConfig, "Assets/Scripts/Checkpoints/CheckpointDebugConfig.asset");
            config = Resources.FindObjectsOfTypeAll<CheckpointDebugConfig>()[0];
        }

        checkpointList = GameObject.FindObjectsOfType<Checkpoint>();
        checkpointStrings = new string[checkpointList.Length];
        for (int i = 0; i < checkpointStrings.Length; i++) {
            checkpointStrings[i] = checkpointList[i].name;
        }
    }

    void OnGUI()
    {
        EditorGUI.BeginChangeCheck();
        config.isOn = EditorGUILayout.Toggle("Is On: ", config.isOn);

        if (EditorGUI.EndChangeCheck()) {
            EditorUtility.SetDirty(config);
            AssetDatabase.SaveAssets();
        }

        string sceneName = EditorSceneManager.GetActiveScene().path;
        EditorGUILayout.LabelField("Active Scene Path: " + sceneName);

        EditorGUI.BeginChangeCheck();
        checkpointIndex = EditorGUILayout.Popup(checkpointIndex, checkpointStrings);

        if (EditorGUI.EndChangeCheck()) {
            Debug.Log("Setting Debug Checkpoint for the active scene: " + sceneName + " to: " + checkpointList[checkpointIndex].name);
            config.lookUp[sceneName] = checkpointList[checkpointIndex].name;
            EditorUtility.SetDirty(config);
            AssetDatabase.SaveAssets();
        }
    }
}