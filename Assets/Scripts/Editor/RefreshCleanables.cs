using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

// Goes through all the cleanables in a scene and gives them a unique bit.
public class RefreshCleanables : MonoBehaviour
{
    [MenuItem("TGIT/Refresh Cleanables")]
    static void Refresh()
    {
        // look up table used to keep track of the bits we are assigning per type.
        Dictionary<CLEANABLE_TYPE, int> lookUp = new Dictionary<CLEANABLE_TYPE, int>();
        foreach (CLEANABLE_TYPE type in Enum.GetValues(typeof(CLEANABLE_TYPE))) {
            lookUp[type] = 0;
        }

        CleanableItem[] cleanableItems = FindObjectsOfType<CleanableItem>();
        for (int i = 0; i < cleanableItems.Length; i++) {
            var item = cleanableItems[i];

            if (item.cleanable != null && item.cleanable.CleanableType() != CLEANABLE_TYPE.NONE) {
                CLEANABLE_BIT bit = (CLEANABLE_BIT)((long)1 << lookUp[item.cleanable.CleanableType()]++); // shift to the next bit.
                if (bit == CLEANABLE_BIT.NONE) { // We've shifted beyond 64 bits.  Make sure this is reported!
                    Debug.LogError(item.cleanable.CleanableType() + " has more than 64 cleanables.  We can only store 64 so delete some!");
                }
                item.cleanableBit = bit;
                EditorUtility.SetDirty(item);
                Debug.Log(item.name + " with type: " + item.cleanable.CleanableType() + " assigned bit: " + item.cleanableBit);
            }
        }

        Debug.Log("Cleanable bits assigned!");
        if (EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene())) {
            Debug.Log(EditorSceneManager.GetActiveScene().name + " scene marked dirty."); 
        }
    }
}